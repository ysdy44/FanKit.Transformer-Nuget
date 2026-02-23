using FanKit.Transformer.Indicators;
using System;

namespace FanKit.Transformer.Sample
{
    public enum IndicatorRowKind : byte
    {
        X = IndicatorKind.X,
        Y = IndicatorKind.Y,
        Width = IndicatorKind.Width,
        Rotation = IndicatorKind.Rotation,
    }
}