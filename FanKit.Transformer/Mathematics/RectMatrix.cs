using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer.Mathematics
{
    // 2x2
    public struct RectMatrix
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        #region Constructors
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RectMatrix(float x, float y, float width, float height)
        {
            X = 1f / width;
            Y = 1f / height;
            Z = -x / width;
            W = -y / height;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RectMatrix(Rectangle rect)
        {
            X = 1f / rect.Width;
            Y = 1f / rect.Height;
            Z = -rect.X / rect.Width;
            W = -rect.Y / rect.Height;
        }
        #endregion Constructors

        #region Public instance methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PerspRectMatrix3x3 ToPerspMatrix(Quadrilateral quad) => new PerspRectMatrix3x3(this, quad);

        // -------------------- 2x2_2x2 -------------------- // 

        public Matrix2x2 Map(float destinationX, float destinationY, float destinationWidth, float destinationHeight) => new Matrix2x2
        {
            // First row
            ScaleX = X * destinationWidth,
            ScaleY = Y * destinationHeight,

            // Second row
            TranslateX = destinationX + destinationWidth * Z,
            TranslateY = destinationY + destinationHeight * W,
        };

        public Matrix2x2 Map(Matrix2x2 destination) => new Matrix2x2
        {
            // First row
            ScaleX = X * destination.ScaleX,
            ScaleY = Y * destination.ScaleY,

            // Second row
            TranslateX = destination.TranslateX + destination.ScaleX * Z,
            TranslateY = destination.TranslateY + destination.ScaleY * W,
        };

        // -------------------- 2x2_3x2 -------------------- // 

        public Matrix3x2 Affine(Matrix3x2 destinationNormalize) => new Matrix3x2
        {
            // First row
            M11 = X * destinationNormalize.M11,
            M12 = X * destinationNormalize.M12,

            // Second row
            M21 = Y * destinationNormalize.M21,
            M22 = Y * destinationNormalize.M22,

            // Third row
            M31 = Z * destinationNormalize.M11 + W * destinationNormalize.M21 + destinationNormalize.M31,
            M32 = Z * destinationNormalize.M12 + W * destinationNormalize.M22 + destinationNormalize.M32
        };
        #endregion Public instance methods

        #region Public Static Methods
        #endregion Public Static Methods

        #region Public Static Operators
        #endregion Public Static Operators
    }
}