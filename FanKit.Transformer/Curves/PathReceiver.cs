using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer.Curves
{
    public partial struct PathReceiver
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
    }
}