using FanKit.Transformer.Indicators;
using System;

namespace FanKit.Transformer.Sample
{
    public enum IndicatorColumnKind : byte
    {
        X = IndicatorKind.X,
        Y = IndicatorKind.Y,
        Height = IndicatorKind.Height,
        Rotation = IndicatorKind.Rotation,
    }
}