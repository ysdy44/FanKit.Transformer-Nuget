using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer.Mathematics
{
    // 1x2
    public readonly struct SizeSource
    {
        public readonly float Width;
        public readonly float Height;

        public readonly float X;
        public readonly float Y;

        #region Constructors
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SizeSource(float width, float height)
        {
            Width = width;
            Height = height;

            X = 1f / Width;
            Y = 1f / Height;
        }
        #endregion Constructors

        #region Public instance methods
        #endregion Public instance methods

        #region Public Static Methods
        #endregion Public Static Methods

        #region Public Static Operators
        #endregion Public Static Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PerspSizeMatrix3x3 ToPerspMatrix(Quadrilateral quad) => new PerspSizeMatrix3x3(this, quad);

        // -------------------- 1x2_2x2 -------------------- // 

        public Matrix2x2 Map(Rectangle dest) => new Matrix2x2
        {
            // First row
            ScaleX = X * dest.Width,

            // Second row
            ScaleY = Y * dest.Height,

            // Third row
            TranslateX = dest.X,
            TranslateY = dest.Y
        };

        public Matrix2x2 Map(Matrix2x2 dest) => new Matrix2x2
        {
            // First row
            ScaleX = X * dest.ScaleX,

            // Second row
            ScaleY = Y * dest.ScaleY,

            // Third row
            TranslateX = dest.TranslateX,
            TranslateY = dest.TranslateY
        };

        public Matrix3x2 Map(float destX, float destY, float destWidth, float destHeight) => new Matrix3x2
        {
            // First row
            M11 = X * destWidth,
            M12 = 0f,

            // Second row
            M21 = 0f,
            M22 = Y * destHeight,

            // Third row
            M31 = destX,
            M32 = destY
        };

        // -------------------- 1x2_3x2 -------------------- // 

        public Matrix3x2 Affine(Matrix3x2 destNorm) => new Matrix3x2
        {
            // First row
            M11 = X * destNorm.M11,
            M12 = X * destNorm.M12,

            // Second row
            M21 = Y * destNorm.M21,
            M22 = Y * destNorm.M22,

            // Third row
            M31 = destNorm.M31,
            M32 = destNorm.M32
        };
    }
}