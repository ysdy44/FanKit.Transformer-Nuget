namespace FanKit.Transformer.Controllers
{
    public enum TransformMode : byte
    {
        SkewLeft = Constants.HandleLeft,
        SkewTop = Constants.HandleTop,
        SkewRight = Constants.HandleRight,
        SkewBottom = Constants.HandleBottom,

        ScaleLeft = Constants.CenterLeft,
        ScaleTop = Constants.CenterTop,
        ScaleRight = Constants.CenterRight,
        ScaleBottom = Constants.CenterBottom,

        ScaleLeftTop = Constants.LeftTop,
        ScaleRightTop = Constants.RightTop,
        ScaleLeftBottom = Constants.LeftBottom,
        ScaleRightBottom = Constants.RightBottom,
    }
}