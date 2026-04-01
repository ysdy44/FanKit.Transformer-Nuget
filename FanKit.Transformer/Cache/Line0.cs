using System.Numerics;

namespace FanKit.Transformer.Cache
{
    partial struct Line0
    {
        #region Public instance methods
        public LineContainsNodeMode0 ContainsNode(Vector2 point, float minSelectedLengthSquared = 044f, float minSideLengthSquared = 044f)
        {
            float x = point.X;
            float y = point.Y;

            // Corners
            Vector2 a = this.Point1;

            float ax = x - a.X;
            float ay = y - a.Y;

            float a2 = ax * ax + ay * ay;
            if (a2 < minSelectedLengthSquared)
                return LineContainsNodeMode0.Point1;

            Vector2 b = this.Point0;

            float bx = x - b.X;
            float by = y - b.Y;

            float b2 = bx * bx + by * by;
            if (b2 < minSelectedLengthSquared)
                return LineContainsNodeMode0.Point0;

            return LineContainsNodeMode0.None;
        }
        #endregion Public instance methods
    }
}