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

            // Sides
            /*
            if (this.LengthSquared > minSideLengthSquared)
            {
                Vector2 c = this.Center;

                float cx = x - c.X;
                float cy = y - c.Y;

                float c2 = cx * cx + cy * cy;
                if (c2 < minSelectedLengthSquared)
                    return LineContainsNodeMode0.Center;
            }
             */

            // Handle Sides
            /*
            Vector2 d = this.Handle;

            float dx = x - d.X;
            float dy = y - d.Y;

            float d2 = dx * dx + dy * dy;
            if (d2 < minSelectedLengthSquared)
                return LineContainsNodeMode0.Handle;
             */

            // Handle Corners
            /*
            Vector2 e = this.Handle0;

            float ex = x - e.X;
            float ey = y - e.Y;

            float e2 = ex * ex + ey * ey;
            if (e2 < minSelectedLengthSquared)
                return LineContainsNodeMode0.Handle0;

            Vector2 f = this.Handle0;

            float fx = x - f.X;
            float fy = y - f.Y;

            float f2 = fx * fx + fy * fy;
            if (f2 < minSelectedLengthSquared)
                return LineContainsNodeMode0.Handle0;
             */

            return LineContainsNodeMode0.None;
        }
        #endregion Public instance methods
    }
}