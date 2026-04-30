using FanKit.Transformer.Cache;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FanKit.Transformer.Polylines
{
    public class Figure3 : IGetFigure
    {
        public bool IsClosed;

        public readonly List<Segment3> Segments = new List<Segment3>();

        public PathSetting Setting { get; } = new PathSetting();
        public int Count => this.Segments.Count;
        public int GetChecksCount() => this.Segments.Count(GetIsChecked);
        private static bool GetIsChecked(Segment3 item) => item.IsChecked;
    }
}