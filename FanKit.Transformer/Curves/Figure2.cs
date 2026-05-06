using FanKit.Transformer.Cache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Segment = FanKit.Transformer.Curves.Segment2;

namespace FanKit.Transformer.Curves
{
    public class Figure2 : IGetFigure
    {
        public Bounds SourceBounds;
        private Bounds b;

        public bool IsClosed;

        public readonly List<Segment> Segments = new List<Segment>();

        public PathSetting Setting { get; } = new PathSetting();
        public int Count => this.Segments.Count;
        public int GetChecksCount() => this.Segments.Count(GetIsChecked);
        private static bool GetIsChecked(Segment item) => item.IsChecked;

        public void Extend()
        {
            this.SourceBounds = Bounds.Infinity;

            for (int i = 1; i < this.Segments.Count; i++)
            {
                Segment previous = this.Segments[i - 1];
                Segment next = this.Segments[i];

                this.b = Segment.Extend(previous, next);
                this.SourceBounds = Bounds.Union(this.SourceBounds, this.b);
            }

            if (this.IsClosed)
            {
                Segment first = this.Segments[0];
                Segment last = this.Segments[this.Segments.Count - 1];

                this.b = Segment.Extend(last, first);
                this.SourceBounds = Bounds.Union(this.SourceBounds, this.b);
            }
        }
    }
}