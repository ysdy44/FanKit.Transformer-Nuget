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

        public static Matrix3x2 Affine(float sourceWidth, float sourceHeight, Triangle dest)
        {
            Matrix3x2 destNorm = dest.Normalize();

            return new Matrix3x2
            {
                // First row
                M11 = destNorm.M11 / sourceWidth,
                M12 = destNorm.M12 / sourceWidth,

                // Second row
                M21 = destNorm.M21 / sourceHeight,
                M22 = destNorm.M22 / sourceHeight,

                // Third row
                M31 = destNorm.M31,
                M32 = destNorm.M32
            };
        }

        public static Matrix3x2 Affine(float sourceWidth, float sourceHeight, Quadrilateral dest)
        {
            Matrix3x2 destNorm = dest.Normalize();

            return new Matrix3x2
            {
                // First row
                M11 = destNorm.M11 / sourceWidth,
                M12 = destNorm.M12 / sourceWidth,

                // Second row
                M21 = destNorm.M21 / sourceHeight,
                M22 = destNorm.M22 / sourceHeight,

                // Third row
                M31 = destNorm.M31,
                M32 = destNorm.M32
            };
        }

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

        public static Matrix3x2 Affine(float sourceX, float sourceY, float sourceWidth, float sourceHeight, Triangle dest)
        {
            Matrix3x2 destNorm = dest.Normalize();

            float x = destNorm.M11 / sourceWidth;
            float y = destNorm.M12 / sourceWidth;
            float w = destNorm.M21 / sourceHeight;
            float z = destNorm.M22 / sourceHeight;

            return new Matrix3x2
            {
                // First row
                M11 = x,
                M12 = y,

                // Second row
                M21 = w,
                M22 = z,

                // Third row
                M31 = destNorm.M31 - sourceX * x - sourceY * w,
                M32 = destNorm.M32 - sourceX * y - sourceY * z,
            };
        }

        public static Matrix3x2 Affine(float sourceX, float sourceY, float sourceWidth, float sourceHeight, Quadrilateral dest)
        {
            Matrix3x2 destNorm = dest.Normalize();

            float x = destNorm.M11 / sourceWidth;
            float y = destNorm.M12 / sourceWidth;
            float w = destNorm.M21 / sourceHeight;
            float z = destNorm.M22 / sourceHeight;

            return new Matrix3x2
            {
                // First row
                M11 = x,
                M12 = y,

                // Second row
                M21 = w,
                M22 = z,

                // Third row
                M31 = destNorm.M31 - sourceX * x - sourceY * w,
                M32 = destNorm.M32 - sourceX * y - sourceY * z,
            };
        }

        public static Matrix3x2 Affine(Rectangle source, Triangle dest)
        {
            Matrix3x2 destNorm = dest.Normalize();

            float x = destNorm.M11 / source.Width;
            float y = destNorm.M12 / source.Width;
            float w = destNorm.M21 / source.Height;
            float z = destNorm.M22 / source.Height;

            return new Matrix3x2
            {
                // First row
                M11 = x,
                M12 = y,

                // Second row
                M21 = w,
                M22 = z,

                // Third row
                M31 = destNorm.M31 - source.X * x - source.Y * w,
                M32 = destNorm.M32 - source.X * y - source.Y * z,
            };
        }

        public static Matrix3x2 Affine(Rectangle source, Quadrilateral dest)
        {
            Matrix3x2 destNorm = dest.Normalize();

            float x = destNorm.M11 / source.Width;
            float y = destNorm.M12 / source.Width;
            float w = destNorm.M21 / source.Height;
            float z = destNorm.M22 / source.Height;

            return new Matrix3x2
            {
                // First row
                M11 = x,
                M12 = y,

                // Second row
                M21 = w,
                M22 = z,

                // Third row
                M31 = destNorm.M31 - source.X * x - source.Y * w,
                M32 = destNorm.M32 - source.X * y - source.Y * z,
            };
        }

        // -------------------- 3x2_1x2 -------------------- // 

        // -------------------- 3x2_2x2 -------------------- // 

        // -------------------- 3x2_3x2 -------------------- // 

        public static Matrix3x2 BidiAffine(this InvertibleMatrix3x2 sourceNorm, Matrix3x2 destNorm)
            => sourceNorm.can ? sourceNorm.inv * destNorm : destNorm;

        public static Matrix3x2 BidiAffine(Triangle source, Triangle dest)
            => source.ToInvertibleMatrix().BidiAffine(dest.Normalize());

        public static Matrix3x2 BidiAffine(Triangle source, Quadrilateral dest)
            => source.ToInvertibleMatrix().BidiAffine(dest.Normalize());

        // -------------------- 1x2_3x3 -------------------- // 

        public static Matrix4x4 Persp(float sourceWidth, float sourceHeight, Quadrilateral dest)
            => new SizeSource(sourceWidth, sourceHeight).ToPerspMatrix(dest);

        // -------------------- 2x2_3x3 -------------------- // 

        public static Matrix4x4 Persp(float sourceX, float sourceY, float sourceWidth, float sourceHeight, Quadrilateral dest)
            => new PerspRectMatrix3x3(ToIdentity(sourceX, sourceY, sourceWidth, sourceHeight), dest);

        public static Matrix4x4 Persp(this Rectangle source, Quadrilateral dest)
            => new PerspRectMatrix3x3(source.ToIdentity(), dest);
    }
}