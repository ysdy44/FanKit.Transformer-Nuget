using System;

namespace FanKit.Transformer.Cache
{
    [Flags]
    public enum SelectedMode : byte
    {
        Empty = 0,
        UnSelected = 1,
        Selected = 2,
        Indeterminate = UnSelected | Selected,
    }
}