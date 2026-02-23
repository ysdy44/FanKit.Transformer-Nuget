using System.Numerics;
using CornerOrign = FanKit.Transformer.QuadrilateralPointKind;

namespace FanKit.Transformer.Controllers
{
    // 18
    internal readonly partial struct Corner
    {
        // 2
        readonly float ox, oy; // LeftTop

        // 8
        readonly float lx, ly; // Left Distance
        readonly float tx, ty; // Top Distance
        readonly float rx, ry; // Right Distance
        readonly float bx, by; // Bottom Distance

        // 8
        readonly float a0, a1; // Line 0
        readonly float b0, b1; // Line 1
        readonly float c0, c1; // Subtract
        readonly float d0, d1; // Multiply

        readonly IntersectionMode m0, m1;

        #region Triangles
        internal Corner(CornerOrign o, Triangle s)
        {
            switch (o)
            {
                case CornerOrign.LeftTop:
                    ox = s.LeftTop.X;
                    oy = s.LeftTop.Y;

                    rx = s.LeftBottom.X - s.LeftTop.X;
                    tx = s.LeftTop.X - s.RightTop.X;

                    if (System.Math.Abs(rx) > float.Epsilon)
                    {
                        ry = s.LeftBottom.Y - s.LeftTop.Y;

                        if (System.Math.Abs(tx) > float.Epsilon)
                        {
                            ty = s.LeftTop.Y - s.RightTop.Y;

                            // N3
                            a1 = ty / tx;
                            b1 = ry / rx;

                            c1 = a1 - b1;

                            if (System.Math.Abs(c1) < float.Epsilon)
                            {
                                d1 = 0f;
                                m1 = IntersectionMode.None;
                            }
                            else
                            {
                                d1 = a1 * ox;
                                m1 = IntersectionMode.N3;
                            }
                        }
                        else
                        {
                            ty = 0f;

                            // N2
                            a1 = 0f;
                            b1 = ry / rx;

                            c1 = 0f;
                            d1 = 0f;

                            m1 = IntersectionMode.N2;
                        }
                    }
                    else
                    {
                        if (System.Math.Abs(tx) > float.Epsilon)
                        {
                            ry = 0f;
                            ty = s.LeftTop.Y - s.RightTop.Y;

                            // N1
                            a1 = ty / tx;
                            b1 = 0f;

                            c1 = 0f;
                            d1 = 0f;

                            m1 = IntersectionMode.N1;
                        }
                        else
                        {
                            ry = 0f;
                            ty = 0f;

                            // N0
                            a1 = 0f;
                            b1 = 0f;

                            c1 = 0f;
                            d1 = 0f;

                            m1 = IntersectionMode.N0;
                        }
                    }

                    bx = s.LeftTop.X - s.RightTop.X;
                    lx = s.LeftTop.X - s.LeftBottom.X;

                    if (System.Math.Abs(bx) > float.Epsilon)
                    {
                        if (System.Math.Abs(lx) > float.Epsilon)
                        {
                            ly = s.LeftTop.Y - s.LeftBottom.Y;
                            by = s.LeftTop.Y - s.RightTop.Y;

                            // N3
                            a0 = ly / lx;
                            b0 = by / bx;

                            c0 = a0 - b0;
                            if (System.Math.Abs(c0) < float.Epsilon)
                            {
                                d0 = 0f;
                                m0 = IntersectionMode.None;
                            }
                            else
                            {
                                d0 = a0 * ox;

                                m0 = IntersectionMode.N3;
                            }
                        }
                        else
                        {
                            ly = 0f;
                            by = s.LeftTop.Y - s.RightTop.Y;

                            // N2
                            a0 = 0f;
                            b0 = by / bx;

                            c0 = 0f;
                            d0 = 0f;

                            m0 = IntersectionMode.N2;
                        }
                    }
                    else
                    {
                        if (System.Math.Abs(lx) > float.Epsilon)
                        {
                            by = 0f;
                            ly = s.LeftTop.Y - s.LeftBottom.Y;

                            // N1
                            a0 = ly / lx;
                            b0 = 0f;

                            c0 = 0f;
                            d0 = 0f;

                            m0 = IntersectionMode.N1;
                        }
                        else
                        {
                            by = 0f;
                            ly = 0f;

                            // N0
                            a0 = 0f;
                            b0 = 0f;

                            c0 = 0f;
                            d0 = 0f;

                            m0 = IntersectionMode.N0;
                        }
                    }
                    break;
                case CornerOrign.RightTop:
                    ox = s.RightTop.X;
                    oy = s.RightTop.Y;

                    rx = s.LeftTop.X - s.RightTop.X;
                    tx = s.LeftTop.X - s.LeftBottom.X;

                    if (System.Math.Abs(rx) > float.Epsilon)
                    {
                        ry = s.LeftBottom.Y - (s.RightTop.Y + s.LeftBottom.Y - s.LeftTop.Y);

                        if (System.Math.Abs(tx) > float.Epsilon)
                        {
                            ty = s.RightTop.Y - (s.RightTop.Y + s.LeftBottom.Y - s.LeftTop.Y);

                            // N3
                            a1 = ty / tx;
                            b1 = ry / rx;

                            c1 = a1 - b1;

                            if (System.Math.Abs(c1) < float.Epsilon)
                            {
                                d1 = 0f;
                                m1 = IntersectionMode.None;
                            }
                            else
                            {
                                d1 = a1 * ox;
                                m1 = IntersectionMode.N3;
                            }
                        }
                        else
                        {
                            ty = 0f;

                            // N2
                            a1 = 0f;
                            b1 = ry / rx;

                            c1 = 0f;
                            d1 = 0f;

                            m1 = IntersectionMode.N2;
                        }
                    }
                    else
                    {
                        if (System.Math.Abs(tx) > float.Epsilon)
                        {
                            ry = 0f;
                            ty = s.RightTop.Y - (s.RightTop.Y + s.LeftBottom.Y - s.LeftTop.Y);

                            // N1
                            a1 = ty / tx;
                            b1 = 0f;

                            c1 = 0f;
                            d1 = 0f;

                            m1 = IntersectionMode.N1;
                        }
                        else
                        {
                            ry = 0f;
                            ty = 0f;

                            // N0
                            a1 = 0f;
                            b1 = 0f;

                            c1 = 0f;
                            d1 = 0f;

                            m1 = IntersectionMode.N0;
                        }
                    }

                    bx = s.LeftTop.X - s.LeftBottom.X;
                    lx = s.RightTop.X - s.LeftTop.X;

                    if (System.Math.Abs(bx) > float.Epsilon)
                    {
                        if (System.Math.Abs(lx) > float.Epsilon)
                        {
                            ly = s.RightTop.Y - s.LeftTop.Y;
                            by = s.LeftTop.Y - s.LeftBottom.Y;

                            // N3
                            a0 = ly / lx;
                            b0 = by / bx;

                            c0 = a0 - b0;
                            if (System.Math.Abs(c0) < float.Epsilon)
                            {
                                d0 = 0f;
                                m0 = IntersectionMode.None;
                            }
                            else
                            {
                                d0 = a0 * ox;

                                m0 = IntersectionMode.N3;
                            }
                        }
                        else
                        {
                            ly = 0f;
                            by = s.LeftTop.Y - s.LeftBottom.Y;

                            // N2
                            a0 = 0f;
                            b0 = by / bx;

                            c0 = 0f;
                            d0 = 0f;

                            m0 = IntersectionMode.N2;
                        }
                    }
                    else
                    {
                        if (System.Math.Abs(lx) > float.Epsilon)
                        {
                            by = 0f;
                            ly = s.RightTop.Y - s.LeftTop.Y;

                            // N1
                            a0 = ly / lx;
                            b0 = 0f;

                            c0 = 0f;
                            d0 = 0f;

                            m0 = IntersectionMode.N1;
                        }
                        else
                        {
                            by = 0f;
                            ly = 0f;

                            // N0
                            a0 = 0f;
                            b0 = 0f;

                            c0 = 0f;
                            d0 = 0f;

                            m0 = IntersectionMode.N0;
                        }
                    }
                    break;
                case CornerOrign.RightBottom:
                    ox = (s.RightTop.X + s.LeftBottom.X - s.LeftTop.X);
                    oy = (s.RightTop.Y + s.LeftBottom.Y - s.LeftTop.Y);

                    rx = s.LeftTop.X - s.LeftBottom.X;
                    tx = (s.RightTop.X + s.LeftBottom.X - s.LeftTop.X) - s.LeftBottom.X;

                    if (System.Math.Abs(rx) > float.Epsilon)
                    {
                        ry = s.LeftTop.Y - s.LeftBottom.Y;

                        if (System.Math.Abs(tx) > float.Epsilon)
                        {
                            ty = (s.RightTop.Y + s.LeftBottom.Y - s.LeftTop.Y) - s.LeftBottom.Y;

                            // N3
                            a1 = ty / tx;
                            b1 = ry / rx;

                            c1 = a1 - b1;

                            if (System.Math.Abs(c1) < float.Epsilon)
                            {
                                d1 = 0f;
                                m1 = IntersectionMode.None;
                            }
                            else
                            {
                                d1 = a1 * ox;
                                m1 = IntersectionMode.N3;
                            }
                        }
                        else
                        {
                            ty = 0f;

                            // N2
                            a1 = 0f;
                            b1 = ry / rx;

                            c1 = 0f;
                            d1 = 0f;

                            m1 = IntersectionMode.N2;
                        }
                    }
                    else
                    {
                        if (System.Math.Abs(tx) > float.Epsilon)
                        {
                            ry = 0f;
                            ty = (s.RightTop.Y + s.LeftBottom.Y - s.LeftTop.Y) - s.LeftBottom.Y;

                            // N1
                            a1 = ty / tx;
                            b1 = 0f;

                            c1 = 0f;
                            d1 = 0f;

                            m1 = IntersectionMode.N1;
                        }
                        else
                        {
                            ry = 0f;
                            ty = 0f;

                            // N0
                            a1 = 0f;
                            b1 = 0f;

                            c1 = 0f;
                            d1 = 0f;

                            m1 = IntersectionMode.N0;
                        }
                    }

                    bx = s.RightTop.X - s.LeftTop.X;
                    lx = (s.RightTop.X + s.LeftBottom.X - s.LeftTop.X) - s.RightTop.X;

                    if (System.Math.Abs(bx) > float.Epsilon)
                    {
                        if (System.Math.Abs(lx) > float.Epsilon)
                        {
                            ly = (s.RightTop.Y + s.LeftBottom.Y - s.LeftTop.Y) - s.RightTop.Y;
                            by = s.RightTop.Y - s.LeftTop.Y;

                            // N3
                            a0 = ly / lx;
                            b0 = by / bx;

                            c0 = a0 - b0;
                            if (System.Math.Abs(c0) < float.Epsilon)
                            {
                                d0 = 0f;
                                m0 = IntersectionMode.None;
                            }
                            else
                            {
                                d0 = a0 * ox;

                                m0 = IntersectionMode.N3;
                            }
                        }
                        else
                        {
                            ly = 0f;
                            by = s.RightTop.Y - s.LeftTop.Y;

                            // N2
                            a0 = 0f;
                            b0 = by / bx;

                            c0 = 0f;
                            d0 = 0f;

                            m0 = IntersectionMode.N2;
                        }
                    }
                    else
                    {
                        if (System.Math.Abs(lx) > float.Epsilon)
                        {
                            by = 0f;
                            ly = (s.RightTop.Y + s.LeftBottom.Y - s.LeftTop.Y) - s.RightTop.Y;

                            // N1
                            a0 = ly / lx;
                            b0 = 0f;

                            c0 = 0f;
                            d0 = 0f;

                            m0 = IntersectionMode.N1;
                        }
                        else
                        {
                            by = 0f;
                            ly = 0f;

                            // N0
                            a0 = 0f;
                            b0 = 0f;

                            c0 = 0f;
                            d0 = 0f;

                            m0 = IntersectionMode.N0;
                        }
                    }
                    break;
                case CornerOrign.LeftBottom:
                    ox = s.LeftBottom.X;
                    oy = s.LeftBottom.Y;

                    rx = s.RightTop.X - s.LeftTop.X;
                    tx = s.LeftBottom.X - s.LeftTop.X;

                    if (System.Math.Abs(rx) > float.Epsilon)
                    {
                        ry = s.RightTop.Y - s.LeftTop.Y;

                        if (System.Math.Abs(tx) > float.Epsilon)
                        {
                            ty = s.LeftBottom.Y - s.LeftTop.Y;

                            // N3
                            a1 = ty / tx;
                            b1 = ry / rx;

                            c1 = a1 - b1;

                            if (System.Math.Abs(c1) < float.Epsilon)
                            {
                                d1 = 0f;
                                m1 = IntersectionMode.None;
                            }
                            else
                            {
                                d1 = a1 * ox;
                                m1 = IntersectionMode.N3;
                            }
                        }
                        else
                        {
                            ty = 0f;

                            // N2
                            a1 = 0f;
                            b1 = ry / rx;

                            c1 = 0f;
                            d1 = 0f;

                            m1 = IntersectionMode.N2;
                        }
                    }
                    else
                    {
                        if (System.Math.Abs(tx) > float.Epsilon)
                        {
                            ry = 0f;
                            ty = s.LeftBottom.Y - s.LeftTop.Y;

                            // N1
                            a1 = ty / tx;
                            b1 = 0f;

                            c1 = 0f;
                            d1 = 0f;

                            m1 = IntersectionMode.N1;
                        }
                        else
                        {
                            ry = 0f;
                            ty = 0f;

                            // N0
                            a1 = 0f;
                            b1 = 0f;

                            c1 = 0f;
                            d1 = 0f;

                            m1 = IntersectionMode.N0;
                        }
                    }

                    bx = (s.RightTop.X + s.LeftBottom.X - s.LeftTop.X) - s.RightTop.X;
                    lx = s.LeftBottom.X - (s.RightTop.X + s.LeftBottom.X - s.LeftTop.X);

                    if (System.Math.Abs(bx) > float.Epsilon)
                    {
                        if (System.Math.Abs(lx) > float.Epsilon)
                        {
                            ly = s.LeftBottom.Y - (s.RightTop.Y + s.LeftBottom.Y - s.LeftTop.Y);
                            by = (s.RightTop.Y + s.LeftBottom.Y - s.LeftTop.Y) - s.RightTop.Y;

                            // N3
                            a0 = ly / lx;
                            b0 = by / bx;

                            c0 = a0 - b0;
                            if (System.Math.Abs(c0) < float.Epsilon)
                            {
                                d0 = 0f;
                                m0 = IntersectionMode.None;
                            }
                            else
                            {
                                d0 = a0 * ox;

                                m0 = IntersectionMode.N3;
                            }
                        }
                        else
                        {
                            ly = 0f;
                            by = (s.RightTop.Y + s.LeftBottom.Y - s.LeftTop.Y) - s.RightTop.Y;

                            // N2
                            a0 = 0f;
                            b0 = by / bx;

                            c0 = 0f;
                            d0 = 0f;

                            m0 = IntersectionMode.N2;
                        }
                    }
                    else
                    {
                        if (System.Math.Abs(lx) > float.Epsilon)
                        {
                            by = 0f;
                            ly = s.LeftBottom.Y - (s.RightTop.Y + s.LeftBottom.Y - s.LeftTop.Y);

                            // N1
                            a0 = ly / lx;
                            b0 = 0f;

                            c0 = 0f;
                            d0 = 0f;

                            m0 = IntersectionMode.N1;
                        }
                        else
                        {
                            by = 0f;
                            ly = 0f;

                            // N0
                            a0 = 0f;
                            b0 = 0f;

                            c0 = 0f;
                            d0 = 0f;

                            m0 = IntersectionMode.N0;
                        }
                    }
                    break;
                default:
                    ox = 0f; oy = 0f;

                    lx = 0f; ly = 0f;
                    tx = 0f; ty = 0f;
                    rx = 0f; ry = 0f;
                    bx = 0f; by = 0f;

                    a0 = 0f; a1 = 0f;
                    b0 = 0f; b1 = 0f;
                    c0 = 0f; c1 = 0f;
                    d0 = 0f; d1 = 0f;

                    m0 = 0f; m1 = 0f;
                    break;
            }
        }

        internal Triangle To(CornerOrign o, Triangle s, Center6 c, Vector2 p, bool center)
        {
            switch (o)
            {
                case CornerOrign.LeftTop:
                    if (center)
                    {
                        Triangle t = new Triangle
                        {
                            LeftTop = new Vector2(c.x2 - p.X, c.y2 - p.Y)
                        };

                        switch (m0)
                        {
                            case IntersectionMode.None:
                                t.LeftBottom = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float h = p.Y - b0 * p.X;
                                float v = (a0 * t.LeftTop.X - t.LeftTop.Y + h) / c0;
                                t.LeftBottom = new Vector2(v,
                                    b0 * v + h);
                                break;
                            case IntersectionMode.N2:
                                t.LeftBottom = new Vector2(t.LeftTop.X,
                                    (p.X - t.LeftTop.X) * (-b0) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.LeftBottom = new Vector2(p.X,
                                    (t.LeftTop.X - p.X) * (-a0) + t.LeftTop.Y);
                                break;
                            case IntersectionMode.N0:
                                t.LeftBottom = Vector2.Zero;
                                //if (System.Math.Abs(p.X - t.LeftTop.X) < float.Epsilon)
                                //    t.LeftBottom = Vector2.Zero;
                                //else
                                //    t.LeftBottom = Vector2.Zero;
                                break;
                            default:
                                t.LeftBottom = Vector2.Zero;
                                break;
                        }

                        switch (m1)
                        {
                            case IntersectionMode.None:
                                t.RightTop = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float h = p.Y - b1 * p.X;
                                float v = (a1 * t.LeftTop.X - t.LeftTop.Y + h) / c1;
                                t.RightTop = new Vector2(v,
                                    b1 * v + h);
                                break;
                            case IntersectionMode.N2:
                                t.RightTop = new Vector2(t.LeftTop.X,
                                    (p.X - t.LeftTop.X) * (-b1) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.RightTop = new Vector2(p.X,
                                    (t.LeftTop.X - p.X) * (-a1) + t.LeftTop.Y);
                                break;
                            case IntersectionMode.N0:
                                if (System.Math.Abs(p.X - t.LeftTop.X) < float.Epsilon)
                                    t.RightTop = Vector2.Zero;
                                else
                                    t.RightTop = Vector2.Zero;
                                break;
                            default:
                                t.RightTop = Vector2.Zero;
                                break;
                        }

                        return t;
                    }
                    else
                    {
                        Triangle t = new Triangle
                        {
                            LeftTop = new Vector2(ox, oy)
                        };

                        switch (m0)
                        {
                            case IntersectionMode.None:
                                t.LeftBottom = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float v = (d0 - b0 * p.X - oy + p.Y) / c0;
                                t.LeftBottom = new Vector2(v,
                                    a0 * v - d0 + oy);
                                break;
                            case IntersectionMode.N2:
                                t.LeftBottom = new Vector2(ox,
                                    (p.X - ox) * (-b0) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.LeftBottom = new Vector2(p.X,
                                    (ox - p.X) * (-a0) + oy);
                                break;
                            case IntersectionMode.N0:
                                t.LeftBottom = Vector2.Zero;
                                //if (System.Math.Abs(ox - p.X) < float.Epsilon)
                                //    t.LeftBottom = Vector2.Zero;
                                //else
                                //    t.LeftBottom = Vector2.Zero;
                                break;
                            default:
                                t.LeftBottom = Vector2.Zero;
                                break;
                        }

                        switch (m1)
                        {
                            case IntersectionMode.None:
                                t.RightTop = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float v = (d1 - b1 * p.X - oy + p.Y) / c1;
                                t.RightTop = new Vector2(v,
                                    a1 * v - d1 + oy);
                                break;
                            case IntersectionMode.N2:
                                t.RightTop = new Vector2(ox,
                                    (p.X - ox) * (-b1) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.RightTop = new Vector2(p.X,
                                    (ox - p.X) * (-a1) + oy);
                                break;
                            case IntersectionMode.N0:
                                t.RightTop = Vector2.Zero;
                                //if (System.Math.Abs(ox - p.X) < float.Epsilon)
                                //    t.RightTop = Vector2.Zero;
                                //else
                                //    t.RightTop = Vector2.Zero;
                                break;
                            default:
                                t.RightTop = Vector2.Zero;
                                break;
                        }

                        return t;
                    }
                case CornerOrign.RightTop:
                    if (center)
                    {
                        Triangle t = new Triangle
                        {
                            LeftBottom = p,
                            RightTop = new Vector2(c.x2 - p.X, c.y2 - p.Y)
                        };

                        switch (m0)
                        {
                            case IntersectionMode.None:
                                t.LeftTop = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float h = p.Y - b0 * p.X;
                                float v = (a0 * t.RightTop.X - t.RightTop.Y + h) / c0;
                                t.LeftTop = new Vector2(v,
                                    b0 * v + h);
                                break;
                            case IntersectionMode.N2:
                                t.LeftTop = new Vector2(t.RightTop.X,
                                    (p.X - t.RightTop.X) * (-b0) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.LeftTop = new Vector2(p.X,
                                    (t.RightTop.X - p.X) * (-a0) + t.RightTop.Y);
                                break;
                            case IntersectionMode.N0:
                                t.LeftTop = Vector2.Zero;
                                //if (System.Math.Abs(p.X - t.RightTop.X) < float.Epsilon)
                                //    t.LeftTop = Vector2.Zero;
                                //else
                                //    t.LeftTop = Vector2.Zero;
                                break;
                            default:
                                t.LeftTop = Vector2.Zero;
                                break;
                        }

                        return t;
                    }
                    else
                    {
                        Triangle t = new Triangle
                        {
                            LeftBottom = p,
                            RightTop = new Vector2(ox, oy)
                        };

                        switch (m0)
                        {
                            case IntersectionMode.None:
                                t.LeftTop = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float v = (d0 - b0 * p.X - oy + p.Y) / c0;
                                t.LeftTop = new Vector2(v,
                                    a0 * v - d0 + oy);
                                break;
                            case IntersectionMode.N2:
                                t.LeftTop = new Vector2(ox,
                                    (p.X - ox) * (-b0) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.LeftTop = new Vector2(p.X,
                                    (ox - p.X) * (-a0) + oy);
                                break;
                            case IntersectionMode.N0:
                                t.LeftTop = Vector2.Zero;
                                //if (System.Math.Abs(ox - p.X) < float.Epsilon)
                                //    t.LeftTop = Vector2.Zero;
                                //else
                                //    t.LeftTop = Vector2.Zero;
                                break;
                            default:
                                t.LeftTop = Vector2.Zero;
                                break;
                        }

                        return t;
                    }
                case CornerOrign.RightBottom:
                    if (center)
                    {
                        float rbx = c.x2 - p.X;
                        float rby = c.y2 - p.Y;

                        Triangle t = new Triangle
                        {
                            LeftTop = p,
                        };

                        switch (m0)
                        {
                            case IntersectionMode.None:
                                t.RightTop = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float h = p.Y - b0 * p.X;
                                float v = (a0 * rbx - rby + h) / c0;
                                t.RightTop = new Vector2(v,
                                    b0 * v + h);
                                break;
                            case IntersectionMode.N2:
                                t.RightTop = new Vector2(rbx,
                                    c.x2 * (-b0) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.RightTop = new Vector2(p.X,
                                    (c.x2 - p.X - p.X) * (-a0) + rby);
                                break;
                            case IntersectionMode.N0:
                                t.RightTop = Vector2.Zero;
                                //if (System.Math.Absaaax < float.Epsilon)
                                //    t.RightTop = Vector2.Zero;
                                //else
                                //    t.RightTop = Vector2.Zero;
                                break;
                            default:
                                t.RightTop = Vector2.Zero;
                                break;
                        }

                        switch (m1)
                        {
                            case IntersectionMode.None:
                                t.LeftBottom = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float h = p.Y - b1 * p.X;
                                float v = (a1 * rbx - rby + h) / c1;
                                t.LeftBottom = new Vector2(v,
                                    b1 * v + h);
                                break;
                            case IntersectionMode.N2:
                                t.LeftBottom = new Vector2(rbx,
                                    c.x2 * (-b1) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.LeftBottom = new Vector2(p.X,
                                    (c.x2 - p.X - p.X) * (-a1) + rby);
                                break;
                            case IntersectionMode.N0:
                                if (System.Math.Abs(c.x2) < float.Epsilon)
                                    t.LeftBottom = Vector2.Zero;
                                else
                                    t.LeftBottom = Vector2.Zero;
                                break;
                            default:
                                t.LeftBottom = Vector2.Zero;
                                break;
                        }

                        return t;
                    }
                    else
                    {
                        Triangle t = new Triangle
                        {
                            LeftTop = p,
                        };

                        switch (m0)
                        {
                            case IntersectionMode.None:
                                t.RightTop = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float v = (d0 - b0 * p.X - oy + p.Y) / c0;
                                t.RightTop = new Vector2(v,
                                    a0 * v - d0 + oy);
                                break;
                            case IntersectionMode.N2:
                                t.RightTop = new Vector2(ox,
                                    (p.X - ox) * (-b0) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.RightTop = new Vector2(p.X,
                                    (ox - p.X) * (-a0) + oy);
                                break;
                            case IntersectionMode.N0:
                                t.RightTop = Vector2.Zero;
                                //if (System.Math.Abs(ox - p.X) < float.Epsilon)
                                //    t.RightTop = Vector2.Zero;
                                //else
                                //    t.RightTop = Vector2.Zero;
                                break;
                            default:
                                t.RightTop = Vector2.Zero;
                                break;
                        }

                        switch (m1)
                        {
                            case IntersectionMode.None:
                                t.LeftBottom = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float v = (d1 - b1 * p.X - oy + p.Y) / c1;
                                t.LeftBottom = new Vector2(v,
                                    a1 * v - d1 + oy);
                                break;
                            case IntersectionMode.N2:
                                t.LeftBottom = new Vector2(ox,
                                    (p.X - ox) * (-b1) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.LeftBottom = new Vector2(p.X,
                                    (ox - p.X) * (-a1) + oy);
                                break;
                            case IntersectionMode.N0:
                                t.LeftBottom = Vector2.Zero;
                                //if (System.Math.Abs(ox - p.X) < float.Epsilon)
                                //    t.LeftBottom = Vector2.Zero;
                                //else
                                //    t.LeftBottom = Vector2.Zero;
                                break;
                            default:
                                t.LeftBottom = Vector2.Zero;
                                break;
                        }

                        return t;
                    }
                case CornerOrign.LeftBottom:
                    if (center)
                    {
                        Triangle t = new Triangle
                        {
                            RightTop = p,
                            LeftBottom = new Vector2(c.x2 - p.X, c.y2 - p.Y)
                        };

                        switch (m1)
                        {
                            case IntersectionMode.None:
                                t.LeftTop = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float h = p.Y - b1 * p.X;
                                float v = (a1 * t.LeftBottom.X - t.LeftBottom.Y + h) / c1;
                                t.LeftTop = new Vector2(v,
                                    b1 * v + h);
                                break;
                            case IntersectionMode.N2:
                                t.LeftTop = new Vector2(t.LeftBottom.X,
                                    (p.X - t.LeftBottom.X) * (-b1) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.LeftTop = new Vector2(p.X,
                                    (t.LeftBottom.X - p.X) * (-a1) + t.LeftBottom.Y);
                                break;
                            case IntersectionMode.N0:
                                if (System.Math.Abs(p.X - t.LeftBottom.X) < float.Epsilon)
                                    t.LeftTop = Vector2.Zero;
                                else
                                    t.LeftTop = Vector2.Zero;
                                break;
                            default:
                                t.LeftTop = Vector2.Zero;
                                break;
                        }

                        return t;
                    }
                    else
                    {
                        Triangle t = new Triangle
                        {
                            RightTop = p,
                            LeftBottom = new Vector2(ox, oy)
                        };

                        switch (m1)
                        {
                            case IntersectionMode.None:
                                t.LeftTop = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float v = (d1 - b1 * p.X - oy + p.Y) / c1;
                                t.LeftTop = new Vector2(v,
                                    a1 * v - d1 + oy);
                                break;
                            case IntersectionMode.N2:
                                t.LeftTop = new Vector2(ox,
                                    (p.X - ox) * (-b1) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.LeftTop = new Vector2(p.X,
                                    (ox - p.X) * (-a1) + oy);
                                break;
                            case IntersectionMode.N0:
                                t.LeftTop = Vector2.Zero;
                                //if (System.Math.Abs(ox - p.X) < float.Epsilon)
                                //    t.LeftTop = Vector2.Zero;
                                //else
                                //    t.LeftTop = Vector2.Zero;
                                break;
                            default:
                                t.LeftTop = Vector2.Zero;
                                break;
                        }

                        return t;
                    }
                default:
                    return default;
            }
        }
        #endregion

        #region Quadrilaterals
        internal Corner(CornerOrign o, Quadrilateral s)
        {
            switch (o)
            {
                case CornerOrign.LeftTop:
                    ox = s.LeftTop.X;
                    oy = s.LeftTop.Y;

                    rx = s.RightBottom.X - s.RightTop.X;
                    tx = s.LeftTop.X - s.RightTop.X;

                    if (System.Math.Abs(rx) > float.Epsilon)
                    {
                        ry = s.RightBottom.Y - s.RightTop.Y;

                        if (System.Math.Abs(tx) > float.Epsilon)
                        {
                            ty = s.LeftTop.Y - s.RightTop.Y;

                            // N3
                            a1 = ty / tx;
                            b1 = ry / rx;

                            c1 = a1 - b1;

                            if (System.Math.Abs(c1) < float.Epsilon)
                            {
                                d1 = 0f;
                                m1 = IntersectionMode.None;
                            }
                            else
                            {
                                d1 = a1 * ox;
                                m1 = IntersectionMode.N3;
                            }
                        }
                        else
                        {
                            ty = 0f;

                            // N2
                            a1 = 0f;
                            b1 = ry / rx;

                            c1 = 0f;
                            d1 = 0f;

                            m1 = IntersectionMode.N2;
                        }
                    }
                    else
                    {
                        if (System.Math.Abs(tx) > float.Epsilon)
                        {
                            ry = 0f;
                            ty = s.LeftTop.Y - s.RightTop.Y;

                            // N1
                            a1 = ty / tx;
                            b1 = 0f;

                            c1 = 0f;
                            d1 = 0f;

                            m1 = IntersectionMode.N1;
                        }
                        else
                        {
                            ry = 0f;
                            ty = 0f;

                            // N0
                            a1 = 0f;
                            b1 = 0f;

                            c1 = 0f;
                            d1 = 0f;

                            m1 = IntersectionMode.N0;
                        }
                    }

                    bx = s.LeftBottom.X - s.RightBottom.X;
                    lx = s.LeftTop.X - s.LeftBottom.X;

                    if (System.Math.Abs(bx) > float.Epsilon)
                    {
                        if (System.Math.Abs(lx) > float.Epsilon)
                        {
                            ly = s.LeftTop.Y - s.LeftBottom.Y;
                            by = s.LeftBottom.Y - s.RightBottom.Y;

                            // N3
                            a0 = ly / lx;
                            b0 = by / bx;

                            c0 = a0 - b0;
                            if (System.Math.Abs(c0) < float.Epsilon)
                            {
                                d0 = 0f;
                                m0 = IntersectionMode.None;
                            }
                            else
                            {
                                d0 = a0 * ox;

                                m0 = IntersectionMode.N3;
                            }
                        }
                        else
                        {
                            ly = 0f;
                            by = s.LeftBottom.Y - s.RightBottom.Y;

                            // N2
                            a0 = 0f;
                            b0 = by / bx;

                            c0 = 0f;
                            d0 = 0f;

                            m0 = IntersectionMode.N2;
                        }
                    }
                    else
                    {
                        if (System.Math.Abs(lx) > float.Epsilon)
                        {
                            by = 0f;
                            ly = s.LeftTop.Y - s.LeftBottom.Y;

                            // N1
                            a0 = ly / lx;
                            b0 = 0f;

                            c0 = 0f;
                            d0 = 0f;

                            m0 = IntersectionMode.N1;
                        }
                        else
                        {
                            by = 0f;
                            ly = 0f;

                            // N0
                            a0 = 0f;
                            b0 = 0f;

                            c0 = 0f;
                            d0 = 0f;

                            m0 = IntersectionMode.N0;
                        }
                    }
                    break;
                case CornerOrign.RightTop:
                    ox = s.RightTop.X;
                    oy = s.RightTop.Y;

                    rx = s.LeftBottom.X - s.RightBottom.X;
                    tx = s.RightTop.X - s.RightBottom.X;

                    if (System.Math.Abs(rx) > float.Epsilon)
                    {
                        ry = s.LeftBottom.Y - s.RightBottom.Y;

                        if (System.Math.Abs(tx) > float.Epsilon)
                        {
                            ty = s.RightTop.Y - s.RightBottom.Y;

                            // N3
                            a1 = ty / tx;
                            b1 = ry / rx;

                            c1 = a1 - b1;

                            if (System.Math.Abs(c1) < float.Epsilon)
                            {
                                d1 = 0f;
                                m1 = IntersectionMode.None;
                            }
                            else
                            {
                                d1 = a1 * ox;
                                m1 = IntersectionMode.N3;
                            }
                        }
                        else
                        {
                            ty = 0f;

                            // N2
                            a1 = 0f;
                            b1 = ry / rx;

                            c1 = 0f;
                            d1 = 0f;

                            m1 = IntersectionMode.N2;
                        }
                    }
                    else
                    {
                        if (System.Math.Abs(tx) > float.Epsilon)
                        {
                            ry = 0f;
                            ty = s.RightTop.Y - s.RightBottom.Y;

                            // N1
                            a1 = ty / tx;
                            b1 = 0f;

                            c1 = 0f;
                            d1 = 0f;

                            m1 = IntersectionMode.N1;
                        }
                        else
                        {
                            ry = 0f;
                            ty = 0f;

                            // N0
                            a1 = 0f;
                            b1 = 0f;

                            c1 = 0f;
                            d1 = 0f;

                            m1 = IntersectionMode.N0;
                        }
                    }

                    bx = s.LeftTop.X - s.LeftBottom.X;
                    lx = s.RightTop.X - s.LeftTop.X;

                    if (System.Math.Abs(bx) > float.Epsilon)
                    {
                        if (System.Math.Abs(lx) > float.Epsilon)
                        {
                            ly = s.RightTop.Y - s.LeftTop.Y;
                            by = s.LeftTop.Y - s.LeftBottom.Y;

                            // N3
                            a0 = ly / lx;
                            b0 = by / bx;

                            c0 = a0 - b0;
                            if (System.Math.Abs(c0) < float.Epsilon)
                            {
                                d0 = 0f;
                                m0 = IntersectionMode.None;
                            }
                            else
                            {
                                d0 = a0 * ox;

                                m0 = IntersectionMode.N3;
                            }
                        }
                        else
                        {
                            ly = 0f;
                            by = s.LeftTop.Y - s.LeftBottom.Y;

                            // N2
                            a0 = 0f;
                            b0 = by / bx;

                            c0 = 0f;
                            d0 = 0f;

                            m0 = IntersectionMode.N2;
                        }
                    }
                    else
                    {
                        if (System.Math.Abs(lx) > float.Epsilon)
                        {
                            by = 0f;
                            ly = s.RightTop.Y - s.LeftTop.Y;

                            // N1
                            a0 = ly / lx;
                            b0 = 0f;

                            c0 = 0f;
                            d0 = 0f;

                            m0 = IntersectionMode.N1;
                        }
                        else
                        {
                            by = 0f;
                            ly = 0f;

                            // N0
                            a0 = 0f;
                            b0 = 0f;

                            c0 = 0f;
                            d0 = 0f;

                            m0 = IntersectionMode.N0;
                        }
                    }
                    break;
                case CornerOrign.RightBottom:
                    ox = s.RightBottom.X;
                    oy = s.RightBottom.Y;

                    rx = s.LeftTop.X - s.LeftBottom.X;
                    tx = s.RightBottom.X - s.LeftBottom.X;

                    if (System.Math.Abs(rx) > float.Epsilon)
                    {
                        ry = s.LeftTop.Y - s.LeftBottom.Y;

                        if (System.Math.Abs(tx) > float.Epsilon)
                        {
                            ty = s.RightBottom.Y - s.LeftBottom.Y;

                            // N3
                            a1 = ty / tx;
                            b1 = ry / rx;

                            c1 = a1 - b1;

                            if (System.Math.Abs(c1) < float.Epsilon)
                            {
                                d1 = 0f;
                                m1 = IntersectionMode.None;
                            }
                            else
                            {
                                d1 = a1 * ox;
                                m1 = IntersectionMode.N3;
                            }
                        }
                        else
                        {
                            ty = 0f;

                            // N2
                            a1 = 0f;
                            b1 = ry / rx;

                            c1 = 0f;
                            d1 = 0f;

                            m1 = IntersectionMode.N2;
                        }
                    }
                    else
                    {
                        if (System.Math.Abs(tx) > float.Epsilon)
                        {
                            ry = 0f;
                            ty = s.RightBottom.Y - s.LeftBottom.Y;

                            // N1
                            a1 = ty / tx;
                            b1 = 0f;

                            c1 = 0f;
                            d1 = 0f;

                            m1 = IntersectionMode.N1;
                        }
                        else
                        {
                            ry = 0f;
                            ty = 0f;

                            // N0
                            a1 = 0f;
                            b1 = 0f;

                            c1 = 0f;
                            d1 = 0f;

                            m1 = IntersectionMode.N0;
                        }
                    }

                    bx = s.RightTop.X - s.LeftTop.X;
                    lx = s.RightBottom.X - s.RightTop.X;

                    if (System.Math.Abs(bx) > float.Epsilon)
                    {
                        if (System.Math.Abs(lx) > float.Epsilon)
                        {
                            ly = s.RightBottom.Y - s.RightTop.Y;
                            by = s.RightTop.Y - s.LeftTop.Y;

                            // N3
                            a0 = ly / lx;
                            b0 = by / bx;

                            c0 = a0 - b0;
                            if (System.Math.Abs(c0) < float.Epsilon)
                            {
                                d0 = 0f;
                                m0 = IntersectionMode.None;
                            }
                            else
                            {
                                d0 = a0 * ox;

                                m0 = IntersectionMode.N3;
                            }
                        }
                        else
                        {
                            ly = 0f;
                            by = s.RightTop.Y - s.LeftTop.Y;

                            // N2
                            a0 = 0f;
                            b0 = by / bx;

                            c0 = 0f;
                            d0 = 0f;

                            m0 = IntersectionMode.N2;
                        }
                    }
                    else
                    {
                        if (System.Math.Abs(lx) > float.Epsilon)
                        {
                            by = 0f;
                            ly = s.RightBottom.Y - s.RightTop.Y;

                            // N1
                            a0 = ly / lx;
                            b0 = 0f;

                            c0 = 0f;
                            d0 = 0f;

                            m0 = IntersectionMode.N1;
                        }
                        else
                        {
                            by = 0f;
                            ly = 0f;

                            // N0
                            a0 = 0f;
                            b0 = 0f;

                            c0 = 0f;
                            d0 = 0f;

                            m0 = IntersectionMode.N0;
                        }
                    }
                    break;
                case CornerOrign.LeftBottom:
                    ox = s.LeftBottom.X;
                    oy = s.LeftBottom.Y;

                    rx = s.RightTop.X - s.LeftTop.X;
                    tx = s.LeftBottom.X - s.LeftTop.X;

                    if (System.Math.Abs(rx) > float.Epsilon)
                    {
                        ry = s.RightTop.Y - s.LeftTop.Y;

                        if (System.Math.Abs(tx) > float.Epsilon)
                        {
                            ty = s.LeftBottom.Y - s.LeftTop.Y;

                            // N3
                            a1 = ty / tx;
                            b1 = ry / rx;

                            c1 = a1 - b1;

                            if (System.Math.Abs(c1) < float.Epsilon)
                            {
                                d1 = 0f;
                                m1 = IntersectionMode.None;
                            }
                            else
                            {
                                d1 = a1 * ox;
                                m1 = IntersectionMode.N3;
                            }
                        }
                        else
                        {
                            ty = 0f;

                            // N2
                            a1 = 0f;
                            b1 = ry / rx;

                            c1 = 0f;
                            d1 = 0f;

                            m1 = IntersectionMode.N2;
                        }
                    }
                    else
                    {
                        if (System.Math.Abs(tx) > float.Epsilon)
                        {
                            ry = 0f;
                            ty = s.LeftBottom.Y - s.LeftTop.Y;

                            // N1
                            a1 = ty / tx;
                            b1 = 0f;

                            c1 = 0f;
                            d1 = 0f;

                            m1 = IntersectionMode.N1;
                        }
                        else
                        {
                            ry = 0f;
                            ty = 0f;

                            // N0
                            a1 = 0f;
                            b1 = 0f;

                            c1 = 0f;
                            d1 = 0f;

                            m1 = IntersectionMode.N0;
                        }
                    }

                    bx = s.RightBottom.X - s.RightTop.X;
                    lx = s.LeftBottom.X - s.RightBottom.X;

                    if (System.Math.Abs(bx) > float.Epsilon)
                    {
                        if (System.Math.Abs(lx) > float.Epsilon)
                        {
                            ly = s.LeftBottom.Y - s.RightBottom.Y;
                            by = s.RightBottom.Y - s.RightTop.Y;

                            // N3
                            a0 = ly / lx;
                            b0 = by / bx;

                            c0 = a0 - b0;
                            if (System.Math.Abs(c0) < float.Epsilon)
                            {
                                d0 = 0f;
                                m0 = IntersectionMode.None;
                            }
                            else
                            {
                                d0 = a0 * ox;

                                m0 = IntersectionMode.N3;
                            }
                        }
                        else
                        {
                            ly = 0f;
                            by = s.RightBottom.Y - s.RightTop.Y;

                            // N2
                            a0 = 0f;
                            b0 = by / bx;

                            c0 = 0f;
                            d0 = 0f;

                            m0 = IntersectionMode.N2;
                        }
                    }
                    else
                    {
                        if (System.Math.Abs(lx) > float.Epsilon)
                        {
                            by = 0f;
                            ly = s.LeftBottom.Y - s.RightBottom.Y;

                            // N1
                            a0 = ly / lx;
                            b0 = 0f;

                            c0 = 0f;
                            d0 = 0f;

                            m0 = IntersectionMode.N1;
                        }
                        else
                        {
                            by = 0f;
                            ly = 0f;

                            // N0
                            a0 = 0f;
                            b0 = 0f;

                            c0 = 0f;
                            d0 = 0f;

                            m0 = IntersectionMode.N0;
                        }
                    }
                    break;
                default:
                    ox = 0f; oy = 0f;

                    lx = 0f; ly = 0f;
                    tx = 0f; ty = 0f;
                    rx = 0f; ry = 0f;
                    bx = 0f; by = 0f;

                    a0 = 0f; a1 = 0f;
                    b0 = 0f; b1 = 0f;
                    c0 = 0f; c1 = 0f;
                    d0 = 0f; d1 = 0f;

                    m0 = 0f; m1 = 0f;
                    break;
            }
        }

        internal Quadrilateral To(CornerOrign o, Quadrilateral s, Center6 c, Vector2 p, bool center)
        {
            switch (o)
            {
                case CornerOrign.LeftTop:
                    if (center)
                    {
                        Quadrilateral t = new Quadrilateral
                        {
                            RightBottom = p,
                            LeftTop = new Vector2(c.x2 - p.X, c.y2 - p.Y)
                        };

                        switch (m0)
                        {
                            case IntersectionMode.None:
                                t.LeftBottom = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float h = p.Y - b0 * p.X;
                                float v = (a0 * t.LeftTop.X - t.LeftTop.Y + h) / c0;
                                t.LeftBottom = new Vector2(v,
                                    b0 * v + h);
                                break;
                            case IntersectionMode.N2:
                                t.LeftBottom = new Vector2(t.LeftTop.X,
                                    (p.X - t.LeftTop.X) * (-b0) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.LeftBottom = new Vector2(p.X,
                                    (t.LeftTop.X - p.X) * (-a0) + t.LeftTop.Y);
                                break;
                            case IntersectionMode.N0:
                                t.LeftBottom = Vector2.Zero;
                                //if (System.Math.Abs(p.X - t.LeftTop.X) < float.Epsilon)
                                //    t.LeftBottom = Vector2.Zero;
                                //else
                                //    t.LeftBottom = Vector2.Zero;
                                break;
                            default:
                                t.LeftBottom = Vector2.Zero;
                                break;
                        }

                        switch (m1)
                        {
                            case IntersectionMode.None:
                                t.RightTop = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float h = p.Y - b1 * p.X;
                                float v = (a1 * t.LeftTop.X - t.LeftTop.Y + h) / c1;
                                t.RightTop = new Vector2(v,
                                    b1 * v + h);
                                break;
                            case IntersectionMode.N2:
                                t.RightTop = new Vector2(t.LeftTop.X,
                                    (p.X - t.LeftTop.X) * (-b1) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.RightTop = new Vector2(p.X,
                                    (t.LeftTop.X - p.X) * (-a1) + t.LeftTop.Y);
                                break;
                            case IntersectionMode.N0:
                                if (System.Math.Abs(p.X - t.LeftTop.X) < float.Epsilon)
                                    t.RightTop = Vector2.Zero;
                                else
                                    t.RightTop = Vector2.Zero;
                                break;
                            default:
                                t.RightTop = Vector2.Zero;
                                break;
                        }

                        return t;
                    }
                    else
                    {
                        Quadrilateral t = new Quadrilateral
                        {
                            RightBottom = p,
                            LeftTop = new Vector2(ox, oy)
                        };

                        switch (m0)
                        {
                            case IntersectionMode.None:
                                t.LeftBottom = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float v = (d0 - b0 * p.X - oy + p.Y) / c0;
                                t.LeftBottom = new Vector2(v,
                                    a0 * v - d0 + oy);
                                break;
                            case IntersectionMode.N2:
                                t.LeftBottom = new Vector2(ox,
                                    (p.X - ox) * (-b0) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.LeftBottom = new Vector2(p.X,
                                    (ox - p.X) * (-a0) + oy);
                                break;
                            case IntersectionMode.N0:
                                t.LeftBottom = Vector2.Zero;
                                //if (System.Math.Abs(ox - p.X) < float.Epsilon)
                                //    t.LeftBottom = Vector2.Zero;
                                //else
                                //    t.LeftBottom = Vector2.Zero;
                                break;
                            default:
                                t.LeftBottom = Vector2.Zero;
                                break;
                        }

                        switch (m1)
                        {
                            case IntersectionMode.None:
                                t.RightTop = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float v = (d1 - b1 * p.X - oy + p.Y) / c1;
                                t.RightTop = new Vector2(v,
                                    a1 * v - d1 + oy);
                                break;
                            case IntersectionMode.N2:
                                t.RightTop = new Vector2(ox,
                                    (p.X - ox) * (-b1) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.RightTop = new Vector2(p.X,
                                    (ox - p.X) * (-a1) + oy);
                                break;
                            case IntersectionMode.N0:
                                t.RightTop = Vector2.Zero;
                                //if (System.Math.Abs(ox - p.X) < float.Epsilon)
                                //    t.RightTop = Vector2.Zero;
                                //else
                                //    t.RightTop = Vector2.Zero;
                                break;
                            default:
                                t.RightTop = Vector2.Zero;
                                break;
                        }

                        return t;
                    }
                case CornerOrign.RightTop:
                    if (center)
                    {
                        Quadrilateral t = new Quadrilateral
                        {
                            LeftBottom = p,
                            RightTop = new Vector2(c.x2 - p.X, c.y2 - p.Y)
                        };

                        switch (m0)
                        {
                            case IntersectionMode.None:
                                t.LeftTop = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float h = p.Y - b0 * p.X;
                                float v = (a0 * t.RightTop.X - t.RightTop.Y + h) / c0;
                                t.LeftTop = new Vector2(v,
                                    b0 * v + h);
                                break;
                            case IntersectionMode.N2:
                                t.LeftTop = new Vector2(t.RightTop.X,
                                    (p.X - t.RightTop.X) * (-b0) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.LeftTop = new Vector2(p.X,
                                    (t.RightTop.X - p.X) * (-a0) + t.RightTop.Y);
                                break;
                            case IntersectionMode.N0:
                                t.LeftTop = Vector2.Zero;
                                //if (System.Math.Abs(p.X - t.RightTop.X) < float.Epsilon)
                                //    t.LeftTop = Vector2.Zero;
                                //else
                                //    t.LeftTop = Vector2.Zero;
                                break;
                            default:
                                t.LeftTop = Vector2.Zero;
                                break;
                        }

                        switch (m1)
                        {
                            case IntersectionMode.None:
                                t.RightBottom = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float h = p.Y - b1 * p.X;
                                float v = (a1 * t.RightTop.X - t.RightTop.Y + h) / c1;
                                t.RightBottom = new Vector2(v,
                                    b1 * v + h);
                                break;
                            case IntersectionMode.N2:
                                t.RightBottom = new Vector2(t.RightTop.X,
                                    (p.X - t.RightTop.X) * (-b1) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.RightBottom = new Vector2(p.X,
                                    (t.RightTop.X - p.X) * (-a1) + t.RightTop.Y);
                                break;
                            case IntersectionMode.N0:
                                if (System.Math.Abs(p.X - t.RightTop.X) < float.Epsilon)
                                    t.RightBottom = Vector2.Zero;
                                else
                                    t.RightBottom = Vector2.Zero;
                                break;
                            default:
                                t.RightBottom = Vector2.Zero;
                                break;
                        }

                        return t;
                    }
                    else
                    {
                        Quadrilateral t = new Quadrilateral
                        {
                            LeftBottom = p,
                            RightTop = new Vector2(ox, oy)
                        };

                        switch (m0)
                        {
                            case IntersectionMode.None:
                                t.LeftTop = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float v = (d0 - b0 * p.X - oy + p.Y) / c0;
                                t.LeftTop = new Vector2(v,
                                    a0 * v - d0 + oy);
                                break;
                            case IntersectionMode.N2:
                                t.LeftTop = new Vector2(ox,
                                    (p.X - ox) * (-b0) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.LeftTop = new Vector2(p.X,
                                    (ox - p.X) * (-a0) + oy);
                                break;
                            case IntersectionMode.N0:
                                t.LeftTop = Vector2.Zero;
                                //if (System.Math.Abs(ox - p.X) < float.Epsilon)
                                //    t.LeftTop = Vector2.Zero;
                                //else
                                //    t.LeftTop = Vector2.Zero;
                                break;
                            default:
                                t.LeftTop = Vector2.Zero;
                                break;
                        }

                        switch (m1)
                        {
                            case IntersectionMode.None:
                                t.RightBottom = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float v = (d1 - b1 * p.X - oy + p.Y) / c1;
                                t.RightBottom = new Vector2(v,
                                    a1 * v - d1 + oy);
                                break;
                            case IntersectionMode.N2:
                                t.RightBottom = new Vector2(ox,
                                    (p.X - ox) * (-b1) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.RightBottom = new Vector2(p.X,
                                    (ox - p.X) * (-a1) + oy);
                                break;
                            case IntersectionMode.N0:
                                t.RightBottom = Vector2.Zero;
                                //if (System.Math.Abs(ox - p.X) < float.Epsilon)
                                //    t.RightBottom = Vector2.Zero;
                                //else
                                //    t.RightBottom = Vector2.Zero;
                                break;
                            default:
                                t.RightBottom = Vector2.Zero;
                                break;
                        }

                        return t;
                    }
                case CornerOrign.RightBottom:
                    if (center)
                    {
                        Quadrilateral t = new Quadrilateral
                        {
                            LeftTop = p,
                            RightBottom = new Vector2(c.x2 - p.X, c.y2 - p.Y)
                        };

                        switch (m0)
                        {
                            case IntersectionMode.None:
                                t.RightTop = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float h = p.Y - b0 * p.X;
                                float v = (a0 * t.RightBottom.X - t.RightBottom.Y + h) / c0;
                                t.RightTop = new Vector2(v,
                                    b0 * v + h);
                                break;
                            case IntersectionMode.N2:
                                t.RightTop = new Vector2(t.RightBottom.X,
                                    (p.X - t.RightBottom.X) * (-b0) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.RightTop = new Vector2(p.X,
                                    (t.RightBottom.X - p.X) * (-a0) + t.RightBottom.Y);
                                break;
                            case IntersectionMode.N0:
                                t.RightTop = Vector2.Zero;
                                //if (System.Math.Abs(p.X - t.RightBottom.X) < float.Epsilon)
                                //    t.RightTop = Vector2.Zero;
                                //else
                                //    t.RightTop = Vector2.Zero;
                                break;
                            default:
                                t.RightTop = Vector2.Zero;
                                break;
                        }

                        switch (m1)
                        {
                            case IntersectionMode.None:
                                t.LeftBottom = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float h = p.Y - b1 * p.X;
                                float v = (a1 * t.RightBottom.X - t.RightBottom.Y + h) / c1;
                                t.LeftBottom = new Vector2(v,
                                    b1 * v + h);
                                break;
                            case IntersectionMode.N2:
                                t.LeftBottom = new Vector2(t.RightBottom.X,
                                    (p.X - t.RightBottom.X) * (-b1) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.LeftBottom = new Vector2(p.X,
                                    (t.RightBottom.X - p.X) * (-a1) + t.RightBottom.Y);
                                break;
                            case IntersectionMode.N0:
                                if (System.Math.Abs(p.X - t.RightBottom.X) < float.Epsilon)
                                    t.LeftBottom = Vector2.Zero;
                                else
                                    t.LeftBottom = Vector2.Zero;
                                break;
                            default:
                                t.LeftBottom = Vector2.Zero;
                                break;
                        }

                        return t;
                    }
                    else
                    {
                        Quadrilateral t = new Quadrilateral
                        {
                            LeftTop = p,
                            RightBottom = new Vector2(ox, oy)
                        };

                        switch (m0)
                        {
                            case IntersectionMode.None:
                                t.RightTop = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float v = (d0 - b0 * p.X - oy + p.Y) / c0;
                                t.RightTop = new Vector2(v,
                                    a0 * v - d0 + oy);
                                break;
                            case IntersectionMode.N2:
                                t.RightTop = new Vector2(ox,
                                    (p.X - ox) * (-b0) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.RightTop = new Vector2(p.X,
                                    (ox - p.X) * (-a0) + oy);
                                break;
                            case IntersectionMode.N0:
                                t.RightTop = Vector2.Zero;
                                //if (System.Math.Abs(ox - p.X) < float.Epsilon)
                                //    t.RightTop = Vector2.Zero;
                                //else
                                //    t.RightTop = Vector2.Zero;
                                break;
                            default:
                                t.RightTop = Vector2.Zero;
                                break;
                        }

                        switch (m1)
                        {
                            case IntersectionMode.None:
                                t.LeftBottom = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float v = (d1 - b1 * p.X - oy + p.Y) / c1;
                                t.LeftBottom = new Vector2(v,
                                    a1 * v - d1 + oy);
                                break;
                            case IntersectionMode.N2:
                                t.LeftBottom = new Vector2(ox,
                                    (p.X - ox) * (-b1) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.LeftBottom = new Vector2(p.X,
                                    (ox - p.X) * (-a1) + oy);
                                break;
                            case IntersectionMode.N0:
                                t.LeftBottom = Vector2.Zero;
                                //if (System.Math.Abs(ox - p.X) < float.Epsilon)
                                //    t.LeftBottom = Vector2.Zero;
                                //else
                                //    t.LeftBottom = Vector2.Zero;
                                break;
                            default:
                                t.LeftBottom = Vector2.Zero;
                                break;
                        }

                        return t;
                    }
                case CornerOrign.LeftBottom:
                    if (center)
                    {
                        Quadrilateral t = new Quadrilateral
                        {
                            RightTop = p,
                            LeftBottom = new Vector2(c.x2 - p.X, c.y2 - p.Y)
                        };

                        switch (m0)
                        {
                            case IntersectionMode.None:
                                t.RightBottom = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float h = p.Y - b0 * p.X;
                                float v = (a0 * t.LeftBottom.X - t.LeftBottom.Y + h) / c0;
                                t.RightBottom = new Vector2(v,
                                    b0 * v + h);
                                break;
                            case IntersectionMode.N2:
                                t.RightBottom = new Vector2(t.LeftBottom.X,
                                    (p.X - t.LeftBottom.X) * (-b0) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.RightBottom = new Vector2(p.X,
                                    (t.LeftBottom.X - p.X) * (-a0) + t.LeftBottom.Y);
                                break;
                            case IntersectionMode.N0:
                                t.RightBottom = Vector2.Zero;
                                //if (System.Math.Abs(p.X - t.LeftBottom.X) < float.Epsilon)
                                //    t.RightBottom = Vector2.Zero;
                                //else
                                //    t.RightBottom = Vector2.Zero;
                                break;
                            default:
                                t.RightBottom = Vector2.Zero;
                                break;
                        }

                        switch (m1)
                        {
                            case IntersectionMode.None:
                                t.LeftTop = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float h = p.Y - b1 * p.X;
                                float v = (a1 * t.LeftBottom.X - t.LeftBottom.Y + h) / c1;
                                t.LeftTop = new Vector2(v,
                                    b1 * v + h);
                                break;
                            case IntersectionMode.N2:
                                t.LeftTop = new Vector2(t.LeftBottom.X,
                                    (p.X - t.LeftBottom.X) * (-b1) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.LeftTop = new Vector2(p.X,
                                    (t.LeftBottom.X - p.X) * (-a1) + t.LeftBottom.Y);
                                break;
                            case IntersectionMode.N0:
                                if (System.Math.Abs(p.X - t.LeftBottom.X) < float.Epsilon)
                                    t.LeftTop = Vector2.Zero;
                                else
                                    t.LeftTop = Vector2.Zero;
                                break;
                            default:
                                t.LeftTop = Vector2.Zero;
                                break;
                        }

                        return t;
                    }
                    else
                    {
                        Quadrilateral t = new Quadrilateral
                        {
                            RightTop = p,
                            LeftBottom = new Vector2(ox, oy)
                        };

                        switch (m0)
                        {
                            case IntersectionMode.None:
                                t.RightBottom = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float v = (d0 - b0 * p.X - oy + p.Y) / c0;
                                t.RightBottom = new Vector2(v,
                                    a0 * v - d0 + oy);
                                break;
                            case IntersectionMode.N2:
                                t.RightBottom = new Vector2(ox,
                                    (p.X - ox) * (-b0) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.RightBottom = new Vector2(p.X,
                                    (ox - p.X) * (-a0) + oy);
                                break;
                            case IntersectionMode.N0:
                                t.RightBottom = Vector2.Zero;
                                //if (System.Math.Abs(ox - p.X) < float.Epsilon)
                                //    t.RightBottom = Vector2.Zero;
                                //else
                                //    t.RightBottom = Vector2.Zero;
                                break;
                            default:
                                t.RightBottom = Vector2.Zero;
                                break;
                        }

                        switch (m1)
                        {
                            case IntersectionMode.None:
                                t.LeftTop = Vector2.Zero;
                                break;
                            case IntersectionMode.N3:
                                float v = (d1 - b1 * p.X - oy + p.Y) / c1;
                                t.LeftTop = new Vector2(v,
                                    a1 * v - d1 + oy);
                                break;
                            case IntersectionMode.N2:
                                t.LeftTop = new Vector2(ox,
                                    (p.X - ox) * (-b1) + p.Y);
                                break;
                            case IntersectionMode.N1:
                                t.LeftTop = new Vector2(p.X,
                                    (ox - p.X) * (-a1) + oy);
                                break;
                            case IntersectionMode.N0:
                                t.LeftTop = Vector2.Zero;
                                //if (System.Math.Abs(ox - p.X) < float.Epsilon)
                                //    t.LeftTop = Vector2.Zero;
                                //else
                                //    t.LeftTop = Vector2.Zero;
                                break;
                            default:
                                t.LeftTop = Vector2.Zero;
                                break;
                        }

                        return t;
                    }
                default:
                    return default;
            }
        }
        #endregion
    }
}