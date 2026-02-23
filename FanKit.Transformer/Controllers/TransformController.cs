using System.Numerics;
using System.Runtime.InteropServices;
using CornerOrign = FanKit.Transformer.QuadrilateralPointKind;

namespace FanKit.Transformer.Controllers
{
    [StructLayout(LayoutKind.Explicit, Size = 54 * 4 + 2)]
    public readonly partial struct TransformController
    {
        const int b1 = 6 * 4;
        const int b2 = 8 * 4;

        const int b3 = 1;
        const int b4 = 1;

        const int b5 = 20 * 4;
        const int b6 = 20 * 4;

        [FieldOffset(0)] internal readonly Center6 c;
        [FieldOffset(b1)] readonly Trans8 ct;

        [FieldOffset(b1 + b2)] readonly ControllerMode v;
        [FieldOffset(b1 + b2 + b3)] readonly HandleOrign ho; // Span 1: A
        [FieldOffset(b1 + b2 + b3)] readonly SideOrign so; // Span 1: B
        [FieldOffset(b1 + b2 + b3)] readonly CornerOrign co; // Span 1: C

        [FieldOffset(b1 + b2 + b3 + b4 + b5)] readonly Handle h; // Span 2: A
        [FieldOffset(b1 + b2 + b3 + b4 + b5)] readonly Side s; // Span 2: B
        [FieldOffset(b1 + b2 + b3 + b4 + b5)] readonly Corner n; // Span 2: C 1
        [FieldOffset(b1 + b2 + b3 + b4 + b5 + b6)] readonly CornerRatio r; // Span 2: C

        // Starting Angle of Transformer
        [FieldOffset(b1 + b2 + b3 + b4 + b5)] internal readonly Angle r0; // Span 2: D

        // Angle of Point
        [FieldOffset(b1 + b2 + b3 + b4 + b5 + b6)] internal readonly Angle r1; // Span 2: D

        public Matrix3x2 Rotate(ControllerRadians rotationAngle)
        {
            return Matrix3x2.CreateRotation(rotationAngle.rs, c.c);
        }

        public ControllerRadians ToRadians(Vector2 point, float stepFrequency = float.NaN)
        {
            return new ControllerRadians(this, point, stepFrequency);
        }

        #region Triangles
        public TransformController(Triangle t, TransformMode m)
        {
            c = new Center6(t);
            ct = new Trans8
            {
                #pragma warning disable IDE0055
                LeftTopX = c.c.X - t.LeftTop.X, LeftTopY = c.c.Y - t.LeftTop.Y,
                RightTopX = c.c.X - t.RightTop.X, RightTopY = c.c.Y - t.RightTop.Y,
                LeftBottomX = c.c.X - t.LeftBottom.X, LeftBottomY = c.c.Y - t.LeftBottom.Y,
                RightBottomX = c.c.X - (t.RightTop.X + t.LeftBottom.X - t.LeftTop.X), RightBottomY = c.c.Y - (t.RightTop.Y + t.LeftBottom.Y - t.LeftTop.Y),
                #pragma warning restore IDE0055
            };

            #region Span 1

            ho = default;
            so = default;
            co = default;

            switch (m)
            {
                case TransformMode.SkewLeft: ho = HandleOrign.Right; break;
                case TransformMode.SkewTop: ho = HandleOrign.Bottom; break;
                case TransformMode.SkewRight: ho = HandleOrign.Left; break;
                case TransformMode.SkewBottom: ho = HandleOrign.Top; break;
                default: break;
            }

            switch (m)
            {
                case TransformMode.ScaleLeft: so = SideOrign.Right; break;
                case TransformMode.ScaleTop: so = SideOrign.Bottom; break;
                case TransformMode.ScaleRight: so = SideOrign.Left; break;
                case TransformMode.ScaleBottom: so = SideOrign.Top; break;
                default: break;
            }

            switch (m)
            {
                case TransformMode.ScaleLeftTop: co = CornerOrign.RightBottom; break;
                case TransformMode.ScaleRightTop: co = CornerOrign.LeftBottom; break;
                case TransformMode.ScaleLeftBottom: co = CornerOrign.RightTop; break;
                case TransformMode.ScaleRightBottom: co = CornerOrign.LeftTop; break;
                default: break;
            }

            #endregion

            #region Span 2

            v = default;
            h = default;
            s = default;
            n = default;
            r = default;

            r0 = r1 = default;

            switch (m)
            {
                case TransformMode.SkewLeft:
                case TransformMode.SkewTop:
                case TransformMode.SkewRight:
                case TransformMode.SkewBottom:
                    v = ControllerMode.SkewHandle;
                    h = new Handle(ho, t, c);
                    break;
                case TransformMode.ScaleLeft:
                case TransformMode.ScaleTop:
                case TransformMode.ScaleRight:
                case TransformMode.ScaleBottom:
                    v = ControllerMode.ScaleSide;
                    s = new Side(so, t, c);
                    break;
                case TransformMode.ScaleLeftTop:
                case TransformMode.ScaleRightTop:
                case TransformMode.ScaleLeftBottom:
                case TransformMode.ScaleRightBottom:
                    v = ControllerMode.ScaleCorner;
                    n = new Corner(co, t);
                    r = new CornerRatio(co, t, c);
                    break;
                default:
                    break;
            }

            #endregion
        }

        public TransformController(Triangle t, Vector2 p)
        {
            c = new Center6(t);
            ct = new Trans8
            {
                #pragma warning disable IDE0055
                LeftTopX = c.c.X - t.LeftTop.X, LeftTopY = c.c.Y - t.LeftTop.Y,
                RightTopX = c.c.X - t.RightTop.X, RightTopY = c.c.Y - t.RightTop.Y,
                LeftBottomX = c.c.X - t.LeftBottom.X, LeftBottomY = c.c.Y - t.LeftBottom.Y,
                RightBottomX = c.c.X - (t.RightTop.X + t.LeftBottom.X - t.LeftTop.X), RightBottomY = c.c.Y - (t.RightTop.Y + t.LeftBottom.Y - t.LeftTop.Y),
                #pragma warning restore IDE0055
            };

            ho = default;
            so = default;
            co = default;

            v = ControllerMode.Rotate;
            h = default;
            s = default;
            n = default;
            r = default;

            r0 = new Angle(t);
            r1 = new Angle(p, c.c);
        }

        public Triangle Transform(Triangle st, Vector2 point, bool keepRatio, bool centeredScaling)
        {
            switch (v)
            {
                case ControllerMode.SkewHandle:
                    return h.To(ho, st, c, point, centeredScaling);
                case ControllerMode.ScaleSide:
                    return s.To(so, st, ct, point, keepRatio, centeredScaling);
                case ControllerMode.ScaleCorner:
                    if (keepRatio)
                        return r.To(co, st, ct, point, centeredScaling);
                    else
                        return n.To(co, st, c, point, centeredScaling);
                default:
                    return st;
            }
        }
        #endregion

        #region Quadrilaterals
        public TransformController(Quadrilateral t, TransformMode m)
        {
            c = new Center6(t);
            ct = new Trans8
            {
                #pragma warning disable IDE0055
                LeftTopX = c.c.X - t.LeftTop.X, LeftTopY = c.c.Y - t.LeftTop.Y,
                RightTopX = c.c.X - t.RightTop.X, RightTopY = c.c.Y - t.RightTop.Y,
                LeftBottomX = c.c.X - t.LeftBottom.X, LeftBottomY = c.c.Y - t.LeftBottom.Y,
                RightBottomX = c.c.X - t.RightBottom.X, RightBottomY = c.c.Y - t.RightBottom.Y,
                #pragma warning restore IDE0055
            };

            #region Span 1

            ho = default;
            so = default;
            co = default;

            switch (m)
            {
                case TransformMode.SkewLeft: ho = HandleOrign.Right; break;
                case TransformMode.SkewTop: ho = HandleOrign.Bottom; break;
                case TransformMode.SkewRight: ho = HandleOrign.Left; break;
                case TransformMode.SkewBottom: ho = HandleOrign.Top; break;
                default: break;
            }

            switch (m)
            {
                case TransformMode.ScaleLeft: so = SideOrign.Right; break;
                case TransformMode.ScaleTop: so = SideOrign.Bottom; break;
                case TransformMode.ScaleRight: so = SideOrign.Left; break;
                case TransformMode.ScaleBottom: so = SideOrign.Top; break;
                default: break;
            }

            switch (m)
            {
                case TransformMode.ScaleLeftTop: co = CornerOrign.RightBottom; break;
                case TransformMode.ScaleRightTop: co = CornerOrign.LeftBottom; break;
                case TransformMode.ScaleLeftBottom: co = CornerOrign.RightTop; break;
                case TransformMode.ScaleRightBottom: co = CornerOrign.LeftTop; break;
                default: break;
            }

            #endregion

            #region Span 2

            v = default;
            h = default;
            s = default;
            n = default;
            r = default;

            r0 = r1 = default;

            switch (m)
            {
                case TransformMode.SkewLeft:
                case TransformMode.SkewTop:
                case TransformMode.SkewRight:
                case TransformMode.SkewBottom:
                    v = ControllerMode.SkewHandle;
                    h = new Handle(ho, t, c);
                    break;
                case TransformMode.ScaleLeft:
                case TransformMode.ScaleTop:
                case TransformMode.ScaleRight:
                case TransformMode.ScaleBottom:
                    v = ControllerMode.ScaleSide;
                    s = new Side(so, t, c);
                    break;
                case TransformMode.ScaleLeftTop:
                case TransformMode.ScaleRightTop:
                case TransformMode.ScaleLeftBottom:
                case TransformMode.ScaleRightBottom:
                    v = ControllerMode.ScaleCorner;
                    n = new Corner(co, t);
                    r = new CornerRatio(co, t, c);
                    break;
                default:
                    break;
            }

            #endregion
        }

        public TransformController(Quadrilateral t, Vector2 p)
        {
            c = new Center6(t);
            ct = new Trans8
            {
                #pragma warning disable IDE0055
                LeftTopX = c.c.X - t.LeftTop.X, LeftTopY = c.c.Y - t.LeftTop.Y,
                RightTopX = c.c.X - t.RightTop.X, RightTopY = c.c.Y - t.RightTop.Y,
                LeftBottomX = c.c.X - t.LeftBottom.X, LeftBottomY = c.c.Y - t.LeftBottom.Y,
                RightBottomX = c.c.X - t.RightBottom.X, RightBottomY = c.c.Y - t.RightBottom.Y,
                #pragma warning restore IDE0055
            };

            r0 = new Angle(t);

            r1 = new Angle(p, c.c);

            ho = default;
            so = default;
            co = default;

            v = ControllerMode.Rotate;
            h = default;
            s = default;
            n = default;
            r = default;
        }

        public Quadrilateral Transform(Quadrilateral st, Vector2 point, bool keepRatio, bool centeredScaling)
        {
            switch (v)
            {
                case ControllerMode.SkewHandle:
                    return h.To(ho, st, c, point, centeredScaling);
                case ControllerMode.ScaleSide:
                    return s.To(so, st, ct, point, keepRatio, centeredScaling);
                case ControllerMode.ScaleCorner:
                    if (keepRatio)
                        return r.To(co, st, ct, point, centeredScaling);
                    else
                        return n.To(co, st, c, point, centeredScaling);
                default:
                    return st;
            }
        }
        #endregion
    }
}