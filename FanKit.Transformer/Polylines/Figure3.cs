using FanKit.Transformer.Cache;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Segment = FanKit.Transformer.Polylines.Segment3;

namespace FanKit.Transformer.Polylines
{
    public class Figure3 : IGetFigure
    {
        public bool IsClosed;

        public readonly List<Segment> Segments = new List<Segment>();

        public PathSetting Setting { get; } = new PathSetting();
        public int Count => this.Segments.Count;
        public int GetChecksCount() => this.Segments.Count(GetIsChecked);
        private static bool GetIsChecked(Segment item) => item.IsChecked;
    }
}