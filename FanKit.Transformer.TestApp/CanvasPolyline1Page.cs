using FanKit.Transformer.Cache;
using FanKit.Transformer.Controllers;
using FanKit.Transformer.Demos;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Input;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Polylines;
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
using Layer = FanKit.Transformer.Demos.DemoPolyline1;
using Segment = FanKit.Transformer.Polylines.Segment1;
using InvalidateModes = FanKit.Transformer.Sample.InvalidateModes123;
using LineContainsNodeMode = FanKit.Transformer.Cache.LineContainsNodeMode1;
using BoxContainsNodeMode = FanKit.Transformer.Cache.BoxContainsNodeMode1;

namespace FanKit.Transformer.TestApp
{
    /// <summary>
    /// <see cref="DemoHostComposer1"/>
    /// </summary>
    public sealed partial class CanvasPolyline1Page : CanvasSolo1Page
    {
        //@Key
        bool IsKeyDown(VirtualKey key) => Window.Current.CoreWindow.GetKeyState(key).HasFlag(CoreVirtualKeyStates.Down);
        bool IsCtrl => this.IsKeyDown(VirtualKey.Control);
        bool IsShift => this.IsKeyDown(VirtualKey.Shift);

        bool NodeCenteredScaling => this.IsCtrl || this.ToolBox6.CenteredScaling;
        bool NodeKeepRatio => this.IsShift || this.ToolBox6.KeepRatio;

        bool HasStepFrequency => this.IsShift;

        //@Const
        const float X = 256f;
        const float Y = 256f;

        const float StepFrequency = (float)System.Math.PI / 12f;
        const float StepFrequencyHalf = (float)System.Math.PI / 24f;

        // MultiCanvas
        Bounds RectChoose;
        TransformedBounds ActualRectChoose;
        bool HasRectChoose;

        // Composer1
        //int Index1 = -1;
        //int Index2 = -1;
        SegmentIndexer Indexer = SegmentIndexer.Empty;
        //NodeController Controller;

        FootPointer FootPoint;
        SegmentInserter Inserter = SegmentInserter.Empty;
        //NodeSmooth Smooth;

        bool PointMode;
        LineContainsNodeMode LineMode;
        BoxContainsNodeMode PanelMode;
        readonly DemoHostComposer1 Composer = new DemoHostComposer1();

        readonly Indicator Indicator = new Indicator();
        readonly Layer Layer = new Layer(new List<Segment>
        {
            new Segment{ Point = new Vector2(X - 144.6047f, Y - 138.5997f) },
            new Segment{ Point = new Vector2(X + 13.37953f, Y - 95.3983f) },
            new Segment{ Point = new Vector2(X - 12.58583f, Y + 87.00745f) },
            new Segment{ Point = new Vector2(X + 144.6047f, Y + 138.5997f) },
        })
        {
            StrokeWidth = 4f,
        };

        readonly TopBar1 TopBar = new TopBar1();
        readonly ToolBox6 ToolBox6 = new ToolBox6
        {
            Title = "Transform Node"
        };

        public CanvasPolyline1Page()
        {
            this.Child = this.TopBar;
            this.Children.Add(this.ToolBox6);

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

            this.ParameterPanel.ModeChanged += (s, e) => this.Indicator.ChangeXY(this.Composer.Panel.Triangle, e);
            this.ParameterPanel.RowModeChanged += (s, e) => this.Indicator.ChangeAll(this.Composer.Line.Point0, this.Composer.Line.Point1, e);
            this.ParameterPanel.ColumnModeChanged += (s, e) => this.Indicator.ChangeAll(this.Composer.Line.Point0, this.Composer.Line.Point1, e);

            this.ParameterPanel.Apply += (s, e) =>
            {
                this.Apply1(e, this.ParameterPanel.Value);
            };

            this.TopBar.ToolTypeChanged += delegate
            {
                switch (this.TopBar.ToolType)
                {
                    case ToolType1.NodeMove:
                        this.ToolBox6.Visibility = Visibility.Collapsed;
                        break;
                    case ToolType1.NodeTransform:
                        this.ToolBox6.Visibility = Visibility.Visible;
                        break;
                    default:
                        break;
                }

                this.Invalidate(InvalidateModes.None
                    | InvalidateModes.InitIndicator
                    | InvalidateModes.CanvasControl);
            };

            this.ToolBox6.CloseClick += delegate { this.NodeClose(); };
            this.ToolBox6.OpenClick += delegate { this.NodeOpen(); };
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
            drawingSession.DrawDashActualPolyline(this.Layer.Data, this.Layer.IsClosed, this.Layer.ActualStrokeWidth);

            switch (this.TopBar.ToolType)
            {
                case ToolType1.NodeMove: this.Draw1(drawingSession); this.Draw2(drawingSession); break;
                case ToolType1.NodeTransform: this.Draw3(drawingSession); break;
                default: break;
            }
        }

        public override void DrawThumb(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawDashPointPolyline(this.Layer.Data, this.Layer.IsClosed, this.Layer.StrokeWidth);
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
            this.Layer.TranslateSelectedItems(this.Composer.HostTranslateX, this.Composer.HostTranslateY);
        }
        private void TransformSelectedItems()
        {
            this.Layer.TransformSelectedItems(this.Composer.HostMatrix);
        }
        private void RectChooseItems()
        {
            this.Layer.RectChooseItems(this.RectChoose);
        }
        #endregion

        #region SetTransformSelectedItems
        private void SetTranslationXSelectedItems()
        {
            this.Layer.SetTranslationXSelectedItems(this.Composer.HostTranslateX);
        }
        private void SetTranslationYSelectedItems()
        {
            this.Layer.SetTranslationYSelectedItems(this.Composer.HostTranslateY);
        }
        private void SetTransformSelectedItems()
        {
            this.Layer.SetTransformSelectedItems(this.Composer.HostMatrix);
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
                foreach (Segment item in this.Layer.Data)
                {
                    this.Composer.Extend(item);
                }
                this.Composer.EndExtendByPoints();

                switch (this.Composer.SizeType)
                {
                    case SizeType.Empty: this.Indicator.ClearAll(); break;
                    case SizeType.Point: this.Indicator.ChangeAll(this.Composer.Point.Point); break;
                    case SizeType.RowLine: this.Indicator.ChangeAll(this.Composer.Line.Point0, this.Composer.Line.Point1, this.ParameterPanel.RowMode); break;
                    case SizeType.ColumnLine: this.Indicator.ChangeAll(this.Composer.Line.Point0, this.Composer.Line.Point1, this.ParameterPanel.ColumnMode); break;
                    case SizeType.Panel: this.Indicator.ChangeAll(this.Composer.Panel.Triangle, this.ParameterPanel.Mode); break;
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

        #region LineTransform1
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
                case LineContainsNodeMode.Handle0: this.Composer.Line.CacheElongation0(); break;
                case LineContainsNodeMode.Handle1: this.Composer.Line.CacheElongation1(); break;
                case LineContainsNodeMode.Handle: this.Composer.Line.CacheRotation(this.StartingPosition); break;
                 */
                case LineContainsNodeMode.Center: this.Composer.Line.CacheTranslation(); break;
                case LineContainsNodeMode.Point0: this.Composer.Line.CacheMovement(); break;
                case LineContainsNodeMode.Point1: this.Composer.Line.CacheMovement(); break;
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
                    this.Composer.Line.ElongatePoint0(this.Indicator, this.ParameterPanel.RowMode, this.StartingPosition, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Handle1:
                    this.Composer.Line.ElongatePoint1(this.Indicator, this.ParameterPanel.RowMode, this.StartingPosition, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Handle:
                    if (this.HasStepFrequency)
                        this.Composer.Line.Rotate(this.Indicator, this.ParameterPanel.RowMode, this.Position, StepFrequency);
                    else
                        this.Composer.Line.Rotate(this.Indicator, this.ParameterPanel.RowMode, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                 */
                case LineContainsNodeMode.Center:
                    this.Composer.Line.Translate(this.Indicator, this.ParameterPanel.RowMode, this.StartingPosition, this.Position);

                    this.TranslateSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Point0:
                    this.Composer.Line.MovePoint0(this.Indicator, this.ParameterPanel.RowMode, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Point1:
                    this.Composer.Line.MovePoint1(this.Indicator, this.ParameterPanel.RowMode, this.Position);

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
                    this.Composer.Line.ElongatePoint0(this.Indicator, this.ParameterPanel.ColumnMode, this.StartingPosition, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Handle1:
                    this.Composer.Line.ElongatePoint1(this.Indicator, this.ParameterPanel.ColumnMode, this.StartingPosition, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Handle:
                    if (this.HasStepFrequency)
                        this.Composer.Line.Rotate(this.Indicator, this.ParameterPanel.ColumnMode, this.Position, StepFrequency);
                    else
                        this.Composer.Line.Rotate(this.Indicator, this.ParameterPanel.ColumnMode, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                 */
                case LineContainsNodeMode.Center:
                    this.Composer.Line.Translate(this.Indicator, this.ParameterPanel.ColumnMode, this.StartingPosition, this.Position);

                    this.TranslateSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Point0:
                    this.Composer.Line.MovePoint0(this.Indicator, this.ParameterPanel.ColumnMode, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Point1:
                    this.Composer.Line.MovePoint1(this.Indicator, this.ParameterPanel.ColumnMode, this.Position);

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

        #region PanelTransform1
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
                case BoxContainsNodeMode.Contains: this.Composer.Panel.CacheTranslation(); break;

                // Multiple Transform 2
                /*
                case BoxContainsNodeMode.HandleLeftTop:
                case BoxContainsNodeMode.HandleRightTop:
                case BoxContainsNodeMode.HandleLeftBottom:
                case BoxContainsNodeMode.HandleRightBottom: this.Composer.Panel.CacheRotation(this.StartingPosition); break;

                case BoxContainsNodeMode.HandleLeft: this.Composer.Panel.CacheTransform(TransformMode.SkewLeft); break;
                case BoxContainsNodeMode.HandleTop: this.Composer.Panel.CacheTransform(TransformMode.SkewTop); break;
                case BoxContainsNodeMode.HandleRight: this.Composer.Panel.CacheTransform(TransformMode.SkewRight); break;
                case BoxContainsNodeMode.HandleBottom: this.Composer.Panel.CacheTransform(TransformMode.SkewBottom); break;
                 */

                case BoxContainsNodeMode.CenterLeft: this.Composer.Panel.CacheTransform(TransformMode.ScaleLeft); break;
                case BoxContainsNodeMode.CenterTop: this.Composer.Panel.CacheTransform(TransformMode.ScaleTop); break;
                case BoxContainsNodeMode.CenterRight: this.Composer.Panel.CacheTransform(TransformMode.ScaleRight); break;
                case BoxContainsNodeMode.CenterBottom: this.Composer.Panel.CacheTransform(TransformMode.ScaleBottom); break;

                case BoxContainsNodeMode.LeftTop: this.Composer.Panel.CacheTransform(TransformMode.ScaleLeftTop); break;
                case BoxContainsNodeMode.RightTop: this.Composer.Panel.CacheTransform(TransformMode.ScaleRightTop); break;
                case BoxContainsNodeMode.LeftBottom: this.Composer.Panel.CacheTransform(TransformMode.ScaleLeftBottom); break;
                case BoxContainsNodeMode.RightBottom: this.Composer.Panel.CacheTransform(TransformMode.ScaleRightBottom); break;
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
                    this.Composer.Panel.Translate(this.Indicator, this.ParameterPanel.Mode, this.StartingPosition, this.Position);

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
                    if (this.HasStepFrequency)
                        this.Composer.Panel.Rotate(this.Indicator, this.ParameterPanel.Mode, this.Position, StepFrequency);
                    else
                        this.Composer.Panel.Rotate(this.Indicator, this.ParameterPanel.Mode, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case BoxContainsNodeMode.HandleLeft:
                case BoxContainsNodeMode.HandleTop:
                case BoxContainsNodeMode.HandleRight:
                case BoxContainsNodeMode.HandleBottom:
                    this.Composer.Panel.TransformSkew(this.Indicator, this.ParameterPanel.Mode, this.Position, this.NodeKeepRatio, this.NodeCenteredScaling);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                 */
                default:
                    this.Composer.Panel.TransformSize(this.Indicator, this.ParameterPanel.Mode, this.Position, this.NodeKeepRatio, this.NodeCenteredScaling);

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

        #region ToolBox6
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
        #endregion

        #region NodeMove
        private void Draw1(CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawBounds(this.Layer.ActualBox);

            foreach (Segment segment in this.Layer.Data)
            {
                if (segment.IsChecked)
                    drawingSession.DrawNode(segment.Actual);
                else
                    drawingSession.DrawNode3(segment.Actual);
            }
        }
        private void Draw2(CanvasDrawingSession drawingSession)
        {
            if (this.HasRectChoose)
            {
                drawingSession.Transform = this.Canvas.Matrix;
                drawingSession.FillRectChoose(new Rectangle(this.RectChoose));
                drawingSession.Transform = Matrix3x2.Identity;

                drawingSession.DrawRectChoose(this.ActualRectChoose);
            }
            else if (this.FootPoint.Contains)
            {
                Vector2 f = this.Canvas.Transform(this.FootPoint.Foot);

                Vector2 e = this.Canvas.Transform(this.FootPoint.LinePoint0);
                Vector2 n = this.Canvas.Transform(this.FootPoint.LinePoint1);

                drawingSession.DrawPreviousLine(f, e);
                drawingSession.DrawNextLine(f, n);

                if (this.IsInContact)
                {
                    drawingSession.DrawLine(this.Point, f);
                    drawingSession.DrawNode(this.Point);
                }
                else
                {
                    Vector2 p = this.Canvas.Transform(this.FootPoint.Point);

                    drawingSession.DrawLine(p, f);
                    drawingSession.DrawNode(p);
                }

                drawingSession.DrawNode(f);
            }
        }
        private void Draw3(CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawBounds(this.Layer.ActualBox);

            foreach (Segment segment in this.Layer.Data)
            {
                if (segment.IsChecked)
                    drawingSession.DrawNode(segment.Actual);
                else
                    drawingSession.DrawNode3(segment.Actual);
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

            this.Indexer = new SegmentIndexer(this.Layer.Data, this.StartingPoint, ds);

            switch (this.Indexer.Mode)
            {
                case SegmentMode.None:
                    float l = 4f * this.Canvas.InverseScaleFactor;
                    float ls = l * l;

                    this.Inserter = new SegmentInserter(ref this.FootPoint, NodePointUnits.Normal, this.Layer.Data, this.Layer.IsClosed, this.Position, ls);

                    if (this.Inserter.Contains != false)
                    {
                        this.Layer.DeselectAll();

                        this.Layer.Data.Insert(this.Inserter.Index, new Segment
                        {
                            IsChecked = true,

                            Starting = this.FootPoint.Foot,

                            Point = this.FootPoint.Foot,
                            Actual = this.Canvas.Transform(this.FootPoint.Foot),
                        });

                        this.Indexer = new SegmentIndexer
                        {
                            Index = this.Inserter.Index,

                            Mode = SegmentMode.PointWithoutChecked,
                        };

                        this.Composer.Reset(this.FootPoint.Foot);
                        this.HasRectChoose = false;
                        this.Invalidate(InvalidateModes.CanvasControl);
                    }
                    else
                    {
                        this.CacheSingle3();
                    }
                    break;
                case SegmentMode.PointWithoutChecked:
                    {
                        this.Layer.Select(this.Indexer.Index);

                        Segment segment = this.Layer.Data[this.Indexer.Index];

                        this.Composer.Reset(segment.Point);

                        this.HasRectChoose = false;

                        this.Invalidate(InvalidateModes.None
                            | InvalidateModes.InitIndicator
                            | InvalidateModes.CanvasControl);
                    }
                    break;
                case SegmentMode.PointWithChecked:
                    switch (this.Composer.SizeType)
                    {
                        case SizeType.Empty:
                            break;
                        case SizeType.Point:
                            this.Composer.Point.CacheTranslation();
                            break;
                        case SizeType.RowLine:
                            this.Composer.Line.CacheTranslation();
                            break;
                        case SizeType.ColumnLine:
                            this.Composer.Line.CacheTranslation();
                            break;
                        case SizeType.Panel:
                            this.Composer.Panel.CacheTranslation();
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
                default:
                    break;
            }
        }

        private void Single2()
        {
            switch (this.Indexer.Mode)
            {
                case SegmentMode.None:
                    this.Single3();
                    break;
                case SegmentMode.PointWithoutChecked:
                    {
                        this.Composer.Point.Translate(this.Indicator, this.StartingPosition, this.Position);

                        this.Layer.SetTranslation(this.Composer.HostTranslateX, this.Composer.HostTranslateY, this.Composer.Point.Point, this.Indexer.Index);

                        this.Invalidate(InvalidateModes.None
                            | InvalidateModes.UpdateLayers
                            | InvalidateModes.UpdateIndicator
                            | InvalidateModes.CanvasControl);
                    }
                    break;
                case SegmentMode.PointWithChecked:
                    switch (this.Composer.SizeType)
                    {
                        case SizeType.Empty:
                            break;
                        case SizeType.Point:
                            this.Composer.Point.Translate(this.Indicator, this.StartingPosition, this.Position);
                            break;
                        case SizeType.RowLine:
                            this.Composer.Line.Translate(this.Indicator, this.ParameterPanel.RowMode, this.StartingPosition, this.Position);
                            break;
                        case SizeType.ColumnLine:
                            this.Composer.Line.Translate(this.Indicator, this.ParameterPanel.ColumnMode, this.StartingPosition, this.Position);
                            break;
                        case SizeType.Panel:
                            this.Composer.Panel.Translate(this.Indicator, this.ParameterPanel.Mode, this.StartingPosition, this.Position);
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

            this.Indexer = new SegmentIndexer(this.Layer.Data, this.Point, ds);

            switch (this.Indexer.Mode)
            {
                case SegmentMode.None:
                    float l = 4f * this.Canvas.InverseScaleFactor;
                    float ls = l * l;

                    if (this.FootPoint.Contains)
                    {
                        this.FootPoint = new FootPointer(NodePointUnits.Normal, this.Layer.Data, this.Layer.IsClosed, this.Position, ls);
                        if (this.FootPoint.Contains)
                        {
                            this.Invalidate(InvalidateModes.CanvasControl);
                            return;
                        }

                        this.FootPoint = default;
                        this.Invalidate(InvalidateModes.CanvasControl);
                    }
                    else
                    {
                        this.FootPoint = new FootPointer(NodePointUnits.Normal, this.Layer.Data, this.Layer.IsClosed, this.Position, ls);
                        if (this.FootPoint.Contains)
                        {
                            this.Invalidate(InvalidateModes.CanvasControl);
                            return;
                        }
                    }
                    break;
                case SegmentMode.PointWithoutChecked:
                    {
                        Segment segment = this.Layer.Data[this.Indexer.Index];

                        this.FootPoint = new FootPointer(segment.Point);

                        this.Invalidate(InvalidateModes.CanvasControl);
                    }
                    break;
                case SegmentMode.PointWithChecked:
                    {
                        Segment segment = this.Layer.Data[this.Indexer.Index];

                        this.FootPoint = new FootPointer(segment.Point);

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

                        this.Composer.Point.SetTranslationX(this.Indicator, translateX);

                        this.SetTranslationXSelectedItems();
                    }
                    break;
                case ComposerSizeType.PointY:
                    {
                        float translateY = value - this.Indicator.Y;

                        this.Composer.Point.SetTranslationY(this.Indicator, translateY);

                        this.SetTranslationYSelectedItems();
                    }
                    break;
                case ComposerSizeType.RowLineX:
                    {
                        RowLineMode mode = this.ParameterPanel.RowMode;

                        float translateX = value - this.Indicator.X;

                        this.Composer.Line.SetTranslationX(this.Indicator, mode, translateX);

                        this.SetTranslationXSelectedItems();
                    }
                    break;
                case ComposerSizeType.RowLineY:
                    {
                        RowLineMode mode = this.ParameterPanel.RowMode;

                        float translateY = value - this.Indicator.Y;

                        this.Composer.Line.SetTranslationY(this.Indicator, mode, translateY);

                        this.SetTranslationYSelectedItems();
                    }
                    break;
                case ComposerSizeType.RowLineWidth:
                    {
                        RowLineMode mode = this.ParameterPanel.RowMode;

                        this.Composer.Line.SetWidth(this.Indicator, mode, value);

                        this.SetTransformSelectedItems();
                    }
                    break;
                case ComposerSizeType.RowLineRotation:
                    {
                        RowLineMode mode = this.ParameterPanel.RowMode;

                        this.Composer.Line.SetRotation(this.Indicator, mode, value);

                        this.SetTransformSelectedItems();
                    }
                    break;
                case ComposerSizeType.ColumnLineX:
                    {
                        ColumnLineMode mode = this.ParameterPanel.ColumnMode;

                        float translateX = value - this.Indicator.X;

                        this.Composer.Line.SetTranslationX(this.Indicator, mode, translateX);

                        this.SetTranslationXSelectedItems();
                    }
                    break;
                case ComposerSizeType.ColumnLineY:
                    {
                        ColumnLineMode mode = this.ParameterPanel.ColumnMode;

                        float translateY = value - this.Indicator.Y;

                        this.Composer.Line.SetTranslationY(this.Indicator, mode, translateY);

                        this.SetTranslationYSelectedItems();
                    }
                    break;
                case ComposerSizeType.ColumnLineHeight:
                    {
                        ColumnLineMode mode = this.ParameterPanel.ColumnMode;

                        this.Composer.Line.SetHeight(this.Indicator, mode, value);

                        this.SetTransformSelectedItems();
                    }
                    break;
                case ComposerSizeType.ColumnLineRotation:
                    {
                        ColumnLineMode mode = this.ParameterPanel.ColumnMode;

                        this.Composer.Line.SetRotation(this.Indicator, mode, value);

                        this.SetTransformSelectedItems();
                    }
                    break;
                case ComposerSizeType.PanelX:
                    {
                        BoxMode mode = this.ParameterPanel.Mode;

                        float translateX = value - this.Indicator.X;

                        this.Composer.Panel.SetTranslationX(this.Indicator, mode, translateX);

                        this.SetTranslationXSelectedItems();
                    }
                    break;
                case ComposerSizeType.PanelY:
                    {
                        BoxMode mode = this.ParameterPanel.Mode;

                        float translateY = value - this.Indicator.Y;

                        this.Composer.Panel.SetTranslationY(this.Indicator, mode, translateY);

                        this.SetTranslationYSelectedItems();
                    }
                    break;
                case ComposerSizeType.PanelWidth:
                    {
                        BoxMode mode = this.ParameterPanel.Mode;

                        this.Composer.Panel.SetWidth(this.Indicator, mode, value, this.NodeKeepRatio);

                        this.SetTransformSelectedItems();
                    }
                    break;
                case ComposerSizeType.PanelHeight:
                    {
                        BoxMode mode = this.ParameterPanel.Mode;

                        this.Composer.Panel.SetHeight(this.Indicator, mode, value, this.NodeKeepRatio);

                        this.SetTransformSelectedItems();
                    }
                    break;
                case ComposerSizeType.PanelRotation:
                    {
                        BoxMode mode = this.ParameterPanel.Mode;

                        this.Composer.Panel.SetRotation(this.Indicator, mode, value);

                        this.SetTransformSelectedItems();
                    }
                    break;
                case ComposerSizeType.PanelSkew:
                    {
                        BoxMode mode = this.ParameterPanel.Mode;

                        this.Composer.Panel.SetSkew(this.Indicator, mode, value);

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
                this.Composer.Point.CacheTranslation();

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
                this.Composer.Point.Translate(this.Indicator, this.StartingPosition, this.Position);

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