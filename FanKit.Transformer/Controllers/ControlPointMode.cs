namespace FanKit.Transformer.Controllers
{
    internal enum ControlPointMode : byte
    {
        LeftEqual,
        RightEqual,

        LeftRatio,
        RightRatio,

        LeftAngleEqual,
        RightAngleEqual,

        LeftAngleRatio,
        RightAngleRatio,

        LeftLength,
        RightLength,
    }
}