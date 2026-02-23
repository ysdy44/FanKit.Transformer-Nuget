using System.Collections.Generic;

namespace FanKit.Transformer.Cache
{
    public interface IGetFigure
    {
        PathSetting Setting { get; }
        int Count { get; }
        int GetChecksCount();
    }
}