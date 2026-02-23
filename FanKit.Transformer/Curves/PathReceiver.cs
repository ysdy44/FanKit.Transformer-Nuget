using System.Numerics;

namespace FanKit.Transformer.Curves
{
    public readonly struct PathReceiver
    {
        const byte b = 0; // BeginFigure
        const byte l = 1; // Line
        const byte q = 2; // QuadraticBezier
        const byte u = 3; // CubicBezier

        readonly byte m; // Mode

        readonly Vector2 s; // StartPoint
        readonly Vector2 c; // ControlPoint2
        readonly Vector2 e; // EndPoint

        #region Constructors
        private PathReceiver(byte m, Vector2 s, Vector2 c, Vector2 e)
        {
            this.m = m;
            this.s = s;
            this.c = c;
            this.e = e;
        }

        // Begin
        public PathReceiver(Vector2 startPoint)
        {
            m = b;
            s = startPoint;
            c = default;
            e = default;
        }

        public PathReceiver ToCubicBezier(Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPoint) => new PathReceiver(u, s, controlPoint2, endPoint);
        public PathReceiver ToQuadraticBezier(Vector2 controlPoint, Vector2 endPoint) => new PathReceiver(q, s, controlPoint, endPoint);
        public PathReceiver ToLine(Vector2 endPoint) => new PathReceiver(l, s, c, endPoint);
        #endregion Constructors

        #region Node
        public Node CubicBezier(Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPoint)
        {
            switch (m)
            {
                case b:
                    return new Node(s, s, controlPoint1);
                case l:
                    return new Node(e, e, controlPoint1);
                case q:
                case u:
                    return new Node(e, c, controlPoint1);
                default:
                    return default;
            }
        }

        public Node QuadraticBezier(Vector2 controlPoint, Vector2 endPoint)
        {
            switch (m)
            {
                case b:
                    return new Node(s, s, controlPoint);
                case l:
                    return new Node(e, e, controlPoint);
                case q:
                case u:
                    return new Node(e, c, controlPoint);
                default:
                    return default;
            }
        }

        public Node Line(Vector2 endPoint)
        {
            switch (m)
            {
                case b:
                    return new Node(s);
                case l:
                    return new Node(e);
                case q:
                case u:
                    return new Node(e, c, endPoint);
                default:
                    return default;
            }
        }

        // Closed
        public Node EndFigure()
        {
            switch (m)
            {
                case b:
                case l:
                    return new Node(e);
                case q:
                case u:
                    return new Node(e, c, e);
                default:
                    return default;
            }
        }
        #endregion

        #region Segment0
        public Segment0 CubicBezier0(Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPoint)
        {
            switch (m)
            {
                case b:
                    return new Segment0(false, new Node(s, s, controlPoint1));
                case l:
                    return new Segment0(false, new Node(e, e, controlPoint1));
                case q:
                case u:
                    return new Segment0(false, new Node(e, c, controlPoint1));
                default:
                    return default;
            }
        }

        public Segment0 QuadraticBezier0(Vector2 controlPoint, Vector2 endPoint)
        {
            switch (m)
            {
                case b:
                    return new Segment0(false, new Node(s, s, controlPoint));
                case l:
                    return new Segment0(false, new Node(e, e, controlPoint));
                case q:
                case u:
                    return new Segment0(false, new Node(e, c, controlPoint));
                default:
                    return default;
            }
        }

        public Segment0 Line0(Vector2 endPoint)
        {
            switch (m)
            {
                case b:
                    return new Segment0(false, s);
                case l:
                    return new Segment0(false, e);
                case q:
                case u:
                    return new Segment0(false, new Node(e, c, endPoint));
                default:
                    return default;
            }
        }

        // Closed
        public Segment0 EndFigure0()
        {
            switch (m)
            {
                case b:
                case l:
                    return new Segment0(false, e);
                case q:
                case u:
                    return new Segment0(false, new Node(e, c, e));
                default:
                    return default;
            }
        }
        #endregion

        #region Segment1
        public Segment1 CubicBezier1(Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPoint, ICanvasMatrix matrix)
        {
            switch (m)
            {
                case b:
                    return new Segment1(false, new Node(s, s, controlPoint1), matrix);
                case l:
                    return new Segment1(false, new Node(e, e, controlPoint1), matrix);
                case q:
                case u:
                    return new Segment1(false, new Node(e, c, controlPoint1), matrix);
                default:
                    return default;
            }
        }

        public Segment1 QuadraticBezier1(Vector2 controlPoint, Vector2 endPoint, ICanvasMatrix matrix)
        {
            switch (m)
            {
                case b:
                    return new Segment1(false, new Node(s, s, controlPoint), matrix);
                case l:
                    return new Segment1(false, new Node(e, e, controlPoint), matrix);
                case q:
                case u:
                    return new Segment1(false, new Node(e, c, controlPoint), matrix);
                default:
                    return default;
            }
        }

        public Segment1 Line1(Vector2 endPoint, ICanvasMatrix matrix)
        {
            switch (m)
            {
                case b:
                    return new Segment1(false, s, matrix);
                case l:
                    return new Segment1(false, e, matrix);
                case q:
                case u:
                    return new Segment1(false, new Node(e, c, endPoint), matrix);
                default:
                    return default;
            }
        }

        // Closed
        public Segment1 EndFigure1(ICanvasMatrix matrix)
        {
            switch (m)
            {
                case b:
                case l:
                    return new Segment1(false, e, matrix);
                case q:
                case u:
                    return new Segment1(false, new Node(e, c, e), matrix);
                default:
                    return default;
            }
        }
        #endregion

        #region Segment2
        public Segment2 CubicBezier2(Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPoint, Matrix3x2 matrix)
        {
            switch (m)
            {
                case b:
                    return new Segment2(false, new Node(s, s, controlPoint1), matrix);
                case l:
                    return new Segment2(false, new Node(e, e, controlPoint1), matrix);
                case q:
                case u:
                    return new Segment2(false, new Node(e, c, controlPoint1), matrix);
                default:
                    return default;
            }
        }

        public Segment2 QuadraticBezier2(Vector2 controlPoint, Vector2 endPoint, Matrix3x2 matrix)
        {
            switch (m)
            {
                case b:
                    return new Segment2(false, new Node(s, s, controlPoint), matrix);
                case l:
                    return new Segment2(false, new Node(e, e, controlPoint), matrix);
                case q:
                case u:
                    return new Segment2(false, new Node(e, c, controlPoint), matrix);
                default:
                    return default;
            }
        }

        public Segment2 Line2(Vector2 endPoint, Matrix3x2 matrix)
        {
            switch (m)
            {
                case b:
                    return new Segment2(false, s, matrix);
                case l:
                    return new Segment2(false, e, matrix);
                case q:
                case u:
                    return new Segment2(false, new Node(e, c, endPoint), matrix);
                default:
                    return default;
            }
        }

        // Closed
        public Segment2 EndFigure2(Matrix3x2 matrix)
        {
            switch (m)
            {
                case b:
                case l:
                    return new Segment2(false, e, matrix);
                case q:
                case u:
                    return new Segment2(false, new Node(e, c, e), matrix);
                default:
                    return default;
            }
        }
        #endregion

        #region Segment3
        public Segment3 CubicBezier3(Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPoint, Matrix3x2 matrix1, ICanvasMatrix matrix2)
        {
            switch (m)
            {
                case b:
                    return new Segment3(false, new Node(s, s, controlPoint1), matrix1, matrix2);
                case l:
                    return new Segment3(false, new Node(e, e, controlPoint1), matrix1, matrix2);
                case q:
                case u:
                    return new Segment3(false, new Node(e, c, controlPoint1), matrix1, matrix2);
                default:
                    return default;
            }
        }

        public Segment3 QuadraticBezier3(Vector2 controlPoint, Vector2 endPoint, Matrix3x2 matrix1, ICanvasMatrix matrix2)
        {
            switch (m)
            {
                case b:
                    return new Segment3(false, new Node(s, s, controlPoint), matrix1, matrix2);
                case l:
                    return new Segment3(false, new Node(e, e, controlPoint), matrix1, matrix2);
                case q:
                case u:
                    return new Segment3(false, new Node(e, c, controlPoint), matrix1, matrix2);
                default:
                    return default;
            }
        }

        public Segment3 Line3(Vector2 endPoint, Matrix3x2 matrix1, ICanvasMatrix matrix2)
        {
            switch (m)
            {
                case b:
                    return new Segment3(false, s, matrix1, matrix2);
                case l:
                    return new Segment3(false, e, matrix1, matrix2);
                case q:
                case u:
                    return new Segment3(false, new Node(e, c, endPoint), matrix1, matrix2);
                default:
                    return default;
            }
        }

        // Closed
        public Segment3 EndFigure3(Matrix3x2 matrix1, ICanvasMatrix matrix2)
        {
            switch (m)
            {
                case b:
                case l:
                    return new Segment3(false, e, matrix1, matrix2);
                case q:
                case u:
                    return new Segment3(false, new Node(e, c, e), matrix1, matrix2);
                default:
                    return default;
            }
        }
        #endregion
    }
}