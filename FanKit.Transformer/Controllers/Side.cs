using System.Numerics;

namespace FanKit.Transformer.Controllers
{
    // 14
    internal readonly partial struct Side
    {
        // 8
        readonly float rx, ry; // Right
        readonly float lx, ly; // Left
        readonly Distance4 od;

        // 6
        readonly float tx, ty; // - RightTop - RightTop
        readonly float bx, by; // - RightBottom - RightBottom
        readonly float sx, sy; // Left Vertical

        #region Triangles
        internal Side(SideOrign o, Triangle s, Center6 c)
        {
            switch (o)
            {
                case SideOrign.Left:
                    rx = s.RightTop.X + s.RightTop.X + s.LeftBottom.X - s.LeftTop.X;
                    ry = s.RightTop.Y + s.RightTop.Y + s.LeftBottom.Y - s.LeftTop.Y;

                    lx = s.LeftTop.X + s.LeftBottom.X;
                    ly = s.LeftTop.Y + s.LeftBottom.Y;

                    od = new Distance4(rx, ry, lx, ly);

                    tx = lx - s.RightTop.X - s.RightTop.X;
                    ty = ly - s.RightTop.Y - s.RightTop.Y;

                    bx = lx - (s.RightTop.X + s.LeftBottom.X - s.LeftTop.X) * 2f;
                    by = ly - (s.RightTop.Y + s.LeftBottom.Y - s.LeftTop.Y) * 2f;

                    sx = s.LeftTop.X - s.LeftBottom.X;
                    sy = s.LeftTop.Y - s.LeftBottom.Y;
                    break;
                case SideOrign.Top:
                    rx = s.LeftBottom.X + s.LeftBottom.X + s.RightTop.X - s.LeftTop.X;
                    ry = s.LeftBottom.Y + s.LeftBottom.Y + s.RightTop.Y - s.LeftTop.Y;

                    lx = s.RightTop.X + s.LeftTop.X;
                    ly = s.RightTop.Y + s.LeftTop.Y;

                    od = new Distance4(rx, ry, lx, ly);

                    tx = lx - (s.RightTop.X + s.LeftBottom.X - s.LeftTop.X) * 2f;
                    ty = ly - (s.RightTop.Y + s.LeftBottom.Y - s.LeftTop.Y) * 2f;

                    bx = lx - s.LeftBottom.X - s.LeftBottom.X;
                    by = ly - s.LeftBottom.Y - s.LeftBottom.Y;

                    sx = s.RightTop.X - s.LeftTop.X;
                    sy = s.RightTop.Y - s.LeftTop.Y;
                    break;
                case SideOrign.Right:
                    rx = s.LeftTop.X + s.LeftBottom.X;
                    ry = s.LeftTop.Y + s.LeftBottom.Y;

                    lx = s.RightTop.X + s.RightTop.X + s.LeftBottom.X - s.LeftTop.X;
                    ly = s.RightTop.Y + s.RightTop.Y + s.LeftBottom.Y - s.LeftTop.Y;

                    od = new Distance4(rx, ry, lx, ly);

                    tx = lx - s.LeftBottom.X - s.LeftBottom.X;
                    ty = ly - s.LeftBottom.Y - s.LeftBottom.Y;

                    bx = lx - s.LeftTop.X - s.LeftTop.X;
                    by = ly - s.LeftTop.Y - s.LeftTop.Y;

                    sx = s.LeftBottom.X - s.LeftTop.X;
                    sy = s.LeftBottom.Y - s.LeftTop.Y;
                    break;
                case SideOrign.Bottom:
                    rx = s.RightTop.X + s.LeftTop.X;
                    ry = s.RightTop.Y + s.LeftTop.Y;

                    lx = s.LeftBottom.X + s.LeftBottom.X + s.RightTop.X - s.LeftTop.X;
                    ly = s.LeftBottom.Y + s.LeftBottom.Y + s.RightTop.Y - s.LeftTop.Y;

                    od = new Distance4(rx, ry, lx, ly);

                    tx = lx - s.LeftTop.X - s.LeftTop.X;
                    ty = ly - s.LeftTop.Y - s.LeftTop.Y;

                    bx = lx - s.RightTop.X - s.RightTop.X;
                    by = ly - s.RightTop.Y - s.RightTop.Y;

                    sx = s.LeftTop.X - s.RightTop.X;
                    sy = s.LeftTop.Y - s.RightTop.Y;
                    break;
                default:
                    rx = 0f; ry = 0f;
                    lx = 0f; ly = 0f;
                    od = default;
                    tx = 0f; ty = 0f;
                    bx = 0f; by = 0f;
                    sx = 0f; sy = 0f;
                    break;
            }
        }

        internal Triangle To(SideOrign o, Triangle s, Trans8 ct, Vector2 p, bool ratio, bool center)
        {
            if (ratio)
            {
                if (center)
                {
                    float s4 = od.S4(p);

                    return new Triangle
                    {
                        LeftTop = new Vector2(s.LeftTop.X + ct.LeftTopX * s4,
                        s.LeftTop.Y + ct.LeftTopY * s4),

                        RightTop = new Vector2(s.RightTop.X + ct.RightTopX * s4,
                        s.RightTop.Y + ct.RightTopY * s4),

                        LeftBottom = new Vector2(s.LeftBottom.X + ct.LeftBottomX * s4,
                        s.LeftBottom.Y + ct.LeftBottomY * s4),
                    };
                }
                else
                {
                    float sds = od.S(p);
                    float x = sx * sds;
                    float y = sy * sds;

                    switch (o)
                    {
                        case SideOrign.Left:
                            return new Triangle
                            {
                                LeftTop = new Vector2(s.LeftTop.X - x, s.LeftTop.Y - y),
                                LeftBottom = new Vector2(s.LeftBottom.X + x, s.LeftBottom.Y + y),

                                RightTop = new Vector2(s.RightTop.X + tx * sds, s.RightTop.Y + ty * sds),
                            };
                        case SideOrign.Top:
                            return new Triangle
                            {
                                RightTop = new Vector2(s.RightTop.X - x, s.RightTop.Y - y),
                                LeftTop = new Vector2(s.LeftTop.X + x, s.LeftTop.Y + y),

                                LeftBottom = new Vector2(s.LeftBottom.X + bx * sds, s.LeftBottom.Y + by * sds),
                            };
                        case SideOrign.Right:
                            return new Triangle
                            {
                                RightTop = new Vector2(s.RightTop.X + x, s.RightTop.Y + y),

                                LeftBottom = new Vector2(s.LeftBottom.X + tx * sds, s.LeftBottom.Y + ty * sds),
                                LeftTop = new Vector2(s.LeftTop.X + bx * sds, s.LeftTop.Y + by * sds),
                            };
                        case SideOrign.Bottom:
                            return new Triangle
                            {
                                LeftBottom = new Vector2(s.LeftBottom.X - x, s.LeftBottom.Y - y),

                                LeftTop = new Vector2(s.LeftTop.X + tx * sds, s.LeftTop.Y + ty * sds),
                                RightTop = new Vector2(s.RightTop.X + bx * sds, s.RightTop.Y + by * sds),
                            };
                        default:
                            return new Triangle
                            {
                                LeftTop = new Vector2(s.LeftTop.X, s.LeftTop.Y),
                                RightTop = new Vector2(s.RightTop.X, s.RightTop.Y),
                                LeftBottom = new Vector2(s.LeftBottom.X, s.LeftBottom.Y),
                            };
                    }
                }
            }
            else
            {
                float sds = od.S(p);
                float vx = od.hx * sds;
                float vy = od.hy * sds;

                switch (o)
                {
                    case SideOrign.Left:
                        if (center)
                            return new Triangle
                            {
                                LeftTop = new Vector2(s.LeftTop.X + vx, s.LeftTop.Y + vy),
                                LeftBottom = new Vector2(s.LeftBottom.X + vx, s.LeftBottom.Y + vy),

                                RightTop = new Vector2(s.RightTop.X - vx, s.RightTop.Y - vy),
                            };
                        else
                            return new Triangle
                            {
                                LeftTop = new Vector2(s.LeftTop.X, s.LeftTop.Y),
                                LeftBottom = new Vector2(s.LeftBottom.X, s.LeftBottom.Y),

                                RightTop = new Vector2(s.RightTop.X - vx, s.RightTop.Y - vy),
                            };
                    case SideOrign.Top:
                        if (center)
                            return new Triangle
                            {
                                RightTop = new Vector2(s.RightTop.X + vx, s.RightTop.Y + vy),
                                LeftTop = new Vector2(s.LeftTop.X + vx, s.LeftTop.Y + vy),

                                LeftBottom = new Vector2(s.LeftBottom.X - vx, s.LeftBottom.Y - vy),
                            };
                        else
                            return new Triangle
                            {
                                RightTop = new Vector2(s.RightTop.X, s.RightTop.Y),
                                LeftTop = new Vector2(s.LeftTop.X, s.LeftTop.Y),

                                LeftBottom = new Vector2(s.LeftBottom.X - vx, s.LeftBottom.Y - vy),
                            };
                    case SideOrign.Right:
                        if (center)
                            return new Triangle
                            {
                                RightTop = new Vector2(s.RightTop.X + vx, s.RightTop.Y + vy),

                                LeftTop = new Vector2(s.LeftTop.X - vx, s.LeftTop.Y - vy),
                                LeftBottom = new Vector2(s.LeftBottom.X - vx, s.LeftBottom.Y - vy),
                            };
                        else
                            return new Triangle
                            {
                                RightTop = new Vector2(s.RightTop.X, s.RightTop.Y),

                                LeftTop = new Vector2(s.LeftTop.X - vx, s.LeftTop.Y - vy),
                                LeftBottom = new Vector2(s.LeftBottom.X - vx, s.LeftBottom.Y - vy),
                            };
                    case SideOrign.Bottom:
                        if (center)
                            return new Triangle
                            {
                                LeftBottom = new Vector2(s.LeftBottom.X + vx, s.LeftBottom.Y + vy),

                                RightTop = new Vector2(s.RightTop.X - vx, s.RightTop.Y - vy),
                                LeftTop = new Vector2(s.LeftTop.X - vx, s.LeftTop.Y - vy),
                            };
                        else
                            return new Triangle
                            {
                                LeftBottom = new Vector2(s.LeftBottom.X, s.LeftBottom.Y),

                                RightTop = new Vector2(s.RightTop.X - vx, s.RightTop.Y - vy),
                                LeftTop = new Vector2(s.LeftTop.X - vx, s.LeftTop.Y - vy),
                            };
                    default:
                        return new Triangle
                        {
                            LeftTop = new Vector2(s.LeftTop.X, s.LeftTop.Y),
                            RightTop = new Vector2(s.RightTop.X, s.RightTop.Y),
                            LeftBottom = new Vector2(s.LeftBottom.X, s.LeftBottom.Y),
                        };
                }
            }
        }
        #endregion

        #region Quadrilaterals
        internal Side(SideOrign o, Quadrilateral s, Center6 c)
        {
            switch (o)
            {
                case SideOrign.Left:
                    rx = s.RightBottom.X + s.RightTop.X;
                    ry = s.RightBottom.Y + s.RightTop.Y;

                    lx = s.LeftTop.X + s.LeftBottom.X;
                    ly = s.LeftTop.Y + s.LeftBottom.Y;

                    od = new Distance4(rx, ry, lx, ly);

                    tx = lx - s.RightTop.X - s.RightTop.X;
                    ty = ly - s.RightTop.Y - s.RightTop.Y;

                    bx = lx - s.RightBottom.X - s.RightBottom.X;
                    by = ly - s.RightBottom.Y - s.RightBottom.Y;

                    sx = s.LeftTop.X - s.LeftBottom.X;
                    sy = s.LeftTop.Y - s.LeftBottom.Y;
                    break;
                case SideOrign.Top:
                    rx = s.LeftBottom.X + s.RightBottom.X;
                    ry = s.LeftBottom.Y + s.RightBottom.Y;

                    lx = s.RightTop.X + s.LeftTop.X;
                    ly = s.RightTop.Y + s.LeftTop.Y;

                    od = new Distance4(rx, ry, lx, ly);

                    tx = lx - s.RightBottom.X - s.RightBottom.X;
                    ty = ly - s.RightBottom.Y - s.RightBottom.Y;

                    bx = lx - s.LeftBottom.X - s.LeftBottom.X;
                    by = ly - s.LeftBottom.Y - s.LeftBottom.Y;

                    sx = s.RightTop.X - s.LeftTop.X;
                    sy = s.RightTop.Y - s.LeftTop.Y;
                    break;
                case SideOrign.Right:
                    rx = s.LeftTop.X + s.LeftBottom.X;
                    ry = s.LeftTop.Y + s.LeftBottom.Y;

                    lx = s.RightBottom.X + s.RightTop.X;
                    ly = s.RightBottom.Y + s.RightTop.Y;

                    od = new Distance4(rx, ry, lx, ly);

                    tx = lx - s.LeftBottom.X - s.LeftBottom.X;
                    ty = ly - s.LeftBottom.Y - s.LeftBottom.Y;

                    bx = lx - s.LeftTop.X - s.LeftTop.X;
                    by = ly - s.LeftTop.Y - s.LeftTop.Y;

                    sx = s.RightBottom.X - s.RightTop.X;
                    sy = s.RightBottom.Y - s.RightTop.Y;
                    break;
                case SideOrign.Bottom:
                    rx = s.RightTop.X + s.LeftTop.X;
                    ry = s.RightTop.Y + s.LeftTop.Y;

                    lx = s.LeftBottom.X + s.RightBottom.X;
                    ly = s.LeftBottom.Y + s.RightBottom.Y;

                    od = new Distance4(rx, ry, lx, ly);

                    tx = lx - s.LeftTop.X - s.LeftTop.X;
                    ty = ly - s.LeftTop.Y - s.LeftTop.Y;

                    bx = lx - s.RightTop.X - s.RightTop.X;
                    by = ly - s.RightTop.Y - s.RightTop.Y;

                    sx = s.LeftBottom.X - s.RightBottom.X;
                    sy = s.LeftBottom.Y - s.RightBottom.Y;
                    break;
                default:
                    rx = 0f; ry = 0f;
                    lx = 0f; ly = 0f;
                    od = default;
                    tx = 0f; ty = 0f;
                    bx = 0f; by = 0f;
                    sx = 0f; sy = 0f;
                    break;
            }
        }

        internal Quadrilateral To(SideOrign o, Quadrilateral s, Trans8 ct, Vector2 p, bool ratio, bool center)
        {
            if (ratio)
            {
                if (center)
                {
                    float s4 = od.S4(p);

                    return new Quadrilateral
                    {
                        LeftTop = new Vector2(s.LeftTop.X + ct.LeftTopX * s4,
                        s.LeftTop.Y + ct.LeftTopY * s4),

                        RightTop = new Vector2(s.RightTop.X + ct.RightTopX * s4,
                        s.RightTop.Y + ct.RightTopY * s4),

                        LeftBottom = new Vector2(s.LeftBottom.X + ct.LeftBottomX * s4,
                        s.LeftBottom.Y + ct.LeftBottomY * s4),

                        RightBottom = new Vector2(s.RightBottom.X + ct.RightBottomX * s4,
                        s.RightBottom.Y + ct.RightBottomY * s4),
                    };
                }
                else
                {
                    float sds = od.S(p);
                    float x = sx * sds;
                    float y = sy * sds;

                    switch (o)
                    {
                        case SideOrign.Left:
                            return new Quadrilateral
                            {
                                LeftTop = new Vector2(s.LeftTop.X - x, s.LeftTop.Y - y),
                                LeftBottom = new Vector2(s.LeftBottom.X + x, s.LeftBottom.Y + y),

                                RightTop = new Vector2(s.RightTop.X + tx * sds, s.RightTop.Y + ty * sds),
                                RightBottom = new Vector2(s.RightBottom.X + bx * sds, s.RightBottom.Y + by * sds),
                            };
                        case SideOrign.Top:
                            return new Quadrilateral
                            {
                                RightTop = new Vector2(s.RightTop.X - x, s.RightTop.Y - y),
                                LeftTop = new Vector2(s.LeftTop.X + x, s.LeftTop.Y + y),

                                RightBottom = new Vector2(s.RightBottom.X + tx * sds, s.RightBottom.Y + ty * sds),
                                LeftBottom = new Vector2(s.LeftBottom.X + bx * sds, s.LeftBottom.Y + by * sds),
                            };
                        case SideOrign.Right:
                            return new Quadrilateral
                            {
                                RightBottom = new Vector2(s.RightBottom.X - x, s.RightBottom.Y - y),
                                RightTop = new Vector2(s.RightTop.X + x, s.RightTop.Y + y),

                                LeftBottom = new Vector2(s.LeftBottom.X + tx * sds, s.LeftBottom.Y + ty * sds),
                                LeftTop = new Vector2(s.LeftTop.X + bx * sds, s.LeftTop.Y + by * sds),
                            };
                        case SideOrign.Bottom:
                            return new Quadrilateral
                            {
                                LeftBottom = new Vector2(s.LeftBottom.X - x, s.LeftBottom.Y - y),
                                RightBottom = new Vector2(s.RightBottom.X + x, s.RightBottom.Y + y),

                                LeftTop = new Vector2(s.LeftTop.X + tx * sds, s.LeftTop.Y + ty * sds),
                                RightTop = new Vector2(s.RightTop.X + bx * sds, s.RightTop.Y + by * sds),
                            };
                        default:
                            return new Quadrilateral
                            {
                                LeftTop = new Vector2(s.LeftTop.X, s.LeftTop.Y),
                                RightTop = new Vector2(s.RightTop.X, s.RightTop.Y),
                                RightBottom = new Vector2(s.RightBottom.X, s.RightBottom.Y),
                                LeftBottom = new Vector2(s.LeftBottom.X, s.LeftBottom.Y),
                            };
                    }
                }
            }
            else
            {
                float sds = od.S(p);
                float vx = od.hx * sds;
                float vy = od.hy * sds;

                switch (o)
                {
                    case SideOrign.Left:
                        if (center)
                            return new Quadrilateral
                            {
                                LeftTop = new Vector2(s.LeftTop.X + vx, s.LeftTop.Y + vy),
                                LeftBottom = new Vector2(s.LeftBottom.X + vx, s.LeftBottom.Y + vy),

                                RightBottom = new Vector2(s.RightBottom.X - vx, s.RightBottom.Y - vy),
                                RightTop = new Vector2(s.RightTop.X - vx, s.RightTop.Y - vy),
                            };
                        else
                            return new Quadrilateral
                            {
                                LeftTop = new Vector2(s.LeftTop.X, s.LeftTop.Y),
                                LeftBottom = new Vector2(s.LeftBottom.X, s.LeftBottom.Y),

                                RightBottom = new Vector2(s.RightBottom.X - vx, s.RightBottom.Y - vy),
                                RightTop = new Vector2(s.RightTop.X - vx, s.RightTop.Y - vy),
                            };
                    case SideOrign.Top:
                        if (center)
                            return new Quadrilateral
                            {
                                RightTop = new Vector2(s.RightTop.X + vx, s.RightTop.Y + vy),
                                LeftTop = new Vector2(s.LeftTop.X + vx, s.LeftTop.Y + vy),

                                LeftBottom = new Vector2(s.LeftBottom.X - vx, s.LeftBottom.Y - vy),
                                RightBottom = new Vector2(s.RightBottom.X - vx, s.RightBottom.Y - vy),
                            };
                        else
                            return new Quadrilateral
                            {
                                RightTop = new Vector2(s.RightTop.X, s.RightTop.Y),
                                LeftTop = new Vector2(s.LeftTop.X, s.LeftTop.Y),

                                LeftBottom = new Vector2(s.LeftBottom.X - vx, s.LeftBottom.Y - vy),
                                RightBottom = new Vector2(s.RightBottom.X - vx, s.RightBottom.Y - vy),
                            };
                    case SideOrign.Right:
                        if (center)
                            return new Quadrilateral
                            {
                                RightBottom = new Vector2(s.RightBottom.X + vx, s.RightBottom.Y + vy),
                                RightTop = new Vector2(s.RightTop.X + vx, s.RightTop.Y + vy),

                                LeftTop = new Vector2(s.LeftTop.X - vx, s.LeftTop.Y - vy),
                                LeftBottom = new Vector2(s.LeftBottom.X - vx, s.LeftBottom.Y - vy),
                            };
                        else
                            return new Quadrilateral
                            {
                                RightBottom = new Vector2(s.RightBottom.X, s.RightBottom.Y),
                                RightTop = new Vector2(s.RightTop.X, s.RightTop.Y),

                                LeftTop = new Vector2(s.LeftTop.X - vx, s.LeftTop.Y - vy),
                                LeftBottom = new Vector2(s.LeftBottom.X - vx, s.LeftBottom.Y - vy),
                            };
                    case SideOrign.Bottom:
                        if (center)
                            return new Quadrilateral
                            {
                                LeftBottom = new Vector2(s.LeftBottom.X + vx, s.LeftBottom.Y + vy),
                                RightBottom = new Vector2(s.RightBottom.X + vx, s.RightBottom.Y + vy),

                                RightTop = new Vector2(s.RightTop.X - vx, s.RightTop.Y - vy),
                                LeftTop = new Vector2(s.LeftTop.X - vx, s.LeftTop.Y - vy),
                            };
                        else
                            return new Quadrilateral
                            {
                                LeftBottom = new Vector2(s.LeftBottom.X, s.LeftBottom.Y),
                                RightBottom = new Vector2(s.RightBottom.X, s.RightBottom.Y),

                                RightTop = new Vector2(s.RightTop.X - vx, s.RightTop.Y - vy),
                                LeftTop = new Vector2(s.LeftTop.X - vx, s.LeftTop.Y - vy),
                            };
                    default:
                        return new Quadrilateral
                        {
                            LeftTop = new Vector2(s.LeftTop.X, s.LeftTop.Y),
                            RightTop = new Vector2(s.RightTop.X, s.RightTop.Y),
                            RightBottom = new Vector2(s.RightBottom.X, s.RightBottom.Y),
                            LeftBottom = new Vector2(s.LeftBottom.X, s.LeftBottom.Y),
                        };
                }
            }
        }
        #endregion
    }
}