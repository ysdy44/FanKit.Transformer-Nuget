namespace FanKit.Transformer.UI
{
    public enum FlyoutPlacementMode : byte
    {
        Fixed = 0,
        Left = FlyoutLocation.Left0 + 1,
        Right = FlyoutLocation.Right0 + 1,
        Top = FlyoutLocation.Top0 + 1,
        Bottom = FlyoutLocation.Bottom0 + 1,
    }
}