using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer.Mathematics
{
    public readonly struct PerspRectMatrix3x3
    {
        internal readonly SparseMatrix3x3 dst;

        // First row
        internal readonly float m11;
        internal readonly float m14;

        // Second row
        internal readonly float m22;
        internal readonly float m24;

        // Fourth row
        internal readonly float m41;
        internal readonly float m42;
        internal readonly float m44;

        #region Constructors
        public PerspRectMatrix3x3(RectMatrix srcNorm, Quadrilateral quad)
        {
            dst = new SparseMatrix3x3(quad);

            // First row
            m11 = srcNorm.X * dst.sx;
            m14 = srcNorm.X * dst.rx;

            // Second row
            m22 = srcNorm.Y * dst.sy;
            m24 = srcNorm.Y * dst.ry;

            // Fourth row
            m41 = srcNorm.Z * dst.sx;
            m42 = srcNorm.W * dst.sy;
            m44 = srcNorm.Z * dst.rx + srcNorm.W * dst.ry + 1f;
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
                M41 = matrix.m41 * matrix.dst.mat.M11 + matrix.m42 * matrix.dst.mat.M21 + matrix.m44 * matrix.dst.mat.M31,
                M42 = matrix.m41 * matrix.dst.mat.M12 + matrix.m42 * matrix.dst.mat.M22 + matrix.m44 * matrix.dst.mat.M32,
                M43 = 0f,
                M44 = matrix.m44
            };
        }
        #endregion Public Static Operators
    }
}