using System;

namespace FanKit.Transformer.Sample
{
    [Flags]
    public enum InvalidateModes1 : byte
    {
        None = 0,

        InitIndicator = 2, // Modes 1

        UpdateIndicator = 4, // Modes 1

        CanvasControl = 1,
    }
}