using static FanKit.Transformer.UI.FlyoutLocation;

namespace FanKit.Transformer.UI
{
    public enum FlyoutPlacementPriority : byte
    {
        Fixed = FlyoutPlacementMode.Fixed,

        LeftTopRightBottom = Left6 + Top4 + Right2 + Bottom0,
        LeftTopBottomRight = Left6 + Top4 + Bottom2 + Right0,
        LeftRightTopBottom = Left6 + Right4 + Top2 + Bottom0,
        LeftRightBottomTop = Left6 + Right4 + Bottom2 + Top0,
        LeftBottomTopRight = Left6 + Bottom4 + Top2 + Right0,
        LeftBottomRightTop = Left6 + Bottom4 + Right2 + Top0,

        TopLeftRightBottom = Top6 + Left4 + Right2 + Bottom0,
        TopLeftBottomRight = Top6 + Left4 + Bottom2 + Right0,
        TopRightLeftBottom = Top6 + Right4 + Left2 + Bottom0,
        TopRightBottomLeft = Top6 + Right4 + Bottom2 + Left0,
        TopBottomLeftRight = Top6 + Bottom4 + Left2 + Right0,
        TopBottomRightLeft = Top6 + Bottom4 + Right2 + Left0,

        RightLeftTopBottom = Right6 + Left4 + Top2 + Bottom0,
        RightLeftBottomTop = Right6 + Left4 + Bottom2 + Top0,
        RightTopLeftBottom = Right6 + Top4 + Left2 + Bottom0,
        RightBottomLeftTop = Right6 + Bottom4 + Left2 + Top0,
        RightTopBottomLeft = Right6 + Top4 + Bottom2 + Left0,
        RightBottomTopLeft = Right6 + Bottom4 + Top2 + Left0,

        BottomLeftTopRight = Bottom6 + Left4 + Top2 + Right0,
        BottomLeftRightTop = Bottom6 + Left4 + Right2 + Top0,
        BottomTopLeftRight = Bottom6 + Top4 + Left2 + Right0,
        BottomRightLeftTop = Bottom6 + Right4 + Left2 + Top0,
        BottomTopRightLeft = Bottom6 + Top4 + Right2 + Left0,
        BottomRightTopLeft = Bottom6 + Right4 + Top2 + Left0,
    }
}