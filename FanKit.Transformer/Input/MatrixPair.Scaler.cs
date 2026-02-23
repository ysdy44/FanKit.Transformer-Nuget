using System.Runtime.CompilerServices;
using Scaler = System.Numerics.Vector2;

namespace FanKit.Transformer.Input
{
    partial struct MatrixPair
    {
        internal static readonly Scaler empty = new Scaler(1f, 1f);

        /*
        internal readonly float val; // Value
        internal readonly float inv; // Inverse Value
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Scaler Scales(float v)
        {
            return new Scaler
            {
                X = v, // val
                Y = 1f / v, // inv
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Scaler Scales(float v, float i)
        {
            return new Scaler
            {
                X = v, // val
                Y = i, // inv
            };
        }
    }
}