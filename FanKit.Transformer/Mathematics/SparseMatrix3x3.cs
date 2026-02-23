using System.Numerics;

namespace FanKit.Transformer.Mathematics
{
    // Copy from Xamarin.SkiaSharpForms\SkiaSharpForms\Demos\Demos\SkiaSharpFormsDemos\Transforms\ShowPerspMatrixPage.xaml.cs
    internal readonly struct SparseMatrix3x3
    {
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

        public SparseMatrix3x3(Quadrilateral quad)
        {
            mat = quad.Normalize();
            x = quad.RightBottom.X;
            y = quad.RightBottom.Y;

            //  A Matrix -> a b
            den = mat.M11 * mat.M22 - mat.M12 * mat.M21;
            a = mat.M22 * x - mat.M21 * y + mat.M21 * mat.M32 - mat.M22 * mat.M31;
            b = mat.M11 * y - mat.M12 * x + mat.M12 * mat.M31 - mat.M11 * mat.M32;

            // compute B Matrix
            // (0, 0)->(0, 0)
            // (0, 1)->(0, 1)
            // (1, 0)->(1, 0)
            // (1, 1)->(a, b)
            ab1 = a + b - den;

            sx = a / ab1; // Scale X
            sy = b / ab1; // Scale Y

            rx = sx - 1f;
            ry = sy - 1f;
            //rx = (den - b) / ab1;
            //ry = (den - a) / ab1;
        }

        public Matrix4x4 Persp() => new Matrix4x4
        {
            // First row
            M11 = sx * mat.M11 + rx * mat.M31,
            M12 = sx * mat.M12 + rx * mat.M32,
            M13 = 0f,
            M14 = rx,

            // Second row
            M21 = sy * mat.M21 + ry * mat.M31,
            M22 = sy * mat.M22 + ry * mat.M32,
            M23 = 0f,
            M24 = ry,

            // Third row
            M31 = 0f,
            M32 = 0f,
            M33 = 1f,
            M34 = 0f,

            // Fourth row
            M41 = mat.M31,
            M42 = mat.M32,
            M43 = 0f,
            M44 = 1f,
        };
    }
}