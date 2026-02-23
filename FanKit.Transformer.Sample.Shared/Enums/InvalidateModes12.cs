using System;

namespace FanKit.Transformer.Sample
{
    [Flags]
    public enum InvalidateModes12 : byte
    {
        None = 0,

        InitLayers = 2, // Modes 2
        InitIndicator = 4, // Modes 1

        UpdateLayers = 8, // Modes 2
        UpdateIndicator = 16, // Modes 1

        CanvasControl = 1,
    }
}