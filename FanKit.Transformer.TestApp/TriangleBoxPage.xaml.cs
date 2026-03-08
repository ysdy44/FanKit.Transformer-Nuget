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
    public abstract partial class TriangleBoxPage : Page
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

        public TransformController Controller;
        public ControllerRadians Radians;

        public Triangle StartingTriangle;
        public Triangle Triangle = new Triangle
        {
            LeftTop = new Vector2(X, Y),
            RightTop = new Vector2(X + W, Y),
            LeftBottom = new Vector2(X, Y + H),
        };

        // Canvas
        readonly CanvasOperator1 CanvasOperator;

        public TriangleBoxPage()
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

    public sealed partial class TriangleBox0Page : TriangleBoxPage
    {
        BoxContainsNodeMode0 Mode;

        Box0 Box;

        public TriangleBox0Page()
        {
            this.Box = new Box0(this.Triangle);
        }

        public override void CacheSingle(double startingX, double startingY)
        {
            const float d = 18f;
            const float ds = d * d;

            this.StartingPoint = this.Point = new Vector2((float)startingX, (float)startingY);

            {
                this.Mode = this.Box.ContainsNode(this.StartingPoint, ds);
            }

            switch (this.Mode)
            {
                case BoxContainsNodeMode0.None: break;
                default: this.StartingTriangle = this.Triangle; break;
            }

            switch (this.Mode)
            {
                case BoxContainsNodeMode0.None: break;
                case BoxContainsNodeMode0.Contains: break;

                case BoxContainsNodeMode0.LeftTop: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleLeftTop); break;
                case BoxContainsNodeMode0.RightTop: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleRightTop); break;
                case BoxContainsNodeMode0.LeftBottom: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleLeftBottom); break;
                case BoxContainsNodeMode0.RightBottom: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleRightBottom); break;
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
                    this.Triangle = this.StartingTriangle + this.Point - this.StartingPoint;
                    this.Box = new Box0(this.Triangle);
                    break;
                default:
                    this.Triangle = this.Controller.Transform(this.StartingTriangle, this.Point, this.KeepRatio, this.CenteredScaling);
                    this.Box = new Box0(this.Triangle);
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

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            const float d = 12 * 2;
            const float ds = d * d;

            drawingSession.Blend = CanvasBlend.Copy;

            drawingSession.FillGeometry(CanvasGeometry.CreatePolygon(resourceCreator, new Vector2[]
            {
                this.Box.LeftTop,
                this.Box.RightTop,
                this.Box.RightBottom,
                this.Box.LeftBottom,
            }), this.Mode == BoxContainsNodeMode0.Contains ? Blue3 : Blue4);

            // Lines
            drawingSession.DrawLine(this.Box.LeftTop, this.Box.RightTop, Blue1, 2f);
            drawingSession.DrawLine(this.Box.RightTop, this.Box.RightBottom, Blue1, 2f);
            drawingSession.DrawLine(this.Box.RightBottom, this.Box.LeftBottom, Blue1, 2f);
            drawingSession.DrawLine(this.Box.LeftBottom, this.Box.LeftTop, Blue1, 2f);

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
            drawingSession.DrawCircle(this.Box.RightBottom, 18f, Blue1, 2f);
        }
    }

    public sealed partial class TriangleBox1Page : TriangleBoxPage
    {
        BoxContainsNodeMode1 Mode;

        Box1 Box;

        public TriangleBox1Page()
        {
            this.Box = new Box1(this.Triangle);
        }

        public override void CacheSingle(double startingX, double startingY)
        {
            const float d = 18f;
            const float ds = d * d;

            this.StartingPoint = this.Point = new Vector2((float)startingX, (float)startingY);

            {
                this.Mode = this.Box.ContainsNode(this.StartingPoint, ds);
            }

            switch (this.Mode)
            {
                case BoxContainsNodeMode1.None: break;
                default: this.StartingTriangle = this.Triangle; break;
            }

            switch (this.Mode)
            {
                case BoxContainsNodeMode1.None: break;
                case BoxContainsNodeMode1.Contains: break;

                case BoxContainsNodeMode1.CenterLeft: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleLeft); break;
                case BoxContainsNodeMode1.CenterTop: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleTop); break;
                case BoxContainsNodeMode1.CenterRight: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleRight); break;
                case BoxContainsNodeMode1.CenterBottom: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleBottom); break;

                case BoxContainsNodeMode1.LeftTop: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleLeftTop); break;
                case BoxContainsNodeMode1.RightTop: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleRightTop); break;
                case BoxContainsNodeMode1.LeftBottom: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleLeftBottom); break;
                case BoxContainsNodeMode1.RightBottom: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleRightBottom); break;
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
                    this.Triangle = this.StartingTriangle + this.Point - this.StartingPoint;
                    this.Box = new Box1(this.Triangle);
                    break;
                default:
                    this.Triangle = this.Controller.Transform(this.StartingTriangle, this.Point, this.KeepRatio, this.CenteredScaling);
                    this.Box = new Box1(this.Triangle);
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

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            const float d = 12 * 2;
            const float ds = d * d;

            drawingSession.Blend = CanvasBlend.Copy;

            drawingSession.FillGeometry(CanvasGeometry.CreatePolygon(resourceCreator, new Vector2[]
            {
                this.Box.LeftTop,
                this.Box.RightTop,
                this.Box.RightBottom,
                this.Box.LeftBottom,
            }), this.Mode == BoxContainsNodeMode1.Contains ? Blue3 : Blue4);

            // Lines
            drawingSession.DrawLine(this.Box.LeftTop, this.Box.RightTop, Blue1, 2f);
            drawingSession.DrawLine(this.Box.RightTop, this.Box.RightBottom, Blue1, 2f);
            drawingSession.DrawLine(this.Box.RightBottom, this.Box.LeftBottom, Blue1, 2f);
            drawingSession.DrawLine(this.Box.LeftBottom, this.Box.LeftTop, Blue1, 2f);

            // Sides
            if (this.Box.SideLeftLengthSquared > ds)
            {
                drawingSession.FillCircle(this.Box.CenterLeft, 18f, this.Mode == BoxContainsNodeMode1.CenterLeft ? Blue2 : Blue3);
            }

            if (this.Box.SideTopLengthSquared > ds)
            {
                drawingSession.FillCircle(this.Box.CenterTop, 18f, this.Mode == BoxContainsNodeMode1.CenterTop ? Blue2 : Blue3);
            }

            if (this.Box.SideRightLengthSquared > ds)
            {
                drawingSession.FillCircle(this.Box.CenterRight, 18f, this.Mode == BoxContainsNodeMode1.CenterRight ? Blue2 : Blue3);
            }

            if (this.Box.SideBottomLengthSquared > ds)
            {
                drawingSession.FillCircle(this.Box.CenterBottom, 18f, this.Mode == BoxContainsNodeMode1.CenterBottom ? Blue2 : Blue3);
            }

            // Sides
            if (this.Box.SideLeftLengthSquared > ds)
            {
                drawingSession.DrawCircle(this.Box.CenterLeft, 18f, Blue1, 2f);
            }

            if (this.Box.SideTopLengthSquared > ds)
            {
                drawingSession.DrawCircle(this.Box.CenterTop, 18f, Blue1, 2f);
            }

            if (this.Box.SideRightLengthSquared > ds)
            {
                drawingSession.DrawCircle(this.Box.CenterRight, 18f, Blue1, 2f);
            }

            if (this.Box.SideBottomLengthSquared > ds)
            {
                drawingSession.DrawCircle(this.Box.CenterBottom, 18f, Blue1, 2f);
            }

            // Corners
            drawingSession.FillCircle(this.Box.LeftTop, 18f, this.Mode == BoxContainsNodeMode1.LeftTop ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.RightTop, 18f, this.Mode == BoxContainsNodeMode1.RightTop ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.LeftBottom, 18f, this.Mode == BoxContainsNodeMode1.LeftBottom ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.RightBottom, 18f, this.Mode == BoxContainsNodeMode1.RightBottom ? Blue2 : Blue3);

            // Corners
            drawingSession.DrawCircle(this.Box.LeftTop, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.RightTop, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.LeftBottom, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.RightBottom, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.RightBottom, 18f, Blue1, 2f);
        }
    }

    public sealed partial class TriangleBox2Page : TriangleBoxPage
    {
        BoxContainsNodeMode2 Mode;

        Box2 Box;

        public TriangleBox2Page()
        {
            this.Box = new Box2(this.Triangle);
        }

        public override void CacheSingle(double startingX, double startingY)
        {
            const float d = 18f;
            const float ds = d * d;

            this.StartingPoint = this.Point = new Vector2((float)startingX, (float)startingY);

            {
                this.Mode = this.Box.ContainsNode(this.StartingPoint, ds);
            }

            switch (this.Mode)
            {
                case BoxContainsNodeMode2.None: break;
                default: this.StartingTriangle = this.Triangle; break;
            }

            switch (this.Mode)
            {
                case BoxContainsNodeMode2.None: break;
                case BoxContainsNodeMode2.Contains: break;

                case BoxContainsNodeMode2.HandleLeft: break;
                case BoxContainsNodeMode2.HandleTop: this.Controller = new TransformController(this.Triangle, this.StartingPoint); break;
                case BoxContainsNodeMode2.HandleRight: this.Controller = new TransformController(this.StartingTriangle, TransformMode.SkewRight); break;
                case BoxContainsNodeMode2.HandleBottom: this.Controller = new TransformController(this.StartingTriangle, TransformMode.SkewBottom); break;

                case BoxContainsNodeMode2.CenterLeft: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleLeft); break;
                case BoxContainsNodeMode2.CenterTop: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleTop); break;
                case BoxContainsNodeMode2.CenterRight: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleRight); break;
                case BoxContainsNodeMode2.CenterBottom: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleBottom); break;

                case BoxContainsNodeMode2.LeftTop: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleLeftTop); break;
                case BoxContainsNodeMode2.RightTop: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleRightTop); break;
                case BoxContainsNodeMode2.LeftBottom: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleLeftBottom); break;
                case BoxContainsNodeMode2.RightBottom: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleRightBottom); break;
                default: break;
            }
        }

        public override void Single(double x, double y)
        {
            this.Point = new Vector2((float)x, (float)y);

            switch (this.Mode)
            {
                case BoxContainsNodeMode2.None:
                    break;
                case BoxContainsNodeMode2.Contains:
                    this.Triangle = this.StartingTriangle + this.Point - this.StartingPoint;
                    this.Box = new Box2(this.Triangle);
                    break;
                case BoxContainsNodeMode2.HandleTop:
                    this.Radians = this.Controller.ToRadians(this.Point);

                    Matrix3x2 matrix = this.Controller.Rotate(this.Radians);
                    this.Triangle = Triangle.Transform(this.StartingTriangle, matrix);
                    this.Box = new Box2(this.Triangle);
                    break;
                default:
                    this.Triangle = this.Controller.Transform(this.StartingTriangle, this.Point, this.KeepRatio, this.CenteredScaling);
                    this.Box = new Box2(this.Triangle);
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

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            const float d = 12 * 2;
            const float ds = d * d;

            drawingSession.Blend = CanvasBlend.Copy;

            drawingSession.FillGeometry(CanvasGeometry.CreatePolygon(resourceCreator, new Vector2[]
            {
                this.Box.LeftTop,
                this.Box.RightTop,
                this.Box.RightBottom,
                this.Box.LeftBottom,
            }), this.Mode == BoxContainsNodeMode2.Contains ? Blue3 : Blue4);

            // Handle Sides
            drawingSession.DrawLine(this.Box.CenterBottom, this.Box.HandleBottom, Blue1, 2f);
            drawingSession.DrawLine(this.Box.CenterRight, this.Box.HandleRight, Blue1, 2f);
            drawingSession.DrawLine(this.Box.CenterTop, this.Box.HandleTop, Blue1, 2f);
            //drawingSession.DrawLine(this.Box.CenterLeft, this.Box.HandleLeft, Blue1, 2f);

            // Lines
            drawingSession.DrawLine(this.Box.LeftTop, this.Box.RightTop, Blue1, 2f);
            drawingSession.DrawLine(this.Box.RightTop, this.Box.RightBottom, Blue1, 2f);
            drawingSession.DrawLine(this.Box.RightBottom, this.Box.LeftBottom, Blue1, 2f);
            drawingSession.DrawLine(this.Box.LeftBottom, this.Box.LeftTop, Blue1, 2f);

            // Handle Sides
            drawingSession.FillCircle(this.Box.HandleBottom, 18f, this.Mode == BoxContainsNodeMode2.HandleBottom ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.HandleRight, 18f, this.Mode == BoxContainsNodeMode2.HandleRight ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.HandleTop, 18f, this.Mode == BoxContainsNodeMode2.HandleTop ? Blue2 : Blue3);
            //drawingSession.FillCircle(this.Box.HandleLeft, 18f, this.Mode == BoxContainsNodeMode2.HandleLeft ? Blue2 : Blue3);

            // Handle Sides
            drawingSession.DrawCircle(this.Box.HandleBottom, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.HandleRight, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.HandleTop, 18f, Blue1, 2f);
            //drawingSession.DrawCircle(this.Box.HandleLeft, 18f, Blue1, 2f);

            // Sides
            if (this.Box.SideLeftLengthSquared > ds)
            {
                drawingSession.FillCircle(this.Box.CenterLeft, 18f, this.Mode == BoxContainsNodeMode2.CenterLeft ? Blue2 : Blue3);
            }

            if (this.Box.SideTopLengthSquared > ds)
            {
                drawingSession.FillCircle(this.Box.CenterTop, 18f, this.Mode == BoxContainsNodeMode2.CenterTop ? Blue2 : Blue3);
            }

            if (this.Box.SideRightLengthSquared > ds)
            {
                drawingSession.FillCircle(this.Box.CenterRight, 18f, this.Mode == BoxContainsNodeMode2.CenterRight ? Blue2 : Blue3);
            }

            if (this.Box.SideBottomLengthSquared > ds)
            {
                drawingSession.FillCircle(this.Box.CenterBottom, 18f, this.Mode == BoxContainsNodeMode2.CenterBottom ? Blue2 : Blue3);
            }

            // Sides
            if (this.Box.SideLeftLengthSquared > ds)
            {
                drawingSession.DrawCircle(this.Box.CenterLeft, 18f, Blue1, 2f);
            }

            if (this.Box.SideTopLengthSquared > ds)
            {
                drawingSession.DrawCircle(this.Box.CenterTop, 18f, Blue1, 2f);
            }

            if (this.Box.SideRightLengthSquared > ds)
            {
                drawingSession.DrawCircle(this.Box.CenterRight, 18f, Blue1, 2f);
            }

            if (this.Box.SideBottomLengthSquared > ds)
            {
                drawingSession.DrawCircle(this.Box.CenterBottom, 18f, Blue1, 2f);
            }

            // Corners
            drawingSession.FillCircle(this.Box.LeftTop, 18f, this.Mode == BoxContainsNodeMode2.LeftTop ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.RightTop, 18f, this.Mode == BoxContainsNodeMode2.RightTop ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.LeftBottom, 18f, this.Mode == BoxContainsNodeMode2.LeftBottom ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.RightBottom, 18f, this.Mode == BoxContainsNodeMode2.RightBottom ? Blue2 : Blue3);

            // Corners
            drawingSession.DrawCircle(this.Box.LeftTop, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.RightTop, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.LeftBottom, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.RightBottom, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.RightBottom, 18f, Blue1, 2f);
        }
    }

    public sealed partial class TriangleBox3Page : TriangleBoxPage
    {
        BoxContainsNodeMode3 Mode;

        Box3 Box;

        public TriangleBox3Page()
        {
            this.Box = new Box3(this.Triangle);
        }

        public override void CacheSingle(double startingX, double startingY)
        {
            const float d = 18f;
            const float ds = d * d;

            this.StartingPoint = this.Point = new Vector2((float)startingX, (float)startingY);

            {
                this.Mode = this.Box.ContainsNode(this.StartingPoint, ds);
            }

            switch (this.Mode)
            {
                case BoxContainsNodeMode3.None: break;
                default: this.StartingTriangle = this.Triangle; break;
            }

            switch (this.Mode)
            {
                case BoxContainsNodeMode3.None: break;
                case BoxContainsNodeMode3.Contains: break;

                case BoxContainsNodeMode3.HandleLeftTop:
                case BoxContainsNodeMode3.HandleRightTop:
                case BoxContainsNodeMode3.HandleLeftBottom:
                case BoxContainsNodeMode3.HandleRightBottom: this.Controller = new TransformController(this.Triangle, this.StartingPoint); break;

                case BoxContainsNodeMode3.HandleLeft: this.Controller = new TransformController(this.StartingTriangle, TransformMode.SkewLeft); break;
                case BoxContainsNodeMode3.HandleTop: this.Controller = new TransformController(this.StartingTriangle, TransformMode.SkewTop); break;
                case BoxContainsNodeMode3.HandleRight: this.Controller = new TransformController(this.StartingTriangle, TransformMode.SkewRight); break;
                case BoxContainsNodeMode3.HandleBottom: this.Controller = new TransformController(this.StartingTriangle, TransformMode.SkewBottom); break;

                case BoxContainsNodeMode3.CenterLeft: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleLeft); break;
                case BoxContainsNodeMode3.CenterTop: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleTop); break;
                case BoxContainsNodeMode3.CenterRight: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleRight); break;
                case BoxContainsNodeMode3.CenterBottom: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleBottom); break;

                case BoxContainsNodeMode3.LeftTop: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleLeftTop); break;
                case BoxContainsNodeMode3.RightTop: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleRightTop); break;
                case BoxContainsNodeMode3.LeftBottom: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleLeftBottom); break;
                case BoxContainsNodeMode3.RightBottom: this.Controller = new TransformController(this.StartingTriangle, TransformMode.ScaleRightBottom); break;
                default: break;
            }
        }

        public override void Single(double x, double y)
        {
            this.Point = new Vector2((float)x, (float)y);

            switch (this.Mode)
            {
                case BoxContainsNodeMode3.None:
                    break;
                case BoxContainsNodeMode3.Contains:
                    this.Triangle = this.StartingTriangle + this.Point - this.StartingPoint;
                    this.Box = new Box3(this.Triangle);
                    break;
                case BoxContainsNodeMode3.HandleLeftTop:
                case BoxContainsNodeMode3.HandleRightTop:
                case BoxContainsNodeMode3.HandleLeftBottom:
                case BoxContainsNodeMode3.HandleRightBottom:
                    this.Radians = this.Controller.ToRadians(this.Point);

                    Matrix3x2 matrix = this.Controller.Rotate(this.Radians);
                    this.Triangle = Triangle.Transform(this.StartingTriangle, matrix);
                    this.Box = new Box3(this.Triangle);
                    break;
                default:
                    this.Triangle = this.Controller.Transform(this.StartingTriangle, this.Point, this.KeepRatio, this.CenteredScaling);
                    this.Box = new Box3(this.Triangle);
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

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            const float d = 12 * 2;
            const float ds = d * d;

            drawingSession.Blend = CanvasBlend.Copy;

            drawingSession.FillGeometry(CanvasGeometry.CreatePolygon(resourceCreator, new Vector2[]
            {
                this.Box.LeftTop,
                this.Box.RightTop,
                this.Box.RightBottom,
                this.Box.LeftBottom,
            }), this.Mode == BoxContainsNodeMode3.Contains ? Blue3 : Blue4);

            // Handle Corners
            drawingSession.DrawLine(this.Box.LeftTop, this.Box.HandleLeftTop, Blue1, 2f);
            drawingSession.DrawLine(this.Box.RightTop, this.Box.HandleRightTop, Blue1, 2f);
            drawingSession.DrawLine(this.Box.RightBottom, this.Box.HandleRightBottom, Blue1, 2f);
            drawingSession.DrawLine(this.Box.LeftBottom, this.Box.HandleLeftBottom, Blue1, 2f);

            // Handle Sides
            drawingSession.DrawLine(this.Box.CenterBottom, this.Box.HandleBottom, Blue1, 2f);
            drawingSession.DrawLine(this.Box.CenterRight, this.Box.HandleRight, Blue1, 2f);
            drawingSession.DrawLine(this.Box.CenterTop, this.Box.HandleTop, Blue1, 2f);
            drawingSession.DrawLine(this.Box.CenterLeft, this.Box.HandleLeft, Blue1, 2f);

            // Lines
            drawingSession.DrawLine(this.Box.LeftTop, this.Box.RightTop, Blue1, 2f);
            drawingSession.DrawLine(this.Box.RightTop, this.Box.RightBottom, Blue1, 2f);
            drawingSession.DrawLine(this.Box.RightBottom, this.Box.LeftBottom, Blue1, 2f);
            drawingSession.DrawLine(this.Box.LeftBottom, this.Box.LeftTop, Blue1, 2f);

            // Handle Corners
            drawingSession.FillCircle(this.Box.HandleLeftTop, 18f, this.Mode == BoxContainsNodeMode3.HandleLeftTop ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.HandleRightTop, 18f, this.Mode == BoxContainsNodeMode3.HandleRightTop ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.HandleLeftBottom, 18f, this.Mode == BoxContainsNodeMode3.HandleLeftBottom ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.HandleRightBottom, 18f, this.Mode == BoxContainsNodeMode3.HandleRightBottom ? Blue2 : Blue3);

            // Handle Corners
            drawingSession.DrawCircle(this.Box.HandleLeftTop, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.HandleRightTop, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.HandleLeftBottom, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.HandleRightBottom, 18f, Blue1, 2f);

            // Handle Sides
            drawingSession.FillCircle(this.Box.HandleBottom, 18f, this.Mode == BoxContainsNodeMode3.HandleBottom ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.HandleRight, 18f, this.Mode == BoxContainsNodeMode3.HandleRight ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.HandleTop, 18f, this.Mode == BoxContainsNodeMode3.HandleTop ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.HandleLeft, 18f, this.Mode == BoxContainsNodeMode3.HandleLeft ? Blue2 : Blue3);

            // Handle Sides
            drawingSession.DrawCircle(this.Box.HandleBottom, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.HandleRight, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.HandleTop, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.HandleLeft, 18f, Blue1, 2f);

            // Sides
            if (this.Box.SideLeftLengthSquared > ds)
            {
                drawingSession.FillCircle(this.Box.CenterLeft, 18f, this.Mode == BoxContainsNodeMode3.CenterLeft ? Blue2 : Blue3);
            }

            if (this.Box.SideTopLengthSquared > ds)
            {
                drawingSession.FillCircle(this.Box.CenterTop, 18f, this.Mode == BoxContainsNodeMode3.CenterTop ? Blue2 : Blue3);
            }

            if (this.Box.SideRightLengthSquared > ds)
            {
                drawingSession.FillCircle(this.Box.CenterRight, 18f, this.Mode == BoxContainsNodeMode3.CenterRight ? Blue2 : Blue3);
            }

            if (this.Box.SideBottomLengthSquared > ds)
            {
                drawingSession.FillCircle(this.Box.CenterBottom, 18f, this.Mode == BoxContainsNodeMode3.CenterBottom ? Blue2 : Blue3);
            }

            // Sides
            if (this.Box.SideLeftLengthSquared > ds)
            {
                drawingSession.DrawCircle(this.Box.CenterLeft, 18f, Blue1, 2f);
            }

            if (this.Box.SideTopLengthSquared > ds)
            {
                drawingSession.DrawCircle(this.Box.CenterTop, 18f, Blue1, 2f);
            }

            if (this.Box.SideRightLengthSquared > ds)
            {
                drawingSession.DrawCircle(this.Box.CenterRight, 18f, Blue1, 2f);
            }

            if (this.Box.SideBottomLengthSquared > ds)
            {
                drawingSession.DrawCircle(this.Box.CenterBottom, 18f, Blue1, 2f);
            }

            // Corners
            drawingSession.FillCircle(this.Box.LeftTop, 18f, this.Mode == BoxContainsNodeMode3.LeftTop ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.RightTop, 18f, this.Mode == BoxContainsNodeMode3.RightTop ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.LeftBottom, 18f, this.Mode == BoxContainsNodeMode3.LeftBottom ? Blue2 : Blue3);
            drawingSession.FillCircle(this.Box.RightBottom, 18f, this.Mode == BoxContainsNodeMode3.RightBottom ? Blue2 : Blue3);

            // Corners
            drawingSession.DrawCircle(this.Box.LeftTop, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.RightTop, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.LeftBottom, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.RightBottom, 18f, Blue1, 2f);
            drawingSession.DrawCircle(this.Box.RightBottom, 18f, Blue1, 2f);
        }
    }
}