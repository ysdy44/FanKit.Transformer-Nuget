using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer.Curves
{
    public struct PathReceiver
    {
        const byte b = 0; // BeginFigure
        const byte l = 1; // Line
        const byte q = 2; // QuadraticBezier
        const byte u = 3; // CubicBezier

        byte m; // Mode

        Vector2 s; // StartPoint
        Vector2 c; // ControlPoint2
        Vector2 e; // EndPoint

        #region Constructors
        // Begin
        public PathReceiver(Vector2 startPoint)
        {
            m = b;
            s = startPoint;
            c = default;
            e = default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private PathReceiver ToCubicBezier(Vector2 controlPoint2, Vector2 endPoint)
        {
            return new PathReceiver
            {
                m = u,
                s = s,
                c = controlPoint2,
                e = endPoint,
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private PathReceiver ToQuadraticBezier(Vector2 controlPoint, Vector2 endPoint)
        {
            return new PathReceiver
            {
                m = q,
                s = s,
                c = controlPoint,
                e = endPoint,
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private PathReceiver ToLine(Vector2 endPoint)
        {
            return new PathReceiver
            {
                m = l,
                s = s,
                c = c,
                e = endPoint,
            };
        }
        #endregion Constructors

        #region Node
        public Node AddCubicBezier(Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPoint, out PathReceiver result)
        {
            Node segment;

            switch (m)
            {
                case b:
                    segment = new Node(s, s, controlPoint1);
                    break;
                case l:
                    segment = new Node(e, e, controlPoint1);
                    break;
                case q:
                case u:
                    segment = new Node(e, c, controlPoint1);
                    break;
                default:
                    segment = default;
                    break;
            }

            result = ToCubicBezier(controlPoint2, endPoint);
            return segment;
        }

        public Node AddQuadraticBezier(Vector2 controlPoint, Vector2 endPoint, out PathReceiver result)
        {
            Node segment;

            switch (m)
            {
                case b:
                    segment = new Node(s, s, controlPoint);
                    break;
                case l:
                    segment = new Node(e, e, controlPoint);
                    break;
                case q:
                case u:
                    segment = new Node(e, c, controlPoint);
                    break;
                default:
                    segment = default;
                    break;
            }

            result = ToQuadraticBezier(controlPoint, endPoint);
            return segment;
        }

        public Node AddLine(Vector2 endPoint, out PathReceiver result)
        {
            Node segment;

            switch (m)
            {
                case b:
                    segment = new Node(s);
                    break;
                case l:
                    segment = new Node(e);
                    break;
                case q:
                case u:
                    segment = new Node(e, c, endPoint);
                    break;
                default:
                    segment = default;
                    break;
            }

            result = ToLine(endPoint);
            return segment;
        }

        // Closed
        public Node EndFigure()
        {
            Node segment;

            switch (m)
            {
                case b:
                case l:
                    segment = new Node(e);
                    break;
                case q:
                case u:
                    segment = new Node(e, c, e);
                    break;
                default:
                    segment = default;
                    break;
            }

            return segment;
        }
        #endregion

        #region Segment0
        public Segment0 AddCubicBezier0(Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPoint, out PathReceiver result)
        {
            Segment0 segment;

            switch (m)
            {
                case b:
                    segment = new Segment0(false, new Node(s, s, controlPoint1));
                    break;
                case l:
                    segment = new Segment0(false, new Node(e, e, controlPoint1));
                    break;
                case q:
                case u:
                    segment = new Segment0(false, new Node(e, c, controlPoint1));
                    break;
                default:
                    segment = default;
                    break;
            }

            result = ToCubicBezier(controlPoint2, endPoint);
            return segment;
        }

        public Segment0 AddQuadraticBezier0(Vector2 controlPoint, Vector2 endPoint, out PathReceiver result)
        {
            Segment0 segment;

            switch (m)
            {
                case b:
                    segment = new Segment0(false, new Node(s, s, controlPoint));
                    break;
                case l:
                    segment = new Segment0(false, new Node(e, e, controlPoint));
                    break;
                case q:
                case u:
                    segment = new Segment0(false, new Node(e, c, controlPoint));
                    break;
                default:
                    segment = default;
                    break;
            }

            result = ToQuadraticBezier(controlPoint, endPoint);
            return segment;
        }

        public Segment0 AddLine0(Vector2 endPoint, out PathReceiver result)
        {
            Segment0 segment;

            switch (m)
            {
                case b:
                    segment = new Segment0(false, s);
                    break;
                case l:
                    segment = new Segment0(false, e);
                    break;
                case q:
                case u:
                    segment = new Segment0(false, new Node(e, c, endPoint));
                    break;
                default:
                    segment = default;
                    break;
            }

            result = ToLine(endPoint);
            return segment;
        }

        // Closed
        public Segment0 EndFigure0()
        {
            Segment0 segment;

            switch (m)
            {
                case b:
                case l:
                    segment = new Segment0(false, e);
                    break;
                case q:
                case u:
                    segment = new Segment0(false, new Node(e, c, e));
                    break;
                default:
                    segment = default;
                    break;
            }

            return segment;
        }
        #endregion

        #region Segment1
        public Segment1 AddCubicBezier1(Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPoint, ICanvasMatrix canvasMatrix, out PathReceiver result)
        {
            Segment1 segment;

            switch (m)
            {
                case b:
                    segment = new Segment1(false, new Node(s, s, controlPoint1), canvasMatrix);
                    break;
                case l:
                    segment = new Segment1(false, new Node(e, e, controlPoint1), canvasMatrix);
                    break;
                case q:
                case u:
                    segment = new Segment1(false, new Node(e, c, controlPoint1), canvasMatrix);
                    break;
                default:
                    segment = default;
                    break;
            }

            result = ToCubicBezier(controlPoint2, endPoint);
            return segment;
        }

        public Segment1 AddQuadraticBezier1(Vector2 controlPoint, Vector2 endPoint, ICanvasMatrix canvasMatrix, out PathReceiver result)
        {
            Segment1 segment;

            switch (m)
            {
                case b:
                    segment = new Segment1(false, new Node(s, s, controlPoint), canvasMatrix);
                    break;
                case l:
                    segment = new Segment1(false, new Node(e, e, controlPoint), canvasMatrix);
                    break;
                case q:
                case u:
                    segment = new Segment1(false, new Node(e, c, controlPoint), canvasMatrix);
                    break;
                default:
                    segment = default;
                    break;
            }

            result = ToQuadraticBezier(controlPoint, endPoint);
            return segment;
        }

        public Segment1 AddLine1(Vector2 endPoint, ICanvasMatrix canvasMatrix, out PathReceiver result)
        {
            Segment1 segment;

            switch (m)
            {
                case b:
                    segment = new Segment1(false, s, canvasMatrix);
                    break;
                case l:
                    segment = new Segment1(false, e, canvasMatrix);
                    break;
                case q:
                case u:
                    segment = new Segment1(false, new Node(e, c, endPoint), canvasMatrix);
                    break;
                default:
                    segment = default;
                    break;
            }

            result = ToLine(endPoint);
            return segment;
        }

        // Closed
        public Segment1 EndFigure1(ICanvasMatrix canvasMatrix)
        {
            Segment1 segment;

            switch (m)
            {
                case b:
                case l:
                    segment = new Segment1(false, e, canvasMatrix);
                    break;
                case q:
                case u:
                    segment = new Segment1(false, new Node(e, c, e), canvasMatrix);
                    break;
                default:
                    segment = default;
                    break;
            }

            return segment;
        }
        #endregion

        #region Segment2
        public Segment2 AddCubicBezier2(Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPoint, Matrix3x2 homographyMatrix, out PathReceiver result)
        {
            Segment2 segment;

            switch (m)
            {
                case b:
                    segment = new Segment2(false, new Node(s, s, controlPoint1), homographyMatrix);
                    break;
                case l:
                    segment = new Segment2(false, new Node(e, e, controlPoint1), homographyMatrix);
                    break;
                case q:
                case u:
                    segment = new Segment2(false, new Node(e, c, controlPoint1), homographyMatrix);
                    break;
                default:
                    segment = default;
                    break;
            }

            result = ToCubicBezier(controlPoint2, endPoint);
            return segment;
        }

        public Segment2 AddQuadraticBezier2(Vector2 controlPoint, Vector2 endPoint, Matrix3x2 homographyMatrix, out PathReceiver result)
        {
            Segment2 segment;

            switch (m)
            {
                case b:
                    segment = new Segment2(false, new Node(s, s, controlPoint), homographyMatrix);
                    break;
                case l:
                    segment = new Segment2(false, new Node(e, e, controlPoint), homographyMatrix);
                    break;
                case q:
                case u:
                    segment = new Segment2(false, new Node(e, c, controlPoint), homographyMatrix);
                    break;
                default:
                    segment = default;
                    break;
            }

            result = ToQuadraticBezier(controlPoint, endPoint);
            return segment;
        }

        public Segment2 AddLine2(Vector2 endPoint, Matrix3x2 homographyMatrix, out PathReceiver result)
        {
            Segment2 segment;

            switch (m)
            {
                case b:
                    segment = new Segment2(false, s, homographyMatrix);
                    break;
                case l:
                    segment = new Segment2(false, e, homographyMatrix);
                    break;
                case q:
                case u:
                    segment = new Segment2(false, new Node(e, c, endPoint), homographyMatrix);
                    break;
                default:
                    segment = default;
                    break;
            }

            result = ToLine(endPoint);
            return segment;
        }

        // Closed
        public Segment2 EndFigure2(Matrix3x2 homographyMatrix)
        {
            Segment2 segment;

            switch (m)
            {
                case b:
                case l:
                    segment = new Segment2(false, e, homographyMatrix);
                    break;
                case q:
                case u:
                    segment = new Segment2(false, new Node(e, c, e), homographyMatrix);
                    break;
                default:
                    segment = default;
                    break;
            }

            return segment;
        }
        #endregion

        #region Segment3
        public Segment3 AddCubicBezier3(Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPoint, Matrix3x2 homographyMatrix, ICanvasMatrix canvasMatrix, out PathReceiver result)
        {
            Segment3 segment;

            switch (m)
            {
                case b:
                    segment = new Segment3(false, new Node(s, s, controlPoint1), homographyMatrix, canvasMatrix);
                    break;
                case l:
                    segment = new Segment3(false, new Node(e, e, controlPoint1), homographyMatrix, canvasMatrix);
                    break;
                case q:
                case u:
                    segment = new Segment3(false, new Node(e, c, controlPoint1), homographyMatrix, canvasMatrix);
                    break;
                default:
                    segment = default;
                    break;
            }

            result = ToCubicBezier(controlPoint2, endPoint);
            return segment;
        }

        public Segment3 AddQuadraticBezier3(Vector2 controlPoint, Vector2 endPoint, Matrix3x2 homographyMatrix, ICanvasMatrix canvasMatrix, out PathReceiver result)
        {
            Segment3 segment;

            switch (m)
            {
                case b:
                    segment = new Segment3(false, new Node(s, s, controlPoint), homographyMatrix, canvasMatrix);
                    break;
                case l:
                    segment = new Segment3(false, new Node(e, e, controlPoint), homographyMatrix, canvasMatrix);
                    break;
                case q:
                case u:
                    segment = new Segment3(false, new Node(e, c, controlPoint), homographyMatrix, canvasMatrix);
                    break;
                default:
                    segment = default;
                    break;
            }

            result = ToQuadraticBezier(controlPoint, endPoint);
            return segment;
        }

        public Segment3 AddLine3(Vector2 endPoint, Matrix3x2 homographyMatrix, ICanvasMatrix canvasMatrix, out PathReceiver result)
        {
            Segment3 segment;

            switch (m)
            {
                case b:
                    segment = new Segment3(false, s, homographyMatrix, canvasMatrix);
                    break;
                case l:
                    segment = new Segment3(false, e, homographyMatrix, canvasMatrix);
                    break;
                case q:
                case u:
                    segment = new Segment3(false, new Node(e, c, endPoint), homographyMatrix, canvasMatrix);
                    break;
                default:
                    segment = default;
                    break;
            }

            result = ToLine(endPoint);
            return segment;
        }

        // Closed
        public Segment3 EndFigure3(Matrix3x2 homographyMatrix, ICanvasMatrix canvasMatrix)
        {
            Segment3 segment;

            switch (m)
            {
                case b:
                case l:
                    segment = new Segment3(false, e, homographyMatrix, canvasMatrix);
                    break;
                case q:
                case u:
                    segment = new Segment3(false, new Node(e, c, e), homographyMatrix, canvasMatrix);
                    break;
                default:
                    segment = default;
                    break;
            }

            return segment;
        }
        #endregion
    }
}