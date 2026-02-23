using FanKit.Transformer.Cache;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FanKit.Transformer.Polylines
{
    public class Figure2 : IGetFigure
    {
        public bool IsClosed;

        public readonly List<Segment2> Data = new List<Segment2>();

        public PathSetting Setting { get; } = new PathSetting();
        public int Count => this.Data.Count;
        public int GetChecksCount() => this.Data.Count(GetIsChecked);
        private static bool GetIsChecked(Segment2 item) => item.IsChecked;
    }
}