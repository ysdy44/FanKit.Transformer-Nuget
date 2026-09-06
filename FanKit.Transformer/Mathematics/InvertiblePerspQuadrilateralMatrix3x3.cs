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

        // Quadrilateral
        readonly SparseMatrix3x3 mat;

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

        public InvertiblePerspQuadrilateralMatrix3x3(Quadrilateral source, Quadrilateral destination)
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

            mat = new SparseMatrix3x3(destination);

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
        }

        public static implicit operator Matrix4x4(InvertiblePerspQuadrilateralMatrix3x3 matrix)
        {
            return new Matrix4x4
            {
                // First row
                M11 = matrix.m11 * matrix.n11 + matrix.m12 * matrix.n21 + matrix.m14 * matrix.n41,
                M12 = matrix.m11 * matrix.n12 + matrix.m12 * matrix.n22 + matrix.m14 * matrix.n42,
                M13 = 0f,
                M14 = matrix.m11 * matrix.mat.rx + matrix.m12 * matrix.mat.ry + matrix.m14,

                // Second row
                M21 = matrix.m21 * matrix.n11 + matrix.m22 * matrix.n21 + matrix.m24 * matrix.n41,
                M22 = matrix.m21 * matrix.n12 + matrix.m22 * matrix.n22 + matrix.m24 * matrix.n42,
                M23 = 0f,
                M24 = matrix.m21 * matrix.mat.rx + matrix.m22 * matrix.mat.ry + matrix.m24,

                // Third row
                M31 = 0f,
                M32 = 0f,
                M33 = 1f,
                M34 = 0f,

                // Fourth row
                M41 = matrix.m41 * matrix.n11 + matrix.m42 * matrix.n21 + m44 * matrix.n41,
                M42 = matrix.m41 * matrix.n12 + matrix.m42 * matrix.n22 + m44 * matrix.n42,
                M43 = 0f,
                M44 = matrix.m41 * matrix.mat.rx + matrix.m42 * matrix.mat.ry + m44,
            };
        }
    }
}