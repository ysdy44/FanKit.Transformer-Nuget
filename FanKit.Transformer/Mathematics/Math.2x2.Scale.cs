using System.Numerics;

namespace FanKit.Transformer.Mathematics
{
    partial class Math
    {
        //public static Vector2 TransformNormal(Vector2 normal, Matrix3x2 matrix) => Vector2.TransformNormal(normal, matrix);
        public static Vector2 TransformNormal(Vector2 normal, Matrix3x2 matrix)
        {
            return new Vector2(
                normal.X * matrix.M11 + normal.Y * matrix.M21,
                normal.X * matrix.M12 + normal.Y * matrix.M22);
        }
        public static Vector2 TransformNormal(float normalX, float normalY, Matrix3x2 matrix)
        {
            return new Vector2(
                normalX * matrix.M11 + normalY * matrix.M21,
                normalX * matrix.M12 + normalY * matrix.M22);
        }
        public static Vector2 TransformNormalX(float normalX, Matrix3x2 matrix)
        {
            return new Vector2(
                normalX * matrix.M11,
                normalX * matrix.M12);
        }
        public static Vector2 TransformNormalY(float normalY, Matrix3x2 matrix)
        {
            return new Vector2(
                normalY * matrix.M21,
                normalY * matrix.M22);
        }

        public static Vector2 Rotate(Vector2 position, Rotation2x2 rotation)
        {
            return new Vector2(
                position.X * rotation.C - position.Y * rotation.S,
                position.X * rotation.S + position.Y * rotation.C);
        }

        public static Vector2 Transform(Vector2 position, Vector2 translate, float scale)
        {
            return new Vector2(
                position.X * scale + translate.X,
                position.Y * scale + translate.Y);
        }
        public static Vector2 Transform(Vector2 position, Vector2 translate, float scale, Vector2 centerPoint)
        {
            return new Vector2(
                (position.X - centerPoint.X) * scale + centerPoint.X + translate.X,
                (position.Y - centerPoint.Y) * scale + centerPoint.Y + translate.Y);
        }

        public static Vector2 Transform(Vector2 position, Vector2 translate, Vector2 scales)
        {
            return new Vector2(
                position.X * scales.X + translate.X,
                position.Y * scales.Y + translate.Y);
        }
        public static Vector2 Transform(Vector2 position, Vector2 translate, Vector2 scales, Vector2 centerPoint)
        {
            return new Vector2(
                (position.X - centerPoint.X) * scales.X + centerPoint.X + translate.X,
                (position.Y - centerPoint.Y) * scales.Y + centerPoint.Y + translate.Y);
        }

        // -------------------- 2x2_2x2 -------------------- // 

        public static Matrix2x2 Transform(Matrix2x2 matrix, Vector2 translate, float scale) => new Matrix2x2
        {
            // First row
            ScaleX = matrix.ScaleX * scale,
            ScaleY = matrix.ScaleY * scale,

            // Second row
            TranslateX = matrix.TranslateX * scale + translate.X,
            TranslateY = matrix.TranslateY * scale + translate.Y,
        };

        public static Matrix2x2 Transform(Matrix2x2 matrix, Vector2 translate, Vector2 scales) => new Matrix2x2
        {
            // First row
            ScaleX = matrix.ScaleX * scales.X,
            ScaleY = matrix.ScaleY * scales.Y,

            // Second row
            TranslateX = matrix.TranslateX * scales.X + translate.X,
            TranslateY = matrix.TranslateY * scales.Y + translate.Y,
        };

        public static Matrix2x2 Transform(Matrix2x2 matrix, Rectangle transform) => new Matrix2x2
        {
            // First row
            ScaleX = matrix.ScaleX * transform.Width,
            ScaleY = matrix.ScaleY * transform.Height,

            // Second row
            TranslateX = matrix.TranslateX * transform.Width + transform.X,
            TranslateY = matrix.TranslateY * transform.Height + transform.Y,
        };

        public static Matrix2x2 Transform(Matrix2x2 matrix, Matrix2x2 transform) => matrix * transform;

        // -------------------- 3x2_2x2 -------------------- // 

        public static Matrix3x2 Transform(Matrix3x2 matrix, Vector2 translate, float scale) => new Matrix3x2
        {
            // First row
            M11 = matrix.M11 * scale,
            M12 = matrix.M12 * scale,

            // Second row
            M21 = matrix.M21 * scale,
            M22 = matrix.M22 * scale,

            // Third row
            M31 = matrix.M31 * scale + translate.X,
            M32 = matrix.M32 * scale + translate.Y,
        };

        public static Matrix3x2 Transform(Matrix3x2 matrix, Vector2 translate, Vector2 scales) => new Matrix3x2
        {
            // First row
            M11 = matrix.M11 * scales.X,
            M12 = matrix.M12 * scales.Y,

            // Second row
            M21 = matrix.M21 * scales.X,
            M22 = matrix.M22 * scales.Y,

            // Third row
            M31 = matrix.M31 * scales.X + translate.X,
            M32 = matrix.M32 * scales.Y + translate.Y,
        };

        public static Matrix3x2 Transform(Matrix3x2 matrix, Rectangle transform) => new Matrix3x2
        {
            // First row
            M11 = matrix.M11 * transform.Width,
            M12 = matrix.M12 * transform.Height,

            // Second row
            M21 = matrix.M21 * transform.Width,
            M22 = matrix.M22 * transform.Height,

            // Third row
            M31 = matrix.M31 * transform.Width + transform.X,
            M32 = matrix.M32 * transform.Height + transform.Y,
        };

        public static Matrix3x2 Transform(Matrix3x2 matrix, Matrix2x2 transform) => new Matrix3x2
        {
            // First row
            M11 = matrix.M11 * transform.ScaleX,
            M12 = matrix.M12 * transform.ScaleY,

            // Second row
            M21 = matrix.M21 * transform.ScaleX,
            M22 = matrix.M22 * transform.ScaleY,

            // Third row
            M31 = matrix.M31 * transform.ScaleX + transform.TranslateX,
            M32 = matrix.M32 * transform.ScaleY + transform.TranslateY,
        };

        // -------------------- 3x3_2x2 -------------------- // 

        public static Matrix4x4 Transform(Matrix4x4 matrix, Vector2 translate, float scale) => new Matrix4x4(
            // First row
            matrix.M11 * scale + matrix.M14 * translate.X,
            matrix.M12 * scale + matrix.M14 * translate.Y,
            matrix.M13,
            matrix.M14,

            // Second row
            matrix.M21 * scale + matrix.M24 * translate.X,
            matrix.M22 * scale + matrix.M24 * translate.Y,
            matrix.M23,
            matrix.M24,

            // Third row
            matrix.M31,
            matrix.M32,
            matrix.M33,
            matrix.M34,

            // Fourth row
            matrix.M41 * scale + matrix.M44 * translate.X,
            matrix.M42 * scale + matrix.M44 * translate.Y,
            matrix.M43,
            matrix.M44);

        public static Matrix4x4 Transform(Matrix4x4 matrix, Vector2 translate, Vector2 scales) => new Matrix4x4(
            // First row
            matrix.M11 * scales.X + matrix.M14 * translate.X,
            matrix.M12 * scales.Y + matrix.M14 * translate.Y,
            matrix.M13,
            matrix.M14,

            // Second row
            matrix.M21 * scales.X + matrix.M24 * translate.X,
            matrix.M22 * scales.Y + matrix.M24 * translate.Y,
            matrix.M23,
            matrix.M24,

            // Third row
            matrix.M31,
            matrix.M32,
            matrix.M33,
            matrix.M34,

            // Fourth row
            matrix.M41 * scales.X + matrix.M44 * translate.X,
            matrix.M42 * scales.Y + matrix.M44 * translate.Y,
            matrix.M43,
            matrix.M44);
    }
}