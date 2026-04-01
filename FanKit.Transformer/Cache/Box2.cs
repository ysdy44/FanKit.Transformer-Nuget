using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Cache
{
    public readonly partial struct Box2
    {
        #region Public instance methods
        public Vector2[] To3Points() => new Vector2[]
        {
            this.LeftTop,
            this.RightTop,
            this.LeftBottom,
        };
        public Vector2[] To4Points() => new Vector2[]
        {
            this.LeftTop,
            this.RightTop,
            this.RightBottom,
            this.LeftBottom,
        };
        public Bounds ToBounds() => new Bounds
        {
            Left = System.Math.Min(System.Math.Min(this.LeftTop.X, this.RightTop.X), System.Math.Min(this.RightBottom.X, this.LeftBottom.X)),
            Top = System.Math.Min(System.Math.Min(this.LeftTop.Y, this.RightTop.Y), System.Math.Min(this.RightBottom.Y, this.LeftBottom.Y)),
            Right = System.Math.Max(System.Math.Max(this.LeftTop.X, this.RightTop.X), System.Math.Max(this.RightBottom.X, this.LeftBottom.X)),
            Bottom = System.Math.Max(System.Math.Max(this.LeftTop.Y, this.RightTop.Y), System.Math.Max(this.RightBottom.Y, this.LeftBottom.Y)),
        };
        public Triangle ToTriangle() => new Triangle
        {
            LeftTop = this.LeftTop,
            RightTop = this.RightTop,
            LeftBottom = this.LeftBottom,
        };
        public Quadrilateral ToQuadrilateral() => new Quadrilateral
        {
            LeftTop = this.LeftTop,
            RightTop = this.RightTop,
            LeftBottom = this.LeftBottom,
            RightBottom = this.RightBottom,
        };

        public bool ContainsPoint(Vector2 point)
        {
            float x = point.X;
            float y = point.Y;

            Vector2 a = this.RightBottom;
            Vector2 b = this.LeftBottom;
            Vector2 c = this.RightTop;
            Vector2 d = this.LeftTop;

            // Contains
            switch (Comparer<float>.Default.Compare((d.X - b.X) * (y - b.Y), (d.Y - b.Y) * (x - b.X)))
            {
                case 1:
                    return (c.X - d.X) * (y - d.Y) > (c.Y - d.Y) * (x - d.X) &&
                        (a.X - c.X) * (y - c.Y) > (a.Y - c.Y) * (x - c.X) &&
                        (b.X - a.X) * (y - a.Y) > (b.Y - a.Y) * (x - a.X);
                case -1:
                    return (c.X - d.X) * (y - d.Y) < (c.Y - d.Y) * (x - d.X) &&
                        (a.X - c.X) * (y - c.Y) < (a.Y - c.Y) * (x - c.X) &&
                        (b.X - a.X) * (y - a.Y) < (b.Y - a.Y) * (x - a.X);
                default:
                    return false;
            }
        }

        public BoxContainsNodeMode2 ContainsNode(Vector2 point, float minSelectedLengthSquared = 144f, float minSideLengthSquared = 144f)
        {
            float x = point.X;
            float y = point.Y;

            // Corners
            Vector2 a = this.RightBottom;

            float ax = x - a.X;
            float ay = y - a.Y;

            float a2 = ax * ax + ay * ay;
            if (a2 < minSelectedLengthSquared)
                return BoxContainsNodeMode2.RightBottom;

            Vector2 b = this.LeftBottom;

            float bx = x - b.X;
            float by = y - b.Y;

            float b2 = bx * bx + by * by;
            if (b2 < minSelectedLengthSquared)
                return BoxContainsNodeMode2.LeftBottom;

            Vector2 c = this.RightTop;

            float cx = x - c.X;
            float cy = y - c.Y;

            float c2 = cx * cx + cy * cy;
            if (c2 < minSelectedLengthSquared)
                return BoxContainsNodeMode2.RightTop;

            Vector2 d = this.LeftTop;

            float dx = x - d.X;
            float dy = y - d.Y;

            float d2 = dx * dx + dy * dy;
            if (d2 < minSelectedLengthSquared)
                return BoxContainsNodeMode2.LeftTop;

            // Sides
            if (this.SideBottomLengthSquared > minSideLengthSquared)
            {
                Vector2 e = this.CenterBottom;

                float ex = x - e.X;
                float ey = y - e.Y;

                float e2 = ex * ex + ey * ey;
                if (e2 < minSelectedLengthSquared)
                    return BoxContainsNodeMode2.CenterBottom;
            }

            if (this.SideRightLengthSquared > minSideLengthSquared)
            {
                Vector2 f = this.CenterRight;

                float fx = x - f.X;
                float fy = y - f.Y;

                float f2 = fx * fx + fy * fy;
                if (f2 < minSelectedLengthSquared)
                    return BoxContainsNodeMode2.CenterRight;
            }

            if (this.SideTopLengthSquared > minSideLengthSquared)
            {
                Vector2 g = this.CenterTop;

                float gx = x - g.X;
                float gy = y - g.Y;

                float g2 = gx * gx + gy * gy;
                if (g2 < minSelectedLengthSquared)
                    return BoxContainsNodeMode2.CenterTop;
            }

            if (this.SideLeftLengthSquared > minSideLengthSquared)
            {
                Vector2 h = this.CenterLeft;
                float hx = x - h.X;
                float hy = y - h.Y;

                float h2 = hx * hx + hy * hy;
                if (h2 < minSelectedLengthSquared)
                    return BoxContainsNodeMode2.CenterLeft;
            }

            // Handle Sides
            Vector2 i = this.HandleBottom;

            float ix = x - i.X;
            float iy = y - i.Y;

            float i2 = ix * ix + iy * iy;
            if (i2 < minSelectedLengthSquared)
                return BoxContainsNodeMode2.HandleBottom;

            Vector2 j = this.HandleRight;

            float jx = x - j.X;
            float jy = y - j.Y;

            float j2 = jx * jx + jy * jy;
            if (j2 < minSelectedLengthSquared)
                return BoxContainsNodeMode2.HandleRight;

            Vector2 k = this.HandleTop;

            float kx = x - k.X;
            float ky = y - k.Y;

            float k2 = kx * kx + ky * ky;
            if (k2 < minSelectedLengthSquared)
                return BoxContainsNodeMode2.HandleTop;

            Vector2 l = this.HandleLeft;

            float lx = x - l.X;
            float ly = y - l.Y;

            float l2 = lx * lx + ly * ly;
            if (l2 < minSelectedLengthSquared)
                return BoxContainsNodeMode2.HandleLeft;

            // Contains
            switch (Comparer<float>.Default.Compare((d.X - b.X) * (y - b.Y), (d.Y - b.Y) * (x - b.X)))
            {
                case 1:
                    if ((c.X - d.X) * (y - d.Y) > (c.Y - d.Y) * (x - d.X) &&
                        (a.X - c.X) * (y - c.Y) > (a.Y - c.Y) * (x - c.X) &&
                        (b.X - a.X) * (y - a.Y) > (b.Y - a.Y) * (x - a.X))
                        return BoxContainsNodeMode2.Contains;
                    else
                        return BoxContainsNodeMode2.None;
                case -1:
                    if ((c.X - d.X) * (y - d.Y) < (c.Y - d.Y) * (x - d.X) &&
                        (a.X - c.X) * (y - c.Y) < (a.Y - c.Y) * (x - c.X) &&
                        (b.X - a.X) * (y - a.Y) < (b.Y - a.Y) * (x - a.X))
                        return BoxContainsNodeMode2.Contains;
                    else
                        return BoxContainsNodeMode2.None;
                default:
                    return BoxContainsNodeMode2.None;
            }
        }
        #endregion Public instance methods
    }
}