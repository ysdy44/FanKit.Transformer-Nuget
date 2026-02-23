using System.Numerics;

namespace FanKit.Transformer.Mathematics
{
    partial class Math
    {
        public static Vector2 Translate(Vector2 position, Vector2 translate)
        {
            return new Vector2(position.X + translate.X, position.Y + translate.Y);
        }

        public static Vector2 Translate(Vector2 position, float translateX, float translateY)
        {
            return new Vector2(position.X + translateX, position.Y + translateY);
        }

        public static Vector2 TranslateX(Vector2 position, float translateX)
        {
            return new Vector2(position.X + translateX, position.Y);
        }

        public static Vector2 TranslateY(Vector2 position, float translateY)
        {
            return new Vector2(position.X, position.Y + translateY);
        }

        public static Vector2 Scale(Vector2 position, float scale)
        {
            return new Vector2(position.X * scale, position.Y * scale);
        }
        public static Vector2 Scale(Vector2 position, float scale, Vector2 centerPoint)
        {
            return new Vector2(
                (position.X - centerPoint.X) * scale + centerPoint.X,
                (position.Y - centerPoint.Y) * scale + centerPoint.Y);
        }

        public static Vector2 Scale(Vector2 position, Vector2 scales)
        {
            return new Vector2(position.X * scales.X, position.Y * scales.Y);
        }
        public static Vector2 Scale(Vector2 position, Vector2 scales, Vector2 centerPoint)
        {
            return new Vector2(
                (position.X - centerPoint.X) * scales.X + centerPoint.X,
                (position.Y - centerPoint.Y) * scales.Y + centerPoint.Y);
        }

        // -------------------- 2x2_1x2 -------------------- // 

        public static Matrix2x2 Translate(Matrix2x2 matrix, float translateX, float translateY) => new Matrix2x2
        {
            // First row
            ScaleX = matrix.ScaleX,
            ScaleY = matrix.ScaleY,

            // Second row
            TranslateX = matrix.TranslateX + translateX,
            TranslateY = matrix.TranslateY + translateY,
        };

        public static Matrix2x2 Translate(Matrix2x2 matrix, Vector2 translate) => new Matrix2x2
        {
            // First row
            ScaleX = matrix.ScaleX,
            ScaleY = matrix.ScaleY,

            // Second row
            TranslateX = matrix.TranslateX + translate.X,
            TranslateY = matrix.TranslateY + translate.Y,
        };

        public static Matrix2x2 TranslateX(Matrix2x2 matrix, float translateX) => new Matrix2x2
        {
            // First row
            ScaleX = matrix.ScaleX,
            ScaleY = matrix.ScaleY,

            // Second row
            TranslateX = matrix.TranslateX + translateX,
            TranslateY = matrix.TranslateY,
        };

        public static Matrix2x2 TranslateY(Matrix2x2 matrix, float translateY) => new Matrix2x2
        {
            // First row
            ScaleX = matrix.ScaleX,
            ScaleY = matrix.ScaleY,

            // Second row
            TranslateX = matrix.TranslateX,
            TranslateY = matrix.TranslateY + translateY,
        };

        //public static Matrix2x2 Scale(Matrix2x2 matrix, float scale) => matrix * scale;
        public static Matrix2x2 Scale(Matrix2x2 matrix, float scale) => new Matrix2x2
        {
            // First row
            ScaleX = matrix.ScaleX * scale,
            ScaleY = matrix.ScaleY * scale,

            // Second row
            TranslateX = matrix.TranslateX * scale,
            TranslateY = matrix.TranslateY * scale,
        };

        //public static Matrix3x2 Scale(Matrix3x2 matrix, Vector2 scales) => matrix * scales;
        public static Matrix2x2 Scale(Matrix2x2 matrix, Vector2 scales) => new Matrix2x2
        {
            // First row
            ScaleX = matrix.ScaleX * scales.X,
            ScaleY = matrix.ScaleY * scales.Y,

            // Second row
            TranslateX = matrix.TranslateX * scales.X,
            TranslateY = matrix.TranslateY * scales.Y,
        };

        public static Matrix2x2 Scale(Matrix2x2 matrix, float xScale, float yScale) => new Matrix2x2
        {
            // First row
            ScaleX = matrix.ScaleX * xScale,
            ScaleY = matrix.ScaleY * yScale,

            // Second row
            TranslateX = matrix.TranslateX * xScale,
            TranslateY = matrix.TranslateY * yScale,
        };

        // -------------------- 3x2_1x2 -------------------- // 

        public static Matrix3x2 Translate(Matrix3x2 matrix, float translateX, float translateY) => new Matrix3x2
        {
            // First row
            M11 = matrix.M11,
            M12 = matrix.M12,

            // Second row
            M21 = matrix.M21,
            M22 = matrix.M22,

            // Third row
            M31 = matrix.M31 + translateX,
            M32 = matrix.M32 + translateY,
        };

        public static Matrix3x2 Translate(Matrix3x2 matrix, Vector2 translate) => new Matrix3x2
        {
            // First row
            M11 = matrix.M11,
            M12 = matrix.M12,

            // Second row
            M21 = matrix.M21,
            M22 = matrix.M22,

            // Third row
            M31 = matrix.M31 + translate.X,
            M32 = matrix.M32 + translate.Y,
        };

        public static Matrix3x2 TranslateX(Matrix3x2 matrix, float translateX) => new Matrix3x2
        {
            // First row
            M11 = matrix.M11,
            M12 = matrix.M12,

            // Second row
            M21 = matrix.M21,
            M22 = matrix.M22,

            // Third row
            M31 = matrix.M31 + translateX,
            M32 = matrix.M32,
        };

        public static Matrix3x2 TranslateY(Matrix3x2 matrix, float translateY) => new Matrix3x2
        {
            // First row
            M11 = matrix.M11,
            M12 = matrix.M12,

            // Second row
            M21 = matrix.M21,
            M22 = matrix.M22,

            // Third row
            M31 = matrix.M31,
            M32 = matrix.M32 + translateY,
        };

        //public static Matrix3x2 Scale(Matrix3x2 matrix, float scale) => matrix * scale;
        public static Matrix3x2 Scale(Matrix3x2 matrix, float scale) => new Matrix3x2
        {
            // First row
            M11 = matrix.M11 * scale,
            M12 = matrix.M12 * scale,

            // Second row
            M21 = matrix.M21 * scale,
            M22 = matrix.M22 * scale,

            // Third row
            M31 = matrix.M31 * scale,
            M32 = matrix.M32 * scale,
        };

        //public static Matrix3x2 Scale(Matrix3x2 matrix, Vector2 scales) => matrix * scales;
        public static Matrix3x2 Scale(Matrix3x2 matrix, Vector2 scales) => new Matrix3x2
        {
            // First row
            M11 = matrix.M11 * scales.X,
            M12 = matrix.M12 * scales.Y,

            // Second row
            M21 = matrix.M21 * scales.X,
            M22 = matrix.M22 * scales.Y,

            // Third row
            M31 = matrix.M31 * scales.X,
            M32 = matrix.M32 * scales.Y,
        };

        public static Matrix3x2 Scale(Matrix3x2 matrix, float xScale, float yScale) => new Matrix3x2
        {
            // First row
            M11 = matrix.M11 * xScale,
            M12 = matrix.M12 * yScale,

            // Second row
            M21 = matrix.M21 * xScale,
            M22 = matrix.M22 * yScale,

            // Third row
            M31 = matrix.M31 * xScale,
            M32 = matrix.M32 * yScale,
        };

        // -------------------- 3x3_1x2 -------------------- // 

        public static Matrix4x4 Translate(Matrix4x4 matrix, float translateX, float translateY) => new Matrix4x4(
            // First row
            matrix.M11 + matrix.M14 * translateX,
            matrix.M12 + matrix.M14 * translateY,
            matrix.M13,
            matrix.M14,

            // Second row
            matrix.M21 + matrix.M24 * translateX,
            matrix.M22 + matrix.M24 * translateY,
            matrix.M23,
            matrix.M24,

            // Third row
            matrix.M31,
            matrix.M32,
            matrix.M33,
            matrix.M34,

            // Fourth row
            matrix.M41 + matrix.M44 * translateX,
            matrix.M42 + matrix.M44 * translateY,
            matrix.M43,
            matrix.M44
        );

        public static Matrix4x4 Translate(Matrix4x4 matrix, Vector2 translate) => new Matrix4x4(
            // First row
            matrix.M11 + matrix.M14 * translate.X,
            matrix.M12 + matrix.M14 * translate.Y,
            matrix.M13,
            matrix.M14,

            // Second row
            matrix.M21 + matrix.M24 * translate.X,
            matrix.M22 + matrix.M24 * translate.Y,
            matrix.M23,
            matrix.M24,

            // Third row
            matrix.M31,
            matrix.M32,
            matrix.M33,
            matrix.M34,

            // Fourth row
            matrix.M41 + matrix.M44 * translate.X,
            matrix.M42 + matrix.M44 * translate.Y,
            matrix.M43,
            matrix.M44
        );

        public static Matrix4x4 TranslateX(Matrix4x4 matrix, float translateX) => new Matrix4x4(
            // First row
            matrix.M11 + matrix.M14 * translateX,
            matrix.M12,
            matrix.M13,
            matrix.M14,

            // Second row
            matrix.M21 + matrix.M24 * translateX,
            matrix.M22,
            matrix.M23,
            matrix.M24,

            // Third row
            matrix.M31,
            matrix.M32,
            matrix.M33,
            matrix.M34,

            // Fourth row
            matrix.M41 + matrix.M44 * translateX,
            matrix.M42,
            matrix.M43,
            matrix.M44
        );

        public static Matrix4x4 TranslateY(Matrix4x4 matrix, float translateY) => new Matrix4x4(
            // First row
            matrix.M11,
            matrix.M12 + matrix.M14 * translateY,
            matrix.M13,
            matrix.M14,

            // Second row
            matrix.M21,
            matrix.M22 + matrix.M24 * translateY,
            matrix.M23,
            matrix.M24,

            // Third row
            matrix.M31,
            matrix.M32,
            matrix.M33,
            matrix.M34,

            // Fourth row
            matrix.M41,
            matrix.M42 + matrix.M44 * translateY,
            matrix.M43,
            matrix.M44
        );

        public static Matrix4x4 Scale(Matrix4x4 matrix, float scale) => new Matrix4x4(
            // First row
            matrix.M11 * scale,
            matrix.M12 * scale,
            matrix.M13,
            matrix.M14,

            // Second row
            matrix.M21 * scale,
            matrix.M22 * scale,
            matrix.M23,
            matrix.M24,

            // Third row
            matrix.M31,
            matrix.M32,
            matrix.M33,
            matrix.M34,

            // Fourth row
            matrix.M41 * scale,
            matrix.M42 * scale,
            matrix.M43,
            matrix.M44);

        public static Matrix4x4 Scale(Matrix4x4 matrix, float xScale, float yScale) => new Matrix4x4(
            // First row
            matrix.M11 * xScale,
            matrix.M12 * yScale,
            matrix.M13,
            matrix.M14,

            // Second row
            matrix.M21 * xScale,
            matrix.M22 * yScale,
            matrix.M23,
            matrix.M24,

            // Third row
            matrix.M31,
            matrix.M32,
            matrix.M33,
            matrix.M34,

            // Fourth row
            matrix.M41 * xScale,
            matrix.M42 * yScale,
            matrix.M43,
            matrix.M44);

        public static Matrix4x4 Scale(Matrix4x4 matrix, Vector2 scales) => new Matrix4x4(
            // First row
            matrix.M11 * scales.X,
            matrix.M12 * scales.Y,
            matrix.M13,
            matrix.M14,

            // Second row
            matrix.M21 * scales.X,
            matrix.M22 * scales.Y,
            matrix.M23,
            matrix.M24,

            // Third row
            matrix.M31,
            matrix.M32,
            matrix.M33,
            matrix.M34,

            // Fourth row
            matrix.M41 * scales.X,
            matrix.M42 * scales.Y,
            matrix.M43,
            matrix.M44);
    }
}