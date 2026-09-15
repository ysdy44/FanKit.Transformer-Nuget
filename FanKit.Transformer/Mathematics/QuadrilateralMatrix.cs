using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer.Mathematics
{
    public struct QuadrilateralMatrix
    {
        static float Abs(float v) => v < 0 ? -v : v;

        readonly Matrix8x8 A;
        readonly Matrix8 b;

        const int n = 8; // b.Length;
        internal readonly Matrix8 x;

        public QuadrilateralMatrix(Quadrilateral quad)
        {
            x = new Matrix8();

            b = new Matrix8
            {
                M0 = 0f,
                M1 = 0f,
                M2 = 1f,
                M3 = 0f,
                M4 = 1f,
                M5 = 1f,
                M6 = 0f,
                M7 = 1f,
            };

            A = new Matrix8x8
            {
                M00 = quad.LeftTop.X,
                M01 = quad.LeftTop.Y,
                M02 = 1f,
                M03 = 0f,
                M04 = 0f,
                M05 = 0f,
                M06 = 0f,
                M07 = 0f,

                M10 = 0f,
                M11 = 0f,
                M12 = 0f,
                M13 = quad.LeftTop.X,
                M14 = quad.LeftTop.Y,
                M15 = 1f,
                M16 = 0f,
                M17 = 0f,

                M20 = quad.RightTop.X,
                M21 = quad.RightTop.Y,
                M22 = 1f,
                M23 = 0f,
                M24 = 0f,
                M25 = 0f,
                M26 = -quad.RightTop.X,
                M27 = -quad.RightTop.Y,

                M30 = 0f,
                M31 = 0f,
                M32 = 0f,
                M33 = quad.RightTop.X,
                M34 = quad.RightTop.Y,
                M35 = 1f,
                M36 = 0f,
                M37 = 0f,

                M40 = quad.RightBottom.X,
                M41 = quad.RightBottom.Y,
                M42 = 1f,
                M43 = 0f,
                M44 = 0f,
                M45 = 0f,
                M46 = -quad.RightBottom.X,
                M47 = -quad.RightBottom.Y,

                M50 = 0f,
                M51 = 0f,
                M52 = 0f,
                M53 = quad.RightBottom.X,
                M54 = quad.RightBottom.Y,
                M55 = 1f,
                M56 = -quad.RightBottom.X,
                M57 = -quad.RightBottom.Y,

                M60 = quad.LeftBottom.X,
                M61 = quad.LeftBottom.Y,
                M62 = 1f,
                M63 = 0f,
                M64 = 0f,
                M65 = 0f,
                M66 = 0f,
                M67 = 0f,

                M70 = 0f,
                M71 = 0f,
                M72 = 0f,
                M73 = quad.LeftBottom.X,
                M74 = quad.LeftBottom.Y,
                M75 = 1f,
                M76 = -quad.LeftBottom.X,
                M77 = -quad.LeftBottom.Y,
            };

            for (int i = 0; i < n; i++)
            {
                int maxRow = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (Abs(A[j, i]) > Abs(A[maxRow, i]))
                        maxRow = j;
                }

                if (maxRow != i)
                {
                    for (int k = 0; k < n; k++)
                    {
                        (A[i, k], A[maxRow, k]) = (A[maxRow, k], A[i, k]);
                    }
                    (b[i], b[maxRow]) = (b[maxRow], b[i]);
                }

                for (int j = i + 1; j < n; j++)
                {
                    float factor = A[j, i] / A[i, i];
                    for (int k = i; k < n; k++)
                    {
                        A[j, k] -= factor * A[i, k];
                    }
                    b[j] -= factor * b[i];
                }
            }

            for (int i = n - 1; i >= 0; i--)
            {
                float sum = 0;
                for (int j = i + 1; j < n; j++)
                {
                    sum += A[i, j] * x[j];
                }
                x[i] = (b[i] - sum) / A[i, i];
            }
        }

        // -------------------- 3x3_1x2 -------------------- // 

        public Matrix4x4 InvPersp(float destinationWidth, float destinationHeight)
        {
            float m11 = this.x.M0 * destinationWidth;
            float m12 = this.x.M3 * destinationHeight;
            float m14 = this.x.M6;

            float m21 = this.x.M1 * destinationWidth;
            float m22 = this.x.M4 * destinationHeight;
            float m24 = this.x.M7;

            float m41 = this.x.M2 * destinationWidth;
            float m42 = this.x.M5 * destinationHeight;

            return new Matrix4x4
            {
                // First row
                M11 = m11,
                M12 = m12,
                M13 = 0f,
                M14 = m14,

                // Second row
                M21 = m21,
                M22 = m22,
                M23 = 0f,
                M24 = m24,

                // Third row
                M31 = 0f,
                M32 = 0f,
                M33 = 1f,
                M34 = 0f,

                // Fourth row
                M41 = m41,
                M42 = m42,
                M43 = 0f,
                M44 = 1f,
            };
        }

        // -------------------- 3x3_2x2 -------------------- //

        public Matrix4x4 InvPersp(Rectangle destination)
        {
            float m11 = this.x.M0 * destination.Width + this.x.M6 * destination.X;
            float m12 = this.x.M3 * destination.Height + this.x.M6 * destination.Y;
            float m14 = this.x.M6;

            float m21 = this.x.M1 * destination.Width + this.x.M7 * destination.X;
            float m22 = this.x.M4 * destination.Height + this.x.M7 * destination.Y;
            float m24 = this.x.M7;

            float m41 = this.x.M2 * destination.Width + destination.X;
            float m42 = this.x.M5 * destination.Height + destination.Y;

            return new Matrix4x4
            {
                // First row
                M11 = m11,
                M12 = m12,
                M13 = 0f,
                M14 = m14,

                // Second row
                M21 = m21,
                M22 = m22,
                M23 = 0f,
                M24 = m24,

                // Third row
                M31 = 0f,
                M32 = 0f,
                M33 = 1f,
                M34 = 0f,

                // Fourth row
                M41 = m41,
                M42 = m42,
                M43 = 0f,
                M44 = 1f,
            };
        }

        // -------------------- 3x3_3x2 -------------------- // 

        public Matrix4x4 InvPersp(Matrix4x4 destinationNormalize)
        {
            float m11 = this.x.M0;
            float m12 = this.x.M3;
            float m14 = this.x.M6;

            float m21 = this.x.M1;
            float m22 = this.x.M4;
            float m24 = this.x.M7;

            float m41 = this.x.M2;
            float m42 = this.x.M5;
            const int m44 = 1;

            Matrix4x4 mat = destinationNormalize;

            // First row
            float n11 = mat.M14 * (mat.M11 + mat.M41) - mat.M41;
            float n12 = mat.M14 * (mat.M12 + mat.M42) - mat.M42;
            float n14 = mat.M14 - 1f;

            // Second row
            float n21 = mat.M24 * (mat.M21 + mat.M41) - mat.M41;
            float n22 = mat.M24 * (mat.M22 + mat.M42) - mat.M42;
            float n24 = mat.M24 - 1f;

            // Third row

            // Fourth row
            float n41 = mat.M41;
            float n42 = mat.M42;
            const int n44 = 1;

            // First row
            float l11 = m11 * n11 + m12 * n21 + m14 * n41;
            float l12 = m11 * n12 + m12 * n22 + m14 * n42;
            float l14 = m11 * n14 + m12 * n24 + m14 * n44;

            // Second row
            float l21 = m21 * n11 + m22 * n21 + m24 * n41;
            float l22 = m21 * n12 + m22 * n22 + m24 * n42;
            float l24 = m21 * n14 + m22 * n24 + m24 * n44;

            // Third row

            // Fourth row
            float l41 = m41 * n11 + m42 * n21 + m44 * n41;
            float l42 = m41 * n12 + m42 * n22 + m44 * n42;
            float l44 = m41 * n14 + m42 * n24 + m44 * n44;

            return new Matrix4x4
            {
                // First row
                M11 = l11,
                M12 = l12,
                M13 = 0f,
                M14 = l14,

                // Second row
                M21 = l21,
                M22 = l22,
                M23 = 0f,
                M24 = l24,

                // Third row
                M31 = 0f,
                M32 = 0f,
                M33 = 1f,
                M34 = 0f,

                // Fourth row
                M41 = l41,
                M42 = l42,
                M43 = 0f,
                M44 = l44,
            };
        }

        public Matrix4x4 InvPersp(Quadrilateral destination) => InvPersp(destination.Normalize());
    }
}