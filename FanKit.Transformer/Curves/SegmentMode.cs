namespace FanKit.Transformer.Curves
{
    public enum SegmentMode : byte
    {
        None,

        PointWithoutChecked,
        PointWithChecked,

        LeftControlPoint,
        RightControlPoint,
    }
}