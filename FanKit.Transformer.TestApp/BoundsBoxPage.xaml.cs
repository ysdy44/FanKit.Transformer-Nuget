using FanKit.Transformer.Cache;
using FanKit.Transformer.Controllers;
using Microsoft.Graphics.Canvas;
using System.Numerics;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public abstract partial class BoundsBoxPage : Page
    {
        //@Key
        bool IsKeyDown(VirtualKey key) => Window.Current.CoreWindow.GetKeyState(key).HasFlag(CoreVirtualKeyStates.Down);
        bool IsCtrl => this.IsKeyDown(VirtualKey.Control);
        bool IsShift => this.IsKeyDown(VirtualKey.Shift);

        public bool CenteredScaling => this.IsCtrl || this.ToolBox1.CenteredScaling;
        public bool KeepRatio => this.IsShift || this.ToolBox1.KeepRatio;

        //@Const
        const float X = 200;
        const float Y = 200;
        const float W = 256;
        const float H = 256;

        public static readonly Windows.UI.Color Blue1 = Windows.UI.Colors.DeepSkyBlue;
        public static readonly Windows.UI.Color Blue2 = Windows.UI.Color.FromArgb(127, 0, 191, 255);
        public static readonly Windows.UI.Color Blue3 = Windows.UI.Color.FromArgb(63, 0, 191, 255);
        public static readonly Windows.UI.Color Blue4 = Windows.UI.Color.FromArgb(31, 0, 191, 255);

        // Singe
        public Vector2 StartingPoint;
        public Vector2 Point;

        public CropController Controller;

        public Bounds StartingBounds;
        public Bounds Bounds = new Bounds
        {
            Left = X,
            Top = Y,
            Right = X + W,
            Bottom = Y + H,
        };

        // Canvas
        readonly CanvasOperator1 CanvasOperator;

        public BoundsBoxPage()
        {
            this.InitializeComponent();
            this.CanvasOperator = new CanvasOperator1(this.CanvasControl);

            this.CanvasControl.Draw += (s, e) => this.Draw(e.DrawingSession);

            this.CanvasOperator.Single_Start += (startingX, startingY, p) =>
            {
                this.CacheSingle(startingX, startingY);
            };
            this.CanvasOperator.Single_Delta += (x, y, p) =>
            {
                this.Single(x, y);
                this.CanvasControl.Invalidate();
            };
            this.CanvasOperator.Single_Complete += (x, y, p) =>
            {
                this.CanvasControl.Invalidate();
            };
            this.CanvasOperator.Pointer_Over += (x, y) =>
            {
                this.Over(x, y);
                this.CanvasControl.Invalidate();
            };
        }

        public abstract void CacheSingle(double startingX, double startingY);
        public abstract void Single(double x, double y);
        public abstract void Over(double x, double y);
        public abstract void Draw(CanvasDrawingSession drawingSession);
    }

    public sealed partial class BoundsBox0Page : BoundsBoxPage
    {
        BoxContainsNodeMode0 Mode;

        Box0 Box;

        public BoundsBox0Page()
        {
            this.Box = new Box0(this.Bounds);
        }

        public override void CacheSingle(double startingX, double startingY)
        {
            const float d = 18f;
            const float ds = d * d;

            this.StartingPoint = this.Point = new Vector2((float)startingX, (float)startingY);

            this.Mode = this.Box.ContainsNode(this.Point, ds);

            this.StartingBounds = this.Bounds;

            switch (this.Mode)
            {
                case BoxContainsNodeMode0.None: break;
                case BoxContainsNodeMode0.Contains: break;
                case BoxContainsNodeMode0.LeftTop: this.Controller = new CropController(this.StartingBounds, CropMode.ScaleLeftTop); break;
                case BoxContainsNodeMode0.RightTop: this.Controller = new CropController(this.StartingBounds, CropMode.ScaleRightTop); break;
                case BoxContainsNodeMode0.LeftBottom: this.Controller = new CropController(this.StartingBounds, CropMode.ScaleLeftBottom); break;
                case BoxContainsNodeMode0.RightBottom: this.Controller = new CropController(this.StartingBounds, CropMode.ScaleRightBottom); break;
                default: break;
            }
        }

        public override void Single(double x, double y)
        {
            this.Point = new Vector2((float)x, (float)y);

            switch (this.Mode)
            {
                case BoxContainsNodeMode0.None:
                    break;
                case BoxContainsNodeMode0.Contains:
                    this.Bounds = this.StartingBounds + this.Point - this.StartingPoint;
                    this.Box = new Box0(this.Bounds);
                    break;
                default:
                    this.Bounds = this.Controller.Crop(this.StartingBounds, this.Point, this.KeepRatio, this.CenteredScaling);
                    this.Box = new Box0(this.Bounds);
                    break;
            }

        }

        public override void Over(double x, double y)
        {
            const float d = 18f;
            const float ds = d * d;

            this.Point = new Vector2((float)x, (float)y);

            this.Mode = this.Box.ContainsNode(this.Point, ds);
        }

        public override void Draw(CanvasDrawingSession drawingSession)
        {
            drawingSession.Blend = CanvasBlend.Copy;

            var rect = this.Bounds.ToRect();

            drawingSession.FillRectangle(rect, this.Mode == BoxContainsNodeMode0.Contains ? Blue3 : Blue4);
            drawingSession.DrawRectangle(rect, Blue1, 2f);

            // Corners
            drawingSession.FillCircle(this.Box.LeftTop, 18f, this.Mode == BoxContainsNodeMode0.LeftTop ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.RightTop, 18f, this.Mode == BoxContainsNodeMode0.RightTop ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.LeftBottom, 18f, this.Mode == BoxContainsNodeMode0.LeftBottom ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.RightBottom, 18f, this.Mode == BoxContainsNodeMode0.RightBottom ? Blue2 : Blue3);

            // Corners
            drawingSession.DrawCircle(this.Box.LeftTop, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.RightTop, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.LeftBottom, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.RightBottom, 18f, Blue1, 2f);
        }
    }

    public sealed partial class BoundsBox1Page : BoundsBoxPage
    {
        BoxContainsNodeMode1 Mode;

        Box1 Box;

        public BoundsBox1Page()
        {
            this.Box = new Box1(this.Bounds);
        }

        public override void CacheSingle(double startingX, double startingY)
        {
            const float d = 18f;
            const float ds = d * d;

            this.StartingPoint = this.Point = new Vector2((float)startingX, (float)startingY);

            this.Mode = this.Box.ContainsNode(this.Point, ds);

            this.StartingBounds = this.Bounds;

            switch (this.Mode)
            {
                case BoxContainsNodeMode1.None: break;
                case BoxContainsNodeMode1.Contains: break;
                case BoxContainsNodeMode1.CenterLeft: this.Controller = new CropController(this.StartingBounds, CropMode.ScaleLeft); break;
                case BoxContainsNodeMode1.CenterTop: this.Controller = new CropController(this.StartingBounds, CropMode.ScaleTop); break;
                case BoxContainsNodeMode1.CenterRight: this.Controller = new CropController(this.StartingBounds, CropMode.ScaleRight); break;
                case BoxContainsNodeMode1.CenterBottom: this.Controller = new CropController(this.StartingBounds, CropMode.ScaleBottom); break;
                case BoxContainsNodeMode1.LeftTop: this.Controller = new CropController(this.StartingBounds, CropMode.ScaleLeftTop); break;
                case BoxContainsNodeMode1.RightTop: this.Controller = new CropController(this.StartingBounds, CropMode.ScaleRightTop); break;
                case BoxContainsNodeMode1.LeftBottom: this.Controller = new CropController(this.StartingBounds, CropMode.ScaleLeftBottom); break;
                case BoxContainsNodeMode1.RightBottom: this.Controller = new CropController(this.StartingBounds, CropMode.ScaleRightBottom); break;
                default: break;
            }
        }

        public override void Single(double x, double y)
        {
            this.Point = new Vector2((float)x, (float)y);

            switch (this.Mode)
            {
                case BoxContainsNodeMode1.None:
                    break;
                case BoxContainsNodeMode1.Contains:
                    this.Bounds = this.StartingBounds + this.Point - this.StartingPoint;
                    this.Box = new Box1(this.Bounds);
                    break;
                default:
                    this.Bounds = this.Controller.Crop(this.StartingBounds, this.Point, this.KeepRatio, this.CenteredScaling);
                    this.Box = new Box1(this.Bounds);
                    break;
            }
        }

        public override void Over(double x, double y)
        {
            const float d = 18f;
            const float ds = d * d;

            this.Point = new Vector2((float)x, (float)y);

            this.Mode = this.Box.ContainsNode(this.Point, ds);
        }

        public override void Draw(CanvasDrawingSession drawingSession)
        {
            drawingSession.Blend = CanvasBlend.Copy;

            var rect = this.Bounds.ToRect();

            drawingSession.FillRectangle(rect, this.Mode == BoxContainsNodeMode1.Contains ? Blue3 : Blue4);
            drawingSession.DrawRectangle(rect, Blue1, 2f);

            // Sides
            drawingSession.FillCircle(this.Box.CenterLeft, 18f, this.Mode == BoxContainsNodeMode1.CenterLeft ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.CenterTop, 18f, this.Mode == BoxContainsNodeMode1.CenterTop ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.CenterRight, 18f, this.Mode == BoxContainsNodeMode1.CenterRight ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.CenterBottom, 18f, this.Mode == BoxContainsNodeMode1.CenterBottom ? Blue2 : Blue3);

            // Corners
            drawingSession.FillCircle(this.Box.LeftTop, 18f, this.Mode == BoxContainsNodeMode1.LeftTop ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.RightTop, 18f, this.Mode == BoxContainsNodeMode1.RightTop ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.LeftBottom, 18f, this.Mode == BoxContainsNodeMode1.LeftBottom ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.RightBottom, 18f, this.Mode == BoxContainsNodeMode1.RightBottom ? Blue2 : Blue3);

            // Sides
            drawingSession.DrawCircle(this.Box.CenterLeft, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.CenterTop, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.CenterRight, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.CenterBottom, 18f, Blue1, 2f);

            // Corners
            drawingSession.DrawCircle(this.Box.LeftTop, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.RightTop, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.LeftBottom, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.RightBottom, 18f, Blue1, 2f);
        }
    }
}