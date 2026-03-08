using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer.Mathematics
{
    partial class Math
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeMatrix Normalize(float sourceWidth, float sourceHeight) => new SizeMatrix(sourceWidth, sourceHeight);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 ToIdentity(float sourceX, float sourceY, float sourceWidth, float sourceHeight) => new Vector4
        {
            X = 1f / sourceWidth,
            Y = 1f / sourceHeight,
            Z = -sourceX / sourceWidth,
            W = -sourceY / sourceHeight,
        };

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
        public static InvertibleMatrix3x2 ToInvertibleMatrix(this Triangle triangle) => new InvertibleMatrix3x2(triangle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static InvertibleMatrix3x2 ToInvertibleMatrix(this Quadrilateral quad) => new InvertibleMatrix3x2(quad);

        // Old Method
        public static Matrix3x2 FindHomography(Quadrilateral source, Quadrilateral dest)
        {
            InvertibleMatrix3x2 src = source.ToInvertibleMatrix();
            Matrix3x2 dstNorm = dest.Normalize();

            return src.BidiAffine(dstNorm);
        }

        // New Method
        public static Matrix3x2 FindHomography(Rectangle source, Quadrilateral dest)
        {
            RectSource src = new RectSource(source);
            Matrix3x2 dstNorm = dest.Normalize();

            return src.Affine(dstNorm);
        }

        // New Method
        public static Matrix3x2 FindHomography(float sourceX, float sourceY, float sourceWidth, float sourceHeight, Quadrilateral dest)
        {
            RectSource src = new RectSource(sourceX, sourceY, sourceWidth, sourceHeight);
            Matrix3x2 dstNorm = dest.Normalize();

            return src.Affine(dstNorm);
        }

        // New Method
        public static Matrix3x2 FindHomography(float sourceWidth, float sourceHeight, Quadrilateral dest)
        {
            SizeMatrix srcNorm = new SizeMatrix(sourceWidth, sourceHeight);
            Matrix3x2 dstNorm = dest.Normalize();

            return Affine(srcNorm, dstNorm);
        }

        // Old Method
        public static Matrix4x4 FindHomography3D(Rectangle source, Quadrilateral dest)
        {
            RectSource src = new RectSource(source);
            PerspRectMatrix3x3 dst = src.ToPerspMatrix(dest);

            return dst;
        }

        // Old Method
        public static Matrix4x4 FindHomography3D(float sourceX, float sourceY, float sourceWidth, float sourceHeight, Quadrilateral dest)
        {
            RectSource src = new RectSource(sourceX, sourceY, sourceWidth, sourceHeight);
            PerspRectMatrix3x3 dst = src.ToPerspMatrix(dest);

            return dst;
        }

        // New Method
        public static Matrix4x4 FindHomography3D(float sourceWidth, float sourceHeight, Quadrilateral dest)
        {
            SizeMatrix srcNorm = new SizeMatrix(sourceWidth, sourceHeight);
            PerspSizeMatrix3x3 dst = new PerspSizeMatrix3x3(srcNorm, dest);

            return dst;
        }
    }
}