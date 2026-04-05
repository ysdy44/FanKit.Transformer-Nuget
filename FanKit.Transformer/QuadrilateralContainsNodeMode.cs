namespace FanKit.Transformer
{
    public enum QuadrilateralContainsNodeMode : byte
    {
        None = Constants.None,
        Contains = Constants.Contains,

        LeftTop = Constants.LeftTop,
        RightTop = Constants.RightTop,
        LeftBottom = Constants.LeftBottom,
        RightBottom = Constants.RightBottom,
    }
}