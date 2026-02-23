using System;

namespace FanKit.Transformer.Indicators
{
    [Flags]
    public enum IndicatorSizeType : byte
    {
        Empty = 0,
        Point = 1,
        RowLine = 2, // OnlyWidth
        ColumnLine = 3, // OnlyHeight
        Panel = 4, // WidthAndHeight
    }
}