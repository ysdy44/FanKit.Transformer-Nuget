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
            for (int i = 0; i < this.Data.Count; i++)
            {
                Segment segment = this.Data[i];
                if (segment.IsChecked)
                {
                    if (segment.IsSmooth)
                    {
                        this.Data[i] = new Segment
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
            int end = this.Data.Count - 1;
            Segment first = this.Data[0];
            Segment last = this.Data[end];

            if (this.IsClosed)
            {
                if (first.IsChecked)
                {
                    this.Data[0] = first.Smooth(last, this.Data[1]);
                }

                for (int i = 1; i < end; i++)
                {
                    Segment segment = this.Data[i];
                    if (segment.IsChecked)
                    {
                        this.Data[i] = segment.Smooth(this.Data[i - 1], this.Data[i + 1]);
                    }
                }

                if (last.IsChecked)
                {
                    this.Data[end] = last.Smooth(this.Data[this.Data.Count - 2], first);
                }
            }
            else
            {
                if (first.IsChecked)
                {
                    this.Data[0] = first.SmoothFirst(this.Data[1]);
                }

                for (int i = 1; i < end; i++)
                {
                    Segment segment = this.Data[i];
                    if (segment.IsChecked)
                    {
                        this.Data[i] = segment.Smooth(this.Data[i - 1], this.Data[i + 1]);
                    }
                }

                if (last.IsChecked)
                {
                    this.Data[end] = last.SmoothLast(this.Data[this.Data.Count - 2]);
                }
            }
        }
    }
}