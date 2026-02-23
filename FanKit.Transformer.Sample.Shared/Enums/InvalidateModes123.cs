using System;

namespace FanKit.Transformer.Sample
{
    [Flags]
    public enum InvalidateModes123 : byte
    {
        None = 0,

        InitCanvas = 2, // Modes 3
        InitLayers = 4, // Modes 2
        InitIndicator = 8, // Modes 1

        UpdateCanvas = 16, // Modes 3
        UpdateLayers = 32, // Modes 2
        UpdateIndicator = 64, // Modes 1

        CanvasControl = 1,
    }
}