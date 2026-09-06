namespace FanKit.Transformer.Mathematics
{
    internal struct InvertibleSparseMatrix3x3
    {
        static float Abs(float v) => v < 0 ? -v : v;

        readonly Matrix8x8 A;
        readonly Matrix8 b;

        const int n = 8; // b.Length;
        public readonly Matrix8 x;

        public InvertibleSparseMatrix3x3(Quadrilateral quad)
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
    }
}