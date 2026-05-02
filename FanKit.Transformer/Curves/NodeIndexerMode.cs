namespace FanKit.Transformer.Curves
{
    public enum SegmentIndexerMode : byte
    {
        None,

        PointWithoutChecked,
        PointWithChecked,

        LeftControlPoint,
        RightControlPoint,
    }
}