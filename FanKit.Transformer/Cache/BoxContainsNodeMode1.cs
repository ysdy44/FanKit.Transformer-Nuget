namespace FanKit.Transformer.Cache
{
    public enum BoxContainsNodeMode1 : byte
    {
        None = Constants.None,
        Contains = Constants.Contains,

        CenterLeft = Constants.CenterLeft,
        CenterTop = Constants.CenterTop,
        CenterRight = Constants.CenterRight,
        CenterBottom = Constants.CenterBottom,

        LeftTop = Constants.LeftTop,
        RightTop = Constants.RightTop,
        LeftBottom = Constants.LeftBottom,
        RightBottom = Constants.RightBottom,
    }
}