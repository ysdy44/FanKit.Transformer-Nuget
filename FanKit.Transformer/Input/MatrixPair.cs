using FanKit.Transformer.Mathematics;
using System.Numerics;
using System.Runtime.CompilerServices;
using Rotation = System.Numerics.Vector2;
using Scaler = System.Numerics.Vector2;
using Translation = System.Numerics.Vector4;
using Vector2 = System.Numerics.Vector2;

namespace FanKit.Transformer.Input
{
    // ndp\fx\src\Numerics\System\Numerics\Matrix3x2.cs
    internal partial struct MatrixPair
    {
        internal Matrix3x2 mat; // Matrix
        internal Matrix3x2 inv; // Inverse Matrix

        /*
        internal MatrixPair(Matrix3x2 m, Matrix3x2 i)
        {
            mat = m;
            inv = i;
        }
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static MatrixPair Pair(Translation t)
        {
            return new MatrixPair
            {
                mat = new Matrix3x2
                (
                    m11: 1f, m12: 0,
                    m21: 0, m22: 1f,
                    m31: t.X, m32: t.Y
                ),

                inv = new Matrix3x2
                (
                    m11: 1f, m12: 0,
                    m21: 0, m22: 1f,
                    m31: t.Z, m32: t.W
                ),
            };
        }

        // 1
        /*
        internal MatrixPair(Rotation r, Translation c)
        {
            mat = Matrix3x2.CreateTranslation(c.rx, c.ry) *
                Matrix3x2.CreateRotation(r.val);

            inv = Matrix3x2.CreateRotation(r.rev) *
                Matrix3x2.CreateTranslation(c.x, c.y);
        }
         */
        internal MatrixPair(Rotation r, Translation c) // Center
        {
            mat = Matrix3x2.CreateRotation(r.X);
            mat.M31 = c.Z * mat.M11 + c.W * mat.M21;
            mat.M32 = c.Z * mat.M12 + c.W * mat.M22;

            inv = Matrix3x2.CreateRotation(r.Y);
            inv.M31 = c.X;
            inv.M32 = c.Y;
        }

        // 2
        /*
        internal MatrixPair(Translation t, Scaler s)
        {
            mat = Matrix3x2.CreateScale(s.val) *
                Matrix3x2.CreateTranslation(t.x, t.y);

            inv = Matrix3x2.CreateTranslation(t.rx, t.ry) *
                Matrix3x2.CreateScale(s.inv);
        }
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static MatrixPair Pair(Translation t, Scaler s)
        {
            return new MatrixPair
            {
                mat = new Matrix3x2
                (
                    m11: s.X, m12: 0,
                    m21: 0, m22: s.X,
                    m31: t.X, m32: t.Y
                ),

                inv = new Matrix3x2
                (
                    m11: s.Y, m12: 0,
                    m21: 0, m22: s.Y,
                    m31: t.Z * s.Y, m32: t.W * s.Y
                ),
            };
        }
        /*
        internal MatrixPair(Translation t, Scaler s, Translation c) // Center
        {
            mat = Matrix3x2.CreateScale(s.val) *
                Matrix3x2.CreateTranslation(c.x, c.y) *
                Matrix3x2.CreateTranslation(t.x, t.y);

            inv = Matrix3x2.CreateTranslation(t.rx, t.ry) *
                Matrix3x2.CreateTranslation(c.rx, c.ry) *
                Matrix3x2.CreateScale(s.inv);
        }
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static MatrixPair Pair(Translation t, Scaler s, Translation c) // Center
        {
            return new MatrixPair
            {
                mat = new Matrix3x2
                (
                    m11: s.X, m12: 0,
                    m21: 0, m22: s.X,
                    m31: c.X + t.X, m32: c.Y + t.Y
                ),

                inv = new Matrix3x2
                (
                    m11: s.Y, m12: 0,
                    m21: 0, m22: s.Y,
                    m31: (c.Z + t.Z) * s.Y, m32: (c.W + t.W) * s.Y
                ),
            };
        }

        // 3
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static MatrixPair Pair(MatrixPair p1, MatrixPair p2)
        {
            return new MatrixPair
            {
                mat = p1.mat * p2.mat,
                inv = p2.inv * p1.inv,
            };
        }

        /*
        internal MatrixPair(Translation c, MatrixPair p2)
        {
            MatrixPair p1 = new MatrixPair
            {
                mat = Matrix3x2.CreateTranslation(c.rx, c.ry),
                inv = Matrix3x2.CreateTranslation(c.x, c.y),
            };

            mat = p1.mat * p2.mat;
            inv = p2.inv * p1.inv;
        }
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static MatrixPair Pair(Translation c, MatrixPair p2)
        {
            return new MatrixPair
            {
                mat = new Matrix3x2
                {
                    // First row
                    M11 = p2.mat.M11,
                    M12 = p2.mat.M12,

                    // Second row
                    M21 = p2.mat.M21,
                    M22 = p2.mat.M22,

                    // Third row
                    M31 = c.Z * p2.mat.M11 + c.W * p2.mat.M21 + p2.mat.M31,
                    M32 = c.Z * p2.mat.M12 + c.W * p2.mat.M22 + p2.mat.M32
                },

                inv = new Matrix3x2
                {
                    // First row
                    M11 = p2.inv.M11,
                    M12 = p2.inv.M12,

                    // Second row
                    M21 = p2.inv.M21,
                    M22 = p2.inv.M22,

                    // Third row
                    M31 = p2.inv.M31 + c.X,
                    M32 = p2.inv.M32 + c.Y
                },
            };
        }

        // [  c  s ]
        // [ -s  c ]
        // [  x  y ]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Vector2 T(float x, float y) // Transform
        {
            return Math.Transform(x, y, mat);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Vector2 T(Vector2 p) // Transform
        {
            return Math.Transform(p, mat);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Vector2 R(float x, float y) // Inverse Transform
        {
            return Math.Transform(x, y, inv);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Vector2 R(Vector2 p) // Inverse Transform
        {
            return Math.Transform(p, inv);
        }
    }
}