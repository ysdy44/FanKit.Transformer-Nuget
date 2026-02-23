namespace FanKit.Transformer.Controllers
{
    public enum CropMode : byte
    {
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