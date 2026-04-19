namespace FanKit.Transformer.Curves
{
    public enum NodeIndexerMode : byte
    {
        None,

        PointWithoutChecked,
        PointWithChecked,

        LeftControlPoint,
        RightControlPoint,
    }
}