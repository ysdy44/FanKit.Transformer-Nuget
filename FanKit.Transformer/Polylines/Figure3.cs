using FanKit.Transformer.Cache;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FanKit.Transformer.Polylines
{
    public class Figure3 : IGetFigure
    {
        public bool IsClosed;

        public readonly List<Segment3> Data = new List<Segment3>();

        public PathSetting Setting { get; } = new PathSetting();
        public int Count => this.Data.Count;
        public int GetChecksCount() => this.Data.Count(GetIsChecked);
        private static bool GetIsChecked(Segment3 item) => item.IsChecked;
    }
}