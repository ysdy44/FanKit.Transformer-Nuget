using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer.Mathematics
{
    partial class Math
    {
        public static Matrix3x2 Affine(float width, float height) => new Matrix3x2
        {
            M11 = width,
            M12 = 0,
            M21 = 0,
            M22 = height,
            M31 = 0,
            M32 = 0,
        };

        public static Matrix3x2 Affine(float x, float y, float width, float height) => new Matrix3x2
        {
            M11 = width,
            M12 = 0,
            M21 = 0,
            M22 = height,
            M31 = x,
            M32 = y,
        };

        // -------------------- 1x2_2x2 -------------------- // 

        public static Matrix3x2 Map(float sourceWidth, float sourceHeight, Rectangle destination) => new Matrix3x2
        {
            // First row
            M11 = destination.Width / sourceWidth,
            M12 = 0f,

            // Second row
            M21 = 0f,
            M22 = destination.Height / sourceHeight,

            // Third row
            M31 = destination.X,
            M32 = destination.Y
        };

        public static Matrix3x2 Map(float sourceWidth, float sourceHeight, float destinationX, float destinationY, float destinationWidth, float destinationHeight) => new Matrix3x2
        {
            // First row
            M11 = destinationWidth / sourceWidth,
            M12 = 0f,

            // Second row
            M21 = 0f,
            M22 = destinationHeight / sourceHeight,

            // Third row
            M31 = destinationX,
            M32 = destinationY
        };

        // -------------------- 1x2_3x2 -------------------- // 

        public static Matrix3x2 Affine(SizeMatrix srcNorm, Matrix3x2 destNorm)
            => srcNorm.Affine(destNorm);

        public static Matrix3x2 Affine(float sourceWidth, float sourceHeight, Triangle destination)
            => new SizeMatrix(sourceWidth, sourceHeight).Affine(destination.Normalize());

        public static Matrix3x2 Affine(float sourceWidth, float sourceHeight, Quadrilateral destination)
            => new SizeMatrix(sourceWidth, sourceHeight).Affine(destination.Normalize());

        // -------------------- 2x2_2x2 -------------------- // 

        public static Matrix2x2 Map(float sourceX, float sourceY, float sourceWidth, float sourceHeight, float destinationX, float destinationY, float destinationWidth, float destinationHeight)
        {
            float sx = destinationWidth / sourceWidth;
            float sy = destinationHeight / sourceHeight;

            return new Matrix2x2
            {
                ScaleX = sx,
                ScaleY = sy,
                TranslateX = destinationX - sourceX * sx,
                TranslateY = destinationY - sourceY * sy,
            };
        }

        public static Matrix2x2 Map(this Matrix2x2 source, Matrix2x2 destination)
        {
            float sx = destination.ScaleX / source.ScaleX;
            float sy = destination.ScaleY / source.ScaleY;

            return new Matrix2x2
            {
                ScaleX = sx,
                ScaleY = sy,
                TranslateX = destination.TranslateX - source.TranslateX * sx,
                TranslateY = destination.TranslateY - source.TranslateY * sy,
            };
        }

        // -------------------- 2x2_3x2 -------------------- // 

        public static Matrix3x2 Affine(RectMatrix srcNorm, Matrix3x2 destNorm)
            => srcNorm.Affine(destNorm);

        public static Matrix3x2 Affine(float sourceX, float sourceY, float sourceWidth, float sourceHeight, Triangle destination)
            => new RectMatrix(sourceX, sourceY, sourceWidth, sourceHeight).Affine(destination.Normalize());

        public static Matrix3x2 Affine(float sourceX, float sourceY, float sourceWidth, float sourceHeight, Quadrilateral destination)
            => new RectMatrix(sourceX, sourceY, sourceWidth, sourceHeight).Affine(destination.Normalize());

        public static Matrix3x2 Affine(Rectangle source, Triangle destination)
            => new RectMatrix(source).Affine(destination.Normalize());

        public static Matrix3x2 Affine(Rectangle source, Quadrilateral destination)
            => new RectMatrix(source).Affine(destination.Normalize());

        // -------------------- 3x2_1x2 -------------------- // 

        // -------------------- 3x2_2x2 -------------------- // 

        // -------------------- 3x2_3x2 -------------------- // 

        public static Matrix3x2 BidiAffine(Triangle source, Triangle destination)
            => source.ToInvertibleMatrix().BidiAffine(destination.Normalize());

        public static Matrix3x2 BidiAffine(Triangle source, Quadrilateral destination)
            => source.ToInvertibleMatrix().BidiAffine(destination.Normalize());

        // -------------------- 1x2_3x3 -------------------- // 

        public static Matrix4x4 Persp(float sourceWidth, float sourceHeight, Quadrilateral destination)
            => new PerspSizeMatrix3x3(new SizeMatrix(sourceWidth, sourceHeight), destination);

        // -------------------- 2x2_3x3 -------------------- // 

        public static Matrix4x4 Persp(float sourceX, float sourceY, float sourceWidth, float sourceHeight, Quadrilateral destination)
            => new PerspRectMatrix3x3(new RectMatrix(sourceX, sourceY, sourceWidth, sourceHeight), destination);

        public static Matrix4x4 Persp(this Rectangle source, Quadrilateral destination)
            => new PerspRectMatrix3x3(new RectMatrix(source), destination);
    }
}