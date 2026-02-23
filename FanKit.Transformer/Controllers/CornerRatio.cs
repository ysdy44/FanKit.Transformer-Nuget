using System.Numerics;
using CornerOrign = FanKit.Transformer.QuadrilateralPointKind;

namespace FanKit.Transformer.Controllers
{
    // 20
    internal readonly partial struct CornerRatio
    {
        // 8
        // od = Orign Transformer
        readonly Trans8 ot;

        // 4
        readonly float rbx, rby;
        readonly float ltx, lty;

        // 8
        // cd = Center Distance
        // od = Orign Distance
        readonly Distance4 cd, od;

        #region Triangles
        internal CornerRatio(CornerOrign o, Triangle s, Center6 c)
        {
            switch (o)
            {
                case CornerOrign.LeftTop:
                    ot = new Trans8
                    {
                        LeftTopX = 0f,
                        LeftTopY = 0f,

                        RightTopX = s.LeftTop.X - s.RightTop.X,
                        RightTopY = s.LeftTop.Y - s.RightTop.Y,

                        LeftBottomX = s.LeftTop.X - s.LeftBottom.X,
                        LeftBottomY = s.LeftTop.Y - s.LeftBottom.Y,

                        RightBottomX = s.LeftTop.X + s.LeftTop.X - s.RightTop.X - s.LeftBottom.X,
                        RightBottomY = s.LeftTop.Y + s.LeftTop.Y - s.RightTop.Y - s.LeftBottom.Y,
                    };

                    rbx = (s.RightTop.X + s.LeftBottom.X - s.LeftTop.X) * 2f;
                    rby = (s.RightTop.Y + s.LeftBottom.Y - s.LeftTop.Y) * 2f;

                    ltx = s.LeftTop.X + s.LeftTop.X;
                    lty = s.LeftTop.Y + s.LeftTop.Y;

                    cd = new Distance4(rbx, rby, c.x2, c.y2);
                    od = new Distance4(rbx, rby, ltx, lty);
                    break;
                case CornerOrign.RightTop:
                    ot = new Trans8
                    {
                        RightTopX = 0f,
                        RightTopY = 0f,

                        RightBottomX = s.LeftTop.X - s.LeftBottom.X,
                        RightBottomY = s.LeftTop.Y - s.LeftBottom.Y,

                        LeftTopX = s.RightTop.X - s.LeftTop.X,
                        LeftTopY = s.RightTop.Y - s.LeftTop.Y,

                        LeftBottomX = s.RightTop.X - s.LeftBottom.X,
                        LeftBottomY = s.RightTop.Y - s.LeftBottom.Y,
                    };

                    rbx = s.LeftBottom.X + s.LeftBottom.X;
                    rby = s.LeftBottom.Y + s.LeftBottom.Y;

                    ltx = s.RightTop.X + s.RightTop.X;
                    lty = s.RightTop.Y + s.RightTop.Y;

                    cd = new Distance4(rbx, rby, c.x2, c.y2);
                    od = new Distance4(rbx, rby, ltx, lty);
                    break;
                case CornerOrign.RightBottom:
                    ot = new Trans8
                    {
                        RightBottomX = 0f,
                        RightBottomY = 0f,

                        LeftBottomX = s.RightTop.X - s.LeftTop.X,
                        LeftBottomY = s.RightTop.Y - s.LeftTop.Y,

                        RightTopX = s.LeftBottom.X - s.LeftTop.X,
                        RightTopY = s.LeftBottom.Y - s.LeftTop.Y,

                        LeftTopX = s.RightTop.X + s.LeftBottom.X - s.LeftTop.X - s.LeftTop.X,
                        LeftTopY = s.RightTop.Y + s.LeftBottom.Y - s.LeftTop.Y - s.LeftTop.Y,
                    };

                    rbx = s.LeftTop.X + s.LeftTop.X;
                    rby = s.LeftTop.Y + s.LeftTop.Y;

                    ltx = (s.RightTop.X + s.LeftBottom.X - s.LeftTop.X) * 2f;
                    lty = (s.RightTop.Y + s.LeftBottom.Y - s.LeftTop.Y) * 2f;

                    cd = new Distance4(rbx, rby, c.x2, c.y2);
                    od = new Distance4(rbx, rby, ltx, lty);
                    break;
                case CornerOrign.LeftBottom:
                    ot = new Trans8
                    {
                        LeftBottomX = 0f,
                        LeftBottomY = 0f,

                        LeftTopX = s.LeftBottom.X - s.LeftTop.X,
                        LeftTopY = s.LeftBottom.Y - s.LeftTop.Y,

                        RightBottomX = s.LeftTop.X - s.RightTop.X,
                        RightBottomY = s.LeftTop.Y - s.RightTop.Y,

                        RightTopX = s.LeftBottom.X - s.RightTop.X,
                        RightTopY = s.LeftBottom.Y - s.RightTop.Y,
                    };

                    rbx = s.RightTop.X + s.RightTop.X;
                    rby = s.RightTop.Y + s.RightTop.Y;

                    ltx = s.LeftBottom.X + s.LeftBottom.X;
                    lty = s.LeftBottom.Y + s.LeftBottom.Y;

                    cd = new Distance4(rbx, rby, c.x2, c.y2);
                    od = new Distance4(rbx, rby, ltx, lty);
                    break;
                default:
                    ot = default;
                    rbx = 0f; rby = 0f;
                    ltx = 0f; lty = 0f;
                    cd = default;
                    od = default;
                    break;
            }
        }

        internal Triangle To(CornerOrign o, Triangle s, Trans8 ct, Vector2 p, bool center)
        {
            if (center)
            {
                float cs = cd.S2(p);

                switch (o)
                {
                    case CornerOrign.LeftTop:
                        return new Triangle
                        {
                            LeftTop = new Vector2(s.LeftTop.X + ct.LeftTopX * cs, s.LeftTop.Y + ct.LeftTopY * cs),
                            RightTop = new Vector2(s.RightTop.X + ct.RightTopX * cs, s.RightTop.Y + ct.RightTopY * cs),
                            LeftBottom = new Vector2(s.LeftBottom.X + ct.LeftBottomX * cs, s.LeftBottom.Y + ct.LeftBottomY * cs),
                        };
                    case CornerOrign.RightTop:
                        return new Triangle
                        {
                            RightTop = new Vector2(s.RightTop.X + ct.RightTopX * cs, s.RightTop.Y + ct.RightTopY * cs),
                            LeftTop = new Vector2(s.LeftTop.X + ct.LeftTopX * cs, s.LeftTop.Y + ct.LeftTopY * cs),
                            LeftBottom = new Vector2(s.LeftBottom.X + ct.LeftBottomX * cs, s.LeftBottom.Y + ct.LeftBottomY * cs),
                        };
                    case CornerOrign.RightBottom:
                        return new Triangle
                        {
                            LeftBottom = new Vector2(s.LeftBottom.X + ct.LeftBottomX * cs, s.LeftBottom.Y + ct.LeftBottomY * cs),
                            RightTop = new Vector2(s.RightTop.X + ct.RightTopX * cs, s.RightTop.Y + ct.RightTopY * cs),
                            LeftTop = new Vector2(s.LeftTop.X + ct.LeftTopX * cs, s.LeftTop.Y + ct.LeftTopY * cs),
                        };
                    case CornerOrign.LeftBottom:
                        return new Triangle
                        {
                            LeftBottom = new Vector2(s.LeftBottom.X + ct.LeftBottomX * cs, s.LeftBottom.Y + ct.LeftBottomY * cs),
                            LeftTop = new Vector2(s.LeftTop.X + ct.LeftTopX * cs, s.LeftTop.Y + ct.LeftTopY * cs),
                            RightTop = new Vector2(s.RightTop.X + ct.RightTopX * cs, s.RightTop.Y + ct.RightTopY * cs),
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
            else
            {
                float os = od.S2(p);

                switch (o)
                {
                    case CornerOrign.LeftTop:
                        return new Triangle
                        {
                            LeftTop = new Vector2(s.LeftTop.X, s.LeftTop.Y),
                            RightTop = new Vector2(s.RightTop.X + ot.RightTopX * os, s.RightTop.Y + ot.RightTopY * os),
                            LeftBottom = new Vector2(s.LeftBottom.X + ot.LeftBottomX * os, s.LeftBottom.Y + ot.LeftBottomY * os),
                        };
                    case CornerOrign.RightTop:
                        return new Triangle
                        {
                            RightTop = new Vector2(s.RightTop.X, s.RightTop.Y),
                            LeftTop = new Vector2(s.LeftTop.X + ot.LeftTopX * os, s.LeftTop.Y + ot.LeftTopY * os),
                            LeftBottom = new Vector2(s.LeftBottom.X + ot.LeftBottomX * os, s.LeftBottom.Y + ot.LeftBottomY * os),
                        };
                    case CornerOrign.RightBottom:
                        return new Triangle
                        {
                            LeftBottom = new Vector2(s.LeftBottom.X + ot.LeftBottomX * os, s.LeftBottom.Y + ot.LeftBottomY * os),
                            RightTop = new Vector2(s.RightTop.X + ot.RightTopX * os, s.RightTop.Y + ot.RightTopY * os),
                            LeftTop = new Vector2(s.LeftTop.X + ot.LeftTopX * os, s.LeftTop.Y + ot.LeftTopY * os),
                        };
                    case CornerOrign.LeftBottom:
                        return new Triangle
                        {
                            LeftBottom = new Vector2(s.LeftBottom.X, s.LeftBottom.Y),
                            LeftTop = new Vector2(s.LeftTop.X + ot.LeftTopX * os, s.LeftTop.Y + ot.LeftTopY * os),
                            RightTop = new Vector2(s.RightTop.X + ot.RightTopX * os, s.RightTop.Y + ot.RightTopY * os),
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
        internal CornerRatio(CornerOrign o, Quadrilateral s, Center6 c)
        {
            switch (o)
            {
                case CornerOrign.LeftTop:
                    ot = new Trans8
                    {
                        LeftTopX = 0f,
                        LeftTopY = 0f,

                        RightTopX = s.LeftTop.X - s.RightTop.X,
                        RightTopY = s.LeftTop.Y - s.RightTop.Y,

                        LeftBottomX = s.LeftTop.X - s.LeftBottom.X,
                        LeftBottomY = s.LeftTop.Y - s.LeftBottom.Y,

                        RightBottomX = s.LeftTop.X - s.RightBottom.X,
                        RightBottomY = s.LeftTop.Y - s.RightBottom.Y,
                    };

                    rbx = s.RightBottom.X + s.RightBottom.X;
                    rby = s.RightBottom.Y + s.RightBottom.Y;

                    ltx = s.LeftTop.X + s.LeftTop.X;
                    lty = s.LeftTop.Y + s.LeftTop.Y;

                    cd = new Distance4(rbx, rby, c.x2, c.y2);
                    od = new Distance4(rbx, rby, ltx, lty);
                    break;
                case CornerOrign.RightTop:
                    ot = new Trans8
                    {
                        RightTopX = 0f,
                        RightTopY = 0f,

                        RightBottomX = s.RightTop.X - s.RightBottom.X,
                        RightBottomY = s.RightTop.Y - s.RightBottom.Y,

                        LeftTopX = s.RightTop.X - s.LeftTop.X,
                        LeftTopY = s.RightTop.Y - s.LeftTop.Y,

                        LeftBottomX = s.RightTop.X - s.LeftBottom.X,
                        LeftBottomY = s.RightTop.Y - s.LeftBottom.Y,
                    };

                    rbx = s.LeftBottom.X + s.LeftBottom.X;
                    rby = s.LeftBottom.Y + s.LeftBottom.Y;

                    ltx = s.RightTop.X + s.RightTop.X;
                    lty = s.RightTop.Y + s.RightTop.Y;

                    cd = new Distance4(rbx, rby, c.x2, c.y2);
                    od = new Distance4(rbx, rby, ltx, lty);
                    break;
                case CornerOrign.RightBottom:
                    ot = new Trans8
                    {
                        RightBottomX = 0f,
                        RightBottomY = 0f,

                        LeftBottomX = s.RightBottom.X - s.LeftBottom.X,
                        LeftBottomY = s.RightBottom.Y - s.LeftBottom.Y,

                        RightTopX = s.RightBottom.X - s.RightTop.X,
                        RightTopY = s.RightBottom.Y - s.RightTop.Y,

                        LeftTopX = s.RightBottom.X - s.LeftTop.X,
                        LeftTopY = s.RightBottom.Y - s.LeftTop.Y,
                    };

                    rbx = s.LeftTop.X + s.LeftTop.X;
                    rby = s.LeftTop.Y + s.LeftTop.Y;

                    ltx = s.RightBottom.X + s.RightBottom.X;
                    lty = s.RightBottom.Y + s.RightBottom.Y;

                    cd = new Distance4(rbx, rby, c.x2, c.y2);
                    od = new Distance4(rbx, rby, ltx, lty);
                    break;
                case CornerOrign.LeftBottom:
                    ot = new Trans8
                    {
                        LeftBottomX = 0f,
                        LeftBottomY = 0f,

                        LeftTopX = s.LeftBottom.X - s.LeftTop.X,
                        LeftTopY = s.LeftBottom.Y - s.LeftTop.Y,

                        RightBottomX = s.LeftBottom.X - s.RightBottom.X,
                        RightBottomY = s.LeftBottom.Y - s.RightBottom.Y,

                        RightTopX = s.LeftBottom.X - s.RightTop.X,
                        RightTopY = s.LeftBottom.Y - s.RightTop.Y,
                    };

                    rbx = s.RightTop.X + s.RightTop.X;
                    rby = s.RightTop.Y + s.RightTop.Y;

                    ltx = s.LeftBottom.X + s.LeftBottom.X;
                    lty = s.LeftBottom.Y + s.LeftBottom.Y;

                    cd = new Distance4(rbx, rby, c.x2, c.y2);
                    od = new Distance4(rbx, rby, ltx, lty);
                    break;
                default:
                    ot = default;
                    rbx = 0f; rby = 0f;
                    ltx = 0f; lty = 0f;
                    cd = default;
                    od = default;
                    break;
            }
        }

        internal Quadrilateral To(CornerOrign o, Quadrilateral s, Trans8 ct, Vector2 p, bool center)
        {
            if (center)
            {
                float cs = cd.S2(p);

                switch (o)
                {
                    case CornerOrign.LeftTop:
                        return new Quadrilateral
                        {
                            LeftTop = new Vector2(s.LeftTop.X + ct.LeftTopX * cs, s.LeftTop.Y + ct.LeftTopY * cs),
                            RightTop = new Vector2(s.RightTop.X + ct.RightTopX * cs, s.RightTop.Y + ct.RightTopY * cs),
                            LeftBottom = new Vector2(s.LeftBottom.X + ct.LeftBottomX * cs, s.LeftBottom.Y + ct.LeftBottomY * cs),
                            RightBottom = new Vector2(s.RightBottom.X + ct.RightBottomX * cs, s.RightBottom.Y + ct.RightBottomY * cs),
                        };
                    case CornerOrign.RightTop:
                        return new Quadrilateral
                        {
                            RightTop = new Vector2(s.RightTop.X + ct.RightTopX * cs, s.RightTop.Y + ct.RightTopY * cs),
                            RightBottom = new Vector2(s.RightBottom.X + ct.RightBottomX * cs, s.RightBottom.Y + ct.RightBottomY * cs),
                            LeftTop = new Vector2(s.LeftTop.X + ct.LeftTopX * cs, s.LeftTop.Y + ct.LeftTopY * cs),
                            LeftBottom = new Vector2(s.LeftBottom.X + ct.LeftBottomX * cs, s.LeftBottom.Y + ct.LeftBottomY * cs),
                        };
                    case CornerOrign.RightBottom:
                        return new Quadrilateral
                        {
                            RightBottom = new Vector2(s.RightBottom.X + ct.RightBottomX * cs, s.RightBottom.Y + ct.RightBottomY * cs),
                            LeftBottom = new Vector2(s.LeftBottom.X + ct.LeftBottomX * cs, s.LeftBottom.Y + ct.LeftBottomY * cs),
                            RightTop = new Vector2(s.RightTop.X + ct.RightTopX * cs, s.RightTop.Y + ct.RightTopY * cs),
                            LeftTop = new Vector2(s.LeftTop.X + ct.LeftTopX * cs, s.LeftTop.Y + ct.LeftTopY * cs),
                        };
                    case CornerOrign.LeftBottom:
                        return new Quadrilateral
                        {
                            LeftBottom = new Vector2(s.LeftBottom.X + ct.LeftBottomX * cs, s.LeftBottom.Y + ct.LeftBottomY * cs),
                            LeftTop = new Vector2(s.LeftTop.X + ct.LeftTopX * cs, s.LeftTop.Y + ct.LeftTopY * cs),
                            RightBottom = new Vector2(s.RightBottom.X + ct.RightBottomX * cs, s.RightBottom.Y + ct.RightBottomY * cs),
                            RightTop = new Vector2(s.RightTop.X + ct.RightTopX * cs, s.RightTop.Y + ct.RightTopY * cs),
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
            else
            {
                float os = od.S2(p);

                switch (o)
                {
                    case CornerOrign.LeftTop:
                        return new Quadrilateral
                        {
                            LeftTop = new Vector2(s.LeftTop.X, s.LeftTop.Y),
                            RightTop = new Vector2(s.RightTop.X + ot.RightTopX * os, s.RightTop.Y + ot.RightTopY * os),
                            LeftBottom = new Vector2(s.LeftBottom.X + ot.LeftBottomX * os, s.LeftBottom.Y + ot.LeftBottomY * os),
                            RightBottom = new Vector2(s.RightBottom.X + ot.RightBottomX * os, s.RightBottom.Y + ot.RightBottomY * os),
                        };
                    case CornerOrign.RightTop:
                        return new Quadrilateral
                        {
                            RightTop = new Vector2(s.RightTop.X, s.RightTop.Y),
                            RightBottom = new Vector2(s.RightBottom.X + ot.RightBottomX * os, s.RightBottom.Y + ot.RightBottomY * os),
                            LeftTop = new Vector2(s.LeftTop.X + ot.LeftTopX * os, s.LeftTop.Y + ot.LeftTopY * os),
                            LeftBottom = new Vector2(s.LeftBottom.X + ot.LeftBottomX * os, s.LeftBottom.Y + ot.LeftBottomY * os),
                        };
                    case CornerOrign.RightBottom:
                        return new Quadrilateral
                        {
                            RightBottom = new Vector2(s.RightBottom.X, s.RightBottom.Y),
                            LeftBottom = new Vector2(s.LeftBottom.X + ot.LeftBottomX * os, s.LeftBottom.Y + ot.LeftBottomY * os),
                            RightTop = new Vector2(s.RightTop.X + ot.RightTopX * os, s.RightTop.Y + ot.RightTopY * os),
                            LeftTop = new Vector2(s.LeftTop.X + ot.LeftTopX * os, s.LeftTop.Y + ot.LeftTopY * os),
                        };
                    case CornerOrign.LeftBottom:
                        return new Quadrilateral
                        {
                            LeftBottom = new Vector2(s.LeftBottom.X, s.LeftBottom.Y),
                            LeftTop = new Vector2(s.LeftTop.X + ot.LeftTopX * os, s.LeftTop.Y + ot.LeftTopY * os),
                            RightBottom = new Vector2(s.RightBottom.X + ot.RightBottomX * os, s.RightBottom.Y + ot.RightBottomY * os),
                            RightTop = new Vector2(s.RightTop.X + ot.RightTopX * os, s.RightTop.Y + ot.RightTopY * os),
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