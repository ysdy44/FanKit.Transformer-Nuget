namespace FanKit.Transformer
{
    public enum TriangleContainsNodeMode : byte
    {
        None = Constants.None,
        Contains = Constants.Contains,

        LeftTop = Constants.LeftTop,
        RightTop = Constants.RightTop,
        LeftBottom = Constants.LeftBottom,
    }
}