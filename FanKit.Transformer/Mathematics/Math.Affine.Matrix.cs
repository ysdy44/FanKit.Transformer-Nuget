using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer.Mathematics
{
    partial class Math
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeMatrix Normalize(float sourceWidth, float sourceHeight) => new SizeMatrix(sourceWidth, sourceHeight);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectMatrix Normalize(float sourceX, float sourceY, float sourceWidth, float sourceHeight) => new RectMatrix(sourceX, sourceY, sourceWidth, sourceHeight);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectMatrix Normalize(this Rectangle rect) => new RectMatrix(rect);

        public static Matrix4x4 ToMatrix3x3(this Matrix3x2 source) => new Matrix4x4
        {
            M11 = source.M11,
            M21 = source.M21,
            M31 = 0f,
            M41 = source.M31,

            M12 = source.M12,
            M22 = source.M22,
            M32 = 0f,
            M42 = source.M32,

            M13 = 0f,
            M23 = 0f,
            M33 = 1f,
            M43 = 0f,

            M14 = 0f,
            M24 = 0f,
            M34 = 0f,
            M44 = 1f
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TriangleMatrix ToInvertibleMatrix(this Triangle triangle) => new TriangleMatrix(triangle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TriangleMatrix ToInvertibleMatrix(this Quadrilateral quad) => new TriangleMatrix(quad);

        // New Method
        public static Matrix3x2 FindHomography(float sourceWidth, float sourceHeight, Quadrilateral destination)
        {
            SizeMatrix srcNorm = new SizeMatrix(sourceWidth, sourceHeight);
            Matrix3x2 dstNorm = destination.Norm();

            return Affine(srcNorm, dstNorm);
        }

        // New Method
        public static Matrix3x2 FindHomography(float sourceX, float sourceY, float sourceWidth, float sourceHeight, Quadrilateral destination)
        {
            RectMatrix srcNorm = new RectMatrix(sourceX, sourceY, sourceWidth, sourceHeight);
            Matrix3x2 dstNorm = destination.Norm();

            return srcNorm.Affine(dstNorm);
        }

        // New Method
        public static Matrix3x2 FindHomography(Rectangle source, Quadrilateral destination)
        {
            RectMatrix srcNorm = new RectMatrix(source);
            Matrix3x2 dstNorm = destination.Norm();

            return Affine(srcNorm, dstNorm);
        }

        // Old Method
        public static Matrix3x2 FindHomography(Quadrilateral source, Quadrilateral destination)
        {
            TriangleMatrix src = source.ToInvertibleMatrix();
            Matrix3x2 dstNorm = destination.Norm();

            return src.BidiAffine(dstNorm);
        }

        // New Method
        public static Matrix4x4 FindHomography3D(float sourceWidth, float sourceHeight, Quadrilateral destination)
        {
            SizeMatrix srcNorm = new SizeMatrix(sourceWidth, sourceHeight);
            Matrix4x4 destNorm = destination.Normalize();
            Matrix4x4 dst = srcNorm.Persp(destNorm);

            return dst;
        }

        // Old Method
        public static Matrix4x4 FindHomography3D(float sourceX, float sourceY, float sourceWidth, float sourceHeight, Quadrilateral destination)
        {
            RectMatrix srcNorm = new RectMatrix(sourceX, sourceY, sourceWidth, sourceHeight);
            Matrix4x4 destNorm = destination.Normalize();
            Matrix4x4 dst = srcNorm.Persp(destNorm);

            return dst;
        }

        // Old Method
        public static Matrix4x4 FindHomography3D(Rectangle source, Quadrilateral destination)
        {
            RectMatrix srcNorm = new RectMatrix(source);
            Matrix4x4 destNorm = destination.Normalize();
            Matrix4x4 dst = srcNorm.Persp(destNorm);

            return dst;
        }
    }
}