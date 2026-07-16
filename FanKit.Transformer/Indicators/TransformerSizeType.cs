namespace FanKit.Transformer.Indicators
{
    public enum TransformsParameterKind : byte
    {
        None,

        X,
        Y,
        Width,
        Height,
        Rotation,
        Skew,

        MultiX,
        MultiY,
        MultiWidth,
        MultiHeight,
        MultiRotation,
        MultiSkew,
    }
}