using System;
using System.Linq;
using System.Numerics;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class ConvertPage : Page
    {
        const float X = 0;
        const float Y = 20;
        const float W = 128;
        const float H = 128;

        Rectangle Rectangle1 = new Rectangle
        {
            X = X,
            Y = Y,
            Width = W,
            Height = H,
        };
        Triangle Triangle2 = new Triangle
        {
            LeftTop = new Vector2(X, Y),
            RightTop = new Vector2(X + W, 10 + Y),
            LeftBottom = new Vector2(100 + X, Y + H),
        };
        Quadrilateral Quadrilateral3 = new Quadrilateral
        {
            LeftTop = new Vector2(X, Y + 20),
            RightTop = new Vector2(X + W, Y - 20),
            LeftBottom = new Vector2(X, Y + H - 20),
            RightBottom = new Vector2(X + W, Y + H + 20),
        };
        Triangle Triangle4 = new Triangle
        {
            LeftTop = new Vector2(X, Y + 20),
            RightTop = new Vector2(X + W, Y - 20),
            LeftBottom = new Vector2(X, Y + H - 20),
        };
        Rectangle Rectangle5 = new Rectangle
        {
            X = X,
            Y = Y,
            Width = W,
            Height = H,
        };

        Bounds Bounds1;
        Bounds Bounds2;
        Bounds Bounds3;
        Quadrilateral Quadrilateral4;
        Quadrilateral Quadrilateral5;

        public ConvertPage()
        {
            this.InitializeComponent();

            this.Bounds1 = new Bounds(this.Rectangle1);
            this.Bounds2 = new Bounds(this.Triangle2);
            this.Bounds3 = new Bounds(this.Quadrilateral3);
            this.Quadrilateral4 = new Quadrilateral(this.Triangle4);
            this.Quadrilateral5 = new Quadrilateral(this.Rectangle5);

            this.CanvasControl1A.Draw += (s, e) =>
            {
                Rectangle t = this.Rectangle5;
                e.DrawingSession.DrawLine(t.X, t.Y, t.X + t.Width, t.Y, Colors.OrangeRed, 2f);
                e.DrawingSession.DrawLine(t.X + t.Width, t.Y, t.X + t.Width, t.Y + t.Height, Colors.Orange, 2f);
                e.DrawingSession.DrawLine(t.X + t.Width, t.Y + t.Height, t.X, t.Y + t.Height, Colors.Gold, 2f);
                e.DrawingSession.DrawLine(t.X, t.Y + t.Height, t.X, t.Y, Colors.GreenYellow, 2f);
            };
            this.CanvasControl1B.Draw += (s, e) =>
            {
                Bounds t = this.Bounds1;
                e.DrawingSession.DrawLine(t.Left, t.Top, t.Right, t.Top, Colors.OrangeRed, 2f);
                e.DrawingSession.DrawLine(t.Right, t.Top, t.Right, t.Bottom, Colors.Orange, 2f);
                e.DrawingSession.DrawLine(t.Right, t.Bottom, t.Left, t.Bottom, Colors.Gold, 2f);
                e.DrawingSession.DrawLine(t.Left, t.Bottom, t.Left, t.Top, Colors.GreenYellow, 2f);
            };

            this.CanvasControl2A.Draw += (s, e) =>
            {
                Triangle t = this.Triangle2;
                e.DrawingSession.DrawLine(t.LeftTop.X, t.LeftTop.Y, t.RightTop.X, t.RightTop.Y, Colors.OrangeRed, 2f);
                e.DrawingSession.DrawLine(t.RightTop.X, t.RightTop.Y, t.LeftBottom.X, t.LeftBottom.Y, Colors.Orange, 2f);
                e.DrawingSession.DrawLine(t.LeftBottom.X, t.LeftBottom.Y, t.LeftTop.X, t.LeftTop.Y, Colors.Gold, 2f);
            };
            this.CanvasControl2B.Draw += (s, e) =>
            {
                Bounds t = this.Bounds2;
                e.DrawingSession.DrawLine(t.Left, t.Top, t.Right, t.Top, Colors.OrangeRed, 2f);
                e.DrawingSession.DrawLine(t.Right, t.Top, t.Right, t.Bottom, Colors.Orange, 2f);
                e.DrawingSession.DrawLine(t.Right, t.Bottom, t.Left, t.Bottom, Colors.Gold, 2f);
                e.DrawingSession.DrawLine(t.Left, t.Bottom, t.Left, t.Top, Colors.GreenYellow, 2f);
            };

            this.CanvasControl3A.Draw += (s, e) =>
            {
                Quadrilateral t = this.Quadrilateral3;
                e.DrawingSession.DrawLine(t.LeftTop.X, t.LeftTop.Y, t.RightTop.X, t.RightTop.Y, Colors.OrangeRed, 2f);
                e.DrawingSession.DrawLine(t.RightTop.X, t.RightTop.Y, t.RightBottom.X, t.RightBottom.Y, Colors.Orange, 2f);
                e.DrawingSession.DrawLine(t.RightBottom.X, t.RightBottom.Y, t.LeftBottom.X, t.LeftBottom.Y, Colors.Gold, 2f);
                e.DrawingSession.DrawLine(t.LeftBottom.X, t.LeftBottom.Y, t.LeftTop.X, t.LeftTop.Y, Colors.GreenYellow, 2f);
            };
            this.CanvasControl3B.Draw += (s, e) =>
            {
                Bounds t = this.Bounds3;
                e.DrawingSession.DrawLine(t.Left, t.Top, t.Right, t.Top, Colors.OrangeRed, 2f);
                e.DrawingSession.DrawLine(t.Right, t.Top, t.Right, t.Bottom, Colors.Orange, 2f);
                e.DrawingSession.DrawLine(t.Right, t.Bottom, t.Left, t.Bottom, Colors.Gold, 2f);
                e.DrawingSession.DrawLine(t.Left, t.Bottom, t.Left, t.Top, Colors.GreenYellow, 2f);
            };

            this.CanvasControl4A.Draw += (s, e) =>
            {
                Triangle t = this.Triangle4;
                e.DrawingSession.DrawLine(t.LeftTop.X, t.LeftTop.Y, t.RightTop.X, t.RightTop.Y, Colors.OrangeRed, 2f);
                e.DrawingSession.DrawLine(t.RightTop.X, t.RightTop.Y, t.LeftBottom.X, t.LeftBottom.Y, Colors.Orange, 2f);
                e.DrawingSession.DrawLine(t.LeftBottom.X, t.LeftBottom.Y, t.LeftTop.X, t.LeftTop.Y, Colors.Gold, 2f);
            };
            this.CanvasControl4B.Draw += (s, e) =>
            {
                Quadrilateral t = this.Quadrilateral4;
                e.DrawingSession.DrawLine(t.LeftTop.X, t.LeftTop.Y, t.RightTop.X, t.RightTop.Y, Colors.OrangeRed, 2f);
                e.DrawingSession.DrawLine(t.RightTop.X, t.RightTop.Y, t.RightBottom.X, t.RightBottom.Y, Colors.Orange, 2f);
                e.DrawingSession.DrawLine(t.RightBottom.X, t.RightBottom.Y, t.LeftBottom.X, t.LeftBottom.Y, Colors.Gold, 2f);
                e.DrawingSession.DrawLine(t.LeftBottom.X, t.LeftBottom.Y, t.LeftTop.X, t.LeftTop.Y, Colors.GreenYellow, 2f);
            };

            this.CanvasControl5A.Draw += (s, e) =>
            {
                Rectangle t = this.Rectangle5;
                e.DrawingSession.DrawLine(t.X, t.Y, t.X + t.Width, t.Y, Colors.OrangeRed, 2f);
                e.DrawingSession.DrawLine(t.X + t.Width, t.Y, t.X + t.Width, t.Y + t.Height, Colors.Orange, 2f);
                e.DrawingSession.DrawLine(t.X + t.Width, t.Y + t.Height, t.X, t.Y + t.Height, Colors.Gold, 2f);
                e.DrawingSession.DrawLine(t.X, t.Y + t.Height, t.X, t.Y, Colors.GreenYellow, 2f);
            };
            this.CanvasControl5B.Draw += (s, e) =>
            {
                Quadrilateral t = this.Quadrilateral5;
                e.DrawingSession.DrawLine(t.LeftTop.X, t.LeftTop.Y, t.RightTop.X, t.RightTop.Y, Colors.OrangeRed, 2f);
                e.DrawingSession.DrawLine(t.RightTop.X, t.RightTop.Y, t.RightBottom.X, t.RightBottom.Y, Colors.Orange, 2f);
                e.DrawingSession.DrawLine(t.RightBottom.X, t.RightBottom.Y, t.LeftBottom.X, t.LeftBottom.Y, Colors.Gold, 2f);
                e.DrawingSession.DrawLine(t.LeftBottom.X, t.LeftBottom.Y, t.LeftTop.X, t.LeftTop.Y, Colors.GreenYellow, 2f);
            };
        }
    }
}