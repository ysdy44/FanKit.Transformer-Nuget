using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer.Mathematics
{
    public readonly struct InvertiblePerspSizeMatrix3x3
    {
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

        public InvertiblePerspSizeMatrix3x3(Quadrilateral source, float destinationWidth, float destinationHeight)
        {
            dst = new InvertibleSparseMatrix3x3(source);

            m11 = dst.x.M0 * destinationWidth;
            m12 = dst.x.M3 * destinationHeight;
            m14 = dst.x.M6;

            m21 = dst.x.M1 * destinationWidth;
            m22 = dst.x.M4 * destinationHeight;
            m24 = dst.x.M7;

            m41 = dst.x.M2 * destinationWidth;
            m42 = dst.x.M5 * destinationHeight;
        }

        public static implicit operator Matrix4x4(InvertiblePerspSizeMatrix3x3 matrix)
        {
            return new Matrix4x4
            {
                // First row
                M11 = matrix.m11,
                M12 = matrix.m12,
                M13 = 0f,
                M14 = matrix.m14,

                // Second row
                M21 = matrix.m21,
                M22 = matrix.m22,
                M23 = 0f,
                M24 = matrix.m24,

                // Third row
                M31 = 0f,
                M32 = 0f,
                M33 = 1f,
                M34 = 0f,

                // Fourth row
                M41 = matrix.m41,
                M42 = matrix.m42,
                M43 = 0f,
                M44 = 1f,
            };
        }
    }
}