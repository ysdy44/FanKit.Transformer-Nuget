using FanKit.Transformer.Cache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace FanKit.Transformer.Curves
{
    public class Figure2 : IGetFigure
    {
        public Bounds SourceBounds;
        private Bounds b;

        public bool IsClosed;

        public readonly List<Segment2> Segments = new List<Segment2>();

        public PathSetting Setting { get; } = new PathSetting();
        public int Count => this.Segments.Count;
        public int GetChecksCount() => this.Segments.Count(GetIsChecked);
        private static bool GetIsChecked(Segment2 item) => item.IsChecked;

        public void Extend()
        {
            this.SourceBounds = Bounds.Infinity;

            for (int i = 1; i < this.Segments.Count; i++)
            {
                Segment2 previous = this.Segments[i - 1];
                Segment2 next = this.Segments[i];

                this.b = Segment2.Extend(previous, next);
                this.SourceBounds = Bounds.Union(this.SourceBounds, this.b);
            }

            if (this.IsClosed)
            {
                Segment2 first = this.Segments[0];
                Segment2 last = this.Segments[this.Segments.Count - 1];

                this.b = Segment2.Extend(last, first);
                this.SourceBounds = Bounds.Union(this.SourceBounds, this.b);
            }
        }
    }
}