using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Sample;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public abstract partial class SplitBezierPage : Page
    {
        //@Const
        public const float Circle = 0.552284749831f;
        public const float W = 400f;
        public const float H = 400f;

        public readonly static Vector2 P0 = Vector2.Zero;
        public readonly static Vector2 P1 = new Vector2(W * Circle, 0f);
        public readonly static Vector2 P12 = new Vector2(W, 0f);
        public readonly static Vector2 P2 = new Vector2(W, H - H * Circle);
        public readonly static Vector2 P3 = new Vector2(W, H);

        readonly Matrix3x2 Offset = Matrix3x2.CreateTranslation(8f, 8f);

        public SplitBezierPage()
        {
            this.InitializeComponent();
            this.Button.Content = 0.5;
            this.Slider.Value = 50d;
            if (this.IsAllVisible)
                this.ComboBox.Visibility = Visibility.Visible;
            else
                this.ComboBox.Visibility = Visibility.Collapsed;

            this.Button.Click += delegate { this.Slider.Value = 50d; };
            this.Slider.ValueChanged += (s, e) =>
            {
                float time = (float)(this.Slider.Value / 100);

                this.Extend(time, 1f - time);
                this.CanvasControl.Invalidate();
            };
            this.ComboBox.SelectionChanged += delegate
            {
                float time = (float)(this.Slider.Value / 100);

                this.Extend(time, 1f - time);
                this.CanvasControl.Invalidate();
            };

            this.ToggleSwitch.Toggled += delegate { this.CanvasControl.Invalidate(); };
            this.CanvasControl.Draw += (s, e) =>
            {
                e.DrawingSession.Transform = this.Offset;
                if (this.ToggleSwitch.IsOn)
                    this.Draw1(s, e.DrawingSession);
                else
                    this.Draw0(s, e.DrawingSession);
            };
        }

        public BezierType Type => (BezierType)this.ComboBox.SelectedIndex;
        public abstract bool IsAllVisible { get; }

        public abstract void Extend(float t, float i);
        public abstract void Draw0(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession);
        public abstract void Draw1(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession);

        public static void DrawLine0(CanvasDrawingSession drawingSession, Linear L, Vector2 P)
        {
            drawingSession.DrawLine(L.L0, L.L1, Colors.DodgerBlue, 2f);

            drawingSession.FillCircle(L.L0, 7f, Colors.White);
            drawingSession.FillCircle(L.L0, 5f, Colors.Orange);
            drawingSession.FillCircle(L.L1, 7f, Colors.White);
            drawingSession.FillCircle(L.L1, 5f, Colors.Orange);

            drawingSession.FillCircle(P, 7f, Colors.White);
            drawingSession.FillCircle(P, 5f, Colors.DodgerBlue);
        }
        public static void DrawLine1(CanvasDrawingSession drawingSession, Linear L, Vector2 P)
        {
            drawingSession.DrawLine(L.L0, P, Colors.Fuchsia, 2f);

            drawingSession.DrawLine(P, L.L1, Colors.Red, 2f);

            drawingSession.FillCircle(L.L0, 7f, Colors.White);
            drawingSession.FillCircle(L.L0, 5f, Colors.Fuchsia);

            drawingSession.FillCircle(L.L1, 7f, Colors.White);
            drawingSession.FillCircle(L.L1, 5f, Colors.Red);

            drawingSession.FillCircle(P, 7f, Colors.White);
            drawingSession.FillCircle(P, 5f, Colors.DodgerBlue);
        }

        public static void DrawQuadratic0(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, Quadratic Q, Linear L, Vector2 P)
        {
            drawingSession.DrawLine(Q.Q0, Q.Q1, Colors.MediumPurple, 2f);
            drawingSession.DrawLine(Q.Q1, Q.Q2, Colors.MediumPurple, 2f);

            drawingSession.DrawLine(L.L0, L.L1, Colors.Orange, 2f);

            using (CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator))
            {
                pathBuilder.BeginFigure(Q.Q0);
                pathBuilder.AddQuadraticBezier(Q.Q1, Q.Q2);
                pathBuilder.EndFigure(CanvasFigureLoop.Open);

                using (CanvasGeometry geometry = CanvasGeometry.CreatePath(pathBuilder))
                {
                    drawingSession.DrawGeometry(geometry, Colors.DodgerBlue, 2f);
                }
            }

            drawingSession.FillCircle(Q.Q0, 7f, Colors.White);
            drawingSession.FillCircle(Q.Q0, 5f, Colors.MediumPurple);
            drawingSession.FillCircle(Q.Q1, 7f, Colors.White);
            drawingSession.FillCircle(Q.Q1, 5f, Colors.MediumPurple);
            drawingSession.FillCircle(Q.Q2, 7f, Colors.White);
            drawingSession.FillCircle(Q.Q2, 5f, Colors.MediumPurple);

            drawingSession.FillCircle(L.L0, 7f, Colors.White);
            drawingSession.FillCircle(L.L0, 5f, Colors.Orange);
            drawingSession.FillCircle(L.L1, 7f, Colors.White);
            drawingSession.FillCircle(L.L1, 5f, Colors.Orange);

            drawingSession.FillCircle(P, 7f, Colors.White);
            drawingSession.FillCircle(P, 5f, Colors.DodgerBlue);
        }
        public static void DrawQuadratic1(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, Quadratic Q, Linear L, Vector2 P)
        {
            drawingSession.DrawLine(L.L0, Q.Q1, Colors.Gray, 2f);
            drawingSession.DrawLine(Q.Q1, L.L1, Colors.Gray, 2f);

            drawingSession.DrawLine(Q.Q0, L.L0, Colors.Fuchsia, 2f);
            drawingSession.DrawLine(L.L0, P, Colors.Fuchsia, 2f);

            drawingSession.DrawLine(P, L.L1, Colors.Red, 2f);
            drawingSession.DrawLine(L.L1, Q.Q2, Colors.Red, 2f);

            using (CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator))
            {
                pathBuilder.BeginFigure(Q.Q0);
                pathBuilder.AddQuadraticBezier(L.L0, P);
                pathBuilder.EndFigure(CanvasFigureLoop.Open);

                using (CanvasGeometry geometry = CanvasGeometry.CreatePath(pathBuilder))
                {
                    drawingSession.DrawGeometry(geometry, Colors.Fuchsia, 2f);
                }
            }

            using (CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator))
            {
                pathBuilder.BeginFigure(P);
                pathBuilder.AddQuadraticBezier(L.L1, Q.Q2);
                pathBuilder.EndFigure(CanvasFigureLoop.Open);

                using (CanvasGeometry geometry = CanvasGeometry.CreatePath(pathBuilder))
                {
                    drawingSession.DrawGeometry(geometry, Colors.Red, 2f);
                }
            }

            drawingSession.FillCircle(Q.Q1, 5f, Colors.Gray);

            drawingSession.FillCircle(Q.Q0, 7f, Colors.White);
            drawingSession.FillCircle(Q.Q0, 5f, Colors.Fuchsia);

            drawingSession.FillCircle(L.L0, 7f, Colors.White);
            drawingSession.FillCircle(L.L0, 5f, Colors.Fuchsia);

            drawingSession.FillCircle(Q.Q2, 7f, Colors.White);
            drawingSession.FillCircle(Q.Q2, 5f, Colors.Red);

            drawingSession.FillCircle(L.L1, 7f, Colors.White);
            drawingSession.FillCircle(L.L1, 5f, Colors.Red);

            drawingSession.FillCircle(P, 7f, Colors.White);
            drawingSession.FillCircle(P, 5f, Colors.DodgerBlue);
        }

        public static void DrawCubic0(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, Cubic C, Quadratic Q, Linear B1, Vector2 P)
        {
            drawingSession.DrawLine(C.C0, C.C1, Colors.LightSeaGreen, 2f);
            drawingSession.DrawLine(C.C1, C.C2, Colors.LightSeaGreen, 2f);
            drawingSession.DrawLine(C.C2, C.C3, Colors.LightSeaGreen, 2f);

            drawingSession.DrawLine(Q.Q0, Q.Q1, Colors.MediumPurple, 2f);
            drawingSession.DrawLine(Q.Q1, Q.Q2, Colors.MediumPurple, 2f);

            drawingSession.DrawLine(B1.L0, B1.L1, Colors.Orange, 2f);

            using (CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator))
            {
                pathBuilder.BeginFigure(C.C0);
                pathBuilder.AddCubicBezier(C.C1, C.C2, C.C3);
                pathBuilder.EndFigure(CanvasFigureLoop.Open);

                using (CanvasGeometry geometry = CanvasGeometry.CreatePath(pathBuilder))
                {
                    drawingSession.DrawGeometry(geometry, Colors.DodgerBlue, 2f);
                }
            }

            drawingSession.FillCircle(C.C0, 7f, Colors.White);
            drawingSession.FillCircle(C.C0, 5f, Colors.LightSeaGreen);
            drawingSession.FillCircle(C.C1, 7f, Colors.White);
            drawingSession.FillCircle(C.C1, 5f, Colors.LightSeaGreen);
            drawingSession.FillCircle(C.C2, 7f, Colors.White);
            drawingSession.FillCircle(C.C2, 5f, Colors.LightSeaGreen);
            drawingSession.FillCircle(C.C3, 7f, Colors.White);
            drawingSession.FillCircle(C.C3, 5f, Colors.LightSeaGreen);

            drawingSession.FillCircle(Q.Q0, 7f, Colors.White);
            drawingSession.FillCircle(Q.Q0, 5f, Colors.MediumPurple);
            drawingSession.FillCircle(Q.Q1, 7f, Colors.White);
            drawingSession.FillCircle(Q.Q1, 5f, Colors.MediumPurple);
            drawingSession.FillCircle(Q.Q2, 7f, Colors.White);
            drawingSession.FillCircle(Q.Q2, 5f, Colors.MediumPurple);

            drawingSession.FillCircle(B1.L0, 7f, Colors.White);
            drawingSession.FillCircle(B1.L0, 5f, Colors.Orange);
            drawingSession.FillCircle(B1.L1, 7f, Colors.White);
            drawingSession.FillCircle(B1.L1, 5f, Colors.Orange);

            drawingSession.FillCircle(P, 7f, Colors.White);
            drawingSession.FillCircle(P, 5f, Colors.DodgerBlue);
        }
        public static void DrawCubic1(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, Cubic C, Quadratic Q, Linear L, Vector2 P)
        {
            drawingSession.DrawLine(Q.Q0, C.C1, Colors.Gray, 2f);
            drawingSession.DrawLine(C.C1, C.C2, Colors.Gray, 2f);
            drawingSession.DrawLine(C.C2, Q.Q2, Colors.Gray, 2f);

            drawingSession.DrawLine(Q.Q0, Q.Q1, Colors.Gray, 2f);
            drawingSession.DrawLine(Q.Q1, Q.Q2, Colors.Gray, 2f);

            drawingSession.DrawLine(C.C0, Q.Q0, Colors.Fuchsia, 2f);
            drawingSession.DrawLine(L.L0, P, Colors.Fuchsia, 2f);

            drawingSession.DrawLine(P, L.L1, Colors.Red, 2f);
            drawingSession.DrawLine(Q.Q2, C.C3, Colors.Red, 2f);

            using (CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator))
            {
                pathBuilder.BeginFigure(C.C0);
                pathBuilder.AddCubicBezier(Q.Q0, L.L0, P);
                pathBuilder.EndFigure(CanvasFigureLoop.Open);

                using (CanvasGeometry geometry = CanvasGeometry.CreatePath(pathBuilder))
                {
                    drawingSession.DrawGeometry(geometry, Colors.Fuchsia, 2f);
                }
            }

            using (CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator))
            {
                pathBuilder.BeginFigure(P);
                pathBuilder.AddCubicBezier(L.L1, Q.Q2, C.C3);
                pathBuilder.EndFigure(CanvasFigureLoop.Open);

                using (CanvasGeometry geometry = CanvasGeometry.CreatePath(pathBuilder))
                {
                    drawingSession.DrawGeometry(geometry, Colors.Red, 2f);
                }
            }

            drawingSession.FillCircle(C.C1, 5f, Colors.Gray);
            drawingSession.FillCircle(C.C2, 5f, Colors.Gray);

            drawingSession.FillCircle(Q.Q0, 5f, Colors.Gray);
            drawingSession.FillCircle(Q.Q1, 5f, Colors.Gray);
            drawingSession.FillCircle(Q.Q2, 5f, Colors.Gray);

            drawingSession.FillCircle(C.C0, 7f, Colors.White);
            drawingSession.FillCircle(C.C0, 5f, Colors.Fuchsia);
            drawingSession.FillCircle(Q.Q0, 7f, Colors.White);
            drawingSession.FillCircle(Q.Q0, 5f, Colors.Fuchsia);
            drawingSession.FillCircle(L.L0, 7f, Colors.White);
            drawingSession.FillCircle(L.L0, 5f, Colors.Fuchsia);

            drawingSession.FillCircle(L.L1, 7f, Colors.White);
            drawingSession.FillCircle(L.L1, 5f, Colors.Red);
            drawingSession.FillCircle(Q.Q2, 7f, Colors.White);
            drawingSession.FillCircle(Q.Q2, 5f, Colors.Red);
            drawingSession.FillCircle(C.C3, 7f, Colors.White);
            drawingSession.FillCircle(C.C3, 5f, Colors.Red);

            drawingSession.FillCircle(P, 7f, Colors.White);
            drawingSession.FillCircle(P, 5f, Colors.DodgerBlue);
        }
    }

    public sealed class SplitLinearBezierPage : SplitBezierPage
    {
        static readonly Linear LS = new Linear
        {
            L0 = P0,
            L1 = P3,
        };

        // Linear Bezier
        Linear L = LS;
        Vector2 P = LS.Lerp(0.5f, 0.5f);

        public override bool IsAllVisible => false;
        public override void Extend(float t, float i)
        {
            this.L = new Linear
            {
                L0 = P0,
                L1 = P3,
            };

            this.P = this.L.Lerp(t, i);
        }
        public override void Draw0(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            DrawLine0(drawingSession, this.L, this.P);
        }
        public override void Draw1(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            DrawLine1(drawingSession, this.L, this.P);
        }
    }

    public sealed class SplitQuadraticBezierPage : SplitBezierPage
    {
        static readonly Quadratic QS = new Quadratic
        {
            Q0 = P0,
            Q1 = P12,
            Q2 = P3,
        };
        static readonly Linear LS = QS.Linear(0.5f, 0.5f);

        // Quadratic Bezier
        Linear L = QS.Linear(0.5f, 0.5f);
        Quadratic Q = QS;
        Vector2 P = LS.Lerp(0.5f, 0.5f);

        public override bool IsAllVisible => false;
        public override void Extend(float t, float i)
        {
            this.Q = new Quadratic
            {
                Q0 = P0,
                Q1 = P12,
                Q2 = P3,
            };

            this.L = this.Q.Linear(t, i);
            this.P = this.L.Lerp(t, i);
        }
        public override void Draw0(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            DrawQuadratic0(resourceCreator, drawingSession, this.Q, this.L, this.P);
        }
        public override void Draw1(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            DrawQuadratic1(resourceCreator, drawingSession, this.Q, this.L, this.P);
        }
    }

    public sealed class SplitCubicBezierPage : SplitBezierPage
    {
        private static readonly Cubic CS = new Cubic
        {
            C0 = P0,
            C1 = P1,
            C2 = P2,
            C3 = P3,
        };
        private static readonly Quadratic QS = CS.Quadratic(0.5f, 0.5f);
        private static readonly Linear LS = QS.Linear(0.5f, 0.5f);

        // Cubic Bezier
        Cubic C = CS;
        Quadratic Q = QS;
        Linear L = LS;
        Vector2 P = LS.Lerp(0.5f, 0.5f);

        public override bool IsAllVisible => false;
        public override void Extend(float t, float i)
        {
            this.C = new Cubic
            {
                C0 = P0,
                C1 = P1,
                C2 = P2,
                C3 = P3,
            };

            this.Q = this.C.Quadratic(t, i);
            this.L = this.Q.Linear(t, i);
            this.P = this.L.Lerp(t, i);
        }
        public override void Draw0(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            DrawCubic0(resourceCreator, drawingSession, this.C, this.Q, this.L, this.P);
        }
        public override void Draw1(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            DrawCubic1(resourceCreator, drawingSession, this.C, this.Q, this.L, this.P);
        }
    }

    public sealed class SplitAllBezierPage : SplitBezierPage
    {
        private static readonly Cubic CS = new Cubic
        {
            C0 = P0,
            C1 = P1,
            C2 = P2,
            C3 = P3,
        };
        private static readonly Quadratic QS = CS.Quadratic(0.5f, 0.5f);
        private static readonly Linear LS = QS.Linear(0.5f, 0.5f);

        // Cubic Bezier
        Cubic C = CS;
        Quadratic Q = QS;
        Linear L = LS;
        Vector2 P = LS.Lerp(0.5f, 0.5f);

        public override bool IsAllVisible => true;
        public override void Extend(float t, float i)
        {
            switch (this.Type)
            {
                case BezierType.Cubic:
                    this.C = new Cubic
                    {
                        C0 = P0,
                        C1 = P1,
                        C2 = P2,
                        C3 = P3,
                    };

                    this.Q = this.C.Quadratic(t, i);
                    this.L = this.Q.Linear(t, i);
                    this.P = this.L.Lerp(t, i);
                    break;
                case BezierType.Quadratic:
                    this.Q = new Quadratic
                    {
                        Q0 = P0,
                        Q1 = P12,
                        Q2 = P3,
                    };
                    this.L = this.Q.Linear(t, i);

                    this.C = this.Q.Cubic();
                    this.P = this.L.Lerp(t, i);
                    break;
                case BezierType.Linear:
                    this.L = new Linear
                    {
                        L0 = P0,
                        L1 = P3,
                    };
                    this.Q = this.L.Quadratic();

                    this.C = this.Q.Cubic();
                    this.P = this.L.Lerp(t, i);
                    break;
                default:
                    break;
            }
        }
        public override void Draw0(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            switch (this.Type)
            {
                case BezierType.Cubic: DrawCubic0(resourceCreator, drawingSession, this.C, this.Q, this.L, this.P); break;
                case BezierType.Quadratic: DrawQuadratic0(resourceCreator, drawingSession, this.Q, this.L, this.P); break;
                case BezierType.Linear: DrawLine0(drawingSession, this.L, this.P); break;
                default: break;
            }
        }
        public override void Draw1(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            switch (this.Type)
            {
                case BezierType.Cubic: DrawCubic1(resourceCreator, drawingSession, this.C, this.Q, this.L, this.P); break;
                case BezierType.Quadratic: DrawQuadratic1(resourceCreator, drawingSession, this.Q, this.L, this.P); break;
                case BezierType.Linear: DrawLine1(drawingSession, this.L, this.P); break;
                default: break;
            }
        }
    }
}