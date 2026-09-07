using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer.Mathematics
{
    public readonly struct InvertiblePerspQuadrilateralMatrix3x3
    {
        const int m44 = 1;

        readonly InvertibleSparseMatrix3x3 dst;

        // Normalize
        readonly float m11;
        readonly float m12;
        readonly float m14;

        readonly float m21;
        readonly float m22;
        readonly float m24;

        readonly float m41;
        readonly float m42;

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

        // First row
        readonly float l11;
        readonly float l12;
        readonly float l14;

        // Second row
        readonly float l21;
        readonly float l22;
        readonly float l24;

        // Third row

        // Fourth row
        readonly float l41;
        readonly float l42;
        readonly float l44;

        #region Constructors
        public InvertiblePerspQuadrilateralMatrix3x3(Quadrilateral source, QuadMatrix destinationNormalize)
        {
            dst = new InvertibleSparseMatrix3x3(source);

            m11 = dst.x.M0;
            m12 = dst.x.M3;
            m14 = dst.x.M6;

            m21 = dst.x.M1;
            m22 = dst.x.M4;
            m24 = dst.x.M7;

            m41 = dst.x.M2;
            m42 = dst.x.M5;

            var mat = destinationNormalize;

            // First row
            n11 = mat.sx * mat.mat.M11 + mat.rx * mat.mat.M31;
            n12 = mat.sx * mat.mat.M12 + mat.rx * mat.mat.M32;

            // Second row
            n21 = mat.sy * mat.mat.M21 + mat.ry * mat.mat.M31;
            n22 = mat.sy * mat.mat.M22 + mat.ry * mat.mat.M32;

            // Third row

            // Fourth row
            n41 = mat.mat.M31;
            n42 = mat.mat.M32;

            // First row
            l11 = m11 * n11 + m12 * n21 + m14 * n41;
            l12 = m11 * n12 + m12 * n22 + m14 * n42;
            l14 = m11 * mat.rx + m12 * mat.ry + m14;

            // Second row
            l21 = m21 * n11 + m22 * n21 + m24 * n41;
            l22 = m21 * n12 + m22 * n22 + m24 * n42;
            l24 = m21 * mat.rx + m22 * mat.ry + m24;

            // Third row

            // Fourth row
            l41 = m41 * n11 + m42 * n21 + m44 * n41;
            l42 = m41 * n12 + m42 * n22 + m44 * n42;
            l44 = m41 * mat.rx + m42 * mat.ry + m44;
        }
        #endregion Constructors

        #region Public Static Methods
        #endregion Public Static Methods

        #region Public operator methods
        #endregion Public operator methods

        #region Public Static Operators

        // -------------------- 3x3_3x3 -------------------- // 

        public static implicit operator Matrix4x4(InvertiblePerspQuadrilateralMatrix3x3 matrix)
        {
            return new Matrix4x4
            {
                // First row
                M11 = matrix.l11,
                M12 = matrix.l12,
                M13 = 0f,
                M14 = matrix.l14,

                // Second row
                M21 = matrix.l21,
                M22 = matrix.l22,
                M23 = 0f,
                M24 = matrix.l24,

                // Third row
                M31 = 0f,
                M32 = 0f,
                M33 = 1f,
                M34 = 0f,

                // Fourth row
                M41 = matrix.l41,
                M42 = matrix.l42,
                M43 = 0f,
                M44 = matrix.l44,
            };
        }
        #endregion Public Static Operators
    }
}