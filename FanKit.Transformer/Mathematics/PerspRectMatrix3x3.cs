using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer.Mathematics
{
    public readonly struct PerspRectMatrix3x3
    {
        // First row
        readonly float m11;
        readonly float m14;

        // Second row
        readonly float m22;
        readonly float m24;

        // Fourth row
        readonly float m41;
        readonly float m42;
        readonly float m44;

        // First row
        readonly float n11;
        readonly float n12;

        // Second row
        readonly float n21;
        readonly float n22;

        // Third row

        // Fourth row
        readonly float n41;
        readonly float n42;

        #region Constructors
        public PerspRectMatrix3x3(RectMatrix sourceNormalize, QuadMatrix destinationNormalize)
        {
            var dst = destinationNormalize;

            // First row
            m11 = sourceNormalize.X * dst.sx;
            m14 = sourceNormalize.X * dst.rx;

            // Second row
            m22 = sourceNormalize.Y * dst.sy;
            m24 = sourceNormalize.Y * dst.ry;

            // Fourth row
            m41 = sourceNormalize.Z * dst.sx;
            m42 = sourceNormalize.W * dst.sy;
            m44 = sourceNormalize.Z * dst.rx + sourceNormalize.W * dst.ry + 1f;

            // First row
            n11 = m11 * dst.mat.M11 + m14 * dst.mat.M31;
            n12 = m11 * dst.mat.M12 + m14 * dst.mat.M32;

            // Second row
            n21 = m22 * dst.mat.M21 + m24 * dst.mat.M31;
            n22 = m22 * dst.mat.M22 + m24 * dst.mat.M32;

            // Third row

            // Fourth row
            n41 = m41 * dst.mat.M11 + m42 * dst.mat.M21 + m44 * dst.mat.M31;
            n42 = m41 * dst.mat.M12 + m42 * dst.mat.M22 + m44 * dst.mat.M32;
        }
        #endregion Constructors

        #region Public Static Methods
        #endregion Public Static Methods

        #region Public operator methods
        #endregion Public operator methods

        #region Public Static Operators
        // -------------------- 2x2_3x3 -------------------- // 

        public static implicit operator Matrix4x4(PerspRectMatrix3x3 matrix)
        {
            return new Matrix4x4
            {
                // First row
                M11 = matrix.n11,
                M12 = matrix.n12,
                M13 = 0f,
                M14 = matrix.m14,

                // Second row
                M21 = matrix.n21,
                M22 = matrix.n22,
                M23 = 0f,
                M24 = matrix.m24,

                // Third row
                M31 = 0f,
                M32 = 0f,
                M33 = 1f,
                M34 = 0f,

                // Fourth row
                M41 = matrix.n41,
                M42 = matrix.n42,
                M43 = 0f,
                M44 = matrix.m44
            };
        }
        #endregion Public Static Operators
    }
}