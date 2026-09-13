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

        private Matrix4x4 P(QuadMatrix dst)
        {
            // First row
            float m11 = this.X * dst.sx;
            float m14 = this.X * dst.rx;

            // Second row
            float m22 = this.Y * dst.sy;
            float m24 = this.Y * dst.ry;

            // First row
            float n11 = m11 * dst.mat.M11 + m14 * dst.mat.M31;
            float n12 = m11 * dst.mat.M12 + m14 * dst.mat.M32;

            // Second row
            float n21 = m22 * dst.mat.M21 + m24 * dst.mat.M31;
            float n22 = m22 * dst.mat.M22 + m24 * dst.mat.M32;

            // Third row

            // Fourth row
            float n41 = dst.mat.M31;
            float n42 = dst.mat.M32;

            return new Matrix4x4
            {
                // First row
                M11 = n11,
                M12 = n12,
                M13 = 0f,
                M14 = m14,

                // Second row
                M21 = n21,
                M22 = n22,
                M23 = 0f,
                M24 = m24,

                // Third row
                M31 = 0f,
                M32 = 0f,
                M33 = 1f,
                M34 = 0f,

                // Fourth row
                M41 = n41,
                M42 = n42,
                M43 = 0f,
                M44 = 1f
            };
        }

        // -------------------- 1x2_2x2 -------------------- //

        public Matrix2x2 Map(Matrix2x2 destinationNormalize) => new Matrix2x2
        {
            // First row
            ScaleX = X * destinationNormalize.ScaleX,

            // Second row
            ScaleY = Y * destinationNormalize.ScaleY,

            // Third row
            TranslateX = destinationNormalize.TranslateX,
            TranslateY = destinationNormalize.TranslateY
        };

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

        // -------------------- 1x2_3x3 -------------------- //

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix4x4 Persp(QuadMatrix destinationNormalize) => P(destinationNormalize);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix4x4 Persp(Quadrilateral destination) => P(new QuadMatrix(destination));

        #endregion Public instance methods

        #region Public Static Methods
        #endregion Public Static Methods

        #region Public Static Operators
        #endregion Public Static Operators
    }
}