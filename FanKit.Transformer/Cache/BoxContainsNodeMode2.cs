namespace FanKit.Transformer.Cache
{
    public enum BoxContainsNodeMode2 : byte
    {
        None = Constants.None,
        Contains = Constants.Contains,

        /*
        HandleLeftTop = Constants.HandleLeftTop,
        HandleRightTop = Constants.HandleRightTop,
        HandleLeftBottom = Constants.HandleLeftBottom,
        HandleRightBottom = Constants.HandleRightBottom,
         */

        HandleLeft = Constants.HandleLeft,
        HandleTop = Constants.HandleTop,
        HandleRight = Constants.HandleRight,
        HandleBottom = Constants.HandleBottom,

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