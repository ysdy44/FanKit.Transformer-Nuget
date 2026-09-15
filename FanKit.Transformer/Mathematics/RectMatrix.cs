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

        private Matrix4x4 P(QuadMatrix dst)
        {
            // First row
            float m11 = this.X * dst.sx;
            float m14 = this.X * dst.rx;

            // Second row
            float m22 = this.Y * dst.sy;
            float m24 = this.Y * dst.ry;

            // Fourth row
            float m41 = this.Z * dst.sx;
            float m42 = this.W * dst.sy;
            float m44 = this.Z * dst.rx + this.W * dst.ry + 1f;

            // First row
            float n11 = m11 * dst.mat.M11 + m14 * dst.mat.M31;
            float n12 = m11 * dst.mat.M12 + m14 * dst.mat.M32;

            // Second row
            float n21 = m22 * dst.mat.M21 + m24 * dst.mat.M31;
            float n22 = m22 * dst.mat.M22 + m24 * dst.mat.M32;

            // Third row

            // Fourth row
            float n41 = m41 * dst.mat.M11 + m42 * dst.mat.M21 + m44 * dst.mat.M31;
            float n42 = m41 * dst.mat.M12 + m42 * dst.mat.M22 + m44 * dst.mat.M32;

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
                M44 = m44
            };
        }

        // -------------------- 2x2_2x2 -------------------- // 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix2x2 Map(Matrix2x2 destinationNormalize) => new Matrix2x2
        {
            // First row
            ScaleX = X * destinationNormalize.ScaleX,
            ScaleY = Y * destinationNormalize.ScaleY,

            // Second row
            TranslateX = destinationNormalize.TranslateX + destinationNormalize.ScaleX * Z,
            TranslateY = destinationNormalize.TranslateY + destinationNormalize.ScaleY * W,
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix2x2 Map(Rectangle destination) => new Matrix2x2
        {
            // First row
            ScaleX = X * destination.Width,
            ScaleY = Y * destination.Height,

            // Second row
            TranslateX = destination.X + destination.Width * Z,
            TranslateY = destination.Y + destination.Height * W,
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix2x2 Map(float destinationX, float destinationY, float destinationWidth, float destinationHeight) => new Matrix2x2
        {
            // First row
            ScaleX = X * destinationWidth,
            ScaleY = Y * destinationHeight,

            // Second row
            TranslateX = destinationX + destinationWidth * Z,
            TranslateY = destinationY + destinationHeight * W,
        };

        // -------------------- 2x2_3x2 -------------------- // 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix3x2 Affine(Triangle destination) => Affine(destination.Normalize());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix3x2 Affine(Quadrilateral destination) => Affine(destination.Norm());

        // -------------------- 2x2_3x3 -------------------- // 

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