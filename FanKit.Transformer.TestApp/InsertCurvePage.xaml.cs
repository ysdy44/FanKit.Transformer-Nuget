using FanKit.Transformer;
using FanKit.Transformer.Curves;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Sample;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class InsertCurvePage : Page
    {
        //@Const
        const float X = 256f;
        const float Y = 256f;

        Vector2 Point;

        int Index0 = -1;
        int Index1 = -1;
        ClosestPointer Closest;

        readonly bool IsClosed = false;
        readonly List<Node> Data = new List<Node>
        {
            new Node
            {
                Point = new Vector2(X - 144.6047f, Y - 138.5997f),
                LeftControlPoint= new Vector2(X - 144.6047f, Y - 138.5997f),
                RightControlPoint = new Vector2(X - 144.6047f, Y - 138.5997f),
            },
            new Node
            {
                Point = new Vector2(X + 13.37953f, Y - 95.3983f),
                LeftControlPoint = new Vector2(X - 8.623611f, Y - 132.9995f),
                RightControlPoint= new Vector2(X + 35.38268f, Y - 57.79712f),
            },
            new Node
            {
                Point = new Vector2(X - 12.58583f, Y + 87.00745f),
                LeftControlPoint = new Vector2(X - 34.4567f, Y + 48.00778f),
                RightControlPoint= new Vector2(X + 9.285034f, Y + 126.0071f),
            },
            new Node
            {
                Point = new Vector2(X + 144.6047f, Y + 138.5997f),
                LeftControlPoint = new Vector2(X + 144.6047f, Y + 138.5997f),
                RightControlPoint= new Vector2(X + 144.6047f, Y + 138.5997f),
            }
        };

        public InsertCurvePage()
        {
            this.InitializeComponent();
            base.Unloaded += delegate
            {
                // Explicitly remove references to allow the Win2D controls to get garbage collected
                this.CanvasControl.RemoveFromVisualTree();
                this.CanvasControl = null;
            };

            this.CanvasControl.Draw += (s, e) =>
            {
                this.Draw(s, e.DrawingSession);
            };
            this.CanvasControl.PointerMoved += (s, e) =>
            {
                PointerPoint pp = e.GetCurrentPoint(this.CanvasControl);

                switch (this.CanvasControl.FlowDirection)
                {
                    case FlowDirection.RightToLeft:
                        this.Over(this.CanvasControl.ActualWidth - pp.Position.X, pp.Position.Y);
                        break;
                    default:
                        this.Over(pp.Position.X, pp.Position.Y);
                        break;
                }
            };
        }

        private void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            using (PathBuilder path = new PathBuilder(resourceCreator))
            {
                path.CreatePointPath(this.Data, this.IsClosed);
                using (CanvasGeometry curve = CanvasGeometry.CreatePath(path.Builder))
                {
                    drawingSession.DrawCurve(curve);
                }
            }

            foreach (Node segment in this.Data)
            {
                drawingSession.DrawNode3(segment.Point);
            }

            if (this.Closest.Contains)
            {
                Node previous = this.Closest.Previous;
                Node current = this.Closest.Current;
                Node next = this.Closest.Next;
                Vector2 point = this.Closest.Point;

                if (this.Closest.CurrentIsSmooth)
                {
                    using (PathBuilder path = new PathBuilder(resourceCreator))
                    {
                        path.CreatePreviousPath(this.Closest);
                        using (CanvasGeometry curve = CanvasGeometry.CreatePath(path.Builder))
                        {
                            drawingSession.DrawPreviousCurve(curve);
                        }
                    }

                    using (PathBuilder path = new PathBuilder(resourceCreator))
                    {
                        path.CreateNextPath(this.Closest);
                        using (CanvasGeometry curve = CanvasGeometry.CreatePath(path.Builder))
                        {
                            drawingSession.DrawNextCurve(curve);
                        }
                    }
                }
                else
                {
                    drawingSession.DrawPreviousLine(current.Point, previous.Point);
                    drawingSession.DrawNextLine(current.Point, next.Point);
                }

                drawingSession.DrawLine(point, current.Point, Windows.UI.Colors.Orange, 2f);

                drawingSession.DrawLine(previous.RightControlPoint, previous.Point);
                drawingSession.DrawLine(previous.Point, previous.LeftControlPoint);

                drawingSession.DrawLine(current.RightControlPoint, current.Point);
                drawingSession.DrawLine(current.Point, current.LeftControlPoint);

                drawingSession.DrawLine(next.RightControlPoint, next.Point);
                drawingSession.DrawLine(next.Point, next.LeftControlPoint);

                drawingSession.DrawNode3(previous.LeftControlPoint);
                drawingSession.DrawNode3(previous.Point);
                drawingSession.DrawNode3(previous.RightControlPoint);

                drawingSession.DrawNode3(current.LeftControlPoint);
                drawingSession.DrawNode3(current.Point);
                drawingSession.DrawNode3(current.RightControlPoint);

                drawingSession.DrawNode3(next.LeftControlPoint);
                drawingSession.DrawNode3(next.Point);
                drawingSession.DrawNode3(next.RightControlPoint);

                drawingSession.DrawNode3(this.Closest.Point);
                drawingSession.DrawNode3(current.Point);
            }
        }

        private void Over(double x, double y)
        {
            const float l = 40f;
            const float ls = l * l;

            this.Point = new Vector2((float)x, (float)y);

            if (this.Closest.Contains)
            {
                if (this.IsClosed)
                {
                    Node last = this.Data[this.Data.Count - 1];
                    Node first = this.Data[0];

                    this.Closest = new ClosestPointer(this.Point, last, first, ls);
                    if (this.Closest.Contains)
                    {
                        this.Index0 = this.Data.Count - 1;
                        this.Index1 = 0;
                        this.CanvasControl.Invalidate();
                        return;
                    }
                }

                for (int i = 1; i < this.Data.Count; i++)
                {
                    Node previous = this.Data[i - 1];
                    Node next = this.Data[i];

                    this.Closest = new ClosestPointer(this.Point, previous, next, ls);
                    if (this.Closest.Contains)
                    {
                        this.Index0 = i - 1;
                        this.Index1 = i;
                        this.CanvasControl.Invalidate();
                        return;
                    }
                }

                this.Index0 = -1;
                this.Index1 = -1;
                this.Closest = default;
                this.CanvasControl.Invalidate();
            }
            else
            {
                if (this.IsClosed)
                {
                    Node last = this.Data[this.Data.Count - 1];
                    Node first = this.Data[0];

                    this.Closest = new ClosestPointer(this.Point, last, first, ls);
                    if (this.Closest.Contains)
                    {
                        this.Index0 = this.Data.Count - 1;
                        this.Index1 = 0;
                        this.CanvasControl.Invalidate();
                        return;
                    }
                }

                for (int i = 1; i < this.Data.Count; i++)
                {
                    Node previous = this.Data[i - 1];
                    Node next = this.Data[i];

                    this.Closest = new ClosestPointer(this.Point, previous, next, ls);
                    if (this.Closest.Contains)
                    {
                        this.Index0 = i - 1;
                        this.Index1 = i;
                        this.CanvasControl.Invalidate();
                        return;
                    }
                }
            }
        }
    }
}