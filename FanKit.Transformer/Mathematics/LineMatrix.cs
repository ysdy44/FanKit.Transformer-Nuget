using System.Numerics;

namespace FanKit.Transformer.Mathematics
{
    public readonly struct LineMatrix
    {
        public readonly bool IsEmpty;

        readonly float x0;
        readonly float y0;

        readonly float x1;
        readonly float y1;

        internal readonly float dx;
        internal readonly float dy;
        internal readonly float r; // Radians

        internal readonly float ls; // Length Squared
        readonly float d; // Distance (Scale of Normalize)
        internal readonly float i; // Inverse Distance

        public LineMatrix(Vector2 linePoint0, Vector2 linePoint1)
        {
            x0 = linePoint0.X;
            y0 = linePoint0.Y;

            x1 = linePoint1.X;
            y1 = linePoint1.Y;

            if (x1 == x0 && y1 == y0)
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

        public Matrix3x2 Pinch(LineMatrix destinationNormalize)
        {
            if (this.IsEmpty || destinationNormalize.IsEmpty)
            {
                return Matrix3x2.Identity;
            }
            else
            {
                if (destinationNormalize.d == this.d)
                {
                    float radians = destinationNormalize.r - this.r;

                    if (radians == 0f)
                    {
                        return Matrix3x2.CreateTranslation(destinationNormalize.x0 - this.x0, destinationNormalize.y0 - this.y0);
                    }
                    else
                    {
                        Matrix3x2 matrix = Matrix3x2.CreateRotation(radians);

                        return new Matrix3x2
                        {
                            // First row
                            M11 = matrix.M11,
                            M12 = matrix.M12,

                            // Second row
                            M21 = matrix.M21,
                            M22 = matrix.M22,

                            // Third row
                            M31 = destinationNormalize.x0 - this.x0 * matrix.M11 - this.y0 * matrix.M21,
                            M32 = destinationNormalize.y0 - this.x0 * matrix.M12 - this.y0 * matrix.M22,
                        };
                    }
                }
                else
                {
                    float radians = destinationNormalize.r - this.r;

                    if (radians == 0f)
                    {
                        float scale = destinationNormalize.d * this.i;

                        return new Matrix3x2
                        {
                            // First row
                            M11 = scale,
                            M12 = 0f,

                            // Second row
                            M21 = 0f,
                            M22 = scale,

                            // Third row
                            M31 = destinationNormalize.x0 - this.x0 * scale,
                            M32 = destinationNormalize.y0 - this.y0 * scale,
                        };
                    }
                    else
                    {
                        float scale = destinationNormalize.d * this.i;
                        float x = this.x0 * scale;
                        float y = this.y0 * scale;

                        Matrix3x2 matrix = Matrix3x2.CreateRotation(radians);

                        return new Matrix3x2
                        {
                            // First row
                            M11 = scale * matrix.M11,
                            M12 = scale * matrix.M12,

                            // Second row
                            M21 = scale * matrix.M21,
                            M22 = scale * matrix.M22,

                            // Third row
                            M31 = destinationNormalize.x0 - x * matrix.M11 - y * matrix.M21 + matrix.M31,
                            M32 = destinationNormalize.y0 - x * matrix.M12 - y * matrix.M22 + matrix.M32,
                        };
                    }
                }
            }
        }
    }
}