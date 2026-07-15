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
        Cropper = 4, // WidthAndHeight
        Transformer = 5, // All
    }
}