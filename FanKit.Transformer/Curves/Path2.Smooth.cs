using FanKit.Transformer.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace FanKit.Transformer.Curves
{
    partial class Path2
    {
        public void SharpSelectedItems()
        {
            foreach (Figure2 figure in this.Data)
            {
                for (int i = 0; i < figure.Data.Count; i++)
                {
                    Segment2 segment = figure.Data[i];
                    if (segment.IsChecked)
                    {
                        if (segment.IsSmooth)
                        {
                            figure.Data[i] = new Segment2
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
            foreach (Figure2 figure in this.Data)
            {
                int end = figure.Data.Count - 1;
                Segment2 first = figure.Data[0];
                Segment2 last = figure.Data[end];

                if (figure.IsClosed)
                {
                    if (first.IsChecked)
                    {
                        figure.Data[0] = first.Smooth(last, figure.Data[1]);
                    }

                    for (int i = 1; i < end; i++)
                    {
                        Segment2 segment = figure.Data[i];
                        if (segment.IsChecked)
                        {
                            figure.Data[i] = segment.Smooth(figure.Data[i - 1], figure.Data[i + 1]);
                        }
                    }

                    if (last.IsChecked)
                    {
                        figure.Data[end] = last.Smooth(figure.Data[figure.Data.Count - 2], first);
                    }
                }
                else
                {
                    if (first.IsChecked)
                    {
                        figure.Data[0] = first.SmoothFirst(figure.Data[1]);
                    }

                    for (int i = 1; i < end; i++)
                    {
                        Segment2 segment = figure.Data[i];
                        if (segment.IsChecked)
                        {
                            figure.Data[i] = segment.Smooth(figure.Data[i - 1], figure.Data[i + 1]);
                        }
                    }

                    if (last.IsChecked)
                    {
                        figure.Data[end] = last.SmoothLast(figure.Data[figure.Data.Count - 2]);
                    }
                }
            }
        }
    }
}