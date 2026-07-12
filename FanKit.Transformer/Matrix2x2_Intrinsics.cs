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

        #region Public Instance Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Matrix2x2 other)
        {
            return
                this.TranslateX == other.TranslateX &&
                this.TranslateY == other.TranslateY &&
                this.ScaleX == other.ScaleX &&
                this.ScaleY == other.ScaleY;
        }
        #endregion Public Instance Methods

        #region Public Static Operators
        public static Matrix2x2 operator +(Matrix2x2 left, Vector2 right) => new Matrix2x2
        {
            TranslateX = left.TranslateX + right.X,
            TranslateY = left.TranslateY + right.Y,
            ScaleX = left.ScaleX,
            ScaleY = left.ScaleY,
        };

        public static Matrix2x2 operator +(Vector2 left, Matrix2x2 right) => new Matrix2x2
        {
            TranslateX = left.X + right.TranslateX,
            TranslateY = left.Y + right.TranslateY,
            ScaleX = right.ScaleX,
            ScaleY = right.ScaleY,
        };

        public static Matrix2x2 operator -(Matrix2x2 left, Vector2 right) => new Matrix2x2
        {
            TranslateX = left.TranslateX - right.X,
            TranslateY = left.TranslateY - right.Y,
            ScaleX = left.ScaleX,
            ScaleY = left.ScaleY,
        };

        public static Matrix2x2 operator -(Vector2 left, Matrix2x2 right) => new Matrix2x2
        {
            TranslateX = left.X - right.TranslateX,
            TranslateY = left.Y - right.TranslateY,
            ScaleX = right.ScaleX,
            ScaleY = right.ScaleY,
        };

        public static Matrix2x2 operator -(Matrix2x2 left, Matrix2x2 right) => new Matrix2x2
        {
            TranslateX = left.TranslateX - right.TranslateX,
            TranslateY = left.TranslateY - right.TranslateY,
            ScaleX = left.ScaleX - right.ScaleX,
            ScaleY = left.ScaleY - right.ScaleY,
        };

        public static Matrix2x2 operator *(Matrix2x2 left, Matrix2x2 right) => new Matrix2x2
        {
            // First row
            ScaleX = left.ScaleX * right.ScaleX,
            ScaleY = left.ScaleY * right.ScaleY,

            // Second row
            TranslateX = left.TranslateX * right.ScaleX + right.TranslateX,
            TranslateY = left.TranslateY * right.ScaleY + right.TranslateY
        };

        public static Matrix2x2 operator *(Matrix2x2 left, Vector2 right) => new Matrix2x2
        {
            TranslateX = left.TranslateX,
            TranslateY = left.TranslateY,
            ScaleX = left.ScaleX * right.X,
            ScaleY = left.ScaleY * right.Y,
        };

        public static Matrix2x2 operator *(Matrix2x2 left, float right) => new Matrix2x2
        {
            TranslateX = left.TranslateX,
            TranslateY = left.TranslateY,
            ScaleX = left.ScaleX * right,
            ScaleY = left.ScaleY * right,
        };

        public static Matrix2x2 operator *(float left, Matrix2x2 right) => new Matrix2x2
        {
            TranslateX = right.TranslateX,
            TranslateY = right.TranslateY,
            ScaleX = left * right.ScaleX,
            ScaleY = left * right.ScaleY,
        };

        public static Matrix2x2 operator /(Matrix2x2 left, Vector2 right) => new Matrix2x2
        {
            TranslateX = left.TranslateX,
            TranslateY = left.TranslateY,
            ScaleX = left.ScaleX / right.X,
            ScaleY = left.ScaleY / right.Y,
        };

        public static Matrix2x2 operator /(Matrix2x2 left, float right) => new Matrix2x2
        {
            TranslateX = left.TranslateX,
            TranslateY = left.TranslateY,
            ScaleX = left.ScaleX / right,
            ScaleY = left.ScaleY / right,
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix2x2 operator -(Matrix2x2 value) => new Matrix2x2
        {
            TranslateX = -value.TranslateX,
            TranslateY = -value.TranslateY,
            ScaleX = -value.ScaleX,
            ScaleY = -value.ScaleY,
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Matrix2x2 left, Matrix2x2 right)
        {
            return left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Matrix2x2 left, Matrix2x2 right)
        {
            return !(left == right);
        }
        #endregion Public Static Operators
    }
}