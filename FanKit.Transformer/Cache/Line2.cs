using System.Numerics;

namespace FanKit.Transformer.Cache
{
    partial struct Line2
    {
        #region Public instance methods
        public LineContainsNodeMode2 ContainsNode(Vector2 point, float minSelectedLengthSquared = 144f, float minSideLengthSquared = 144f)
        {
            float x = point.X;
            float y = point.Y;

            // Corners
            Vector2 a = this.Point1;

            float ax = x - a.X;
            float ay = y - a.Y;

            float a2 = ax * ax + ay * ay;
            if (a2 < minSelectedLengthSquared)
                return LineContainsNodeMode2.Point1;

            Vector2 b = this.Point0;

            float bx = x - b.X;
            float by = y - b.Y;

            float b2 = bx * bx + by * by;
            if (b2 < minSelectedLengthSquared)
                return LineContainsNodeMode2.Point0;

            // Sides
            if (this.LengthSquared > minSideLengthSquared)
            {
                Vector2 c = this.Center;

                float cx = x - c.X;
                float cy = y - c.Y;

                float c2 = cx * cx + cy * cy;
                if (c2 < minSelectedLengthSquared)
                    return LineContainsNodeMode2.Center;
            }

            // Handle Sides
            Vector2 d = this.Handle;

            float dx = x - d.X;
            float dy = y - d.Y;

            float d2 = dx * dx + dy * dy;
            if (d2 < minSelectedLengthSquared)
                return LineContainsNodeMode2.Handle;

            return LineContainsNodeMode2.None;
        }
        #endregion Public instance methods
    }
}