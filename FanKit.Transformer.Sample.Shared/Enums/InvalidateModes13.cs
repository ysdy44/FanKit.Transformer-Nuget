using System;

namespace FanKit.Transformer.Sample
{
    [Flags]
    public enum InvalidateModes13 : byte
    {
        None = 0,

        InitCanvas = 2, // Modes 3
        InitIndicator = 4, // Modes 1

        UpdateCanvas = 8, // Modes 3
        UpdateIndicator = 16, // Modes 1

        CanvasControl = 1,
    }
}