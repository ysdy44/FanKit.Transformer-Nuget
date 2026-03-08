using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer.Mathematics
{
    public readonly struct InvertibleMatrix3x2
    {
        readonly Matrix3x2 mat; // Matrix
        internal readonly bool can; // Invertible
        internal readonly Matrix3x2 inv; // InverseMatrix

        #region Constructors
        public InvertibleMatrix3x2(Matrix3x2 matrix)
        {
            mat = matrix;
            can = Matrix3x2.Invert(mat, out inv);
        }

        public InvertibleMatrix3x2(Triangle triangle)
        {
            mat = triangle.Normalize();
            can = Matrix3x2.Invert(mat, out inv);
        }

        public InvertibleMatrix3x2(Quadrilateral quad)
        {
            mat = quad.Normalize();
            can = Matrix3x2.Invert(mat, out inv);
        }
        #endregion Constructors

        #region Public Static Methods
        #endregion Public Static Methods

        #region Public Static Operators
        #endregion Public Static Operators

        #region Public operator methods
        #endregion Public operator methods

        // -------------------- 3x2_1x2 -------------------- // 

        public Matrix3x2 InvAffine(float destWidth, float destHeight)
            => can ? Math.Scale(inv, destWidth, destHeight) : Math.Affine(destWidth, destHeight);

        // -------------------- 3x2_2x2 -------------------- //

        public Matrix3x2 InvAffine(Rectangle destRect)
            => can ? Math.Transform(inv, destRect) : destRect.ToMatrix3x2();

        public Matrix3x2 InvAffine(Matrix2x2 destNorm)
            => can ? Math.Transform(inv, destNorm) : destNorm.ToMatrix3x2();

        // -------------------- 3x2_3x2 -------------------- // 

        public Matrix3x2 BidiAffine(Matrix3x2 destNorm)
            => can ? inv * destNorm : destNorm;
    }
}