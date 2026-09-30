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

        public LineMatrix(Vector2 linePoint0, Vector2 linePoint1)
        {
            if (linePoint1.X == linePoint0.X && linePoint1.Y == linePoint0.Y)
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

                dx = linePoint1.X - linePoint0.X;
                dy = linePoint1.Y - linePoint0.Y;
                r = (float)System.Math.Atan2(dy, dx);

                ls = dx * dx + dy * dy;
                d = (float)System.Math.Sqrt(ls);
                i = 1f / d;
            }
        }

        public Matrix3x2 Pinch(Vector2 startingLinePoint0, Vector2 linePoint1)
        {
            if (linePoint1.X == startingLinePoint0.X && linePoint1.Y == startingLinePoint0.Y)
            {
                return Matrix3x2.Identity;
            }

            float dx = linePoint1.X - startingLinePoint0.X;
            float dy = linePoint1.Y - startingLinePoint0.Y;
            float ls = dy * this.dy + dx * this.dx;

            float c = ls / this.ls;

            return new Matrix3x2
            {
                // First row
                M11 = c,
                M12 = 0f,

                // Second row
                M21 = 0f,
                M22 = c,

                // Third row
                M31 = startingLinePoint0.X - startingLinePoint0.X * c,
                M32 = startingLinePoint0.Y - startingLinePoint0.Y * c
            };
        }

        public Matrix3x2 Pinch(Vector2 startingLinePoint0, Vector2 linePoint0, Vector2 linePoint1)
        {
            if (linePoint1.X == startingLinePoint0.X && linePoint1.Y == startingLinePoint0.Y)
            {
                return Matrix3x2.Identity;
            }

            float dx = linePoint1.X - linePoint0.X;
            float dy = linePoint1.Y - linePoint0.Y;
            float ls = dx * dx + dy * dy;

            float r = (float)System.Math.Atan2(dy, dx);
            float d = (float)System.Math.Sqrt(ls);
            float a = r - this.r;

            Rotation2x2 r2 = new Rotation2x2(a);
            float c = this.i * r2.C * d;
            float s = this.i * r2.S * d;

            return new Matrix3x2
            {
                // First row
                M11 = c,
                M12 = s,

                // Second row
                M21 = -s,
                M22 = c,

                // Third row
                M31 = linePoint0.X - startingLinePoint0.X * c + startingLinePoint0.Y * s,
                M32 = linePoint0.Y - startingLinePoint0.X * s - startingLinePoint0.Y * c
            };
        }
    }
}