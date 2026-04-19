using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer.Mathematics
{
    // 1x2
    public struct SizeMatrix
    {
        public float X;
        public float Y;

        #region Constructors
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SizeMatrix(float width, float height)
        {
            X = 1f / width;
            Y = 1f / height;
        }
        #endregion Constructors

        #region Public instance methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PerspSizeMatrix3x3 ToPerspMatrix(Quadrilateral quad) => new PerspSizeMatrix3x3(this, quad);

        // -------------------- 1x2_2x2 -------------------- // 

        public Matrix2x2 Map(Rectangle destination) => new Matrix2x2
        {
            // First row
            ScaleX = X * destination.Width,

            // Second row
            ScaleY = Y * destination.Height,

            // Third row
            TranslateX = destination.X,
            TranslateY = destination.Y
        };

        public Matrix2x2 Map(Matrix2x2 destination) => new Matrix2x2
        {
            // First row
            ScaleX = X * destination.ScaleX,

            // Second row
            ScaleY = Y * destination.ScaleY,

            // Third row
            TranslateX = destination.TranslateX,
            TranslateY = destination.TranslateY
        };

        public Matrix3x2 Map(float destinationX, float destinationY, float destinationWidth, float destinationHeight) => new Matrix3x2
        {
            // First row
            M11 = X * destinationWidth,
            M12 = 0f,

            // Second row
            M21 = 0f,
            M22 = Y * destinationHeight,

            // Third row
            M31 = destinationX,
            M32 = destinationY
        };

        // -------------------- 1x2_3x2 -------------------- // 

        public Matrix3x2 Affine(Matrix3x2 destinationNormalize) => new Matrix3x2
        {
            // First row
            M11 = X * destinationNormalize.M11,
            M12 = X * destinationNormalize.M12,

            // Second row
            M21 = Y * destinationNormalize.M21,
            M22 = Y * destinationNormalize.M22,

            // Third row
            M31 = destinationNormalize.M31,
            M32 = destinationNormalize.M32
        };
        #endregion Public instance methods

        #region Public Static Methods
        #endregion Public Static Methods

        #region Public Static Operators
        #endregion Public Static Operators
    }
}