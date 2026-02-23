using System.Numerics;

namespace FanKit.Transformer.Mathematics
{
    partial class Math
    {
        public static Vector2 Transform(Vector2 position, Matrix3x2 matrix) => new Vector2
        {
            X = matrix.M11 * position.X + matrix.M21 * position.Y + matrix.M31,
            Y = matrix.M12 * position.X + matrix.M22 * position.Y + matrix.M32,
        };

        public static Vector2 Transform(float x, float y, Matrix3x2 matrix) => new Vector2
        {
            X = matrix.M11 * x + matrix.M21 * y + matrix.M31,
            Y = matrix.M12 * x + matrix.M22 * y + matrix.M32,
        };

        //public static Matrix2x2 Transform(Matrix2x2 value1, Matrix3x2 value2) => value1 * value2;
        public static Matrix3x2 Transform(Matrix2x2 value1, Matrix3x2 value2) => new Matrix3x2
        {
            // First row
            M11 = value1.ScaleX * value2.M11,
            M12 = value1.ScaleX * value2.M12,

            // Second row
            M21 = value1.ScaleY * value2.M21,
            M22 = value1.ScaleY * value2.M22,

            // Third row
            M31 = value1.TranslateX * value2.M11 + value1.TranslateY * value2.M21 + value2.M31,
            M32 = value1.TranslateX * value2.M12 + value1.TranslateY * value2.M22 + value2.M32
        };

        public static Matrix3x2 Transform(Matrix3x2 value1, Matrix3x2 value2) => value1 * value2;
        /*
        public static Matrix3x2 Transform(Matrix3x2 value1, Matrix3x2 value2) => new Matrix3x2
        {
            // First row
            M11 = value1.M11 * value2.M11 + value1.M12 * value2.M21,
            M12 = value1.M11 * value2.M12 + value1.M12 * value2.M22,

            // Second row
            M21 = value1.M21 * value2.M11 + value1.M22 * value2.M21,
            M22 = value1.M21 * value2.M12 + value1.M22 * value2.M22,

            // Third row
            M31 = value1.M31 * value2.M11 + value1.M32 * value2.M21 + value2.M31,
            M32 = value1.M31 * value2.M12 + value1.M32 * value2.M22 + value2.M32
        };
         */

        // -------------------- 1x2_3x2 -------------------- // 

        public static Matrix3x2 TransformTranslation(Vector2 translate, Matrix3x2 matrix) => new Matrix3x2
        {
            // First row
            M11 = matrix.M11,
            M12 = matrix.M12,

            // Second row
            M21 = matrix.M21,
            M22 = matrix.M22,

            // Third row
            M31 = matrix.M11 * translate.X + matrix.M21 * translate.Y + matrix.M31,
            M32 = matrix.M12 * translate.X + matrix.M22 * translate.Y + matrix.M32,
        };

        public static Matrix3x2 TransformTranslation(float translateX, float translateY, Matrix3x2 matrix) => new Matrix3x2
        {
            // First row
            M11 = matrix.M11,
            M12 = matrix.M12,

            // Second row
            M21 = matrix.M21,
            M22 = matrix.M22,

            // Third row
            M31 = matrix.M11 * translateX + matrix.M21 * translateY + matrix.M31,
            M32 = matrix.M12 * translateX + matrix.M22 * translateY + matrix.M32,
        };

        // -------------------- 3x3_3x2 -------------------- // 

        // float m11 = dst.M11;
        // float m13 = dst.M21;
        // float m15 = dst.M31;

        // float m12 = dst.M12;
        // float m14 = dst.M22;
        // float m16 = dst.M32;
        public static Matrix4x4 Transform(Matrix4x4 value1, Matrix3x2 value2) => new Matrix4x4
        {
            // X 123
            M11 = value2.M11 * value1.M11 + value2.M21 * value1.M12 + value1.M13 + value2.M31 * value1.M14,
            M21 = value2.M11 * value1.M21 + value2.M21 * value1.M22 + value1.M13 + value2.M31 * value1.M24,
            M31 = value1.M31,
            M41 = value2.M11 * value1.M41 + value2.M21 * value1.M42 + value1.M43 + value2.M31 * value1.M44,

            // Y 123
            M12 = value2.M12 * value1.M11 + value2.M22 * value1.M12 + value1.M13 + value2.M32 * value1.M14,
            M22 = value2.M12 * value1.M21 + value2.M22 * value1.M22 + value1.M13 + value2.M32 * value1.M24,
            M32 = value1.M32,
            M42 = value2.M12 * value1.M41 + value2.M22 * value1.M42 + value1.M43 + value2.M32 * value1.M44,

            M13 = value1.M13,
            M23 = value1.M23,
            M33 = value1.M33,
            M43 = value1.M43,

            M14 = value1.M14,
            M24 = value1.M24,
            M34 = value1.M34,
            M44 = value1.M44
        };

        public static Matrix4x4 Multiply(Matrix4x4 value1, Matrix3x2 value2) => new Matrix4x4
        {
            // X 123
            M11 = value1.M11 * value2.M11 + value1.M12 * value2.M21 + value1.M14 * value2.M31,
            M12 = value1.M11 * value2.M12 + value1.M12 * value2.M22 + value1.M14 * value2.M32,
            M13 = value1.M13,
            M14 = value1.M14,

            // Y 123
            M21 = value1.M21 * value2.M11 + value1.M22 * value2.M21 + value1.M24 * value2.M31,
            M22 = value1.M21 * value2.M12 + value1.M22 * value2.M22 + value1.M24 * value2.M32,
            M23 = value1.M23,
            M24 = value1.M24,

            M31 = value1.M31 * value2.M11 + value1.M32 * value2.M21 + value1.M34 * value2.M31,
            M32 = value1.M31 * value2.M12 + value1.M32 * value2.M22 + value1.M34 * value2.M32,
            M33 = value1.M33,
            M34 = value1.M34,

            M41 = value1.M41 * value2.M11 + value1.M42 * value2.M21 + value1.M44 * value2.M31,
            M42 = value1.M41 * value2.M12 + value1.M42 * value2.M22 + value1.M44 * value2.M32,
            M43 = value1.M43,
            M44 = value1.M44
        };
    }
}