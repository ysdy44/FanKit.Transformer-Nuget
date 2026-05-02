using FanKit.Transformer.Polylines;
using Microsoft.Graphics.Canvas;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class MovePolylinePage : Page
    {
        //@Const
        const float X = 256f;
        const float Y = 256f;

        Vector2 StartingPoint;
        Vector2 Point;

        SegmentIndexer Indexer = SegmentIndexer.Empty;

        readonly CanvasOperator1 CanvasOperator;

        readonly bool IsClosed = false;
        readonly List<Vector2> Data = new List<Vector2>
        {
            new Vector2(X - 144.6047f, Y - 138.5997f),
            new Vector2(X + 13.37953f, Y - 95.3983f),
            new Vector2(X - 12.58583f, Y + 87.00745f),
            new Vector2(X + 144.6047f, Y + 138.5997f),
        };

        public MovePolylinePage()
        {
            this.InitializeComponent();
            this.CanvasOperator = new CanvasOperator1(this.CanvasControl);
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

            this.CanvasOperator.Single_Start += (startingX, startingY, p) => this.CacheSingle(startingX, startingY);
            this.CanvasOperator.Single_Delta += (x, y, p) => this.Single(x, y);
            this.CanvasOperator.Single_Complete += (x, y, p) => { };
        }

        private void Draw(CanvasDrawingSession drawingSession)
        {
            for (int i = 1; i < this.Data.Count; i++)
            {
                Vector2 previous = this.Data[i - 1];
                Vector2 next = this.Data[i];
                drawingSession.DrawDashLine(previous, next);
            }

            if (this.IsClosed)
            {
                Vector2 last = this.Data[this.Data.Count - 1];
                Vector2 first = this.Data[0];
                drawingSession.DrawDashLine(last, first);
            }

            foreach (Vector2 segment in this.Data)
            {
                drawingSession.DrawNode(segment);
            }
        }

        private void CacheSingle(double startingX, double startingY)
        {
            const float d = 12f;
            const float ds = d * d;

            this.StartingPoint = this.Point = new Vector2((float)startingX, (float)startingY);

            this.Indexer = new SegmentIndexer(this.Data, this.StartingPoint, ds);
        }

        private void Single(double x, double y)
        {
            this.Point = new Vector2((float)x, (float)y);

            switch (this.Indexer.Mode)
            {
                case SegmentIndexerMode.None:
                    break;
                case SegmentIndexerMode.PointWithoutChecked:
                case SegmentIndexerMode.PointWithChecked:
                    this.Data[this.Indexer.Index] = this.Point;
                    this.CanvasControl.Invalidate();
                    break;
                default:
                    break;
            }
        }
    }
}