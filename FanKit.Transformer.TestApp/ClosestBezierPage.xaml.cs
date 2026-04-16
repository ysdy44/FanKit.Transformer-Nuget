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
    public abstract partial class ClosestBezierPage : Page
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

        readonly Matrix3x2 Offset = Matrix3x2.CreateTranslation(6f, 6f);

        float Value;
        public Vector2 Position;

        public ClosestBezierPage()
        {
            this.InitializeComponent();
            if (this.IsAllVisible)
                this.ComboBox.Visibility = Visibility.Visible;
            else
                this.ComboBox.Visibility = Visibility.Collapsed;

            this.ComboBox.SelectionChanged += delegate
            {
                float time = this.Value;

                this.Extend(time, 1f - time);
                this.CanvasControl.Invalidate();
            };

            this.CanvasControl.PointerMoved += (s, e) =>
            {
                PointerPoint pp = e.GetCurrentPoint(this.CanvasControl);

                double x;
                switch (this.CanvasControl.FlowDirection)
                {
                    case FlowDirection.RightToLeft:
                        x = this.CanvasControl.ActualWidth - pp.Position.X;
                        break;
                    default:
                        x = pp.Position.X;
                        break;
                }

                this.Position = new Vector2
                {
                    X = (float)x,
                    Y = (float)pp.Position.Y,
                };
                this.Value = this.FindClosest();

                float time = this.Value;

                this.Extend(time, 1f - time);
                this.CanvasControl.Invalidate();
            };
            this.CanvasControl.Draw += (s, e) =>
            {
                e.DrawingSession.Transform = this.Offset;
                this.Draw(s, e.DrawingSession);
            };
        }

        public BezierType Type => (BezierType)this.ComboBox.SelectedIndex;
        public abstract bool IsAllVisible { get; }

        public abstract void Extend(float t, float i);
        public abstract float FindClosest();
        public abstract void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession);

        public static void DrawLine(CanvasDrawingSession drawingSession, Linear L, Vector2 P)
        {
            drawingSession.DrawLine(L.L0, L.L1, Colors.DodgerBlue, 2f);
        }

        public static void DrawQuadratic(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, Quadratic Q, Linear L, Vector2 P)
        {
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
        }

        public static void DrawCubic(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, Cubic C, Quadratic Q, Linear B1, Vector2 P)
        {
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
        }
    }

    public sealed class ClosestLinearBezierPage : ClosestBezierPage
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
        public override float FindClosest()
        {
            return this.L.Foot(this.Position);
        }
        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawLine(this.Position, this.P, Colors.DodgerBlue, 2f);
            DrawLine(drawingSession, this.L, this.P);

            drawingSession.FillCircle(this.Position, 7f, Colors.White);
            drawingSession.FillCircle(this.Position, 5f, Colors.LightSeaGreen);
            drawingSession.FillCircle(this.P, 7f, Colors.White);
            drawingSession.FillCircle(this.P, 5f, Colors.LightSeaGreen);
        }
    }

    public sealed class ClosestQuadraticBezierPage : ClosestBezierPage
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
        public override float FindClosest()
        {
            return this.Q.FindClosest(this.Position);
        }
        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawLine(this.Position, this.P, Colors.DodgerBlue, 2f);
            DrawQuadratic(resourceCreator, drawingSession, this.Q, this.L, this.P);

            drawingSession.FillCircle(this.Position, 7f, Colors.White);
            drawingSession.FillCircle(this.Position, 5f, Colors.LightSeaGreen);
            drawingSession.FillCircle(this.P, 7f, Colors.White);
            drawingSession.FillCircle(this.P, 5f, Colors.LightSeaGreen);
        }
    }

    public sealed class ClosestCubicBezierPage : ClosestBezierPage
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
        public override float FindClosest()
        {
            return this.C.FindClosest(this.Position);
        }
        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawLine(this.Position, this.P, Colors.DodgerBlue, 2f);
            DrawCubic(resourceCreator, drawingSession, this.C, this.Q, this.L, this.P);

            drawingSession.FillCircle(this.Position, 7f, Colors.White);
            drawingSession.FillCircle(this.Position, 5f, Colors.LightSeaGreen);
            drawingSession.FillCircle(this.P, 7f, Colors.White);
            drawingSession.FillCircle(this.P, 5f, Colors.LightSeaGreen);
        }
    }

    public sealed class ClosestAllBezierPage : ClosestBezierPage
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
        public override float FindClosest()
        {
            switch (this.Type)
            {
                case BezierType.Cubic:
                    return this.C.FindClosest(this.Position);
                case BezierType.Quadratic:
                    return this.Q.FindClosest(this.Position);
                case BezierType.Linear:
                    return this.L.Foot(this.Position);
                default:
                    return 0.5f;
            }
        }
        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawLine(this.Position, this.P, Colors.DodgerBlue, 2f);

            switch (this.Type)
            {
                case BezierType.Cubic: DrawCubic(resourceCreator, drawingSession, this.C, this.Q, this.L, this.P); break;
                case BezierType.Quadratic: DrawQuadratic(resourceCreator, drawingSession, this.Q, this.L, this.P); break;
                case BezierType.Linear: DrawLine(drawingSession, this.L, this.P); break;
                default: break;
            }

            drawingSession.FillCircle(this.Position, 7f, Colors.White);
            drawingSession.FillCircle(this.Position, 5f, Colors.LightSeaGreen);
            drawingSession.FillCircle(this.P, 7f, Colors.White);
            drawingSession.FillCircle(this.P, 5f, Colors.LightSeaGreen);
        }
    }
}