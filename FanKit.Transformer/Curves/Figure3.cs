using FanKit.Transformer.Cache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace FanKit.Transformer.Curves
{
    public class Figure3 : IGetFigure
    {
        public Bounds SourceBounds;
        private Bounds b;

        public bool IsClosed;

        public readonly List<Segment3> Data = new List<Segment3>();

        public PathSetting Setting { get; } = new PathSetting();
        public int Count => this.Data.Count;
        public int GetChecksCount() => this.Data.Count(GetIsChecked);
        private static bool GetIsChecked(Segment3 item) => item.IsChecked;

        public void Extend()
        {
            this.SourceBounds = Bounds.Infinity;

            for (int i = 1; i < this.Data.Count; i++)
            {
                Segment3 previous = this.Data[i - 1];
                Segment3 next = this.Data[i];

                this.b = Segment3.Extend(previous, next);
                this.SourceBounds = Bounds.Union(this.SourceBounds, this.b);
            }

            if (this.IsClosed)
            {
                Segment3 first = this.Data[0];
                Segment3 last = this.Data[this.Data.Count - 1];

                this.b = Segment3.Extend(last, first);
                this.SourceBounds = Bounds.Union(this.SourceBounds, this.b);
            }
        }
    }
}