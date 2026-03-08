using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class TrianglePage : Page
    {
        //@Const
        const float X = 200;
        const float Y = 200;
        const float W = 256;
        const float H = 256;

        static readonly Windows.UI.Color Blue1 = Windows.UI.Colors.DeepSkyBlue;
        static readonly Windows.UI.Color Blue2 = Windows.UI.Color.FromArgb(127, 0, 191, 255);
        static readonly Windows.UI.Color Blue3 = Windows.UI.Color.FromArgb(63, 0, 191, 255);
        static readonly Windows.UI.Color Blue4 = Windows.UI.Color.FromArgb(31, 0, 191, 255);

        // Singe
        Vector2 StartingPoint;
        Vector2 Point;

        TriangleContainsNodeMode Mode;

        TrianglePointKind PointKind;
        Triangle StartingTriangle;
        Triangle Triangle = new Triangle
        {
            LeftTop = new Vector2(X, Y),
            RightTop = new Vector2(X + W, Y),
            LeftBottom = new Vector2(X, Y + H),
        };

        // Canvas
        readonly CanvasOperator1 CanvasOperator;

        public TrianglePage()
        {
            this.InitializeComponent();
            this.CanvasOperator = new CanvasOperator1(this.CanvasControl);

            this.CanvasControl.Draw += (s, e) => this.Draw(s, e.DrawingSession);

            this.CanvasOperator.Single_Start += (startingX, startingY, p) => this.CacheSingle(startingX, startingY);
            this.CanvasOperator.Single_Delta += (x, y, p) => this.Single(x, y);
            this.CanvasOperator.Single_Complete += (x, y, p) => { };
            this.CanvasOperator.Pointer_Over += (x, y) => this.Over(x, y);
        }

        private void CacheSingle(double startingX, double startingY)
        {
            const float d = 18f;
            const float ds = d * d;

            this.StartingPoint = this.Point = new Vector2((float)startingX, (float)startingY);

            this.Mode = this.Triangle.ContainsNode(this.Point, ds);

            this.StartingTriangle = this.Triangle;

            switch (this.Mode)
            {
                case TriangleContainsNodeMode.None:
                    break;
                case TriangleContainsNodeMode.Contains:
                    break;
                case TriangleContainsNodeMode.LeftTop:
                    this.PointKind = TrianglePointKind.LeftTop;
                    break;
                case TriangleContainsNodeMode.RightTop:
                    this.PointKind = TrianglePointKind.RightTop;
                    break;
                case TriangleContainsNodeMode.LeftBottom:
                    this.PointKind = TrianglePointKind.LeftBottom;
                    break;
                default:
                    break;
            }

            this.CanvasControl.Invalidate();
        }

        private void Single(double x, double y)
        {
            this.Point = new Vector2((float)x, (float)y);

            switch (this.Mode)
            {
                case TriangleContainsNodeMode.None:
                    break;
                case TriangleContainsNodeMode.Contains:
                    this.Triangle = this.StartingTriangle + this.Point - this.StartingPoint;
                    break;
                default:
                    this.Triangle = this.StartingTriangle.MovePoint(this.PointKind, this.Point);
                    break;
            }

            this.CanvasControl.Invalidate();
        }

        private void Over(double x, double y)
        {
            const float d = 18f;
            const float ds = d * d;

            this.Point = new Vector2((float)x, (float)y);

            this.Mode = this.Triangle.ContainsNode(this.Point, ds);

            this.CanvasControl.Invalidate();
        }

        private void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            drawingSession.Blend = CanvasBlend.Copy;

            drawingSession.FillGeometry(CanvasGeometry.CreatePolygon(resourceCreator, new Vector2[]
            {
                this.Triangle.LeftTop,
                this.Triangle.RightTop,
                this.Triangle.LeftBottom,
            }), this.Mode == TriangleContainsNodeMode.Contains ? Blue3 : Blue4);

            drawingSession.DrawLine(this.Triangle.LeftTop, this.Triangle.RightTop, Blue1, 2f);
            drawingSession.DrawLine(this.Triangle.RightTop, this.Triangle.LeftBottom, Blue1, 2f);
            drawingSession.DrawLine(this.Triangle.LeftBottom, this.Triangle.LeftTop, Blue1, 2f);

            drawingSession.FillCircle(this.Triangle.LeftTop, 18f, this.Mode == TriangleContainsNodeMode.LeftTop ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Triangle.RightTop, 18f, this.Mode == TriangleContainsNodeMode.RightTop ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Triangle.LeftBottom, 18f, this.Mode == TriangleContainsNodeMode.LeftBottom ? Blue2 : Blue3);

            drawingSession.DrawCircle(this.Triangle.LeftTop, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Triangle.RightTop, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Triangle.LeftBottom, 18f, Blue1, 2f);
        }
    }
}