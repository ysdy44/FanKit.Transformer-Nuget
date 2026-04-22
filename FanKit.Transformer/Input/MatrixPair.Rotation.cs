using System.Runtime.CompilerServices;
using Rotation = System.Numerics.Vector2;

namespace FanKit.Transformer.Input
{
    partial struct MatrixPair
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Rotation Rotate(float v)
        {
            if (v > Constants.PI)
            {
                return R(v % Constants.PiTwice); // val
            }
            else if (v < Constants.InvPI)
            {
                return R(v % Constants.PiTwice); // val
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