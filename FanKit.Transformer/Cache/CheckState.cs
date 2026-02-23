using System;

namespace FanKit.Transformer.Cache
{
    [Flags]
    public enum CheckState : byte
    {
        Empty = 0,
        AllUnchecked = 1,
        AllChecked = 2,
        Indeterminate = AllUnchecked | AllChecked,
    }
}