namespace FanKit.Transformer.Mathematics
{
    public class PerspMatrix
    {
        static float Abs(float v) => v < 0 ? -v : v;

        readonly float[,] A = new float[8, 8];
        readonly float[] b = new float[8];

        const int n = 8; // b.Length;
        internal readonly float[] x = new float[n];

        internal void Normalize(Quadrilateral src)
        {
            A[0, 0] = src.LeftTop.X;
            A[0, 1] = src.LeftTop.Y;
            A[0, 2] = 1;
            A[0, 3] = 0;
            A[0, 4] = 0;
            A[0, 5] = 0;
            A[0, 6] = 0f;
            A[0, 7] = 0f;
            b[0] = 0f;

            A[1, 0] = 0;
            A[1, 1] = 0;
            A[1, 2] = 0;
            A[1, 3] = src.LeftTop.X;
            A[1, 4] = src.LeftTop.Y;
            A[1, 5] = 1;
            A[1, 6] = 0f;
            A[1, 7] = 0f;
            b[1] = 0f;

            A[2, 0] = src.RightTop.X;
            A[2, 1] = src.RightTop.Y;
            A[2, 2] = 1;
            A[2, 3] = 0;
            A[2, 4] = 0;
            A[2, 5] = 0;
            A[2, 6] = -src.RightTop.X;
            A[2, 7] = -src.RightTop.Y;
            b[2] = 1f;

            A[3, 0] = 0;
            A[3, 1] = 0;
            A[3, 2] = 0;
            A[3, 3] = src.RightTop.X;
            A[3, 4] = src.RightTop.Y;
            A[3, 5] = 1;
            A[3, 6] = 0f;
            A[3, 7] = 0f;
            b[3] = 0f;

            A[4, 0] = src.RightBottom.X;
            A[4, 1] = src.RightBottom.Y;
            A[4, 2] = 1;
            A[4, 3] = 0;
            A[4, 4] = 0;
            A[4, 5] = 0;
            A[4, 6] = -src.RightBottom.X;
            A[4, 7] = -src.RightBottom.Y;
            b[4] = 1f;

            A[5, 0] = 0;
            A[5, 1] = 0;
            A[5, 2] = 0;
            A[5, 3] = src.RightBottom.X;
            A[5, 4] = src.RightBottom.Y;
            A[5, 5] = 1;
            A[5, 6] = -src.RightBottom.X;
            A[5, 7] = -src.RightBottom.Y;
            b[5] = 1f;

            A[6, 0] = src.LeftBottom.X;
            A[6, 1] = src.LeftBottom.Y;
            A[6, 2] = 1;
            A[6, 3] = 0;
            A[6, 4] = 0;
            A[6, 5] = 0;
            A[6, 6] = 0f;
            A[6, 7] = 0f;
            b[6] = 0f;

            A[7, 0] = 0;
            A[7, 1] = 0;
            A[7, 2] = 0;
            A[7, 3] = src.LeftBottom.X;
            A[7, 4] = src.LeftBottom.Y;
            A[7, 5] = 1;
            A[7, 6] = -src.LeftBottom.X;
            A[7, 7] = -src.LeftBottom.Y;
            b[7] = 1f;

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