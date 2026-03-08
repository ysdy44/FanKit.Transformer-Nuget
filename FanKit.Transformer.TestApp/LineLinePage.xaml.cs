using FanKit.Transformer.Cache;
using FanKit.Transformer.Controllers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public abstract partial class LineLinePage : Page
    {
        //@Key
        bool IsKeyDown(VirtualKey key) => Window.Current.CoreWindow.GetKeyState(key).HasFlag(CoreVirtualKeyStates.Down);
        bool IsCtrl => this.IsKeyDown(VirtualKey.Control);
        bool IsShift => this.IsKeyDown(VirtualKey.Shift);

        public bool HasStepFrequency => this.IsShift;

        //@Const
        const float X = 256f;
        const float Y = 256f;

        public const float StepFrequency = (float)System.Math.PI / 12f;
        public const float StepFrequencyHalf = (float)System.Math.PI / 24f;

        public static readonly Windows.UI.Color Blue1 = Windows.UI.Colors.DeepSkyBlue;
        public static readonly Windows.UI.Color Blue2 = Windows.UI.Color.FromArgb(127, 0, 191, 255);
        public static readonly Windows.UI.Color Blue3 = Windows.UI.Color.FromArgb(63, 0, 191, 255);
        public static readonly Windows.UI.Color Blue4 = Windows.UI.Color.FromArgb(31, 0, 191, 255);

        // Singe
        public Vector2 StartingPoint;
        public Vector2 Point;

        public LineControllerFoot Foot;
        public LineController Controller;
        public ControllerRadians Radians;

        public Vector2 StartingPoint0;
        public Vector2 StartingPoint1;
        public Vector2 Point0 = new Vector2(X - 100, Y);
        public Vector2 Point1 = new Vector2(X + 100, Y);

        // Canvas
        readonly CanvasOperator1 CanvasOperator;

        public LineLinePage()
        {
            this.InitializeComponent();
            this.CanvasOperator = new CanvasOperator1(this.CanvasControl);

            this.CanvasControl.Draw += (s, e) => this.Draw(s, e.DrawingSession);

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
        public abstract void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession);
    }

    public sealed partial class LineLine0Page : LineLinePage
    {
        LineContainsNodeMode0 Mode;

        Line0 Line;

        public LineLine0Page()
        {
            this.Line = new Line0(this.Point0, this.Point1);
        }

        public override void CacheSingle(double startingX, double startingY)
        {
            const float d = 18f;
            const float ds = d * d;

            this.StartingPoint = this.Point = new Vector2((float)startingX, (float)startingY);

            this.Mode = this.Line.ContainsNode(this.StartingPoint, ds);

            switch (this.Mode)
            {
                case LineContainsNodeMode0.None:
                    break;
                case LineContainsNodeMode0.Point0:
                case LineContainsNodeMode0.Point1:
                    this.StartingPoint0 = this.Point0;
                    this.StartingPoint1 = this.Point1;
                    break;
                default:
                    break;
            }
        }

        public override void Single(double x, double y)
        {
            this.Point = new Vector2((float)x, (float)y);

            switch (this.Mode)
            {
                case LineContainsNodeMode0.None:
                    break;
                case LineContainsNodeMode0.Point0:
                    this.Point0 = this.Point;

                    this.Line = new Line0(this.Point0, this.Point1);
                    break;
                case LineContainsNodeMode0.Point1:
                    this.Point1 = this.Point;

                    this.Line = new Line0(this.Point0, this.Point1);
                    break;
                default:
                    break;
            }
        }

        public override void Over(double x, double y)
        {
            const float d = 18f;
            const float ds = d * d;

            this.Point = new Vector2((float)x, (float)y);

            this.Mode = this.Line.ContainsNode(this.Point, ds);
        }

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            const float d = 12 * 2;
            const float ds = d * d;

            drawingSession.Blend = CanvasBlend.Copy;

            drawingSession.DrawLine(this.Line.Point0, this.Line.Point1, Blue1, 2f);

            // Lines
            drawingSession.DrawLine(this.Line.Point0, this.Line.Point1);

            // Line
            drawingSession.FillCircle(this.Line.Point0, 18f, this.Mode == LineContainsNodeMode0.Point0 ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Line.Point1, 18f, this.Mode == LineContainsNodeMode0.Point1 ? Blue2 : Blue3);

            // Line
            drawingSession.DrawCircle(this.Line.Point0, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Line.Point1, 18f, Blue1, 2f);
        }
    }

    public sealed partial class LineLine1Page : LineLinePage
    {
        LineContainsNodeMode1 Mode;

        Line1 Line;

        public LineLine1Page()
        {
            this.Line = new Line1(this.Point0, this.Point1);
        }

        public override void CacheSingle(double startingX, double startingY)
        {
            const float d = 18f;
            const float ds = d * d;

            this.StartingPoint = this.Point = new Vector2((float)startingX, (float)startingY);

            this.Mode = this.Line.ContainsNode(this.StartingPoint, ds);

            switch (this.Mode)
            {
                case LineContainsNodeMode1.None:
                    break;
                case LineContainsNodeMode1.Center:
                case LineContainsNodeMode1.Point0:
                case LineContainsNodeMode1.Point1:
                    this.StartingPoint0 = this.Point0;
                    this.StartingPoint1 = this.Point1;
                    break;
                default:
                    break;
            }
        }

        public override void Single(double x, double y)
        {
            this.Point = new Vector2((float)x, (float)y);

            switch (this.Mode)
            {
                case LineContainsNodeMode1.None:
                    break;
                case LineContainsNodeMode1.Center:
                    this.Point0 = this.StartingPoint0 + this.Point - this.StartingPoint;
                    this.Point1 = this.StartingPoint1 + this.Point - this.StartingPoint;

                    this.Line = new Line1(this.Point0, this.Point1);
                    break;
                case LineContainsNodeMode1.Point0:
                    this.Point0 = this.Point;

                    this.Line = new Line1(this.Point0, this.Point1);
                    break;
                case LineContainsNodeMode1.Point1:
                    this.Point1 = this.Point;

                    this.Line = new Line1(this.Point0, this.Point1);
                    break;
                default:
                    break;
            }
        }

        public override void Over(double x, double y)
        {
            const float d = 18f;
            const float ds = d * d;

            this.Point = new Vector2((float)x, (float)y);

            this.Mode = this.Line.ContainsNode(this.Point, ds);
        }

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            const float d = 12 * 2;
            const float ds = d * d;

            drawingSession.Blend = CanvasBlend.Copy;

            drawingSession.DrawLine(this.Line.Point0, this.Line.Point1, Blue1, 2f);

            // Lines
            drawingSession.DrawLine(this.Line.Point0, this.Line.Point1);

            // Sides
            if (this.Line.LengthSquared > ds)
            {
                drawingSession.FillCircle(this.Line.Center, 18f, this.Mode == LineContainsNodeMode1.Center ? Blue2 : Blue3);
                drawingSession.DrawCircle(this.Line.Center, 18f, Blue1, 2f);
            }

            // Line
            drawingSession.FillCircle(this.Line.Point0, 18f, this.Mode == LineContainsNodeMode1.Point0 ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Line.Point1, 18f, this.Mode == LineContainsNodeMode1.Point1 ? Blue2 : Blue3);

            // Line
            drawingSession.DrawCircle(this.Line.Point0, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Line.Point1, 18f, Blue1, 2f);
        }
    }

    public sealed partial class LineLine2Page : LineLinePage
    {
        LineContainsNodeMode2 Mode;

        Line2 Line;

        public LineLine2Page()
        {
            this.Line = new Line2(this.Point0, this.Point1);
        }

        public override void CacheSingle(double startingX, double startingY)
        {
            const float d = 18f;
            const float ds = d * d;

            this.StartingPoint = this.Point = new Vector2((float)startingX, (float)startingY);

            this.Mode = this.Line.ContainsNode(this.StartingPoint, ds);

            switch (this.Mode)
            {
                case LineContainsNodeMode2.None:
                    break;
                case LineContainsNodeMode2.Handle:
                    this.StartingPoint0 = this.Point0;
                    this.StartingPoint1 = this.Point1;

                    this.Controller = new LineController(this.StartingPoint0, this.StartingPoint1, this.StartingPoint);
                    break;
                case LineContainsNodeMode2.Center:
                case LineContainsNodeMode2.Point0:
                case LineContainsNodeMode2.Point1:
                    this.StartingPoint0 = this.Point0;
                    this.StartingPoint1 = this.Point1;
                    break;
                default:
                    break;
            }
        }

        public override void Single(double x, double y)
        {
            this.Point = new Vector2((float)x, (float)y);

            switch (this.Mode)
            {
                case LineContainsNodeMode2.None:
                    break;
                case LineContainsNodeMode2.Handle:
                    if (this.HasStepFrequency)
                        this.Radians = this.Controller.ToRadians(this.Point, StepFrequency);
                    else
                        this.Radians = this.Controller.ToRadians(this.Point);

                    Matrix3x2 matrix = this.Controller.Rotate(this.Radians);

                    this.Point0 = Vector2.Transform(this.StartingPoint0, matrix);
                    this.Point1 = Vector2.Transform(this.StartingPoint1, matrix);

                    this.Line = new Line2(this.Point0, this.Point1);
                    break;
                case LineContainsNodeMode2.Center:
                    this.Point0 = this.StartingPoint0 + this.Point - this.StartingPoint;
                    this.Point1 = this.StartingPoint1 + this.Point - this.StartingPoint;

                    this.Line = new Line2(this.Point0, this.Point1);
                    break;
                case LineContainsNodeMode2.Point0:
                    this.Point0 = this.Point;

                    this.Line = new Line2(this.Point0, this.Point1);
                    break;
                case LineContainsNodeMode2.Point1:
                    this.Point1 = this.Point;

                    this.Line = new Line2(this.Point0, this.Point1);
                    break;
                default:
                    break;
            }
        }

        public override void Over(double x, double y)
        {
            const float d = 18f;
            const float ds = d * d;

            this.Point = new Vector2((float)x, (float)y);

            this.Mode = this.Line.ContainsNode(this.Point, ds);
        }

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            const float d = 12 * 2;
            const float ds = d * d;

            drawingSession.Blend = CanvasBlend.Copy;

            drawingSession.DrawLine(this.Line.Point0, this.Line.Point1, Blue1, 2f);

            // Handle Sides
            drawingSession.DrawLine(this.Line.Center, this.Line.Handle);

            // Lines
            drawingSession.DrawLine(this.Line.Point0, this.Line.Point1);

            // Handle Sides
            drawingSession.FillCircle(this.Line.Handle, 18f, this.Mode == LineContainsNodeMode2.Handle ? Blue2 : Blue3);

            // Handle Sides
            drawingSession.DrawCircle(this.Line.Handle, 18f, Blue1, 2f);

            // Sides
            if (this.Line.LengthSquared > ds)
            {
                drawingSession.FillCircle(this.Line.Center, 18f, this.Mode == LineContainsNodeMode2.Center ? Blue2 : Blue3);
                drawingSession.DrawCircle(this.Line.Center, 18f, Blue1, 2f);
            }

            // Line
            drawingSession.FillCircle(this.Line.Point0, 18f, this.Mode == LineContainsNodeMode2.Point0 ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Line.Point1, 18f, this.Mode == LineContainsNodeMode2.Point1 ? Blue2 : Blue3);

            // Line
            drawingSession.DrawCircle(this.Line.Point0, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Line.Point1, 18f, Blue1, 2f);
        }
    }

    public sealed partial class LineLine3Page : LineLinePage
    {
        LineContainsNodeMode3 Mode;

        Line3 Line;

        public LineLine3Page()
        {
            this.Line = new Line3(this.Point0, this.Point1);
        }

        public override void CacheSingle(double startingX, double startingY)
        {
            const float d = 18f;
            const float ds = d * d;

            this.StartingPoint = this.Point = new Vector2((float)startingX, (float)startingY);

            this.Mode = this.Line.ContainsNode(this.StartingPoint, ds);

            switch (this.Mode)
            {
                case LineContainsNodeMode3.None: 
                    break;
                case LineContainsNodeMode3.Handle0:
                case LineContainsNodeMode3.Handle1:
                    this.StartingPoint0 = this.Point0;
                    this.StartingPoint1 = this.Point1;

                    this.Controller = new LineController(this.StartingPoint0, this.StartingPoint1);
                    break;
                case LineContainsNodeMode3.Handle:
                    this.StartingPoint0 = this.Point0;
                    this.StartingPoint1 = this.Point1;

                    this.Controller = new LineController(this.StartingPoint0, this.StartingPoint1, this.StartingPoint);
                    break;
                case LineContainsNodeMode3.Center:
                case LineContainsNodeMode3.Point0:
                case LineContainsNodeMode3.Point1:
                    this.StartingPoint0 = this.Point0;
                    this.StartingPoint1 = this.Point1;
                    break;
                default: 
                    break;
            }
        }

        public override void Single(double x, double y)
        {
            this.Point = new Vector2((float)x, (float)y);

            switch (this.Mode)
            {
                case LineContainsNodeMode3.None:
                    break;
                case LineContainsNodeMode3.Handle0:
                    this.Foot = new LineControllerFoot(this.Controller, this.StartingPoint1, this.Point, this.StartingPoint - this.StartingPoint0);
                    this.Point0 = this.Foot.Foot;

                    this.Line = new Line3(this.Point0, this.Point1);
                    break;
                case LineContainsNodeMode3.Handle1:
                    this.Foot = new LineControllerFoot(this.Controller, this.StartingPoint0, this.Point, this.StartingPoint - this.StartingPoint1);
                    this.Point1 = this.Foot.Foot;

                    this.Line = new Line3(this.Point0, this.Point1);
                    break;
                case LineContainsNodeMode3.Handle:
                    if (this.HasStepFrequency)
                        this.Radians = this.Controller.ToRadians(this.Point, StepFrequency);
                    else
                        this.Radians = this.Controller.ToRadians(this.Point);

                    Matrix3x2 matrix = this.Controller.Rotate(this.Radians);

                    this.Point0 = Vector2.Transform(this.StartingPoint0, matrix);
                    this.Point1 = Vector2.Transform(this.StartingPoint1, matrix);

                    this.Line = new Line3(this.Point0, this.Point1);
                    break;
                case LineContainsNodeMode3.Center:
                    this.Point0 = this.StartingPoint0 + this.Point - this.StartingPoint;
                    this.Point1 = this.StartingPoint1 + this.Point - this.StartingPoint;

                    this.Line = new Line3(this.Point0, this.Point1);
                    break;
                case LineContainsNodeMode3.Point0:
                    this.Point0 = this.Point;

                    this.Line = new Line3(this.Point0, this.Point1);
                    break;
                case LineContainsNodeMode3.Point1:
                    this.Point1 = this.Point;

                    this.Line = new Line3(this.Point0, this.Point1);
                    break;
                default:
                    break;
            }
        }

        public override void Over(double x, double y)
        {
            const float d = 18f;
            const float ds = d * d;

            this.Point = new Vector2((float)x, (float)y);

            this.Mode = this.Line.ContainsNode(this.Point, ds);
        }

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            const float d = 12 * 2;
            const float ds = d * d;

            drawingSession.Blend = CanvasBlend.Copy;

            drawingSession.DrawLine(this.Line.Point0, this.Line.Point1, Blue1, 2f);

            // Handle Sides
            drawingSession.DrawLine(this.Line.Center, this.Line.Handle);

            // Lines
            drawingSession.DrawLine(this.Line.Point0, this.Line.Point1);

            // Handle Corners
            drawingSession.FillCircle(this.Line.Handle0, 18f, this.Mode == LineContainsNodeMode3.Handle0 ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Line.Handle1, 18f, this.Mode == LineContainsNodeMode3.Handle1 ? Blue2 : Blue3);

            // Handle Corners
            drawingSession.DrawCircle(this.Line.Handle0, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Line.Handle1, 18f, Blue1, 2f);

            // Handle Sides
            drawingSession.FillCircle(this.Line.Handle, 18f, this.Mode == LineContainsNodeMode3.Handle ? Blue2 : Blue3);

            // Handle Sides
            drawingSession.DrawCircle(this.Line.Handle, 18f, Blue1, 2f);

            // Sides
            if (this.Line.LengthSquared > ds)
            {
                drawingSession.FillCircle(this.Line.Center, 18f, this.Mode == LineContainsNodeMode3.Center ? Blue2 : Blue3);
                drawingSession.DrawCircle(this.Line.Center, 18f, Blue1, 2f);
            }

            // Line
            drawingSession.FillCircle(this.Line.Point0, 18f, this.Mode == LineContainsNodeMode3.Point0 ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Line.Point1, 18f, this.Mode == LineContainsNodeMode3.Point1 ? Blue2 : Blue3);

            // Line
            drawingSession.DrawCircle(this.Line.Point0, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Line.Point1, 18f, Blue1, 2f);
        }
    }
}