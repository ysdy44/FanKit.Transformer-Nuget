using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer.Mathematics
{
    partial class Math
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Transform(Vector2 position, Matrix2x2 matrix)
        {
            return new Vector2
            {
                X = matrix.ScaleX * position.X + matrix.TranslateX,
                Y = matrix.ScaleY * position.Y + matrix.TranslateY,
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Transform(float xPosition, float yPosition, Matrix2x2 matrix)
        {
            return new Vector2
            {
                X = matrix.ScaleX * xPosition + matrix.TranslateX,
                Y = matrix.ScaleY * yPosition + matrix.TranslateY,
            };
        }

        // -------------------- 1x2_3x2 -------------------- //

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix2x2 TransformTranslation(Vector2 translate, Matrix2x2 matrix)
        {
            return new Matrix2x2
            {
                // First row
                ScaleX = matrix.ScaleX,
                ScaleY = matrix.ScaleY,

                // Second row
                TranslateX = matrix.ScaleX * translate.X + matrix.TranslateX,
                TranslateY = matrix.ScaleY * translate.Y + matrix.TranslateY,
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix2x2 TransformTranslation(float translateX, float translateY, Matrix2x2 matrix)
        {
            return new Matrix2x2
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
}