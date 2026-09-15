using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer.Mathematics
{
    public readonly struct TriangleMatrix
    {
        readonly Matrix3x2 mat; // Matrix
        internal readonly bool can; // Invertible
        internal readonly Matrix3x2 inv; // InverseMatrix

        #region Constructors
        public TriangleMatrix(Matrix3x2 matrix)
        {
            mat = matrix;
            can = Matrix3x2.Invert(mat, out inv);
        }

        public TriangleMatrix(Triangle triangle)
        {
            mat = triangle.Normalize();
            can = Matrix3x2.Invert(mat, out inv);
        }

        public TriangleMatrix(Quadrilateral quad)
        {
            mat = quad.Norm();
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

        public Matrix3x2 InvAffine(float destinationWidth, float destinationHeight)
            => can ? Math.Scale(inv, destinationWidth, destinationHeight) : Math.Affine(destinationWidth, destinationHeight);

        // -------------------- 3x2_2x2 -------------------- //

        public Matrix3x2 InvAffine(Matrix2x2 destinationNormalize)
            => can ? Math.Transform(inv, destinationNormalize) : destinationNormalize.ToMatrix3x2();

        public Matrix3x2 InvAffine(Rectangle destination)
            => can ? Math.Transform(inv, destination) : destination.ToMatrix3x2();

        // -------------------- 3x2_3x2 -------------------- // 

        public Matrix3x2 BidiAffine(Matrix3x2 destinationNormalize)
            => can ? inv * destinationNormalize : destinationNormalize;

        public Matrix3x2 BidiAffine(Triangle destination)
            => BidiAffine(destination.Normalize());
    }
}