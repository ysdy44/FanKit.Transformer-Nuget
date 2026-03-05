using System;

namespace FanKit.Transformer.Indicators
{
    [Flags]
    public enum SizeType : byte
    {
        Empty = 0,
        Point = 1,
        RowLine = 2,
        ColumnLine = 3,
        Panel = 4,
    }
}