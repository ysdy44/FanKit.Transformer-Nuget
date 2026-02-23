namespace FanKit.Transformer.Controllers
{
    internal enum ControllerMode : byte
    {
        Translate,
        Rotate,
        SkewHandle,
        ScaleSide,
        ScaleCorner,
    }
}