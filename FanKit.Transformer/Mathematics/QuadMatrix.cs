using System.Numerics;

namespace FanKit.Transformer
{
    // Copy from Xamarin.SkiaSharpForms\SkiaSharpForms\Demos\Demos\SkiaSharpFormsDemos\Transforms\ShowPerspMatrixPage.xaml.cs
    partial struct Quadrilateral
    {
        /*
        internal readonly Matrix3x2 mat; // Identity Matrix
        readonly float x; // RightBottom.X
        readonly float y; // RightBottom.Y

        readonly float den;
        readonly float a;
        readonly float b;

        readonly float ab1;

        // Matrix
        internal readonly float sx;
        internal readonly float sy;
        internal readonly float rx;
        internal readonly float ry;
         */

        public Matrix4x4 Normalize()
        {
            Matrix4x4 mat = new Matrix4x4
            {
                M11 = this.RightTop.X - this.LeftTop.X,
                M12 = this.RightTop.Y - this.LeftTop.Y,
                M21 = this.LeftBottom.X - this.LeftTop.X,
                M22 = this.LeftBottom.Y - this.LeftTop.Y,
                M41 = this.LeftTop.X,
                M42 = this.LeftTop.Y
            };

            float x = this.RightBottom.X;
            float y = this.RightBottom.Y;

            //  A Matrix -> a b
            float den = mat.M11 * mat.M22 - mat.M12 * mat.M21;
            float a = mat.M22 * x - mat.M21 * y + mat.M21 * mat.M42 - mat.M22 * mat.M41;
            float b = mat.M11 * y - mat.M12 * x + mat.M12 * mat.M41 - mat.M11 * mat.M42;

            // compute B Matrix
            // (0, 0)->(0, 0)
            // (0, 1)->(0, 1)
            // (1, 0)->(1, 0)
            // (1, 1)->(a, b)
            float ab1 = a + b - den;

            float sx = a / ab1; // Scale X
            float sy = b / ab1; // Scale Y

            //rx = sx - 1f;
            //ry = sy - 1f;
            //rx = (den - b) / ab1;
            //ry = (den - a) / ab1;

            mat.M14 = sx;
            mat.M24 = sy;
            return mat;
        }

        internal static Matrix4x4 Persp(Matrix4x4 sourceNormalize) => new Matrix4x4
        {
            // First row
            M11 = sourceNormalize.M14 * (sourceNormalize.M11 + sourceNormalize.M41) - sourceNormalize.M41,
            M12 = sourceNormalize.M14 * (sourceNormalize.M12 + sourceNormalize.M42) - sourceNormalize.M42,
            M13 = 0f,
            M14 = sourceNormalize.M14 - 1f,

            // Second row
            M21 = sourceNormalize.M24 * (sourceNormalize.M21 + sourceNormalize.M41) - sourceNormalize.M41,
            M22 = sourceNormalize.M24 * (sourceNormalize.M22 + sourceNormalize.M42) - sourceNormalize.M42,
            M23 = 0f,
            M24 = sourceNormalize.M24 - 1f,

            // Third row
            M31 = 0f,
            M32 = 0f,
            M33 = 1f,
            M34 = 0f,

            // Fourth row
            M41 = sourceNormalize.M41,
            M42 = sourceNormalize.M42,
            M43 = 0f,
            M44 = 1f,
        };
    }
}