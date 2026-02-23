using System;

namespace FanKit.Transformer.Sample
{
    [Flags]
    public enum InvalidateModes12345 : ushort
    {
        None = 0,

        InitCanvas = 2, // Modes 3
        InitLayers = 4, // Modes 2
        InitComposer = 8, InitTransformer = 16, // Modes 1

        UpdateCanvas = 32, // Modes 3
        UpdateLayers = 64, // Modes 2
        UpdateComposer = 128, UpdateTransformer = 256, // Modes 1

        CanvasControl = 1,
    }
}