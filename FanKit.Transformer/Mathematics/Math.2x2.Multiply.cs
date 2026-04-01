using System.Numerics;
using System.Security.Principal;

namespace FanKit.Transformer.Mathematics
{
    partial class Math
    {
        public static Vector2 Transform(Vector2 position, Matrix2x2 matrix) => new Vector2
        {
            X = matrix.ScaleX * position.X + matrix.TranslateX,
            Y = matrix.ScaleY * position.Y + matrix.TranslateY,
        };

        public static Vector2 Transform(float x, float y, Matrix2x2 matrix) => new Vector2
        {
            X = matrix.ScaleX * x + matrix.TranslateX,
            Y = matrix.ScaleY * y + matrix.TranslateY,
        };

        public static Matrix2x2 Transform(Matrix2x2 value1, Matrix2x2 value2) => value1 * value2;

        // -------------------- 1x2_3x2 -------------------- // 

        public static Matrix2x2 TransformTranslation(Vector2 translate, Matrix2x2 matrix) => new Matrix2x2
        {
            // First row
            ScaleX = matrix.ScaleX,
            ScaleY = matrix.ScaleY,

            // Second row
            TranslateX = matrix.ScaleX * translate.X + matrix.TranslateX,
            TranslateY = matrix.ScaleY * translate.Y + matrix.TranslateY,
        };

        public static Matrix2x2 TransformTranslation(float translateX, float translateY, Matrix2x2 matrix) => new Matrix2x2
        {
            // First row
            ScaleX = matrix.ScaleX,
            ScaleY = matrix.ScaleY,

            // Second row
            TranslateX = matrix.ScaleX * translateX + matrix.TranslateX,
            TranslateY = matrix.ScaleY * translateY + matrix.TranslateY,
        };
    }
}