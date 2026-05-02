using FanKit.Transformer.Cache;
using FanKit.Transformer.Controllers;
using FanKit.Transformer.Curves;
using FanKit.Transformer.Demos;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Input;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Sample;
using FanKit.Transformer.Transforms;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls.Primitives;
using Layer = FanKit.Transformer.Demos.DemoCurve1;
using Segment = FanKit.Transformer.Curves.Segment1;
using InvalidateModes = FanKit.Transformer.Sample.InvalidateModes123;
using LineContainsNodeMode = FanKit.Transformer.Cache.LineContainsNodeMode2;
using BoxContainsNodeMode = FanKit.Transformer.Cache.BoxContainsNodeMode2;

namespace FanKit.Transformer.TestApp
{
    /// <summary>
    /// <see cref="DemoHostComposer2"/>
    /// </summary>
    public sealed partial class CanvasCurve2Page : CanvasSolo3Page
    {
        //@Key
        bool IsKeyDown(VirtualKey key) => Window.Current.CoreWindow.GetKeyState(key).HasFlag(CoreVirtualKeyStates.Down);
        bool IsCtrl => this.IsKeyDown(VirtualKey.Control);
        bool IsShift => this.IsKeyDown(VirtualKey.Shift);

        bool NodeCenteredScaling => this.IsCtrl || this.ToolBox8.CenteredScaling;
        bool NodeKeepRatio => this.IsShift || this.ToolBox8.KeepRatio;

        bool HasStepFrequency => this.IsShift;

        bool Disconnected => this.ToolBox5.Disconnected;
        SelfControlPointMode Mode1 => this.ToolBox5.Mode1;
        EachControlPointLengthMode Mode2 => this.ToolBox5.Mode2;

        //@Const
        const float X = 256f;
        const float Y = 256f;

        const float StepFrequency = (float)System.Math.PI / 12f;
        const float StepFrequencyHalf = (float)System.Math.PI / 24f;

        // MultiCanvas
        Bounds RectChoose;
        TransformedBounds ActualRectChoose;
        bool HasRectChoose;

        // Composer3
        //int LayerIndex = -1;
        //int FigureIndex = -1;
        SegmentIndexer Indexer = SegmentIndexer.Empty;
        NodeController Controller;

        ClosestPointer Closest;
        SegmentInserter Inserter = SegmentInserter.Empty;
        //NodeSmooth Smooth;

        bool PointMode;
        LineContainsNodeMode LineMode;
        BoxContainsNodeMode PanelMode;
        readonly DemoHostComposer2 Composer = new DemoHostComposer2();

        readonly Indicator Indicator = new Indicator();
        readonly Layer Layer = new Layer(new List<Segment>
        {
             new Segment
             {
                 IsChecked = true,
                 IsSmooth = false,
                 Point = new Node
                 {
                     Point = new Vector2(X - 144.6047f, Y - 138.5997f),
                     LeftControlPoint= new Vector2(X - 144.6047f, Y - 138.5997f),
                     RightControlPoint = new Vector2(X - 144.6047f, Y - 138.5997f),
                 }
             },
             new Segment
             {
                 IsChecked = true,
                 IsSmooth = true,
                 Point = new Node
                 {
                     Point = new Vector2(X + 13.37953f, Y - 95.3983f),
                     LeftControlPoint = new Vector2(X - 8.623611f, Y - 132.9995f),
                     RightControlPoint= new Vector2(X + 35.38268f, Y - 57.79712f),
                 }
             },
             new Segment
             {
                 IsChecked = true,
                 IsSmooth = true,
                 Point = new Node
                 {
                     Point = new Vector2(X - 12.58583f, Y + 87.00745f),
                     LeftControlPoint = new Vector2(X - 34.4567f, Y + 48.00778f),
                     RightControlPoint= new Vector2(X + 9.285034f, Y + 126.0071f),
                 }
             },
             new Segment
             {
                 IsChecked = true,
                 IsSmooth = false,
                 Point = new Node
                 {
                     Point = new Vector2(X + 144.6047f, Y + 138.5997f),
                     LeftControlPoint = new Vector2(X + 144.6047f, Y + 138.5997f),
                     RightControlPoint= new Vector2(X + 144.6047f, Y + 138.5997f),
                 }
             }
        })
        {
            StrokeWidth = 4f,
        };

        readonly TopBar1 TopBar = new TopBar1();
        readonly ToolBox5 ToolBox5 = new ToolBox5
        {
            Title = "Node"
        };
        readonly ToolBox8 ToolBox8 = new ToolBox8
        {
            Title = "Transform Node"
        };

        public CanvasCurve2Page()
        {
            this.Child = this.TopBar;
            this.Children.Add(this.ToolBox5);
            this.Children.Add(this.ToolBox8);

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

            this.ParameterPanel.ModeChanged += (s, e) => this.Indicator.ChangeXY(this.Composer.PanelDestination, e);
            this.ParameterPanel.RowModeChanged += (s, e) => this.Indicator.ChangeAll(this.Composer.LinePoint0, this.Composer.LinePoint1, e);
            this.ParameterPanel.ColumnModeChanged += (s, e) => this.Indicator.ChangeAll(this.Composer.LinePoint0, this.Composer.LinePoint1, e);

            this.ParameterPanel.Apply += (s, e) =>
            {
                this.Apply1(e, this.ParameterPanel.Value);
            };

            this.TopBar.ToolTypeChanged += delegate
            {
                switch (this.TopBar.ToolType)
                {
                    case ToolType1.NodeMove:
                        this.ToolBox5.Visibility = Visibility.Visible;
                        this.ToolBox8.Visibility = Visibility.Collapsed;
                        break;
                    case ToolType1.NodeTransform:
                        this.ToolBox5.Visibility = Visibility.Collapsed;
                        this.ToolBox8.Visibility = Visibility.Visible;
                        break;
                    default:
                        break;
                }

                this.Invalidate(InvalidateModes.None
                    | InvalidateModes.InitIndicator
                    | InvalidateModes.CanvasControl);
            };

            this.ToolBox8.CloseClick += delegate { this.NodeClose(); };
            this.ToolBox8.OpenClick += delegate { this.NodeOpen(); };
            this.ToolBox8.SharpClick += delegate { this.NodeSharp(); };
            this.ToolBox8.SmoothClick += delegate { this.NodeSmooth(); };
        }

        public override void CreateResources(ICanvasResourceCreator resourceCreator, CanvasCreateResourcesEventArgs args)
        {
            this.Invalidate(InvalidateModes.None
                | InvalidateModes.InitCanvas
                | InvalidateModes.InitLayers
                | InvalidateModes.InitIndicator);
        }

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            using (PathBuilder path = new PathBuilder(resourceCreator))
            {
                path.CreateActualPath(this.Layer.Segments, this.Layer.IsClosed);
                using (CanvasGeometry curve = CanvasGeometry.CreatePath(path.Builder))
                {
                    drawingSession.DrawDashCurve(curve, this.Layer.ActualStrokeWidth);
                }
            }

            switch (this.TopBar.ToolType)
            {
                case ToolType1.NodeMove: this.Draw1(drawingSession); this.Draw2(resourceCreator, drawingSession); break;
                case ToolType1.NodeTransform: this.Draw3(drawingSession); break;
                default: break;
            }
        }

        public override void DrawThumb(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            using (PathBuilder path = new PathBuilder(resourceCreator))
            {
                path.CreatePointPath(this.Layer.Segments, this.Layer.IsClosed);
                using (CanvasGeometry curve = CanvasGeometry.CreatePath(path.Builder))
                {
                    drawingSession.DrawDashCurve(curve, this.Layer.StrokeWidth);
                }
            }
        }

        public override void CacheSingle()
        {
            switch (this.TopBar.ToolType)
            {
                case ToolType1.NodeMove: this.CacheSingle2(); break;
                case ToolType1.NodeTransform:
                    switch (this.Composer.SizeType)
                    {
                        case SizeType.Empty: this.CacheSingle3(); break;
                        case SizeType.Point: this.CacheSingle4(); break;
                        case SizeType.RowLine:
                        case SizeType.ColumnLine: this.CacheSingle5(); break;
                        case SizeType.Panel: this.CacheSingle6(); break;
                        default: break;
                    }
                    break;
                default: break;
            }
        }

        public override void Single()
        {
            switch (this.TopBar.ToolType)
            {
                case ToolType1.NodeMove: this.Single2(); break;
                case ToolType1.NodeTransform:
                    switch (this.Composer.SizeType)
                    {
                        case SizeType.Empty: this.Single3(); break;
                        case SizeType.Point: this.Single4(); break;
                        case SizeType.RowLine: this.SingleRow5(); break;
                        case SizeType.ColumnLine: this.SingleColumn5(); break;
                        case SizeType.Panel: this.Single6(); break;
                        default: break;
                    }
                    break;
                default: break;
            }
        }

        public override void DisposeSingle()
        {
            switch (this.TopBar.ToolType)
            {
                case ToolType1.NodeMove: this.DisposeSingle2(); break;
                case ToolType1.NodeTransform:
                    switch (this.Composer.SizeType)
                    {
                        case SizeType.Empty: this.DisposeSingle3(); break;
                        case SizeType.Point: this.DisposeSingle4(); break;
                        case SizeType.RowLine:
                        case SizeType.ColumnLine: this.DisposeSingle5(); break;
                        case SizeType.Panel: this.DisposeSingle6(); break;
                        default: break;
                    }
                    break;
                default: break;
            }
        }

        public override void Over()
        {
            switch (this.TopBar.ToolType)
            {
                case ToolType1.NodeMove: this.Over2(); break;
                case ToolType1.NodeTransform: break;
                default: break;
            }
        }

        public override void UpdateCanvasControl1()
        {
            this.Invalidate(InvalidateModes.None
                | InvalidateModes.UpdateCanvas
                //| InvalidateModes.UpdateLayers
                | InvalidateModes.CanvasControl);
        }

        public override void UpdateCanvasControl2()
        {
            this.Invalidate(InvalidateModes.None
                | InvalidateModes.UpdateCanvas
                | InvalidateModes.UpdateLayers
                | InvalidateModes.UpdateIndicator
                | InvalidateModes.CanvasControl);
        }

        #region TransformSelectedItems
        private void CacheTranslationSelectedItems()
        {
            this.Layer.CacheTranslationSelectedItems();
        }
        private void CacheTransformSelectedItems()
        {
            this.Layer.CacheTransformSelectedItems();
        }
        private void TranslateSelectedItems()
        {
            this.Layer.TranslateSelectedItems(this.Composer.TranslationX, this.Composer.TranslationY);
        }
        private void TransformSelectedItems()
        {
            this.Layer.TransformSelectedItems(this.Composer.TransformMatrix);
        }
        private void RectChooseItems()
        {
            this.Layer.RectChooseItems(this.RectChoose);
        }
        #endregion

        #region SetTransformSelectedItems
        private void SetTranslationXSelectedItems()
        {
            this.Layer.SetTranslationXSelectedItems(this.Composer.TranslationX);
        }
        private void SetTranslationYSelectedItems()
        {
            this.Layer.SetTranslationYSelectedItems(this.Composer.TranslationY);
        }
        private void SetTransformSelectedItems()
        {
            this.Layer.SetTransformSelectedItems(this.Composer.TransformMatrix);
        }
        #endregion

        private void Invalidate(InvalidateModes modes)
        {
            if (modes.HasFlag(InvalidateModes.InitCanvas))
            {
                this.InitCanvas();
            }

            if (modes.HasFlag(InvalidateModes.InitLayers))
            {
                this.Layer.UpdateCanvas(this.Canvas);
            }

            if (modes.HasFlag(InvalidateModes.InitIndicator))
            {
                this.Composer.BeginExtend();
                foreach (Segment item in this.Layer.Segments)
                {
                    this.Composer.Extend(item);
                }
                this.Composer.EndExtendByPoints();

                switch (this.Composer.SizeType)
                {
                    case SizeType.Empty: this.Indicator.ClearAll(); break;
                    case SizeType.Point: this.Indicator.ChangeAll(this.Composer.PointPoint); break;
                    case SizeType.RowLine: this.Indicator.ChangeAll(this.Composer.LinePoint0, this.Composer.LinePoint1, this.ParameterPanel.RowMode); break;
                    case SizeType.ColumnLine: this.Indicator.ChangeAll(this.Composer.LinePoint0, this.Composer.LinePoint1, this.ParameterPanel.ColumnMode); break;
                    case SizeType.Panel: this.Indicator.ChangeAll(this.Composer.PanelDestination, this.ParameterPanel.Mode); break;
                    default: break;
                }
                this.ParameterPanel.UpdateAll(this.Indicator);

                this.Composer.UpdateCanvas(this.Canvas);
            }

            if (modes.HasFlag(InvalidateModes.UpdateCanvas))
            {
                this.UpdateCanvas();
            }

            if (modes.HasFlag(InvalidateModes.UpdateLayers))
            {
                this.Layer.UpdateCanvas(this.Canvas);
            }

            if (modes.HasFlag(InvalidateModes.UpdateIndicator))
            {
                this.ParameterPanel.UpdateAll(this.Indicator);

                this.Composer.UpdateCanvas(this.Canvas);
            }

            if (modes.HasFlag(InvalidateModes.CanvasControl))
            {
                this.Invalidate();
            }
        }

        #region LineTransform2
        private void CacheSingle5()
        {
            const float d = 12f;
            const float ds = d * d;

            this.LineMode = this.Composer.ActualLine.ContainsNode(this.StartingPoint, ds);

            if (this.LineMode != LineContainsNodeMode.None)
            {
                this.CacheTransformSelectedItems();
            }

            switch (this.LineMode)
            {
                case LineContainsNodeMode.None: this.CacheSingle3(); break;
                /*
                case LineContainsNodeMode.Handle0: this.Composer.LineCacheElongation0(); break;
                case LineContainsNodeMode.Handle1: this.Composer.LineCacheElongation1(); break;
                 */
                case LineContainsNodeMode.Handle: this.Composer.LineCacheRotation(this.StartingPosition); break;
                case LineContainsNodeMode.Center: this.Composer.LineCacheTranslation(); break;
                case LineContainsNodeMode.Point0: this.Composer.LineCacheMovement(); break;
                case LineContainsNodeMode.Point1: this.Composer.LineCacheMovement(); break;
                default: break;
            }
        }

        private void SingleRow5()
        {
            switch (this.LineMode)
            {
                case LineContainsNodeMode.None:
                    this.Single3();
                    break;
                /*
                case LineContainsNodeMode.Handle0:
                    this.Composer.LineElongatePoint0(this.Indicator, this.ParameterPanel.RowMode, this.StartingPosition, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Handle1:
                    this.Composer.LineElongatePoint1(this.Indicator, this.ParameterPanel.RowMode, this.StartingPosition, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                 */
                case LineContainsNodeMode.Handle:
                    if (this.HasStepFrequency)
                        this.Composer.LineRotate(this.Indicator, this.ParameterPanel.RowMode, this.Position, StepFrequency);
                    else
                        this.Composer.LineRotate(this.Indicator, this.ParameterPanel.RowMode, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Center:
                    this.Composer.LineTranslate(this.Indicator, this.ParameterPanel.RowMode, this.StartingPosition, this.Position);

                    this.TranslateSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Point0:
                    this.Composer.LineMovePoint0(this.Indicator, this.ParameterPanel.RowMode, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Point1:
                    this.Composer.LineMovePoint1(this.Indicator, this.ParameterPanel.RowMode, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                default:
                    break;
            }
        }

        private void SingleColumn5()
        {
            switch (this.LineMode)
            {
                case LineContainsNodeMode.None:
                    this.Single3();
                    break;
                /*
                case LineContainsNodeMode.Handle0:
                    this.Composer.LineElongatePoint0(this.Indicator, this.ParameterPanel.ColumnMode, this.StartingPosition, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Handle1:
                    this.Composer.LineElongatePoint1(this.Indicator, this.ParameterPanel.ColumnMode, this.StartingPosition, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                 */
                case LineContainsNodeMode.Handle:
                    if (this.HasStepFrequency)
                        this.Composer.LineRotate(this.Indicator, this.ParameterPanel.ColumnMode, this.Position, StepFrequency);
                    else
                        this.Composer.LineRotate(this.Indicator, this.ParameterPanel.ColumnMode, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Center:
                    this.Composer.LineTranslate(this.Indicator, this.ParameterPanel.ColumnMode, this.StartingPosition, this.Position);

                    this.TranslateSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Point0:
                    this.Composer.LineMovePoint0(this.Indicator, this.ParameterPanel.ColumnMode, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Point1:
                    this.Composer.LineMovePoint1(this.Indicator, this.ParameterPanel.ColumnMode, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                default:
                    break;
            }
        }

        private void DisposeSingle5()
        {
            switch (this.LineMode)
            {
                case LineContainsNodeMode.None:
                    this.DisposeSingle3();
                    break;
                default:
                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.InitIndicator
                        | InvalidateModes.CanvasControl);
                    break;
            }
        }
        #endregion

        #region PanelTransform2
        private void CacheSingle6()
        {
            const float d = 12f;
            const float ds = d * d;

            switch (this.Composer.Count)
            {
                case 0:
                    this.PanelMode = BoxContainsNodeMode.None;
                    break;
                default:
                    this.PanelMode = this.Composer.ActualBox.ContainsNode(this.StartingPoint, ds);
                    break;
            }

            switch (this.PanelMode)
            {
                case BoxContainsNodeMode.None:
                    break;
                case BoxContainsNodeMode.Contains:
                    // Multiple Translation 1
                    this.CacheTranslationSelectedItems();
                    break;
                default:
                    // Multiple Transform 1
                    this.CacheTransformSelectedItems();
                    break;
            }

            switch (this.PanelMode)
            {
                case BoxContainsNodeMode.None: this.CacheSingle3(); break;
                // Multiple Translation 2
                case BoxContainsNodeMode.Contains: this.Composer.PanelCacheTranslation(); break;

                // Multiple Transform 2
                /*
                case BoxContainsNodeMode.HandleLeftTop:
                case BoxContainsNodeMode.HandleRightTop:
                case BoxContainsNodeMode.HandleLeftBottom:
                case BoxContainsNodeMode.HandleRightBottom: this.Composer.PanelCacheRotation(this.StartingPosition); break;
                 */

                case BoxContainsNodeMode.HandleLeft: this.Composer.PanelCacheTransform(TransformMode.SkewLeft); break;
                case BoxContainsNodeMode.HandleTop: this.Composer.PanelCacheTransform(TransformMode.SkewTop); break;
                case BoxContainsNodeMode.HandleRight: this.Composer.PanelCacheTransform(TransformMode.SkewRight); break;
                case BoxContainsNodeMode.HandleBottom: this.Composer.PanelCacheTransform(TransformMode.SkewBottom); break;

                case BoxContainsNodeMode.CenterLeft: this.Composer.PanelCacheTransform(TransformMode.ScaleLeft); break;
                case BoxContainsNodeMode.CenterTop: this.Composer.PanelCacheTransform(TransformMode.ScaleTop); break;
                case BoxContainsNodeMode.CenterRight: this.Composer.PanelCacheTransform(TransformMode.ScaleRight); break;
                case BoxContainsNodeMode.CenterBottom: this.Composer.PanelCacheTransform(TransformMode.ScaleBottom); break;

                case BoxContainsNodeMode.LeftTop: this.Composer.PanelCacheTransform(TransformMode.ScaleLeftTop); break;
                case BoxContainsNodeMode.RightTop: this.Composer.PanelCacheTransform(TransformMode.ScaleRightTop); break;
                case BoxContainsNodeMode.LeftBottom: this.Composer.PanelCacheTransform(TransformMode.ScaleLeftBottom); break;
                case BoxContainsNodeMode.RightBottom: this.Composer.PanelCacheTransform(TransformMode.ScaleRightBottom); break;
                default: break;
            }
        }

        private void Single6()
        {
            switch (this.PanelMode)
            {
                case BoxContainsNodeMode.None:
                    this.Single3();
                    break;
                case BoxContainsNodeMode.Contains:
                    this.Composer.PanelTranslate(this.Indicator, this.ParameterPanel.Mode, this.StartingPosition, this.Position);

                    this.TranslateSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
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
                        this.Composer.PanelRotate(this.Indicator, this.ParameterPanel.Mode, this.Position, StepFrequency);
                    else
                        this.Composer.PanelRotate(this.Indicator, this.ParameterPanel.Mode, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case BoxContainsNodeMode.HandleRight:
                case BoxContainsNodeMode.HandleBottom:
                    this.Composer.PanelTransformSkew(this.Indicator, this.ParameterPanel.Mode, this.Position, this.NodeKeepRatio, this.NodeCenteredScaling);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                default:
                    this.Composer.PanelTransformSize(this.Indicator, this.ParameterPanel.Mode, this.Position, this.NodeKeepRatio, this.NodeCenteredScaling);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
            }
        }

        private void DisposeSingle6()
        {
            switch (this.PanelMode)
            {
                case BoxContainsNodeMode.None:
                    this.DisposeSingle3();
                    break;
                default:
                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.InitIndicator
                        | InvalidateModes.CanvasControl);
                    break;
            }
        }
        #endregion

        #region ToolBox8
        private void NodeClose()
        {
            if (this.Layer.IsClosed is false)
            {
                this.Layer.IsClosed = true;

                this.Invalidate(InvalidateModes.None
                    //| InvalidateModes.InitIndicator 
                    | InvalidateModes.CanvasControl);
            }
        }

        private void NodeOpen()
        {
            if (this.Layer.IsClosed)
            {
                this.Layer.IsClosed = false;

                this.Invalidate(InvalidateModes.None
                    //| InvalidateModes.InitIndicator 
                    | InvalidateModes.CanvasControl);
            }
        }

        private void NodeSharp()
        {
            this.Layer.SharpSelectedItems();

            this.Invalidate(InvalidateModes.None
                | InvalidateModes.InitLayers
                | InvalidateModes.InitIndicator
                | InvalidateModes.CanvasControl);
        }

        private void NodeSmooth()
        {
            this.Layer.SmoothSelectedItems();

            this.Invalidate(InvalidateModes.None
                | InvalidateModes.InitLayers
                | InvalidateModes.InitIndicator
                | InvalidateModes.CanvasControl);
        }
        #endregion

        #region NodeMove
        private void Draw1(CanvasDrawingSession drawingSession)
        {
            foreach (Segment segment in this.Layer.Segments)
            {
                if (segment.IsChecked)
                {
                    drawingSession.DrawLine(segment.Actual.Point, segment.Actual.LeftControlPoint, Windows.UI.Colors.DeepSkyBlue);
                    drawingSession.DrawLine(segment.Actual.Point, segment.Actual.RightControlPoint, Windows.UI.Colors.DeepSkyBlue);
                }
            }

            drawingSession.DrawBounds(this.Layer.ActualBox);

            foreach (Segment segment in this.Layer.Segments)
            {
                if (segment.IsChecked)
                    drawingSession.DrawNode(segment.Actual);
                else
                    drawingSession.DrawNode3(segment.Actual.Point);
            }
        }
        private void Draw2(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            if (this.HasRectChoose)
            {
                drawingSession.Transform = this.Canvas.Matrix;
                drawingSession.FillRectChoose(new Rectangle(this.RectChoose));
                drawingSession.Transform = Matrix3x2.Identity;

                drawingSession.DrawRectChoose(this.ActualRectChoose);
            }
            else if (this.Closest.Contains)
            {
                Vector2 f = this.Canvas.Transform(this.Closest.Current.Point);

                if (this.Closest.CurrentIsSmooth)
                {
                    using (PathBuilder path = new PathBuilder(resourceCreator))
                    {
                        path.CreatePreviousPath(this.Closest, this.Canvas);
                        using (CanvasGeometry curve = CanvasGeometry.CreatePath(path.Builder))
                        {
                            drawingSession.DrawPreviousCurve(curve);
                        }
                    }

                    using (PathBuilder path = new PathBuilder(resourceCreator))
                    {
                        path.CreateNextPath(this.Closest, this.Canvas);
                        using (CanvasGeometry curve = CanvasGeometry.CreatePath(path.Builder))
                        {
                            drawingSession.DrawNextCurve(curve);
                        }
                    }
                }
                else
                {
                    Vector2 e = this.Canvas.Transform(this.Closest.Previous.Point);
                    Vector2 n = this.Canvas.Transform(this.Closest.Next.Point);

                    drawingSession.DrawPreviousLine(f, e);
                    drawingSession.DrawNextLine(f, n);
                }

                if (this.IsInContact)
                {
                    drawingSession.DrawLine(this.Point, f);
                    drawingSession.DrawNode(this.Point);
                }
                else
                {
                    Vector2 p = this.Canvas.Transform(this.Closest.Point);

                    drawingSession.DrawLine(p, f);
                    drawingSession.DrawNode(p);
                }

                drawingSession.DrawNode(f);
            }
        }
        private void Draw3(CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawBounds(this.Layer.ActualBox);

            foreach (Segment segment in this.Layer.Segments)
            {
                if (segment.IsChecked)
                    drawingSession.DrawNode(segment.Actual.Point);
                else
                    drawingSession.DrawNode3(segment.Actual.Point);
            }

            if (this.HasRectChoose)
            {
                drawingSession.Transform = this.Canvas.Matrix;
                drawingSession.FillRectChoose(new Rectangle(this.RectChoose));
                drawingSession.Transform = Matrix3x2.Identity;

                drawingSession.DrawRectChoose(this.ActualRectChoose);
            }
            else
            {
                switch (this.Composer.SizeType)
                {
                    case SizeType.Empty:
                        break;
                    case SizeType.Point:
                        drawingSession.DrawNode(this.Composer.ActualPoint);
                        break;
                    case SizeType.RowLine:
                    case SizeType.ColumnLine:
                        drawingSession.DrawLine(this.Composer.ActualLine);
                        break;
                    case SizeType.Panel:
                        drawingSession.DrawBox(this.Composer.ActualBox);
                        break;
                    default:
                        break;
                }
            }
        }

        private void CacheSingle2()
        {
            const float d = 12f;
            const float ds = d * d;

            const float cd = 10f;
            const float cds = cd * cd;

            this.Indexer = new SegmentIndexer(this.Layer.Segments, this.StartingPoint, ds, cds);

            switch (this.Indexer.Mode)
            {
                case SegmentIndexerMode.None:
                    float l = 4f * this.Canvas.InverseScaleFactor;
                    float ls = l * l;

                    this.Inserter = new SegmentInserter(ref this.Closest, NodePointUnits.Normal, this.Layer.Segments, this.Layer.IsClosed, this.Position, ls);

                    if (this.Inserter.Mode != SegmentInserterMode.None)
                    {
                        this.Layer.DeselectAll();

                        if (this.Inserter.Mode == SegmentInserterMode.Smooth)
                        {
                            this.Layer.Segments[this.Inserter.Previous] = new Segment
                            {
                                IsChecked = false,
                                IsSmooth = this.Closest.PreviousIsSmooth,

                                Starting = this.Closest.Previous,

                                Point = this.Closest.Previous,
                                Actual = this.Canvas.Transform(this.Closest.Previous),
                            };
                            this.Layer.Segments[this.Inserter.Next] = new Segment
                            {
                                IsChecked = false,
                                IsSmooth = this.Closest.NextIsSmooth,

                                Starting = this.Closest.Next,

                                Point = this.Closest.Next,
                                Actual = this.Canvas.Transform(this.Closest.Next),
                            };
                        }

                        this.Layer.Segments.Insert(this.Inserter.Current, new Segment
                        {
                            IsChecked = true,
                            IsSmooth = this.Closest.CurrentIsSmooth,

                            Starting = this.Closest.Current,

                            Point = this.Closest.Current,
                            Actual = this.Canvas.Transform(this.Closest.Current),
                        });

                        this.Indexer = new SegmentIndexer
                        {
                            Index = this.Inserter.Current,

                            Mode = SegmentIndexerMode.PointWithoutChecked,
                        };

                        this.Composer.Reset(this.Closest.Current.Point);
                        this.HasRectChoose = false;
                        this.Invalidate(InvalidateModes.CanvasControl);
                    }
                    else
                    {
                        this.CacheSingle3();
                    }
                    break;
                case SegmentIndexerMode.PointWithoutChecked:
                    {
                        this.Layer.Select(this.Indexer.Index);

                        Segment segment = this.Layer.Segments[this.Indexer.Index];

                        this.Composer.Reset(segment.Point.Point);

                        this.HasRectChoose = false;

                        this.Invalidate(InvalidateModes.None
                            | InvalidateModes.InitIndicator
                            | InvalidateModes.CanvasControl);
                    }
                    break;
                case SegmentIndexerMode.PointWithChecked:
                    switch (this.Composer.SizeType)
                    {
                        case SizeType.Empty:
                            break;
                        case SizeType.Point:
                            this.Composer.PointCacheTranslation();
                            break;
                        case SizeType.RowLine:
                            this.Composer.LineCacheTranslation();
                            break;
                        case SizeType.ColumnLine:
                            this.Composer.LineCacheTranslation();
                            break;
                        case SizeType.Panel:
                            this.Composer.PanelCacheTranslation();
                            break;
                        default:
                            break;
                    }

                    switch (this.Composer.SizeType)
                    {
                        case SizeType.Empty:
                            break;
                        default:
                            this.CacheTranslationSelectedItems();
                            break;
                    }
                    break;
                case SegmentIndexerMode.LeftControlPoint:
                case SegmentIndexerMode.RightControlPoint:
                    {
                        Segment segment = this.Layer.Segments[this.Indexer.Index];

                        bool isLeft = this.Indexer.Mode == SegmentIndexerMode.LeftControlPoint;
                        this.Controller = new NodeController(segment.Point, isLeft, this.Mode1, this.Mode2);

                        this.Layer.Segments[this.Indexer.Index] = new Segment
                        {
                            IsChecked = true,
                            IsSmooth = segment.IsSmooth,
                            Starting = segment.Point,

                            Point = segment.Point,
                            Actual = segment.Actual,
                        };

                        this.Layer.Complete();
                        this.Layer.UpdateCanvas(this.Canvas);

                        this.HasRectChoose = false;
                    }
                    break;
                default:
                    break;
            }
        }

        private void Single2()
        {
            switch (this.Indexer.Mode)
            {
                case SegmentIndexerMode.None:
                    this.Single3();
                    break;
                case SegmentIndexerMode.PointWithoutChecked:
                    {
                        this.Composer.PointTranslate(this.Indicator, this.StartingPosition, this.Position);

                        this.Layer.SetTranslation(this.Composer.TranslationX, this.Composer.TranslationY, this.Composer.PointPoint, this.Indexer.Index);

                        this.Invalidate(InvalidateModes.None
                            | InvalidateModes.UpdateLayers
                            | InvalidateModes.UpdateIndicator
                            | InvalidateModes.CanvasControl);
                    }
                    break;
                case SegmentIndexerMode.PointWithChecked:
                    switch (this.Composer.SizeType)
                    {
                        case SizeType.Empty:
                            break;
                        case SizeType.Point:
                            this.Composer.PointTranslate(this.Indicator, this.StartingPosition, this.Position);
                            break;
                        case SizeType.RowLine:
                            this.Composer.LineTranslate(this.Indicator, this.ParameterPanel.RowMode, this.StartingPosition, this.Position);
                            break;
                        case SizeType.ColumnLine:
                            this.Composer.LineTranslate(this.Indicator, this.ParameterPanel.ColumnMode, this.StartingPosition, this.Position);
                            break;
                        case SizeType.Panel:
                            this.Composer.PanelTranslate(this.Indicator, this.ParameterPanel.Mode, this.StartingPosition, this.Position);
                            break;
                        default:
                            break;
                    }

                    switch (this.Composer.SizeType)
                    {
                        case SizeType.Empty:
                            break;
                        default:
                            this.TranslateSelectedItems();
                            this.Layer.UpdateCanvas(this.Canvas);

                            this.Invalidate(InvalidateModes.None
                                | InvalidateModes.UpdateLayers
                                | InvalidateModes.UpdateIndicator
                                | InvalidateModes.CanvasControl);
                            break;
                    }
                    break;
                case SegmentIndexerMode.LeftControlPoint:
                case SegmentIndexerMode.RightControlPoint:
                    {
                        Segment segment = this.Layer.Segments[this.Indexer.Index];
                        Node point = this.Controller.ToNode(this.Position, this.Disconnected);

                        this.Layer.Segments[this.Indexer.Index] = new Segment
                        {
                            IsChecked = true,
                            IsSmooth = true,
                            Starting = segment.Starting,

                            Point = point,
                            Actual = Node.Transform(point, this.Canvas.Matrix),
                        };

                        this.Layer.Complete();
                        this.Layer.UpdateCanvas(this.Canvas);

                        this.Invalidate(InvalidateModes.None
                            | InvalidateModes.UpdateLayers
                            | InvalidateModes.UpdateIndicator
                            | InvalidateModes.CanvasControl);
                    }
                    break;
                default:
                    break;
            }
        }

        private void DisposeSingle2()
        {
            switch (this.Composer.SizeType)
            {
                case SizeType.Empty:
                    this.DisposeSingle3();
                    break;
                case SizeType.Point:
                    if (this.HasRectChoose)
                        goto case SizeType.Empty;

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case SizeType.RowLine:
                case SizeType.ColumnLine:
                    if (this.HasRectChoose)
                        goto case SizeType.Empty;

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case SizeType.Panel:
                    if (this.HasRectChoose)
                        goto case SizeType.Empty;

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                default:
                    break;
            }
        }

        private void Over2()
        {
            const float d = 12f;
            const float ds = d * d;

            const float cd = 10f;
            const float cds = cd * cd;

            this.Indexer = new SegmentIndexer(this.Layer.Segments, this.Point, ds, cds);

            switch (this.Indexer.Mode)
            {
                case SegmentIndexerMode.None:
                    float l = 4f * this.Canvas.InverseScaleFactor;
                    float ls = l * l;

                    if (this.Closest.Contains)
                    {
                        this.Closest = new ClosestPointer(NodePointUnits.Normal, this.Layer.Segments, this.Layer.IsClosed, this.Position, ls);
                        if (this.Closest.Contains)
                        {
                            this.Invalidate(InvalidateModes.CanvasControl);
                            return;
                        }

                        this.Closest = default;
                        this.Invalidate(InvalidateModes.CanvasControl);
                    }
                    else
                    {
                        this.Closest = new ClosestPointer(NodePointUnits.Normal, this.Layer.Segments, this.Layer.IsClosed, this.Position, ls);
                        if (this.Closest.Contains)
                        {
                            this.Invalidate(InvalidateModes.CanvasControl);
                            return;
                        }
                    }
                    break;
                case SegmentIndexerMode.PointWithoutChecked:
                    {
                        Segment segment = this.Layer.Segments[this.Indexer.Index];

                        this.Closest = new ClosestPointer(segment.Point.Point);

                        this.Invalidate(InvalidateModes.CanvasControl);
                    }
                    break;
                case SegmentIndexerMode.PointWithChecked:
                    {
                        Segment segment = this.Layer.Segments[this.Indexer.Index];

                        this.Closest = new ClosestPointer(segment.Point.Point);

                        this.Invalidate(InvalidateModes.CanvasControl);
                    }
                    break;
                case SegmentIndexerMode.LeftControlPoint:
                    {
                        Segment segment = this.Layer.Segments[this.Indexer.Index];

                        this.Closest = new ClosestPointer(segment.Point.LeftControlPoint);

                        this.Invalidate(InvalidateModes.CanvasControl);
                    }
                    break;
                case SegmentIndexerMode.RightControlPoint:
                    {
                        Segment segment = this.Layer.Segments[this.Indexer.Index];

                        this.Closest = new ClosestPointer(segment.Point.RightControlPoint);

                        this.Invalidate(InvalidateModes.CanvasControl);
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region NodeTransform
        private void Apply1(IndicatorKind kind, float value)
        {
            switch (this.Composer.ComposerSizeType(kind))
            {
                case ComposerSizeType.Empty:
                    break;
                case ComposerSizeType.PointX:
                    {
                        float translateX = value - this.Indicator.X;

                        this.Composer.PointSetTranslationX(this.Indicator, translateX);

                        this.SetTranslationXSelectedItems();
                    }
                    break;
                case ComposerSizeType.PointY:
                    {
                        float translateY = value - this.Indicator.Y;

                        this.Composer.PointSetTranslationY(this.Indicator, translateY);

                        this.SetTranslationYSelectedItems();
                    }
                    break;
                case ComposerSizeType.RowLineX:
                    {
                        RowLineMode mode = this.ParameterPanel.RowMode;

                        float translateX = value - this.Indicator.X;

                        this.Composer.LineSetTranslationX(this.Indicator, mode, translateX);

                        this.SetTranslationXSelectedItems();
                    }
                    break;
                case ComposerSizeType.RowLineY:
                    {
                        RowLineMode mode = this.ParameterPanel.RowMode;

                        float translateY = value - this.Indicator.Y;

                        this.Composer.LineSetTranslationY(this.Indicator, mode, translateY);

                        this.SetTranslationYSelectedItems();
                    }
                    break;
                case ComposerSizeType.RowLineWidth:
                    {
                        RowLineMode mode = this.ParameterPanel.RowMode;

                        this.Composer.LineSetWidth(this.Indicator, mode, value);

                        this.SetTransformSelectedItems();
                    }
                    break;
                case ComposerSizeType.RowLineRotation:
                    {
                        RowLineMode mode = this.ParameterPanel.RowMode;

                        this.Composer.LineSetRotation(this.Indicator, mode, value);

                        this.SetTransformSelectedItems();
                    }
                    break;
                case ComposerSizeType.ColumnLineX:
                    {
                        ColumnLineMode mode = this.ParameterPanel.ColumnMode;

                        float translateX = value - this.Indicator.X;

                        this.Composer.LineSetTranslationX(this.Indicator, mode, translateX);

                        this.SetTranslationXSelectedItems();
                    }
                    break;
                case ComposerSizeType.ColumnLineY:
                    {
                        ColumnLineMode mode = this.ParameterPanel.ColumnMode;

                        float translateY = value - this.Indicator.Y;

                        this.Composer.LineSetTranslationY(this.Indicator, mode, translateY);

                        this.SetTranslationYSelectedItems();
                    }
                    break;
                case ComposerSizeType.ColumnLineHeight:
                    {
                        ColumnLineMode mode = this.ParameterPanel.ColumnMode;

                        this.Composer.LineSetHeight(this.Indicator, mode, value);

                        this.SetTransformSelectedItems();
                    }
                    break;
                case ComposerSizeType.ColumnLineRotation:
                    {
                        ColumnLineMode mode = this.ParameterPanel.ColumnMode;

                        this.Composer.LineSetRotation(this.Indicator, mode, value);

                        this.SetTransformSelectedItems();
                    }
                    break;
                case ComposerSizeType.PanelX:
                    {
                        BoxMode mode = this.ParameterPanel.Mode;

                        float translateX = value - this.Indicator.X;

                        this.Composer.PanelSetTranslationX(this.Indicator, mode, translateX);

                        this.SetTranslationXSelectedItems();
                    }
                    break;
                case ComposerSizeType.PanelY:
                    {
                        BoxMode mode = this.ParameterPanel.Mode;

                        float translateY = value - this.Indicator.Y;

                        this.Composer.PanelSetTranslationY(this.Indicator, mode, translateY);

                        this.SetTranslationYSelectedItems();
                    }
                    break;
                case ComposerSizeType.PanelWidth:
                    {
                        BoxMode mode = this.ParameterPanel.Mode;

                        this.Composer.PanelSetWidth(this.Indicator, mode, value, this.NodeKeepRatio);

                        this.SetTransformSelectedItems();
                    }
                    break;
                case ComposerSizeType.PanelHeight:
                    {
                        BoxMode mode = this.ParameterPanel.Mode;

                        this.Composer.PanelSetHeight(this.Indicator, mode, value, this.NodeKeepRatio);

                        this.SetTransformSelectedItems();
                    }
                    break;
                case ComposerSizeType.PanelRotation:
                    {
                        BoxMode mode = this.ParameterPanel.Mode;

                        this.Composer.PanelSetRotation(this.Indicator, mode, value);

                        this.SetTransformSelectedItems();
                    }
                    break;
                case ComposerSizeType.PanelSkew:
                    {
                        BoxMode mode = this.ParameterPanel.Mode;

                        this.Composer.PanelSetSkew(this.Indicator, mode, value);

                        this.SetTransformSelectedItems();
                    }
                    break;
                default:
                    break;
            }

            this.Invalidate(InvalidateModes.None
                | InvalidateModes.UpdateLayers
                | InvalidateModes.UpdateIndicator
                | InvalidateModes.CanvasControl);
        }

        private void CacheSingle3()
        {
            this.RectChoose = new Bounds(this.StartingPosition);
            this.ActualRectChoose = new TransformedBounds(this.StartingPoint);
            this.HasRectChoose = true;

            this.Invalidate(InvalidateModes.CanvasControl);
        }

        private void Single3()
        {
            this.RectChoose = new Bounds(this.StartingPosition, this.Position);
            this.ActualRectChoose = this.RectChoose * this.Canvas.Matrix;

            this.Invalidate(InvalidateModes.CanvasControl);
        }

        private void DisposeSingle3()
        {
            this.RectChoose = new Bounds(this.StartingPosition, this.Position);
            this.ActualRectChoose = this.RectChoose * this.Canvas.Matrix;

            this.RectChooseItems();

            this.HasRectChoose = false;

            this.Invalidate(InvalidateModes.None
                | InvalidateModes.InitIndicator
                | InvalidateModes.CanvasControl);
        }
        #endregion

        #region Point
        private void CacheSingle4()
        {
            const float d = 12f;
            const float ds = d * d;

            this.PointMode = this.Composer.ActualPoint.ContainsNode(this.StartingPoint, ds);

            if (this.PointMode)
            {
                this.Composer.PointCacheTranslation();

                this.CacheTranslationSelectedItems();
            }
            else
            {
                this.CacheSingle3();
            }
        }

        private void Single4()
        {
            if (this.PointMode)
            {
                this.Composer.PointTranslate(this.Indicator, this.StartingPosition, this.Position);

                this.TranslateSelectedItems();

                this.Invalidate(InvalidateModes.None
                    | InvalidateModes.UpdateLayers
                    | InvalidateModes.UpdateIndicator
                    | InvalidateModes.CanvasControl);
            }
            else
            {
                this.Single3();
            }
        }

        private void DisposeSingle4()
        {
            if (this.PointMode)
            {
                this.Invalidate(InvalidateModes.None
                    //| InvalidateModes.InitIndicator
                    | InvalidateModes.CanvasControl);
            }
            else
            {
                this.DisposeSingle3();
            }
        }
        #endregion
    }
}