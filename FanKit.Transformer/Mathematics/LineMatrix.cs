using System.Numerics;

namespace FanKit.Transformer.Mathematics
{
    public readonly struct LineMatrix
    {
        public readonly bool IsEmpty;

        internal readonly float dx;
        internal readonly float dy;
        internal readonly float r; // Radians

        internal readonly float ls; // Length Squared
        readonly float d; // Distance (Scale of Normalize)
        internal readonly float i; // Inverse Distance

        #region Constructors
        public LineMatrix(Vector2 p0, Vector2 p1)
        {
            if (p1.X == p0.X && p1.Y == p0.Y)
            {
                IsEmpty = true;

                dx = 0f;
                dy = 0f;
                r = 0f;

                ls = 0f;
                d = 0f;
                i = 1f;
            }
            else
            {
                IsEmpty = false;

                dx = p1.X - p0.X;
                dy = p1.Y - p0.Y;
                r = (float)System.Math.Atan2(dy, dx);

                ls = dx * dx + dy * dy;
                d = (float)System.Math.Sqrt(ls);
                i = 1f / d;
            }
        }
        #endregion Constructors
    }
}