using System.Numerics;

namespace FanKit.Transformer.Mathematics
{
    internal class PerspQuadrilateral : PerspMatrix
    {
        // Normalize
        float m11;
        float m12;
        float m14;

        float m21;
        float m22;
        float m24;

        float m41;
        float m42;

        // Quadrilateral
        SparseMatrix3x3 mat;

        // First row
        float n11;
        float n12;

        // Second row
        float n21;
        float n22;

        // Third row

        // Fourth row
        float n41;
        float n42;

        // Result
        internal Matrix4x4 m;

        internal void FindHomography(Quadrilateral src, Quadrilateral dst)
        {
            Normalize(src);

            const int m44 = 1;

            m11 = x[0];
            m12 = x[3];
            m14 = x[6];

            m21 = x[1];
            m22 = x[4];
            m24 = x[7];

            m41 = x[2];
            m42 = x[5];

            mat = new SparseMatrix3x3(dst);

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

            m = new Matrix4x4
            {
                // First row
                M11 = m11 * n11 + m12 * n21 + m14 * n41,
                M12 = m11 * n12 + m12 * n22 + m14 * n42,
                M13 = 0f,
                M14 = m11 * mat.rx + m12 * mat.ry + m14,

                // Second row
                M21 = m21 * n11 + m22 * n21 + m24 * n41,
                M22 = m21 * n12 + m22 * n22 + m24 * n42,
                M23 = 0f,
                M24 = m21 * mat.rx + m22 * mat.ry + m24,

                // Third row
                M31 = 0f,
                M32 = 0f,
                M33 = 1f,
                M34 = 0f,

                // Fourth row
                M41 = m41 * n11 + m42 * n21 + m44 * n41,
                M42 = m41 * n12 + m42 * n22 + m44 * n42,
                M43 = 0f,
                M44 = m41 * mat.rx + m42 * mat.ry + m44,
            };
        }
    }
}