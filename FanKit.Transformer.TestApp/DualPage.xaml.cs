using FanKit.Transformer.Cache;
using FanKit.Transformer.Controllers;
using FanKit.Transformer.Demos;
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

namespace FanKit.Transformer.TestApp
{
    /// <summary>
    /// <see cref="CanvasOperator1"/>
    /// </summary>
    public abstract partial class DualPage : Page
    {
        public Vector2 StartingPoint;
        public Vector2 Point;

        readonly Matrix3x2 ThumbMatrix = Matrix3x2.CreateScale(0.2f);
        readonly CanvasOperator1 CanvasOperatorSource;
        readonly CanvasOperator1 CanvasOperatorDestination;

        public bool IsInContact => this.CanvasOperatorSource.IsInContact;

        public DualPage()
        {
            this.InitializeComponent();
            this.CanvasOperatorSource = new CanvasOperator1(this.CanvasControlSource);
            this.CanvasOperatorDestination = new CanvasOperator1(this.CanvasControlDestination);
            base.Unloaded += delegate
            {
                // Explicitly remove references to allow the Win2D controls to get garbage collected
                this.CanvasControlSource.RemoveFromVisualTree();
                this.CanvasControlDestination.RemoveFromVisualTree();
                this.ThumbCanvasControl.RemoveFromVisualTree();
                this.CanvasControlSource = null;
                this.CanvasControlDestination = null;
                this.ThumbCanvasControl = null;
            };

            this.CanvasControlSource.CreateResources += (s, args) =>
            {
                this.CreateResources(s, args);
            };
            this.CanvasControlDestination.CreateResources += (s, args) =>
            {
                this.CreateResources(s, args);
            };
            this.CanvasControlSource.Draw += (s, e) =>
            {
                this.DrawSource(s, e.DrawingSession);
            };
            this.CanvasControlDestination.Draw += (s, e) =>
            {
                this.DrawDestination(s, e.DrawingSession);
            };

            this.CanvasOperatorSource.Single_Start += (startingX, startingY, p) =>
            {
                this.StartingPoint = this.Point = new Vector2((float)startingX, (float)startingY);

                this.CacheSingle();
            };
            /*
            this.CanvasOperatorDestination.Single_Start += (startingX, startingY, p) =>
            {
                this.StartingPoint = this.Point = new Vector2((float)startingX, (float)startingY);

                this.CacheSingle();
            };
             */
            this.CanvasOperatorSource.Single_Delta += (x, y, p) =>
            {
                this.Point = new Vector2((float)x, (float)y);

                this.Single();
            };
            /*
            this.CanvasOperatorDestination.Single_Delta += (x, y, p) =>
            {
                this.Point = new Vector2((float)x, (float)y);

                this.Single();
            };
             */
            this.CanvasOperatorSource.Single_Complete += (x, y, p) =>
            {
                this.Point = new Vector2((float)x, (float)y);

                this.DisposeSingle();
            };
            /*
            this.CanvasOperatorDestination.Single_Complete += (x, y, p) =>
            {
                this.Point = new Vector2((float)x, (float)y);

                this.DisposeSingle();
            };
             */

            this.ThumbCanvasControl.CreateResources += (s, args) =>
            {
                this.CreateResources(s, args);
            };
            this.ThumbCanvasControl.Draw += (s, e) =>
            {
                e.DrawingSession.Transform = this.ThumbMatrix;

                this.DrawThumb(s, e.DrawingSession);
            };

            this.BottomBar.HideToggled += delegate
            {
                this.ToolBox3.Visibility =
                this.ThumbBorder.Visibility =
                this.ParameterPanel.Visibility =
                this.BottomBar.IsHidden ? Visibility.Collapsed : Visibility.Visible;
            };
        }

        public abstract void CreateResources(ICanvasResourceCreator resourceCreator, CanvasCreateResourcesEventArgs args);
        public abstract void DrawSource(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession);
        public abstract void DrawDestination(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession);
        public abstract void DrawThumb(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession);

        public abstract void CacheSingle();
        public abstract void Single();
        public abstract void DisposeSingle();
        public abstract void Over();

        public abstract void UpdateCanvasControl1();
        public abstract void UpdateCanvasControl2();

        public void Invalidate()
        {
            this.CanvasControlSource.Invalidate();
            this.CanvasControlDestination.Invalidate();
            this.ThumbCanvasControl.Invalidate(); // Invalidate
        }
    }
}