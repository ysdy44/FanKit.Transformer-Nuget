using System.Numerics;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace FanKit.Transformer
{
    partial struct Matrix2x2
    {
        public float TranslateX;
        public float TranslateY;
        public float ScaleX;
        public float ScaleY;

        #region Constructors
        public Matrix2x2(float scaleX, float scaleY, float translateX, float translateY)
        {
            this.ScaleX = scaleX;
            this.ScaleY = scaleY;
            this.TranslateX = translateX;
            this.TranslateY = translateY;
        }
        #endregion Constructors

        #region Public Static Operators
        public static Matrix2x2 operator *(Matrix2x2 left, Matrix2x2 right) => new Matrix2x2
        {
            // First row
            ScaleX = left.ScaleX * right.ScaleX,
            ScaleY = left.ScaleY * right.ScaleY,

            // Second row
            TranslateX = left.TranslateX * right.ScaleX + right.TranslateX,
            TranslateY = left.TranslateY * right.ScaleY + right.TranslateY
        };
        #endregion Public Static Operators
    }
}