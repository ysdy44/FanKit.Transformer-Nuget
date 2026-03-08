using FanKit.Transformer.Controllers;
using FanKit.Transformer.Curves;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class MoveCurvePage : Page
    {
        //@Key
        bool Disconnected => this.ToolBox5.Disconnected;
        SelfControlPointMode Mode1 => this.ToolBox5.Mode1;
        EachControlPointLengthMode Mode2 => this.ToolBox5.Mode2;

        //@Const
        const float X = 128f + 256f;
        const float Y = 256f;

        Vector2 StartingPoint;
        Vector2 Point;

        Node Node;
        SegmentIndexer Indexer = SegmentIndexer.Empty;
        NodeController Controller;

        readonly CanvasOperator1 CanvasOperator;

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
            },
        };

        public MoveCurvePage()
        {
            this.InitializeComponent();
            this.CanvasOperator = new CanvasOperator1(this.CanvasControl);
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

            this.CanvasOperator.Single_Start += (startingX, startingY, p) => this.CacheSingle(startingX, startingY);
            this.CanvasOperator.Single_Delta += (x, y, p) => this.Single(x, y);
            this.CanvasOperator.Single_Complete += (x, y, p) => { };
        }

        private void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            using (PathBuilder path = new PathBuilder(resourceCreator))
            {
                path.CreatePointPath(this.Data, this.IsClosed);
                using (CanvasGeometry curve = CanvasGeometry.CreatePath(path.Builder))
                {
                    drawingSession.DrawDashCurve(curve);
                }
            }

            foreach (Node segment in this.Data)
            {
                drawingSession.DrawLine(segment.Point, segment.LeftControlPoint, Windows.UI.Colors.DodgerBlue);
                drawingSession.DrawLine(segment.Point, segment.RightControlPoint, Windows.UI.Colors.DodgerBlue);
            }

            foreach (Node segment in this.Data)
            {
                drawingSession.DrawNode(segment);
            }
        }

        private void CacheSingle(double startingX, double startingY)
        {
            const float d = 12f;
            const float ds = d * d;

            const float cd = 10f;
            const float cds = cd * cd;

            this.StartingPoint = this.Point = new Vector2((float)startingX, (float)startingY);

            this.Indexer = new SegmentIndexer(this.Data, this.StartingPoint, ds, cds);

            switch (this.Indexer.Mode)
            {
                case SegmentMode.None:
                    break;
                case SegmentMode.PointWithoutChecked:
                case SegmentMode.PointWithChecked:
                    this.Node = this.Data[this.Indexer.Index];
                    this.CanvasControl.Invalidate();
                    break;
                case SegmentMode.LeftControlPoint:
                    const bool Left = true;
                    this.Node = this.Data[this.Indexer.Index];
                    this.Controller = new NodeController(this.Node, Left, this.Mode1, this.Mode2);
                    this.CanvasControl.Invalidate();
                    break;
                case SegmentMode.RightControlPoint:
                    const bool Right = false;
                    this.Node = this.Data[this.Indexer.Index];
                    this.Controller = new NodeController(this.Node, Right, this.Mode1, this.Mode2);
                    this.CanvasControl.Invalidate();
                    break;
                default:
                    break;
            }
        }

        private void Single(double x, double y)
        {
            this.Point = new Vector2((float)x, (float)y);

            switch (this.Indexer.Mode)
            {
                case SegmentMode.None:
                    break;
                case SegmentMode.PointWithoutChecked:
                case SegmentMode.PointWithChecked:
                    this.Data[this.Indexer.Index] = this.Node.MovePoint(this.Point);
                    this.CanvasControl.Invalidate();
                    break;
                case SegmentMode.LeftControlPoint:
                case SegmentMode.RightControlPoint:
                    this.Data[this.Indexer.Index] = this.Controller.ToNode(this.Point, this.Disconnected);
                    this.CanvasControl.Invalidate();
                    break;
                default:
                    break;
            }
        }
    }
}