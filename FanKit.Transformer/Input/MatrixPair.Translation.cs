using System.Runtime.CompilerServices;
using Translation = System.Numerics.Vector4;

namespace FanKit.Transformer.Input
{
    partial struct MatrixPair
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Translation Translate(float x, float y)
        {
            return new Translation
            {
                X = x, // x
                Y = y, // y

                Z = -x, // rx
                W = -y, // ry
            };
        }
    }
}