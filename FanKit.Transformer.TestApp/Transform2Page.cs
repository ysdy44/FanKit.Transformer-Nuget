using FanKit.Transformer.Cache;
using FanKit.Transformer.Controllers;
using FanKit.Transformer.Demos;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Input;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Sample;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FanKit.Transformer.Transforms;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Controls.Primitives;
using InvalidateModes = FanKit.Transformer.Sample.InvalidateModes1;
using BoxContainsNodeMode = FanKit.Transformer.Cache.BoxContainsNodeMode2;

namespace FanKit.Transformer.TestApp
{
    /// <summary>
    /// <see cref="DemoSizeTriangle2"/>
    /// </summary>
    public sealed partial class Transform2Page : SoloPage
    {
        //@Key
        bool IsKeyDown(VirtualKey key) => Window.Current.CoreWindow.GetKeyState(key).HasFlag(CoreVirtualKeyStates.Down);
        bool IsCtrl => this.IsKeyDown(VirtualKey.Control);
        bool IsShift => this.IsKeyDown(VirtualKey.Shift);

        bool CenteredScaling => this.IsCtrl || this.ToolBox1.CenteredScaling;
        bool KeepRatio => this.IsShift || this.ToolBox1.KeepRatio;

        bool HasStepFrequency => this.IsShift;

        //@Const
        const float X = 200;
        const float Y = 200;
        const float W = 256;
        const float H = 256;

        const float StepFrequency = (float)System.Math.PI / 12f;
        const float StepFrequencyHalf = (float)System.Math.PI / 24f;

        // Transformer2
        BoxContainsNodeMode Mode;
        readonly DemoSizeTriangle2 Transformer = new DemoSizeTriangle2
        (
            sourceWidth: W,
            sourceHeight: H,
            destination: new Triangle
            {
                LeftTop = new Vector2(X, Y),
                RightTop = new Vector2(X + W, 10 + Y),
                LeftBottom = new Vector2(100 + X, Y + H),
            }
        );

        CanvasBitmap Bitmap;
        readonly Indicator Indicator = new Indicator();

        readonly ToolBox1 ToolBox1 = new ToolBox1
        {
            Title = "Transform"
        };

        public Transform2Page()
        {
            this.Children.Add(this.ToolBox1);

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

            this.ParameterPanel.ModeChanged += (s, e) => this.Indicator.ChangeXY(this.Transformer.Destination, e);

            this.ParameterPanel.Apply += (s, e) =>
            {
                this.Apply(e, this.ParameterPanel.Value);
            };
        }

        public override void CreateResources(ICanvasResourceCreator resourceCreator, CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResourcesAsync(resourceCreator).AsAsyncAction());
        }
        private async Task CreateResourcesAsync(ICanvasResourceCreator sender)
        {
            if (this.Bitmap != null)
                return;

            this.Bitmap = await CanvasBitmap.LoadAsync(sender, "Images/avatar.jpg");

            float width = (float)this.Bitmap.Size.Width;
            float height = (float)this.Bitmap.Size.Height;
            this.Transformer.UpdateSource(width, height);

            this.Invalidate(InvalidateModes.None
                //| InvalidateModes.InitCanvas
                //| InvalidateModes.InitLayers
                | InvalidateModes.InitIndicator);
        }

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            if (this.Bitmap != null)
            {
                drawingSession.DrawImage(new Transform2DEffect
                {
                    TransformMatrix = this.Transformer.ActualMatrix,
                    Source = this.Bitmap,
                });
            }

            drawingSession.DrawBox(this.Transformer.ActualBox);
        }

        public override void DrawThumb(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            if (this.Bitmap != null)
            {
                drawingSession.DrawImage(new Transform2DEffect
                {
                    TransformMatrix = this.Transformer.HomographyMatrix,
                    Source = this.Bitmap,
                });
            }
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
                //| InvalidateModes.UpdateCanvas
                //| InvalidateModes.UpdateLayers
                | InvalidateModes.CanvasControl);
        }

        public override void UpdateCanvasControl2()
        {
            this.Invalidate(InvalidateModes.None
                //| InvalidateModes.UpdateCanvas
                //| InvalidateModes.UpdateLayers
                | InvalidateModes.UpdateIndicator
                | InvalidateModes.CanvasControl);
        }

        private void Invalidate(InvalidateModes modes)
        {
            if (modes.HasFlag(InvalidateModes.InitIndicator))
            {
                this.Indicator.ChangeAll(this.Transformer.Destination, this.ParameterPanel.Mode);

                this.ParameterPanel.UpdateAll(this.Indicator);

                this.Transformer.UpdateCanvas();
            }

            if (modes.HasFlag(InvalidateModes.UpdateIndicator))
            {
                this.ParameterPanel.UpdateAll(this.Indicator);

                this.Transformer.UpdateCanvas();
            }

            if (modes.HasFlag(InvalidateModes.CanvasControl))
            {
                this.Invalidate();
            }
        }

        #region Transform2
        private void CacheSingle0()
        {
            const float d = 12f;
            const float ds = d * d;

            this.Mode = this.Transformer.ActualBox.ContainsNode(this.StartingPoint, ds);

            switch (this.Mode)
            {
                case BoxContainsNodeMode.None: break;
                // Multiple Translation 2
                case BoxContainsNodeMode.Contains: this.Transformer.CacheTranslation(); break;

                // Multiple Transform 2
                /*
                case BoxContainsNodeMode.HandleLeftTop:
                case BoxContainsNodeMode.HandleRightTop:
                case BoxContainsNodeMode.HandleLeftBottom:
                case BoxContainsNodeMode.HandleRightBottom: break;
                 */

                case BoxContainsNodeMode.HandleLeft: break;
                case BoxContainsNodeMode.HandleTop: this.Transformer.CacheRotation(this.StartingPoint); break;
                case BoxContainsNodeMode.HandleRight: this.Transformer.CacheTransform(TransformMode.SkewRight); break;
                case BoxContainsNodeMode.HandleBottom: this.Transformer.CacheTransform(TransformMode.SkewBottom); break;

                case BoxContainsNodeMode.CenterLeft: this.Transformer.CacheTransform(TransformMode.ScaleLeft); break;
                case BoxContainsNodeMode.CenterTop: this.Transformer.CacheTransform(TransformMode.ScaleTop); break;
                case BoxContainsNodeMode.CenterRight: this.Transformer.CacheTransform(TransformMode.ScaleRight); break;
                case BoxContainsNodeMode.CenterBottom: this.Transformer.CacheTransform(TransformMode.ScaleBottom); break;

                case BoxContainsNodeMode.LeftTop: this.Transformer.CacheTransform(TransformMode.ScaleLeftTop); break;
                case BoxContainsNodeMode.RightTop: this.Transformer.CacheTransform(TransformMode.ScaleRightTop); break;
                case BoxContainsNodeMode.LeftBottom: this.Transformer.CacheTransform(TransformMode.ScaleLeftBottom); break;
                case BoxContainsNodeMode.RightBottom: this.Transformer.CacheTransform(TransformMode.ScaleRightBottom); break;
                default: break;
            }
        }

        private void Single0()
        {
            switch (this.Mode)
            {
                case BoxContainsNodeMode.None:
                    break;
                case BoxContainsNodeMode.Contains:
                    this.Transformer.Translate(this.Indicator, this.ParameterPanel.Mode, this.StartingPoint, this.Point);

                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                /*
                case BoxContainsNodeMode.HandleLeftTop:
                case BoxContainsNodeMode.HandleRightTop:
                case BoxContainsNodeMode.HandleLeftBottom:
                case BoxContainsNodeMode.HandleRightBottom:
                    break;
                 */
                case BoxContainsNodeMode.HandleLeft:
                    break;
                case BoxContainsNodeMode.HandleTop:
                    if (this.HasStepFrequency)
                        this.Transformer.Rotate(this.Indicator, this.ParameterPanel.Mode, this.Point, StepFrequency);
                    else
                        this.Transformer.Rotate(this.Indicator, this.ParameterPanel.Mode, this.Point);

                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case BoxContainsNodeMode.HandleRight:
                case BoxContainsNodeMode.HandleBottom:
                    this.Transformer.TransformSkew(this.Indicator, this.ParameterPanel.Mode, this.Point, this.KeepRatio, this.CenteredScaling);

                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                default:
                    this.Transformer.TransformSize(this.Indicator, this.ParameterPanel.Mode, this.Point, this.KeepRatio, this.CenteredScaling);

                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
            }
        }
        #endregion

        #region Panel
        private void Apply(IndicatorKind kind, float value)
        {
            BoxMode mode = this.ParameterPanel.Mode;

            switch (kind)
            {
                case IndicatorKind.X:
                    float translateX = value - this.Indicator.X;

                    this.Transformer.SetTranslationX(this.Indicator, mode, translateX);
                    break;
                case IndicatorKind.Y:
                    float translateY = value - this.Indicator.Y;

                    this.Transformer.SetTranslationY(this.Indicator, mode, translateY);
                    break;
                case IndicatorKind.Width:
                    this.Transformer.SetWidth(this.Indicator, mode, value, this.KeepRatio);
                    break;
                case IndicatorKind.Height:
                    this.Transformer.SetHeight(this.Indicator, mode, value, this.KeepRatio);
                    break;
                case IndicatorKind.Rotation:
                    this.Transformer.SetRotation(this.Indicator, mode, value);
                    break;
                case IndicatorKind.Skew:
                    this.Transformer.SetSkew(this.Indicator, mode, value);
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