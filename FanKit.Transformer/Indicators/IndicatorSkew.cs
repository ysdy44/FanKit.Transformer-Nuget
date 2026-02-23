namespace FanKit.Transformer.Indicators
{
    public readonly partial struct IndicatorSkew
    {
        const float PI = (float)System.Math.PI;
        const float PI2 = (float)(System.Math.PI / 2d);

        const float radians2degrees = (float)(180.0f / System.Math.PI);
        const float degrees2radians = (float)(System.Math.PI / 180.0);

        readonly float rx;
        readonly float ry;

        readonly float rad;

        readonly float tan;
        readonly float v;
        internal readonly float fx0, fx1;

        readonly float vy0, vy1;
        readonly float vx0, vx1;

        readonly float s0, s1;
        readonly float u0, u1;
        readonly float q0, q1;

        readonly float f0, f1;
        internal readonly float fy0, fy1;

        #region Triangles
        public IndicatorSkew(float hr, Triangle t, BoxMode m, float d, float min, float max)
        {
            switch (m)
            {
                case BoxMode.LeftTop:
                case BoxMode.CenterTop:
                case BoxMode.RightTop:
                    //y = tan*(x - rx) + ry;
                    // (y-y2)/ (y1-y2) = (x-x2)/ (x1-x2)
                    rx = (t.LeftTop.X + t.RightTop.X) / 2f;
                    ry = (t.LeftTop.Y + t.RightTop.Y) / 2f;

                    if (d < min)
                        rad = PI2 + hr + min * degrees2radians;
                    else if (d > max)
                        rad = PI2 + hr - max * degrees2radians;
                    else
                        rad = PI2 + hr - d * degrees2radians;

                    tan = (float)System.Math.Tan(rad);
                    v = tan * rx;

                    fx1 = default;

                    vy1 = default;
                    vx1 = default;

                    s1 = default;
                    u1 = default;
                    q1 = default;

                    f1 = default;
                    fy1 = default;

                    if (t.LeftTop.X == t.RightTop.X)
                    {
                        vy0 = default;
                        vx0 = default;

                        s0 = default;
                        u0 = default;
                        q0 = default;

                        fx0 = t.LeftBottom.X;
                    }
                    else
                    {
                        vy0 = t.LeftTop.Y - t.RightTop.Y;
                        vx0 = t.LeftTop.X - t.RightTop.X;

                        s0 = vy0 / vx0;
                        u0 = (t.RightTop.X + t.LeftBottom.X - t.LeftTop.X) * s0;
                        q0 = t.RightTop.Y + t.LeftBottom.Y - t.LeftTop.Y - ry - u0 + v;

                        fx0 = q0 / (tan - s0);
                    }

                    f0 = tan * fx0;
                    fy0 = f0 - v + ry;
                    break;
                case BoxMode.LeftBottom:
                case BoxMode.CenterBottom:
                case BoxMode.RightBottom:
                    //y = tan*(x - rx) + ry;
                    // (y-y2)/ (y1-y2) = (x-x2)/ (x1-x2)
                    rx = t.LeftBottom.X + (t.RightTop.X - t.LeftTop.X) / 2f;
                    ry = t.LeftBottom.Y + (t.RightTop.Y - t.LeftTop.Y) / 2f;

                    if (d < min)
                        rad = PI2 + hr + min * degrees2radians;
                    else if (d > max)
                        rad = PI2 + hr - max * degrees2radians;
                    else
                        rad = PI2 + hr - d * degrees2radians;

                    tan = (float)System.Math.Tan(rad);
                    v = tan * rx;

                    fx0 = default;

                    vy0 = default;
                    vx0 = default;

                    s0 = default;
                    u0 = default;
                    q0 = default;

                    f0 = default;
                    fy0 = default;

                    if (t.RightTop.X == t.LeftTop.X)
                    {
                        vy1 = default;
                        vx1 = default;

                        s1 = default;
                        u1 = default;
                        q1 = default;

                        fx1 = t.RightTop.X;
                    }
                    else
                    {
                        vy1 = t.RightTop.Y - t.LeftTop.Y;
                        vx1 = t.RightTop.X - t.LeftTop.X;

                        s1 = vy1 / vx1;
                        u1 = t.LeftTop.X * s1;
                        q1 = t.LeftTop.Y - ry - u1 + v;

                        fx1 = q1 / (tan - s1);
                    }

                    f1 = tan * fx1;
                    fy1 = f1 - v + ry;
                    break;
                case BoxMode.CenterLeft:
                case BoxMode.CenterRight:
                case BoxMode.Center:
                    //y = tan*(x - rx) + ry;
                    // (y-y2)/ (y1-y2) = (x-x2)/ (x1-x2)
                    rx = (t.RightTop.X + t.LeftBottom.X) / 2f;
                    ry = (t.RightTop.Y + t.LeftBottom.Y) / 2f;

                    if (d < min)
                        rad = PI2 + hr + min * degrees2radians;
                    else if (d > max)
                        rad = PI2 + hr - max * degrees2radians;
                    else
                        rad = PI2 + hr - d * degrees2radians;

                    tan = (float)System.Math.Tan(rad);
                    v = tan * rx;

                    if (t.LeftTop.X == t.RightTop.X)
                    {
                        vy0 = default;
                        vx0 = default;

                        s0 = default;
                        u0 = default;
                        q0 = default;

                        fx0 = t.LeftBottom.X;
                    }
                    else
                    {
                        vy0 = t.LeftTop.Y - t.RightTop.Y;
                        vx0 = t.LeftTop.X - t.RightTop.X;

                        s0 = vy0 / vx0;
                        u0 = (t.RightTop.X + t.LeftBottom.X - t.LeftTop.X) * s0;
                        q0 = t.RightTop.Y + t.LeftBottom.Y - t.LeftTop.Y - ry - u0 + v;

                        fx0 = q0 / (tan - s0);
                    }

                    if (t.RightTop.X == t.LeftTop.X)
                    {
                        vy1 = default;
                        vx1 = default;

                        s1 = default;
                        u1 = default;
                        q1 = default;

                        fx1 = t.RightTop.X;
                    }
                    else
                    {
                        vy1 = t.RightTop.Y - t.LeftTop.Y;
                        vx1 = t.RightTop.X - t.LeftTop.X;

                        s1 = vy1 / vx1;
                        u1 = t.LeftTop.X * s1;
                        q1 = t.LeftTop.Y - ry - u1 + v;

                        fx1 = q1 / (tan - s1);
                    }

                    f0 = tan * fx0;
                    fy0 = f0 - v + ry;

                    f1 = tan * fx1;
                    fy1 = f1 - v + ry;
                    break;
                default:
                    rx = default;
                    ry = default;

                    rad = default;

                    tan = default;
                    v = default;
                    fx0 = default; fx1 = default;

                    vy0 = default; vy1 = default;
                    vx0 = default; vx1 = default;

                    s0 = default; s1 = default;
                    u0 = default; u1 = default;
                    q0 = default; q1 = default;

                    f0 = default; f1 = default;
                    fy0 = default; fy1 = default;
                    break;
            }
        }
        #endregion

        #region Quadrilaterals
        public IndicatorSkew(float hr, Quadrilateral t, BoxMode m, float d, float min, float max)
        {
            switch (m)
            {
                case BoxMode.LeftTop:
                case BoxMode.CenterTop:
                case BoxMode.RightTop:
                    //y = tan*(x - rx) + ry;
                    // (y-y2)/ (y1-y2) = (x-x2)/ (x1-x2)
                    rx = (t.LeftTop.X + t.RightTop.X) / 2f;
                    ry = (t.LeftTop.Y + t.RightTop.Y) / 2f;

                    if (d < min)
                        rad = PI2 + hr + min * degrees2radians;
                    else if (d > max)
                        rad = PI2 + hr - max * degrees2radians;
                    else
                        rad = PI2 + hr - d * degrees2radians;

                    tan = (float)System.Math.Tan(rad);
                    v = tan * rx;

                    fx1 = default;

                    vy1 = default;
                    vx1 = default;

                    s1 = default;
                    u1 = default;
                    q1 = default;

                    f1 = default;
                    fy1 = default;

                    if (t.LeftBottom.X == t.RightBottom.X)
                    {
                        vy0 = default;
                        vx0 = default;

                        s0 = default;
                        u0 = default;
                        q0 = default;

                        fx0 = t.LeftBottom.X;
                    }
                    else
                    {
                        vy0 = t.LeftBottom.Y - t.RightBottom.Y;
                        vx0 = t.LeftBottom.X - t.RightBottom.X;

                        s0 = vy0 / vx0;
                        u0 = t.RightBottom.X * s0;
                        q0 = t.RightBottom.Y - ry - u0 + v;

                        fx0 = q0 / (tan - s0);
                    }

                    f0 = tan * fx0;
                    fy0 = f0 - v + ry;
                    break;
                case BoxMode.LeftBottom:
                case BoxMode.CenterBottom:
                case BoxMode.RightBottom:
                    //y = tan*(x - rx) + ry;
                    // (y-y2)/ (y1-y2) = (x-x2)/ (x1-x2)
                    rx = (t.RightBottom.X + t.LeftBottom.X) / 2f;
                    ry = (t.RightBottom.Y + t.LeftBottom.Y) / 2f;

                    if (d < min)
                        rad = PI2 + hr + min * degrees2radians;
                    else if (d > max)
                        rad = PI2 + hr - max * degrees2radians;
                    else
                        rad = PI2 + hr - d * degrees2radians;

                    tan = (float)System.Math.Tan(rad);
                    v = tan * rx;

                    fx0 = default;

                    vy0 = default;
                    vx0 = default;

                    s0 = default;
                    u0 = default;
                    q0 = default;

                    f0 = default;
                    fy0 = default;

                    if (t.RightTop.X == t.LeftTop.X)
                    {
                        vy1 = default;
                        vx1 = default;

                        s1 = default;
                        u1 = default;
                        q1 = default;

                        fx1 = t.RightTop.X;
                    }
                    else
                    {
                        vy1 = t.RightTop.Y - t.LeftTop.Y;
                        vx1 = t.RightTop.X - t.LeftTop.X;

                        s1 = vy1 / vx1;
                        u1 = t.LeftTop.X * s1;
                        q1 = t.LeftTop.Y - ry - u1 + v;

                        fx1 = q1 / (tan - s1);
                    }

                    f1 = tan * fx1;
                    fy1 = f1 - v + ry;
                    break;
                case BoxMode.CenterLeft:
                case BoxMode.CenterRight:
                case BoxMode.Center:
                    //y = tan*(x - rx) + ry;
                    // (y-y2)/ (y1-y2) = (x-x2)/ (x1-x2)
                    rx = (t.LeftTop.X + t.RightTop.X + t.RightBottom.X + t.LeftBottom.X) / 4f;
                    ry = (t.LeftTop.Y + t.RightTop.Y + t.RightBottom.Y + t.LeftBottom.Y) / 4f;

                    if (d < min)
                        rad = PI2 + hr + min * degrees2radians;
                    else if (d > max)
                        rad = PI2 + hr - max * degrees2radians;
                    else
                        rad = PI2 + hr - d * degrees2radians;

                    tan = (float)System.Math.Tan(rad);
                    v = tan * rx;

                    if (t.LeftBottom.X == t.RightBottom.X)
                    {
                        vy0 = default;
                        vx0 = default;

                        s0 = default;
                        u0 = default;
                        q0 = default;

                        fx0 = t.LeftBottom.X;
                    }
                    else
                    {
                        vy0 = t.LeftBottom.Y - t.RightBottom.Y;
                        vx0 = t.LeftBottom.X - t.RightBottom.X;

                        s0 = vy0 / vx0;
                        u0 = t.RightBottom.X * s0;
                        q0 = t.RightBottom.Y - ry - u0 + v;

                        fx0 = q0 / (tan - s0);
                    }

                    if (t.RightTop.X == t.LeftTop.X)
                    {
                        vy1 = default;
                        vx1 = default;

                        s1 = default;
                        u1 = default;
                        q1 = default;

                        fx1 = t.RightTop.X;
                    }
                    else
                    {
                        vy1 = t.RightTop.Y - t.LeftTop.Y;
                        vx1 = t.RightTop.X - t.LeftTop.X;

                        s1 = vy1 / vx1;
                        u1 = t.LeftTop.X * s1;
                        q1 = t.LeftTop.Y - ry - u1 + v;

                        fx1 = q1 / (tan - s1);
                    }

                    f0 = tan * fx0;
                    fy0 = f0 - v + ry;

                    f1 = tan * fx1;
                    fy1 = f1 - v + ry;
                    break;
                default:
                    rx = default;
                    ry = default;

                    rad = default;

                    tan = default;
                    v = default;
                    fx0 = default; fx1 = default;

                    vy0 = default; vy1 = default;
                    vx0 = default; vx1 = default;

                    s0 = default; s1 = default;
                    u0 = default; u1 = default;
                    q0 = default; q1 = default;

                    f0 = default; f1 = default;
                    fy0 = default; fy1 = default;
                    break;
            }
        }
        #endregion
    }
}