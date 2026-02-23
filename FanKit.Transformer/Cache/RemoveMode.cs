using System;

namespace FanKit.Transformer.Cache
{
    [Flags]
    public enum RemoveMode : byte
    {
        NoRemove,
        RemoveCurve,
        RemoveNodes,
    }
}