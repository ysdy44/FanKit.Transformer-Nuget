using FanKit.Transformer.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Segment = FanKit.Transformer.Curves.Segment1;

namespace FanKit.Transformer.Curves
{
    partial class Path1
    {
        public void SharpSelectedItems()
        {
            for (int i = 0; i < this.Segments.Count; i++)
            {
                Segment segment = this.Segments[i];
                if (segment.IsChecked)
                {
                    if (segment.IsSmooth)
                    {
                        this.Segments[i] = new Segment
                        {
                            IsChecked = true,
                            IsSmooth = false,

                            Point = new Node(segment.Point.Point),
                            Actual = new Node(segment.Actual.Point),
                        };
                    }
                }
            }
        }

        public void SmoothSelectedItems()
        {
            int end = this.Segments.Count - 1;
            Segment first = this.Segments[0];
            Segment last = this.Segments[end];

            if (this.IsClosed)
            {
                if (first.IsChecked)
                {
                    this.Segments[0] = first.Smooth(last, this.Segments[1]);
                }

                for (int i = 1; i < end; i++)
                {
                    Segment segment = this.Segments[i];
                    if (segment.IsChecked)
                    {
                        this.Segments[i] = segment.Smooth(this.Segments[i - 1], this.Segments[i + 1]);
                    }
                }

                if (last.IsChecked)
                {
                    this.Segments[end] = last.Smooth(this.Segments[this.Segments.Count - 2], first);
                }
            }
            else
            {
                if (first.IsChecked)
                {
                    this.Segments[0] = first.SmoothFirst(this.Segments[1]);
                }

                for (int i = 1; i < end; i++)
                {
                    Segment segment = this.Segments[i];
                    if (segment.IsChecked)
                    {
                        this.Segments[i] = segment.Smooth(this.Segments[i - 1], this.Segments[i + 1]);
                    }
                }

                if (last.IsChecked)
                {
                    this.Segments[end] = last.SmoothLast(this.Segments[this.Segments.Count - 2]);
                }
            }
        }
    }
}