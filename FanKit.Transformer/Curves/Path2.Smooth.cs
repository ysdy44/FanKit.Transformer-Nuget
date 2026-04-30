using FanKit.Transformer.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Figure = FanKit.Transformer.Curves.Figure2;
using Segment = FanKit.Transformer.Curves.Segment2;

namespace FanKit.Transformer.Curves
{
    partial class Path2
    {
        public void SharpSelectedItems()
        {
            foreach (Figure figure in this.Figures)
            {
                for (int i = 0; i < figure.Segments.Count; i++)
                {
                    Segment segment = figure.Segments[i];
                    if (segment.IsChecked)
                    {
                        if (segment.IsSmooth)
                        {
                            figure.Segments[i] = new Segment
                            {
                                IsChecked = true,
                                IsSmooth = false,

                                Raw = new Node(segment.Raw.Point),
                                Map = new Node(segment.Map.Point),
                            };
                        }
                    }
                }
            }
        }

        public void SmoothSelectedItems()
        {
            foreach (Figure figure in this.Figures)
            {
                int end = figure.Segments.Count - 1;
                Segment first = figure.Segments[0];
                Segment last = figure.Segments[end];

                if (figure.IsClosed)
                {
                    if (first.IsChecked)
                    {
                        figure.Segments[0] = first.Smooth(last, figure.Segments[1]);
                    }

                    for (int i = 1; i < end; i++)
                    {
                        Segment segment = figure.Segments[i];
                        if (segment.IsChecked)
                        {
                            figure.Segments[i] = segment.Smooth(figure.Segments[i - 1], figure.Segments[i + 1]);
                        }
                    }

                    if (last.IsChecked)
                    {
                        figure.Segments[end] = last.Smooth(figure.Segments[figure.Segments.Count - 2], first);
                    }
                }
                else
                {
                    if (first.IsChecked)
                    {
                        figure.Segments[0] = first.SmoothFirst(figure.Segments[1]);
                    }

                    for (int i = 1; i < end; i++)
                    {
                        Segment segment = figure.Segments[i];
                        if (segment.IsChecked)
                        {
                            figure.Segments[i] = segment.Smooth(figure.Segments[i - 1], figure.Segments[i + 1]);
                        }
                    }

                    if (last.IsChecked)
                    {
                        figure.Segments[end] = last.SmoothLast(figure.Segments[figure.Segments.Count - 2]);
                    }
                }
            }
        }
    }
}