using Microsoft.Graphics.Canvas;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class BoundsPage : Page
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

        QuadrilateralContainsNodeMode Mode;

        QuadrilateralPointKind PointKind;
        Bounds StartingBounds;
        Bounds Bounds = new Bounds
        {
            Left = X,
            Top = Y,
            Right = X + W,
            Bottom = Y + H,
        };

        // Canvas
        readonly CanvasOperator1 CanvasOperator;

        public BoundsPage()
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

            this.Mode = this.Bounds.ContainsNode(this.Point, ds);

            this.StartingBounds = this.Bounds;

            switch (this.Mode)
            {
                case QuadrilateralContainsNodeMode.None:
                    break;
                case QuadrilateralContainsNodeMode.Contains:
                    break;
                case QuadrilateralContainsNodeMode.LeftTop:
                    this.PointKind = QuadrilateralPointKind.LeftTop;
                    break;
                case QuadrilateralContainsNodeMode.RightTop:
                    this.PointKind = QuadrilateralPointKind.RightTop;
                    break;
                case QuadrilateralContainsNodeMode.LeftBottom:
                    this.PointKind = QuadrilateralPointKind.LeftBottom;
                    break;
                case QuadrilateralContainsNodeMode.RightBottom:
                    this.PointKind = QuadrilateralPointKind.RightBottom;
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
                case QuadrilateralContainsNodeMode.None:
                    break;
                case QuadrilateralContainsNodeMode.Contains:
                    this.Bounds = this.StartingBounds + this.Point - this.StartingPoint;
                    break;
                default:
                    this.Bounds = this.StartingBounds.MovePoint(this.PointKind, this.Point);
                    break;
            }

            this.CanvasControl.Invalidate();
        }

        private void Over(double x, double y)
        {
            const float d = 18f;
            const float ds = d * d;

            this.Point = new Vector2((float)x, (float)y);

            this.Mode = this.Bounds.ContainsNode(this.Point, ds);

            this.CanvasControl.Invalidate();
        }

        private void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            drawingSession.Blend = CanvasBlend.Copy;

            var rect = this.Bounds.ToRect();

            drawingSession.FillRectangle(rect, this.Mode == QuadrilateralContainsNodeMode.Contains ? Blue3 : Blue4);
            drawingSession.DrawRectangle(rect, Blue1, 2f);

            drawingSession.FillCircle(this.Bounds.Left, this.Bounds.Top, 18f, this.Mode == QuadrilateralContainsNodeMode.LeftTop ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Bounds.Right, this.Bounds.Top, 18f, this.Mode == QuadrilateralContainsNodeMode.RightTop ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Bounds.Left, this.Bounds.Bottom, 18f, this.Mode == QuadrilateralContainsNodeMode.LeftBottom ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Bounds.Right, this.Bounds.Bottom, 18f, this.Mode == QuadrilateralContainsNodeMode.RightBottom ? Blue2 : Blue3);

            drawingSession.DrawCircle(this.Bounds.Left, this.Bounds.Top, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Bounds.Right, this.Bounds.Top, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Bounds.Left, this.Bounds.Bottom, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Bounds.Right, this.Bounds.Bottom, 18f, Blue1, 2f);
        }
    }
}