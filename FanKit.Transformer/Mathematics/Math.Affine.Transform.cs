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

        public static Matrix3x2 Map(float sourceWidth, float sourceHeight, Rectangle dest) => new Matrix3x2
        {
            // First row
            M11 = dest.Width / sourceWidth,
            M12 = 0f,

            // Second row
            M21 = 0f,
            M22 = dest.Height / sourceHeight,

            // Third row
            M31 = dest.X,
            M32 = dest.Y
        };

        public static Matrix3x2 Map(float sourceWidth, float sourceHeight, float destX, float destY, float destWidth, float destHeight) => new Matrix3x2
        {
            // First row
            M11 = destWidth / sourceWidth,
            M12 = 0f,

            // Second row
            M21 = 0f,
            M22 = destHeight / sourceHeight,

            // Third row
            M31 = destX,
            M32 = destY
        };

        // -------------------- 1x2_3x2 -------------------- // 

        public static Matrix3x2 Affine(SizeMatrix srcNorm, Matrix3x2 destNorm)
            => srcNorm.Affine(destNorm);

        public static Matrix3x2 Affine(float sourceWidth, float sourceHeight, Triangle dest)
            => new SizeMatrix(sourceWidth, sourceHeight).Affine(dest.Normalize());

        public static Matrix3x2 Affine(float sourceWidth, float sourceHeight, Quadrilateral dest)
            => new SizeMatrix(sourceWidth, sourceHeight).Affine(dest.Normalize());

        // -------------------- 2x2_2x2 -------------------- // 

        public static Matrix2x2 Map(float sourceX, float sourceY, float sourceWidth, float sourceHeight, float destX, float destY, float destWidth, float destHeight)
        {
            float sx = destWidth / sourceWidth;
            float sy = destHeight / sourceHeight;

            return new Matrix2x2
            {
                ScaleX = sx,
                ScaleY = sy,
                TranslateX = destX - sourceX * sx,
                TranslateY = destY - sourceY * sy,
            };
        }

        public static Matrix2x2 Map(this Matrix2x2 source, Matrix2x2 dest)
        {
            float sx = dest.ScaleX / source.ScaleX;
            float sy = dest.ScaleY / source.ScaleY;

            return new Matrix2x2
            {
                ScaleX = sx,
                ScaleY = sy,
                TranslateX = dest.TranslateX - source.TranslateX * sx,
                TranslateY = dest.TranslateY - source.TranslateY * sy,
            };
        }

        // -------------------- 2x2_3x2 -------------------- // 

        public static Matrix3x2 Affine(RectMatrix srcNorm, Matrix3x2 destNorm)
            => srcNorm.Affine(destNorm);

        public static Matrix3x2 Affine(float sourceX, float sourceY, float sourceWidth, float sourceHeight, Triangle dest)
            => new RectMatrix(sourceX, sourceY, sourceWidth, sourceHeight).Affine(dest.Normalize());

        public static Matrix3x2 Affine(float sourceX, float sourceY, float sourceWidth, float sourceHeight, Quadrilateral dest)
            => new RectMatrix(sourceX, sourceY, sourceWidth, sourceHeight).Affine(dest.Normalize());

        public static Matrix3x2 Affine(Rectangle source, Triangle dest)
            => new RectMatrix(source).Affine(dest.Normalize());

        public static Matrix3x2 Affine(Rectangle source, Quadrilateral dest)
            => new RectMatrix(source).Affine(dest.Normalize());

        // -------------------- 3x2_1x2 -------------------- // 

        // -------------------- 3x2_2x2 -------------------- // 

        // -------------------- 3x2_3x2 -------------------- // 

        public static Matrix3x2 BidiAffine(Triangle source, Triangle dest)
            => source.ToInvertibleMatrix().BidiAffine(dest.Normalize());

        public static Matrix3x2 BidiAffine(Triangle source, Quadrilateral dest)
            => source.ToInvertibleMatrix().BidiAffine(dest.Normalize());

        // -------------------- 1x2_3x3 -------------------- // 

        public static Matrix4x4 Persp(float sourceWidth, float sourceHeight, Quadrilateral dest)
            => new PerspSizeMatrix3x3(new SizeMatrix(sourceWidth, sourceHeight), dest);

        // -------------------- 2x2_3x3 -------------------- // 

        public static Matrix4x4 Persp(float sourceX, float sourceY, float sourceWidth, float sourceHeight, Quadrilateral dest)
            => new PerspRectMatrix3x3(new RectMatrix(sourceX, sourceY, sourceWidth, sourceHeight), dest);

        public static Matrix4x4 Persp(this Rectangle source, Quadrilateral dest)
            => new PerspRectMatrix3x3(new RectMatrix(source), dest);
    }
}