using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer.Mathematics
{
    partial class Math
    {
        public static Vector2 TransformNormal(Vector2 position, Matrix4x4 matrix)
        {
            float tx = matrix.M11 * position.X + matrix.M21 * position.Y + matrix.M41;
            float ty = matrix.M12 * position.X + matrix.M22 * position.Y + matrix.M42;
            float tz = matrix.M14 * position.X + matrix.M24 * position.Y + matrix.M44;

            return new Vector2
            {
                X = tx / tz - matrix.M41 / matrix.M44,
                Y = ty / tz - matrix.M42 / matrix.M44,
            };
        }

        public static Vector2 TransformNormal(float x, float y, Matrix4x4 matrix)
        {
            float tx = matrix.M11 * x + matrix.M21 * y + matrix.M41;
            float ty = matrix.M12 * x + matrix.M22 * y + matrix.M42;
            float tz = matrix.M14 * x + matrix.M24 * y + matrix.M44;

            return new Vector2
            {
                X = tx / tz - matrix.M41 / matrix.M44,
                Y = ty / tz - matrix.M42 / matrix.M44,
            };
        }

        public static Vector2 Transform(Vector2 position, Matrix4x4 matrix)
        {
            float tx = matrix.M11 * position.X + matrix.M21 * position.Y + matrix.M41;
            float ty = matrix.M12 * position.X + matrix.M22 * position.Y + matrix.M42;
            float tz = matrix.M14 * position.X + matrix.M24 * position.Y + matrix.M44;
            return new Vector2(tx / tz, ty / tz);
        }

        public static Vector2 Transform(float x, float y, Matrix4x4 matrix)
        {
            float tx = matrix.M11 * x + matrix.M21 * y + matrix.M41;
            float ty = matrix.M12 * x + matrix.M22 * y + matrix.M42;
            float tz = matrix.M14 * x + matrix.M24 * y + matrix.M44;
            return new Vector2(tx / tz, ty / tz);
        }

        public static Vector3 Transform(Vector3 position, Matrix4x4 matrix) => new Vector3
        {
            X = matrix.M11 * position.X + matrix.M21 * position.Y + matrix.M41 * position.Z,
            Y = matrix.M12 * position.X + matrix.M22 * position.Y + matrix.M42 * position.Z,
            Z = matrix.M14 * position.X + matrix.M24 * position.Y + matrix.M44 * position.Z,
        };

        public static Matrix3x2 Transform(Matrix3x2 value1, Matrix4x4 value2) => new Matrix3x2
        {
            // XYZ 1
            M11 = value2.M11 * value1.M11 + value2.M21 * value1.M21 + value2.M41 * value1.M31,
            M21 = value2.M12 * value1.M11 + value2.M22 * value1.M21 + value2.M42 * value1.M31,
            M31 = value2.M14 * value1.M11 + value2.M24 * value1.M21 + value2.M44 * value1.M31,

            // XYZ 2
            M12 = value2.M11 * value1.M12 + value2.M21 * value1.M22 + value2.M41 * value1.M32,
            M22 = value2.M12 * value1.M12 + value2.M22 * value1.M22 + value2.M42 * value1.M32,
            M32 = value2.M14 * value1.M12 + value2.M24 * value1.M22 + value2.M44 * value1.M32
        };
    }
}