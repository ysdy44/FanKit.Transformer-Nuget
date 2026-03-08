using Microsoft.Graphics.Canvas;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class RectanglePage : Page
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

        bool Mode;

        Rectangle StartingRect;
        Rectangle Rect = new Rectangle
        {
            X = X,
            Y = Y,
            Width = W,
            Height = H,
        };

        // Canvas
        readonly CanvasOperator1 CanvasOperator;

        public RectanglePage()
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
            this.StartingPoint = this.Point = new Vector2((float)startingX, (float)startingY);

            this.Mode = this.Rect.Contains(this.Point);

            this.StartingRect = this.Rect;

            this.CanvasControl.Invalidate();
        }

        private void Single(double x, double y)
        {
            this.Point = new Vector2((float)x, (float)y);

            if (this.Mode)
            {
                this.Rect = this.StartingRect + this.Point - this.StartingPoint;
            }

            this.CanvasControl.Invalidate();
        }

        private void Over(double x, double y)
        {
            this.Point = new Vector2((float)x, (float)y);

            this.Mode = this.Rect.Contains(this.Point);

            this.CanvasControl.Invalidate();
        }

        private void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            var rect = this.Rect.ToRect();

            drawingSession.FillRectangle(rect, this.Mode ? Blue3 : Blue4);
            drawingSession.DrawRectangle(rect, Blue1, 2f);
        }
    }
}