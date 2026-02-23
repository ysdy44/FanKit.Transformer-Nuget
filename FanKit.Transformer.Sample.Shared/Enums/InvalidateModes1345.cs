using System;

namespace FanKit.Transformer.Sample
{
    [Flags]
    public enum InvalidateModes1345 : ushort
    {
        None = 0,

        InitLayers = 2, // Modes 2
        InitComposer = 4, InitTransformer = 8, // Modes 1

        UpdateLayers = 16, // Modes 2
        UpdateComposer = 32, UpdateTransformer = 64, // Modes 1

        CanvasControl = 1,
    }
}