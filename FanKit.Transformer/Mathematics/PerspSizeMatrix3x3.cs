using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer.Mathematics
{
    public readonly struct PerspSizeMatrix3x3
    {
        internal readonly SparseMatrix3x3 dst;

        // First row
        internal readonly float m11;
        internal readonly float m14;

        // Second row
        internal readonly float m22;
        internal readonly float m24;

        #region Constructors
        internal PerspSizeMatrix3x3(SizeMatrix sourceNormalize, Quadrilateral destination)
        {
            dst = new SparseMatrix3x3(destination);

            // First row
            m11 = sourceNormalize.X * dst.sx;
            m14 = sourceNormalize.X * dst.rx;

            // Second row
            m22 = sourceNormalize.Y * dst.sy;
            m24 = sourceNormalize.Y * dst.ry;
        }
        #endregion Constructors

        #region Public Static Methods
        #endregion Public Static Methods

        #region Public operator methods
        #endregion Public operator methods

        #region Public Static Operators

        // -------------------- 1x2_3x3 -------------------- // 

        public static implicit operator Matrix4x4(PerspSizeMatrix3x3 matrix)
        {
            return new Matrix4x4
            {
                // First row
                M11 = matrix.m11 * matrix.dst.mat.M11 + matrix.m14 * matrix.dst.mat.M31,
                M12 = matrix.m11 * matrix.dst.mat.M12 + matrix.m14 * matrix.dst.mat.M32,
                M13 = 0f,
                M14 = matrix.m14,

                // Second row
                M21 = matrix.m22 * matrix.dst.mat.M21 + matrix.m24 * matrix.dst.mat.M31,
                M22 = matrix.m22 * matrix.dst.mat.M22 + matrix.m24 * matrix.dst.mat.M32,
                M23 = 0f,
                M24 = matrix.m24,

                // Third row
                M31 = 0f,
                M32 = 0f,
                M33 = 1f,
                M34 = 0f,

                // Fourth row
                M41 = matrix.dst.mat.M31,
                M42 = matrix.dst.mat.M32,
                M43 = 0f,
                M44 = 1f
            };
        }
        #endregion Public Static Operators
    }
}