using System.Runtime.CompilerServices;
using Rotation = System.Numerics.Vector2;

namespace FanKit.Transformer.Input
{
    partial struct MatrixPair
    {
        const float p2 = (float)(System.Math.PI + System.Math.PI);
        const float rp = -(float)System.Math.PI;
        const float p = (float)System.Math.PI;

        /*
        internal readonly float val; // Value
        internal readonly float rev; // Reverse Value
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Rotation Rotate(float v)
        {
            if (v > p)
            {
                return R(v % p2); // val
            }
            else if (v < rp)
            {
                return R(v % p2); // val
            }
            else
            {
                return R(v); // val
            }
        }

        private static Rotation R(float X)
        {
            return new Rotation
            {
                X = X,
                Y = -X
            };
        }
    }
}