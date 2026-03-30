using FanKit.Transformer.Cache;
using FanKit.Transformer.Demos;
using FanKit.Transformer.Indicators;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;
using System.Numerics;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Sample;
using InvalidateModes = FanKit.Transformer.Sample.InvalidateModes1;
using LineContainsNodeMode = FanKit.Transformer.Cache.LineContainsNodeMode1;

namespace FanKit.Transformer.TestApp
{
    /// <summary>
    /// <see cref="DemoHostLine1"/>
    /// </summary>
    public sealed partial class Line1Page : SoloPage
    {
        //@Key
        bool IsKeyDown(VirtualKey key) => Window.Current.CoreWindow.GetKeyState(key).HasFlag(CoreVirtualKeyStates.Down);
        bool IsCtrl => this.IsKeyDown(VirtualKey.Control);
        bool IsShift => this.IsKeyDown(VirtualKey.Shift);

        bool HasStepFrequency => this.IsShift;

        //@Const
        const float X = 256f;
        const float Y = 256f;

        const float StepFrequency = (float)System.Math.PI / 12f;
        const float StepFrequencyHalf = (float)System.Math.PI / 24f;

        // Line1
        LineContainsNodeMode Mode;
        readonly DemoHostLine1 Linear = new DemoHostLine1
        {
            Point0 = new Vector2(X - 100, Y),
            Point1 = new Vector2(X + 100, Y),
        };

        const float StrokeWidth = 4f;
        readonly Indicator Indicator = new Indicator();

        public Line1Page()
        {
            this.Indicator.SizeTypeChanged += (s, e) =>
            {
                IndicatorSizeType type = e;
                this.ParameterPanel.UpdateSizeType(type);
            };
            this.Indicator.XChanged += (s, e) => this.ParameterPanel.UpdateX(e);
            this.Indicator.YChanged += (s, e) => this.ParameterPanel.UpdateY(e);
            this.Indicator.WidthChanged += (s, e) => this.ParameterPanel.UpdateWidth(e);
            this.Indicator.HeightChanged += (s, e) => this.ParameterPanel.UpdateHeight(e);
            this.Indicator.RotationChanged += (s, e) => this.ParameterPanel.UpdateRotation(e);
            this.Indicator.SkewChanged += (s, e) => this.ParameterPanel.UpdateSkew(e);

            //this.ParameterPanel.ModeChanged += (s, e) => this.Indicator.ChangeXY(this.Composer.PanelTriangle, e);
            this.ParameterPanel.RowModeChanged += (s, e) => this.Indicator.ChangeAll(this.Linear.Point0, this.Linear.Point1, e);
            //this.ParameterPanel.ColumnModeChanged += (s, e) => this.Indicator.ChangeAll(this.Linear.Point0, this.Linear.Point1, e);

            this.ParameterPanel.Apply += (s, e) =>
            {
                this.ApplyRow(e, this.ParameterPanel.Value);
            };
        }

        public override void CreateResources(ICanvasResourceCreator resourceCreator, CanvasCreateResourcesEventArgs args)
        {
            this.Invalidate(InvalidateModes.None
                | InvalidateModes.InitIndicator);
        }

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawDashLine(this.Linear.ActualLine.Point0, this.Linear.ActualLine.Point1, StrokeWidth);
            drawingSession.DrawLine(this.Linear.ActualLine);
        }

        public override void DrawThumb(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawDashLine(this.Linear.Point0, this.Linear.Point1, StrokeWidth);
        }

        public override void CacheSingle()
        {
            this.CacheSingle0();
        }

        public override void Single()
        {
            this.Single0();
        }

        public override void DisposeSingle()
        {
        }

        public override void Over()
        {
        }

        public override void UpdateCanvasControl1()
        {
            this.Invalidate(InvalidateModes.None
                | InvalidateModes.CanvasControl);
        }

        public override void UpdateCanvasControl2()
        {
            this.Invalidate(InvalidateModes.None
                | InvalidateModes.UpdateIndicator
                | InvalidateModes.CanvasControl);
        }

        private void Invalidate(InvalidateModes modes)
        {
            if (modes.HasFlag(InvalidateModes.InitIndicator))
            {
                this.ParameterPanel.UpdateAll(this.Indicator);

                this.Linear.UpdateCanvas();
            }

            if (modes.HasFlag(InvalidateModes.UpdateIndicator))
            {
                this.ParameterPanel.UpdateAll(this.Indicator);

                this.Linear.UpdateCanvas();
            }

            if (modes.HasFlag(InvalidateModes.CanvasControl))
            {
                this.Invalidate();
            }
        }

        #region LineTransform1
        private void CacheSingle0()
        {
            const float d = 12f;
            const float ds = d * d;

            this.Mode = this.Linear.ActualLine.ContainsNode(this.StartingPoint, ds);

            switch (this.Mode)
            {
                case LineContainsNodeMode.None: break;
                /*
                case LineContainsNodeMode.Handle0: this.Linear.CacheElongation0(); break;
                case LineContainsNodeMode.Handle1: this.Linear.CacheElongation1(); break;
                case LineContainsNodeMode.Handle: this.Linear.CacheRotation(this.StartingPoint); break;
                 */
                case LineContainsNodeMode.Center: this.Linear.CacheTranslation(); break;
                case LineContainsNodeMode.Point0: this.Linear.CacheMovement(); break;
                case LineContainsNodeMode.Point1: this.Linear.CacheMovement(); break;
                default: break;
            }
        }

        private void Single0()
        {
            this.SingleRow();
        }

        private void SingleRow()
        {
            switch (this.Mode)
            {
                case LineContainsNodeMode.None:
                    break;
                /*
                case LineContainsNodeMode.Handle0:
                    this.Linear.ElongatePoint0(this.Indicator, this.ParameterPanel.RowMode, this.StartingPoint, this.Point);

                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Handle1:
                    this.Linear.ElongatePoint1(this.Indicator, this.ParameterPanel.RowMode, this.StartingPoint, this.Point);

                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Handle:
                    if (this.HasStepFrequency)
                        this.Linear.Rotate(this.Indicator, this.ParameterPanel.RowMode, this.Point, StepFrequency);
                    else
                        this.Linear.Rotate(this.Indicator, this.ParameterPanel.RowMode, this.Point);

                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                 */
                case LineContainsNodeMode.Center:
                    this.Linear.Translate(this.Indicator, this.ParameterPanel.RowMode, this.StartingPoint, this.Point);

                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Point0:
                    this.Linear.MovePoint0(this.Indicator, this.ParameterPanel.RowMode, this.Point);

                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Point1:
                    this.Linear.MovePoint1(this.Indicator, this.ParameterPanel.RowMode, this.Point);

                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                default:
                    break;
            }
        }

        private void SingleColumn()
        {
            switch (this.Mode)
            {
                case LineContainsNodeMode.None:
                    break;
                /*
                case LineContainsNodeMode.Handle0:
                    this.Linear.ElongatePoint0(this.Indicator, this.ParameterPanel.ColumnMode, this.StartingPoint, this.Point);

                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Handle1:
                    this.Linear.ElongatePoint1(this.Indicator, this.ParameterPanel.ColumnMode, this.StartingPoint, this.Point);

                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Handle:
                    if (this.HasStepFrequency)
                        this.Linear.Rotate(this.Indicator, this.ParameterPanel.ColumnMode, this.Point, StepFrequency);
                    else
                        this.Linear.Rotate(this.Indicator, this.ParameterPanel.ColumnMode, this.Point);

                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                 */
                case LineContainsNodeMode.Center:
                    this.Linear.Translate(this.Indicator, this.ParameterPanel.ColumnMode, this.StartingPoint, this.Point);

                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Point0:
                    this.Linear.MovePoint0(this.Indicator, this.ParameterPanel.ColumnMode, this.Point);

                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Point1:
                    this.Linear.MovePoint1(this.Indicator, this.ParameterPanel.ColumnMode, this.Point);

                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Line
        private void ApplyRow(IndicatorKind kind, float value)
        {
            RowLineMode mode = this.ParameterPanel.RowMode;

            switch ((IndicatorRowKind)(byte)kind)
            {
                case IndicatorRowKind.X:
                    float translateX = value - this.Indicator.X;

                    this.Linear.SetTranslationX(this.Indicator, mode, translateX);
                    break;
                case IndicatorRowKind.Y:
                    float translateY = value - this.Indicator.Y;

                    this.Linear.SetTranslationY(this.Indicator, mode, translateY);
                    break;
                case IndicatorRowKind.Width:
                    this.Linear.SetWidth(this.Indicator, mode, value);
                    break;
                case IndicatorRowKind.Rotation:
                    this.Linear.SetRotation(this.Indicator, mode, value);
                    break;
                default:
                    break;
            }

            this.Invalidate(InvalidateModes.None
                //| InvalidateModes.UpdateLayers
                | InvalidateModes.UpdateIndicator
                | InvalidateModes.CanvasControl);
        }
        private void ApplyColumn(IndicatorKind kind, float value)
        {
            ColumnLineMode mode = this.ParameterPanel.ColumnMode;

            switch ((IndicatorColumnKind)(byte)kind)
            {
                case IndicatorColumnKind.X:
                    float translateX = value - this.Indicator.X;

                    this.Linear.SetTranslationX(this.Indicator, mode, translateX);
                    break;
                case IndicatorColumnKind.Y:
                    float translateY = value - this.Indicator.Y;

                    this.Linear.SetTranslationY(this.Indicator, mode, translateY);
                    break;
                case IndicatorColumnKind.Height:
                    this.Linear.SetHeight(this.Indicator, mode, value);
                    break;
                case IndicatorColumnKind.Rotation:
                    this.Linear.SetRotation(this.Indicator, mode, value);
                    break;
                default:
                    break;
            }

            this.Invalidate(InvalidateModes.None
                //| InvalidateModes.UpdateLayers
                | InvalidateModes.UpdateIndicator
                | InvalidateModes.CanvasControl);
        }
        #endregion
    }
}