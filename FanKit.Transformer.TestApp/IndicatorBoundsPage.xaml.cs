using FanKit.Transformer.Cache;
using FanKit.Transformer.Controllers;
using FanKit.Transformer.Demos;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Input;
using FanKit.Transformer.Sample;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class IndicatorBoundsPage : Page
    {
        //@Key
        bool IsKeyDown(VirtualKey key) => Window.Current.CoreWindow.GetKeyState(key).HasFlag(CoreVirtualKeyStates.Down);
        bool IsCtrl => this.IsKeyDown(VirtualKey.Control);
        bool IsShift => this.IsKeyDown(VirtualKey.Shift);

        //bool CenteredScaling => this.IsCtrl || this.CenterButton.IsOn;
        bool KeepRatio => this.IsShift || this.RatioButton.IsOn;

        const int SourceX = 100;
        const int SourceY = 100;
        const int SourceWidth = 300;
        const int SourceHeight = 300;

        //const float SkewAngleInDegreesMinimum = -85f;
        //const float SkewAngleInDegreesMaximum = 85f;

        static readonly Bounds ResetDestination = new Bounds
        {
            Left = SourceX,
            Top = SourceY,
            Right = SourceX + SourceWidth,
            Bottom = SourceY + SourceHeight,
        };

        readonly Indicator Indicator = new Indicator();

        PanelAnchorMode AnchorMode = PanelAnchorMode.LeftTop;
        Bounds StartingDestination = ResetDestination;
        Bounds Destination = ResetDestination;
        bool DisableSlider = false;

        public IndicatorBoundsPage()
        {
            this.InitializeComponent();
            this.Indicator.ChangeAll(this.Destination, this.AnchorMode);

            this.ResetUI();
            this.ResetSliders();

            this.AnchorModeListView.ItemsSource = new List<PanelAnchorMode>
            {
                PanelAnchorMode.LeftTop,
                PanelAnchorMode.RightTop,
                PanelAnchorMode.LeftBottom,
                PanelAnchorMode.RightBottom,

                PanelAnchorMode.CenterLeft,
                PanelAnchorMode.CenterTop,
                PanelAnchorMode.CenterRight,
                PanelAnchorMode.CenterBottom,

                PanelAnchorMode.Center,
            };
            this.AnchorModeListView.SelectedIndex = 0; // PanelAnchorMode.LeftTop

            this.XSlider.ValueChanged += (s, e) =>
            {
                if (this.DisableSlider)
                    return;

                float value = (float)e.NewValue;
                float translateX = value - this.Indicator.X;

                this.StartingDestination = this.Destination;
                this.Destination = Bounds.TranslateX(this.StartingDestination, translateX);

                this.Indicator.ChangeX(this.Destination, this.AnchorMode);
                this.ResetUI();
            };
            this.YSlider.ValueChanged += (s, e) =>
            {
                if (this.DisableSlider)
                    return;

                float value = (float)e.NewValue;
                float translateY = value - this.Indicator.Y;

                this.StartingDestination = this.Destination;
                this.Destination = Bounds.TranslateY(this.StartingDestination, translateY);

                this.Indicator.ChangeY(this.Destination, this.AnchorMode);
                this.ResetUI();
            };

            this.WidthSlider.ValueChanged += (s, e) =>
            {
                if (this.DisableSlider)
                    return;

                float value = (float)e.NewValue;

                this.StartingDestination = this.Destination;
                this.Destination = this.Indicator.CreateWidth(this.StartingDestination, this.AnchorMode, value, KeepRatio);

                this.Indicator.ChangeXYWH(this.Destination, this.AnchorMode);
                this.ResetUI();
            };
            this.HeightSlider.ValueChanged += (s, e) =>
            {
                if (this.DisableSlider)
                    return;

                float value = (float)e.NewValue;

                this.StartingDestination = this.Destination;
                this.Destination = this.Indicator.CreateHeight(this.StartingDestination, this.AnchorMode, value, KeepRatio);

                this.Indicator.ChangeXYWH(this.Destination, this.AnchorMode);
                this.ResetUI();
            };

            /*
            this.RotationSlider.ValueChanged += (s, e) =>
            {
                if (this.DisableSlider)
                    return;

                var rotationAngleInDegrees = (float)e.NewValue;

                var hostMatrix = this.Indicator.CreateRotation(rotationAngleInDegrees);

                this.StartingDestination = this.Destination;
                this.Destination = Bounds.Transform(this.StartingDestination, hostMatrix);

                this.Indicator.ChangeXYWHRS(this.Destination, this.AnchorMode);
                this.ResetUI();
            };
            this.SkewSlider.ValueChanged += (s, e) =>
            {
                if (this.DisableSlider)
                    return;

                var skewAngleInDegrees = (float)e.NewValue;

                this.StartingDestination = this.Destination;
                this.Destination = this.Indicator.CreateSkew(this.StartingDestination, this.AnchorMode, skewAngleInDegrees, SkewAngleInDegreesMinimum, SkewAngleInDegreesMaximum);

                this.Indicator.ChangeXYWHRS(this.Destination, this.AnchorMode);
                this.ResetUI();
            };
             */

            this.ResetButton.Click += delegate
            {
                this.Destination = ResetDestination;

                this.DisableSlider = true;
                this.Indicator.ChangeAll(this.Destination, this.AnchorMode);
                this.ResetUI();
                this.ResetSliders();
                this.DisableSlider = false;
            };
            this.AnchorModeListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is PanelAnchorMode item)
                {
                    this.AnchorMode = item;
                    this.Destination = ResetDestination;

                    this.DisableSlider = true;
                    this.Indicator.ChangeAll(this.Destination, this.AnchorMode);
                    this.ResetUI();
                    this.ResetSliders();
                    this.DisableSlider = false;
                }
            };
        }

        private void ResetUI()
        {
            this.LeftRun.Text = this.Destination.Left.ToString();
            this.TopRun.Text = this.Destination.Top.ToString();
            this.RightRun.Text = this.Destination.Right.ToString();
            this.BottomRun.Text = this.Destination.Bottom.ToString();

            this.Line30.X2 = this.Line01.X1 = this.Destination.Left;
            this.Line30.Y2 = this.Line01.Y1 = this.Destination.Top;

            this.Line01.X2 = this.Line12.X1 = this.Destination.Right;
            this.Line01.Y2 = this.Line12.Y1 = this.Destination.Top;

            this.Line12.X2 = this.Line23.X1 = this.Destination.Right;
            this.Line12.Y2 = this.Line23.Y1 = this.Destination.Bottom;

            this.Line23.X2 = this.Line30.X1 = this.Destination.Left;
            this.Line23.Y2 = this.Line30.Y1 = this.Destination.Bottom;

            Canvas.SetLeft(this.Ellipse, this.Indicator.X - 4f);
            Canvas.SetTop(this.Ellipse, this.Indicator.Y - 4f);
        }

        private void ResetSliders()
        {
            this.XSlider.Value = this.Indicator.X;
            this.YSlider.Value = this.Indicator.Y;
            this.WidthSlider.Value = this.Indicator.Width;
            this.HeightSlider.Value = this.Indicator.Height;
            //this.RotationSlider.Value = this.Indicator.Rotation;
            //this.SkewSlider.Value = this.Indicator.Skew;
        }
    }
}