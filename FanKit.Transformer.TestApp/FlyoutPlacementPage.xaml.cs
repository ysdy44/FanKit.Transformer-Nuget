using FanKit.Transformer.UI;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public sealed class FlyoutPlacementGroupingList : List<FlyoutPlacementGrouping>
    {
        public FlyoutPlacementGroupingList() : base(
            System.Enum.GetValues(typeof(FlyoutPlacementPriority))
            .Cast<FlyoutPlacementPriority>()
            .Reverse()
            .GroupBy(p => p.ToString().First())
            .Select(g => new FlyoutPlacementGrouping(g.Key, g)))
        {
        }
    }

    public sealed class FlyoutPlacementGrouping : List<FlyoutPlacementPriority>, IList<FlyoutPlacementPriority>, IGrouping<char, FlyoutPlacementPriority>
    {
        public char Key { get; }
        public FlyoutPlacementGrouping(char key, IEnumerable<FlyoutPlacementPriority> collection) : base(collection) => this.Key = key;
        public override string ToString() => this.Key.ToString();
    }

    public sealed partial class FlyoutPlacementPage : Page
    {
        FlyoutMetrics Metrics;
        FlyoutLocation Location = new FlyoutLocation
        {
            Priority = FlyoutPlacementPriority.BottomRightTopLeft,

            BoundsPadding = new Bounds(12),
            ContentMargin = new Bounds(12),
            CornerRadius = 32,

            ArrowHeight = 70,
            ArrowWidth = 40,

            ContentWidth = 200f,
            ContentHeight = 200f,
        };

        public FlyoutPlacementPage()
        {
            this.InitializeComponent();

            this.Slider0.Value = this.Location.ArrowHeight;
            this.Slider1.Value = this.Location.ArrowWidth;
            this.Slider2.Value = this.Location.CornerRadius;
            this.Slider3.Value = this.Location.ContentMargin.Left;
            this.Slider4.Value = this.Location.BoundsPadding.Left;

            this.XListView.ItemsSource = new List<string>
            {
                nameof(HorizontalAlignment.Left),
                nameof(HorizontalAlignment.Center),
                nameof(HorizontalAlignment.Right),
                nameof(HorizontalAlignment.Stretch),
            };
            this.YListView.ItemsSource = new List<string>
            {
                nameof(VerticalAlignment.Top),
                nameof(VerticalAlignment.Center),
                nameof(VerticalAlignment.Bottom),
                nameof(VerticalAlignment.Stretch),
            };

            this.XListView.SelectedIndex = this.Location.FixedHorizontalAlignment;
            this.YListView.SelectedIndex = this.Location.FixedVerticalAlignment;

            this.ListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is FlyoutPlacementPriority item)
                {
                    this.Location.Priority = item;
                    this.Update();
                }
            };

            this.Slider0.ValueChanged += (s, e) => { this.Location.ArrowHeight = (float)e.NewValue; this.Update(); };
            this.Slider1.ValueChanged += (s, e) => { this.Location.ArrowWidth = (float)e.NewValue; this.Update(); };
            this.Slider2.ValueChanged += (s, e) => { this.Location.CornerRadius = (float)e.NewValue; this.Update(); };
            this.Slider3.ValueChanged += (s, e) => { this.Location.ContentMargin = new Bounds((float)e.NewValue); this.Update(); };
            this.Slider4.ValueChanged += (s, e) =>
            {
                this.Location.BoundsPadding = new Bounds((float)e.NewValue);

                Canvas.SetLeft(this.BoundsPadding, this.Location.BoundsPadding.Left);
                Canvas.SetTop(this.BoundsPadding, this.Location.BoundsPadding.Top);

                this.BoundsPadding.Width = this.Canvas.ActualWidth - this.Location.BoundsPadding.Left - this.Location.BoundsPadding.Right;
                this.BoundsPadding.Height = this.Canvas.ActualHeight - this.Location.BoundsPadding.Top - this.Location.BoundsPadding.Bottom;

                this.Update();
            };

            this.XListView.ItemClick += (s, e) =>
            {
                switch (e.ClickedItem.ToString())
                {
                    case nameof(HorizontalAlignment.Left): this.Location.FixedHorizontalAlignment = FlyoutLocation.Left; this.Update(); break;
                    case nameof(HorizontalAlignment.Center): this.Location.FixedHorizontalAlignment = FlyoutLocation.Center; this.Update(); break;
                    case nameof(HorizontalAlignment.Right): this.Location.FixedHorizontalAlignment = FlyoutLocation.Right; this.Update(); break;
                    case nameof(HorizontalAlignment.Stretch): this.Location.FixedHorizontalAlignment = FlyoutLocation.Stretch; this.Update(); break;
                    default: break;
                }
            };
            this.YListView.ItemClick += (s, e) =>
            {
                switch (e.ClickedItem.ToString())
                {
                    case nameof(VerticalAlignment.Top): this.Location.FixedVerticalAlignment = FlyoutLocation.Top; this.Update(); break;
                    case nameof(VerticalAlignment.Center): this.Location.FixedVerticalAlignment = FlyoutLocation.Center; this.Update(); break;
                    case nameof(VerticalAlignment.Bottom): this.Location.FixedVerticalAlignment = FlyoutLocation.Bottom; this.Update(); break;
                    case nameof(VerticalAlignment.Stretch): this.Location.FixedVerticalAlignment = FlyoutLocation.Stretch; this.Update(); break;
                    default: break;
                }
            };

            this.Canvas.SizeChanged += (s, e) =>
            {
                if (e.NewSize == Size.Empty) return;
                if (e.NewSize == e.PreviousSize) return;

                this.Location.Target = new Rectangle
                {
                    X = (float)Canvas.GetLeft(this.Target),
                    Y = (float)Canvas.GetTop(this.Target),
                    Width = (float)this.Target.ActualWidth,
                    Height = (float)this.Target.ActualHeight
                };
                this.Location.Bounds = new Rectangle
                {
                    X = 0,
                    Y = 0,
                    Width = (float)e.NewSize.Width,
                    Height = (float)e.NewSize.Height
                };

                Canvas.SetLeft(this.BoundsPadding, this.Location.BoundsPadding.Left);
                Canvas.SetTop(this.BoundsPadding, this.Location.BoundsPadding.Top);
                this.BoundsPadding.Width = e.NewSize.Width - this.Location.BoundsPadding.Left - this.Location.BoundsPadding.Right;
                this.BoundsPadding.Height = e.NewSize.Height - this.Location.BoundsPadding.Top - this.Location.BoundsPadding.Bottom;

                this.Update();
            };

            #region Pointer

            this.Target.ManipulationStarted += delegate
            {
                this.Location.Target = new Rectangle
                {
                    X = (float)Canvas.GetLeft(this.Target),
                    Y = (float)Canvas.GetTop(this.Target),
                    Width = (float)this.Target.ActualWidth,
                    Height = (float)this.Target.ActualHeight
                };
            };
            this.Target.ManipulationDelta += (s, e) =>
            {
                this.Location.Target.X += (float)e.Delta.Translation.X;
                this.Location.Target.Y += (float)e.Delta.Translation.Y;
                Canvas.SetLeft(this.Target, this.Location.Target.X);
                Canvas.SetTop(this.Target, this.Location.Target.Y);
                this.Update();
            };
            this.Target.ManipulationCompleted += delegate
            {
            };

            #endregion
        }

        private void Update()
        {
            this.Metrics = new FlyoutMetrics(this.Location);
            if (this.Metrics.IsFixed)
            {
                this.Tail.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.Tail.Visibility = Visibility.Visible;
                this.Tail.Points[0] = this.Metrics.TailLeft.ToPoint();
                this.Tail.Points[1] = this.Metrics.Arrow.ToPoint();
                this.Tail.Points[2] = this.Metrics.TailRight.ToPoint();
            }

            Canvas.SetLeft(this.Body, this.Metrics.Body.X);
            Canvas.SetTop(this.Body, this.Metrics.Body.Y);
            this.Body.Width = this.Metrics.Body.Width;
            this.Body.Height = this.Metrics.Body.Height;

            Canvas.SetLeft(this.content, this.Metrics.Content.X);
            Canvas.SetTop(this.content, this.Metrics.Content.Y);
            this.content.Width = this.Metrics.Content.Width;
            this.content.Height = this.Metrics.Content.Height;
        }
    }
}