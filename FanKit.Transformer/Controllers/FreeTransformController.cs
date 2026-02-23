using System.Numerics;
using CornerOrign = FanKit.Transformer.QuadrilateralPointKind;

namespace FanKit.Transformer.Controllers
{
    // 29
    public readonly struct FreeTransformController
    {
        readonly CornerOrign co;

        // 6
        readonly float x;
        readonly float y;
        readonly float s;

        readonly float l;
        readonly float mx;
        readonly float my;

        // 6
        readonly float dx; // diagonal
        readonly float lx; // left
        readonly float rx; // right

        readonly float dy; // diagonal
        readonly float ly; // left
        readonly float ry; // right

        // 9
        readonly float x1;
        readonly float y1;
        readonly float s1;

        readonly float x2;
        readonly float y2;
        readonly float s2;

        readonly float x3;
        readonly float y3;
        readonly float s3;

        public FreeTransformController(Quadrilateral t, FreeTransformMode mode, float margin)
        {
            Quadrilateral st = t;
            switch (mode)
            {
                case FreeTransformMode.MoveLeftTop: co = CornerOrign.LeftTop; break;
                case FreeTransformMode.MoveRightTop: co = CornerOrign.RightTop; break;
                case FreeTransformMode.MoveLeftBottom: co = CornerOrign.LeftBottom; break;
                case FreeTransformMode.MoveRightBottom: co = CornerOrign.RightBottom; break;
                default: co = default; break;
            }

            if (margin > 0f)
            {
                switch (co)
                {
                    case CornerOrign.LeftTop:
                        x = st.RightBottom.X + st.RightBottom.X - st.RightTop.X - st.LeftBottom.X;
                        y = st.RightBottom.Y + st.RightBottom.Y - st.RightTop.Y - st.LeftBottom.Y;
                        s = x * x + y * y;

                        l = (float)System.Math.Sqrt(s);
                        mx = x * margin / l;
                        my = y * margin / l;

                        dx = st.RightBottom.X + mx;
                        lx = st.RightTop.X - mx;
                        rx = st.LeftBottom.X - mx;

                        dy = st.RightBottom.Y + my;
                        ly = st.RightTop.Y - my;
                        ry = st.LeftBottom.Y - my;
                        break;
                    case CornerOrign.RightTop:
                        x = st.LeftBottom.X + st.LeftBottom.X - st.RightBottom.X - st.LeftTop.X;
                        y = st.LeftBottom.Y + st.LeftBottom.Y - st.RightBottom.Y - st.LeftTop.Y;
                        s = x * x + y * y;

                        l = (float)System.Math.Sqrt(s);
                        mx = x * margin / l;
                        my = y * margin / l;

                        dx = st.LeftBottom.X + mx;
                        lx = st.RightBottom.X - mx;
                        rx = st.LeftTop.X - mx;

                        dy = st.LeftBottom.Y + my;
                        ly = st.RightBottom.Y - my;
                        ry = st.LeftTop.Y - my;
                        break;
                    case CornerOrign.LeftBottom:
                        x = st.RightTop.X + st.RightTop.X - st.LeftTop.X - st.RightBottom.X;
                        y = st.RightTop.Y + st.RightTop.Y - st.LeftTop.Y - st.RightBottom.Y;
                        s = x * x + y * y;

                        l = (float)System.Math.Sqrt(s);
                        mx = x * margin / l;
                        my = y * margin / l;

                        dx = st.RightTop.X + mx;
                        lx = st.LeftTop.X - mx;
                        rx = st.RightBottom.X - mx;

                        dy = st.RightTop.Y + my;
                        ly = st.LeftTop.Y - my;
                        ry = st.RightBottom.Y - my;
                        break;
                    case CornerOrign.RightBottom:
                        x = st.LeftTop.X + st.LeftTop.X - st.LeftBottom.X - st.RightTop.X;
                        y = st.LeftTop.Y + st.LeftTop.Y - st.LeftBottom.Y - st.RightTop.Y;
                        s = x * x + y * y;

                        l = (float)System.Math.Sqrt(s);
                        mx = x * margin / l;
                        my = y * margin / l;

                        dx = st.LeftTop.X + mx;
                        lx = st.LeftBottom.X - mx;
                        rx = st.RightTop.X - mx;

                        dy = st.LeftTop.Y + my;
                        ly = st.LeftBottom.Y - my;
                        ry = st.RightTop.Y - my;
                        break;
                    default:
                        x = default;
                        y = default;
                        s = default;

                        l = default;
                        mx = default;
                        my = default;

                        dx = default;
                        lx = default;
                        rx = default;

                        dy = default;
                        ly = default;
                        ry = default;
                        break;
                }
            }
            else
            {
                x = default;
                y = default;
                s = default;

                l = default;
                mx = default;
                my = default;

                switch (co)
                {
                    case CornerOrign.LeftTop:
                        dx = st.RightBottom.X;
                        lx = st.RightTop.X;
                        rx = st.LeftBottom.X;

                        dy = st.RightBottom.Y;
                        ly = st.RightTop.Y;
                        ry = st.LeftBottom.Y;
                        break;
                    case CornerOrign.RightTop:
                        dx = st.LeftBottom.X;
                        lx = st.RightBottom.X;
                        rx = st.LeftTop.X;

                        dy = st.LeftBottom.Y;
                        ly = st.RightBottom.Y;
                        ry = st.LeftTop.Y;
                        break;
                    case CornerOrign.LeftBottom:
                        dx = st.RightTop.X;
                        lx = st.LeftTop.X;
                        rx = st.RightBottom.X;

                        dy = st.RightTop.Y;
                        ly = st.LeftTop.Y;
                        ry = st.RightBottom.Y;
                        break;
                    case CornerOrign.RightBottom:
                        dx = st.LeftTop.X;
                        lx = st.LeftBottom.X;
                        rx = st.RightTop.X;

                        dy = st.LeftTop.Y;
                        ly = st.LeftBottom.Y;
                        ry = st.RightTop.Y;
                        break;
                    default:
                        dx = default;
                        lx = default;
                        rx = default;

                        dy = default;
                        ly = default;
                        ry = default;
                        break;
                }
            }

            x1 = lx - rx;
            y1 = ly - ry;
            s1 = x1 * x1 + y1 * y1;

            x2 = dx - rx;
            y2 = dy - ry;
            s2 = x2 * x2 + y2 * y2;

            x3 = lx - dx;
            y3 = ly - dy;
            s3 = x3 * x3 + y3 * y3;
        }

        public CornerOrign PointKind => this.co;

        public Quadrilateral MovePointOfConvexQuadrilateral(Quadrilateral st, Vector2 point)
        {
            float px1 = lx - point.X;
            float py1 = ly - point.Y;

            if (x1 * py1 - y1 * px1 < 0f)
            {
                float p = py1 * y1 + px1 * x1;
                point.X = lx - x1 * p / s1;
                point.Y = ly - y1 * p / s1;
            }

            float px2 = dx - point.X;
            float py2 = dy - point.Y;

            if (x2 * py2 - y2 * px2 < 0f)
            {
                float p = py2 * y2 + px2 * x2;

                point.X = dx - x2 * p / s2;
                point.Y = dy - y2 * p / s2;
            }

            float px3 = lx - point.X;
            float py3 = ly - point.Y;

            if (x3 * py3 - y3 * px3 < 0f)
            {
                float p = py3 * y3 + px3 * x3;

                point.X = lx - x3 * p / s3;
                point.Y = ly - y3 * p / s3;
            }

            float px4 = lx - point.X;
            float py4 = ly - point.Y;

            if (x1 * py4 - y1 * px4 < 0f)
            {
                float p = py4 * y1 + px4 * x1;

                point.X = lx - x1 * p / s1;
                point.Y = ly - y1 * p / s1;
            }

            switch (co)
            {
                case CornerOrign.LeftTop:
                    return new Quadrilateral
                    {
                        LeftTop = point,
                        RightTop = st.RightTop,
                        LeftBottom = st.LeftBottom,
                        RightBottom = st.RightBottom,
                    };
                case CornerOrign.RightTop:
                    return new Quadrilateral
                    {
                        LeftTop = st.LeftTop,
                        RightTop = point,
                        LeftBottom = st.LeftBottom,
                        RightBottom = st.RightBottom,
                    };
                case CornerOrign.LeftBottom:
                    return new Quadrilateral
                    {
                        LeftTop = st.LeftTop,
                        RightTop = st.RightTop,
                        LeftBottom = point,
                        RightBottom = st.RightBottom,
                    };
                case CornerOrign.RightBottom:
                    return new Quadrilateral
                    {
                        LeftTop = st.LeftTop,
                        RightTop = st.RightTop,
                        LeftBottom = st.LeftBottom,
                        RightBottom = point,
                    };
                default:
                    return default;
            }
        }
    }
}