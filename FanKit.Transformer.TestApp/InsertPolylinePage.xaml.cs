using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Polylines;
using Microsoft.Graphics.Canvas;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class InsertPolylinePage : Page
    {
        //@Const
        const float X = 256f;
        const float Y = 256f;

        Vector2 Point;

        FootPointer FootPoint;

        readonly bool IsClosed = false;
        readonly List<Vector2> Data = new List<Vector2>
        {
            new Vector2(X - 144.6047f, Y - 138.5997f),
            new Vector2(X + 13.37953f, Y - 95.3983f),
            new Vector2(X - 12.58583f, Y + 87.00745f),
            new Vector2(X + 144.6047f, Y + 138.5997f),
        };

        public InsertPolylinePage()
        {
            this.InitializeComponent();
            base.Unloaded += delegate
            {
                // Explicitly remove references to allow the Win2D controls to get garbage collected
                this.CanvasControl.RemoveFromVisualTree();
                this.CanvasControl = null;
            };

            this.CanvasControl.CreateResources += delegate
            {
                this.CanvasControl.Invalidate();
            };
            this.CanvasControl.Draw += (s, e) =>
            {
                this.Draw(e.DrawingSession);
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

        private void Draw(CanvasDrawingSession drawingSession)
        {
            for (int i = 1; i < this.Data.Count; i++)
            {
                Vector2 previous = this.Data[i - 1];
                Vector2 next = this.Data[i];
                drawingSession.DrawLine(previous, next);
            }

            if (this.IsClosed)
            {
                Vector2 last = this.Data[this.Data.Count - 1];
                Vector2 first = this.Data[0];
                drawingSession.DrawLine(last, first);
            }

            foreach (Vector2 segment in this.Data)
            {
                drawingSession.DrawNode(segment);
            }

            if (this.FootPoint.Contains)
            {
                Vector2 f = this.FootPoint.Foot;

                Vector2 e = this.FootPoint.LinePoint0;
                Vector2 n = this.FootPoint.LinePoint1;
                drawingSession.DrawPreviousLine(f, e);
                drawingSession.DrawNextLine(f, n);

                Vector2 p = this.FootPoint.Point;
                drawingSession.DrawLine(f, p, Windows.UI.Colors.Orange, 2f);

                drawingSession.DrawNode(p);
                drawingSession.DrawNode(f);
            }
        }

        private void Over(double x, double y)
        {
            const float l = 40f;
            const float ls = l * l;

            this.Point = new Vector2((float)x, (float)y);

            if (this.FootPoint.Contains)
            {
                for (int i = 1; i < this.Data.Count; i++)
                {
                    Vector2 previous = this.Data[i - 1];
                    Vector2 next = this.Data[i];

                    this.FootPoint = new FootPointer(this.Point, previous, next, ls);
                    if (this.FootPoint.Contains)
                    {
                        this.CanvasControl.Invalidate();
                        return;
                    }
                }

                this.FootPoint = default;
                this.CanvasControl.Invalidate();
            }
            else
            {
                for (int i = 1; i < this.Data.Count; i++)
                {
                    Vector2 previous = this.Data[i - 1];
                    Vector2 next = this.Data[i];

                    this.FootPoint = new FootPointer(this.Point, previous, next, ls);
                    if (this.FootPoint.Contains)
                    {
                        this.CanvasControl.Invalidate();
                        return;
                    }
                }
            }
        }
    }
}