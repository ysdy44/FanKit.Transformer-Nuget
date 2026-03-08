using FanKit.Transformer.UI;
using System;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class FlyoutPage : Page
    {
        public event EventHandler<double> OK;

        private bool IsInit; // Shown once
        private bool HasValue;
        private double Value;

        FlyoutPathData Data;
        FlyoutMetrics Metrics;
        FlyoutLocation Location = new FlyoutLocation
        {
            Priority = FlyoutPlacementPriority.BottomRightTopLeft,
            FixedHorizontalAlignment = FlyoutLocation.Center,
            FixedVerticalAlignment = FlyoutLocation.Center,

            BoundsPadding = new Bounds(12),
            ContentMargin = new Bounds(12),
            CornerRadius = 10f,

            ArrowHeight = 14f,
            ArrowWidth = 30f,

            ContentWidth = 200f,
            ContentHeight = 200f,
        };

        //public InlineCollection Inlines => this.TextBlock.Inlines;

        public FlyoutPlacementPriority Priority
        {
            get => this.Location.Priority;
            set => this.Location.Priority = value;
        }

        public FlyoutPage()
        {
            this.InitializeComponent();

            // Case 1: Single UI
            // If the TextBlock in ContentPresenter
            // Measure TextBlock UI, overwisz, width and height is zero.
            //this.Loaded += delegate
            //{
            //    this.TextBlock.Measure(Size.Empty);
            //
            //    this.Location.ContentWidth = this.TextBlock.ActualWidth;
            //    this.Location.ContentHeight = this.TextBlock.ActualHeight;
            //};

            // Case 2: Muti UI
            // If there some UI in ContentPresenter
            // Use Opacity = "0" instead of Visibility = "Collapsed" on ContentPresenter
            //this.ContentPresenter.SizeChanged += (s, e) =>
            //{
            //    if (e.NewSize == Size.Empty) return;
            //    if (e.NewSize == e.PreviousSize) return;
            //
            //    this.Location.ContentWidth = e.NewSize.Width;
            //    this.Location.ContentHeight = e.NewSize.Height;
            //
            //    this.ContentPresenter.Visibility = Visibility.Collapsed;
            //    this.ContentPresenter.Opacity = 1f;
            //};

            this.Canvas.SizeChanged += (s, e) =>
            {
                if (e.NewSize == Size.Empty) return;
                if (e.NewSize == e.PreviousSize) return;

                // Hide Flyout
                //this.HIde();

                this.DismissRectangle.Width = e.NewSize.Width;
                this.DismissRectangle.Height = e.NewSize.Height;
            };

            this.FixedButton.Click += delegate { this.Location.Priority = FlyoutPlacementPriority.Fixed; this.Show(this.BottomButton); };

            this.TopButton.Click += delegate { this.Location.Priority = FlyoutPlacementPriority.TopBottomLeftRight; this.Show(this.TopButton); };
            this.BottomButton.Click += delegate { this.Location.Priority = FlyoutPlacementPriority.BottomLeftRightTop; this.Show(this.BottomButton); };

            this.LeftTopButton.Click += delegate { this.Location.Priority = FlyoutPlacementPriority.TopBottomLeftRight; this.Show(this.LeftTopButton); };
            this.LeftBottomButton.Click += delegate { this.Location.Priority = FlyoutPlacementPriority.BottomLeftRightTop; this.Show(this.LeftBottomButton); };

            this.TopLeftButton.Click += delegate { this.Location.Priority = FlyoutPlacementPriority.LeftBottomRightTop; this.Show(this.TopLeftButton); };
            this.TopRightButton.Click += delegate { this.Location.Priority = FlyoutPlacementPriority.RightBottomLeftTop; this.Show(this.TopRightButton); };

            this.RightTopButton.Click += delegate { this.Location.Priority = FlyoutPlacementPriority.TopBottomLeftRight; this.Show(this.RightTopButton); };
            this.RightBottomButton.Click += delegate { this.Location.Priority = FlyoutPlacementPriority.BottomLeftRightTop; this.Show(this.RightBottomButton); };

            this.BottomLeftButton.Click += delegate { this.Location.Priority = FlyoutPlacementPriority.LeftBottomRightTop; this.Show(this.BottomLeftButton); };
            this.BottomRightButton.Click += delegate { this.Location.Priority = FlyoutPlacementPriority.RightBottomLeftTop; this.Show(this.BottomRightButton); };

            this.DismissRectangle.Tapped += (s, e) =>
            {
                Point point = e.GetPosition(this.Canvas);

                if (this.Metrics.Body.Contains(point.ToVector2()))
                    return;

                this.HIde();
            };

            #region Result

            this.ButtonOK.Click += delegate
            {
                this.HasValue = double.TryParse(this.TargetTextBlock.Text, out this.Value);

                if (this.HasValue)
                    this.OK?.Invoke(this, this.Value);

                this.HIde();
            };
            this.ButtonCancel.Click += delegate
            {
                this.HIde();
            };

            #endregion

            #region Zero

            this.ButtonClear.Click += delegate
            {
                this.TargetTextBlock.Text = "0";
            };
            this.ButtonDelete.Click += delegate
            {
                int length = this.TargetTextBlock.Text.Length;

                switch (length)
                {
                    case 0:
                    case 1:
                        this.TargetTextBlock.Text = "0";
                        break;
                    case 2:
                        this.TargetTextBlock.Text = this.TargetTextBlock.Text.First().ToString();
                        break;
                    default:
                        this.TargetTextBlock.Text = this.TargetTextBlock.Text.Remove(length - 1);
                        break;
                }
            };
            this.ButtonDot.Click += delegate
            {
                foreach (char item in this.TargetTextBlock.Text)
                {
                    switch (item)
                    {
                        case '.':
                            return;
                        default:
                            break;
                    }
                }

                this.TargetTextBlock.Text += ".";
            };
            this.Button0.Click += delegate
            {
                if (this.IsInit)
                {
                    this.IsInit = false;
                    this.TargetTextBlock.Text = "0";
                    return;
                }

                switch (this.TargetTextBlock.Text)
                {
                    case "":
                    case "0":
                        break;
                    default:
                        this.TargetTextBlock.Text += "0";
                        break;
                }
            };

            #endregion

            #region Num123

            this.Button1.Click += delegate
            {
                if (this.IsInit)
                {
                    this.IsInit = false;
                    this.TargetTextBlock.Text = "1";
                    return;
                }

                switch (this.TargetTextBlock.Text)
                {
                    case "":
                    case "0":
                        this.TargetTextBlock.Text = "1";
                        break;
                    default:
                        this.TargetTextBlock.Text += "1";
                        break;
                }
            };
            this.Button2.Click += delegate
            {
                if (this.IsInit)
                {
                    this.IsInit = false;
                    this.TargetTextBlock.Text = "2";
                    return;
                }

                switch (this.TargetTextBlock.Text)
                {
                    case "":
                    case "0":
                        this.TargetTextBlock.Text = "2";
                        break;
                    default:
                        this.TargetTextBlock.Text += "2";
                        break;
                }
            };
            this.Button3.Click += delegate
            {
                if (this.IsInit)
                {
                    this.IsInit = false;
                    this.TargetTextBlock.Text = "3";
                    return;
                }

                switch (this.TargetTextBlock.Text)
                {
                    case "":
                    case "0":
                        this.TargetTextBlock.Text = "3";
                        break;
                    default:
                        this.TargetTextBlock.Text += "3";
                        break;
                }
            };

            #endregion

            #region Num456

            this.Button4.Click += delegate
            {
                if (this.IsInit)
                {
                    this.IsInit = false;
                    this.TargetTextBlock.Text = "4";
                    return;
                }

                switch (this.TargetTextBlock.Text)
                {
                    case "":
                    case "0":
                        this.TargetTextBlock.Text = "4";
                        break;
                    default:
                        this.TargetTextBlock.Text += "4";
                        break;
                }
            };
            this.Button5.Click += delegate
            {
                if (this.IsInit)
                {
                    this.IsInit = false;
                    this.TargetTextBlock.Text = "5";
                    return;
                }

                switch (this.TargetTextBlock.Text)
                {
                    case "":
                    case "0":
                        this.TargetTextBlock.Text = "5";
                        break;
                    default:
                        this.TargetTextBlock.Text += "5";
                        break;
                }
            };
            this.Button6.Click += delegate
            {
                if (this.IsInit)
                {
                    this.IsInit = false;
                    this.TargetTextBlock.Text = "6";
                    return;
                }

                switch (this.TargetTextBlock.Text)
                {
                    case "":
                    case "0":
                        this.TargetTextBlock.Text = "6";
                        break;
                    default:
                        this.TargetTextBlock.Text += "6";
                        break;
                }
            };

            #endregion

            #region Num789

            this.Button7.Click += delegate
            {
                if (this.IsInit)
                {
                    this.IsInit = false;
                    this.TargetTextBlock.Text = "7";
                    return;
                }

                switch (this.TargetTextBlock.Text)
                {
                    case "":
                    case "0":
                        this.TargetTextBlock.Text = "7";
                        break;
                    default:
                        this.TargetTextBlock.Text += "7";
                        break;
                }
            };
            this.Button8.Click += delegate
            {
                if (this.IsInit)
                {
                    this.IsInit = false;
                    this.TargetTextBlock.Text = "8";
                    return;
                }

                switch (this.TargetTextBlock.Text)
                {
                    case "":
                    case "0":
                        this.TargetTextBlock.Text = "8";
                        break;
                    default:
                        this.TargetTextBlock.Text += "8";
                        break;
                }
            };
            this.Button9.Click += delegate
            {
                if (this.IsInit)
                {
                    this.IsInit = false;
                    this.TargetTextBlock.Text = "9";
                    return;
                }

                switch (this.TargetTextBlock.Text)
                {
                    case "":
                    case "0":
                        this.TargetTextBlock.Text = "9";
                        break;
                    default:
                        this.TargetTextBlock.Text += "9";
                        break;
                }
            };

            #endregion
        }

        public void HIde()
        {
            this.ContentPresenter.Visibility = Visibility.Collapsed;
            this.Path.Visibility = Visibility.Collapsed;
            this.TargetBorder.Visibility = Visibility.Collapsed;
            this.DismissRectangle.Visibility = Visibility.Collapsed;
        }

        public void Show(Button button) => this.Show(button, $"{button.Content}");

        public void Show(FrameworkElement placementTarget, string text)
        {
            Point p = placementTarget.TransformToVisual(this.Canvas).TransformPoint(default);

            this.Location.Target = new Rectangle
            {
                X = (float)p.X,
                Y = (float)p.Y,
                Width = (float)placementTarget.ActualWidth,
                Height = (float)placementTarget.ActualHeight
            };
            this.Location.Bounds = new Rectangle
            {
                X = 0,
                Y = 0,
                Width = (float)this.Canvas.ActualWidth,
                Height = (float)this.Canvas.ActualHeight
            };

            this.Metrics = new FlyoutMetrics(this.Location);
            this.Data = new FlyoutPathData(this.Location, this.Metrics);

            Canvas.SetLeft(this.ContentPresenter, this.Metrics.Content.X);
            Canvas.SetTop(this.ContentPresenter, this.Metrics.Content.Y);
            this.ContentPresenter.Width = this.Metrics.Content.Width;
            this.ContentPresenter.Height = this.Metrics.Content.Height;

            Canvas.SetLeft(this.TargetBorder, this.Location.Target.X);
            Canvas.SetTop(this.TargetBorder, this.Location.Target.Y);
            this.TargetBorder.Width = this.Location.Target.Width;
            this.TargetBorder.Height = this.Location.Target.Height;

            this.PathFigure.StartPoint = this.Data.StartPoint.ToPoint();

            this.Line0.Point = this.Data.Line0Point.ToPoint();
            this.Line1.Point = this.Data.Line1Point.ToPoint();
            this.Line2.Point = this.Data.Line2Point.ToPoint();
            this.Line3.Point = this.Data.Line3Point.ToPoint();
            this.Line4.Point = this.Data.Line4Point.ToPoint();

            this.Arc3.Size = this.Data.ArcSize.ToSize();
            this.Arc2.Size = this.Data.ArcSize.ToSize();
            this.Arc1.Size = this.Data.ArcSize.ToSize();
            this.Arc0.Size = this.Data.ArcSize.ToSize();

            this.Arc0.Point = this.Data.Arc0Point.ToPoint();
            this.Arc1.Point = this.Data.Arc1Point.ToPoint();
            this.Arc2.Point = this.Data.Arc2Point.ToPoint();
            this.Arc3.Point = this.Data.Arc3Point.ToPoint();

            this.Bezier0.Point1 = this.Data.Bezier0Point1.ToPoint();
            this.Bezier0.Point2 = this.Data.Bezier0Point2.ToPoint();
            this.Bezier0.Point3 = this.Data.Bezier0Point3.ToPoint();

            this.Bezier1.Point1 = this.Data.Bezier1Point1.ToPoint();
            this.Bezier1.Point2 = this.Data.Bezier1Point2.ToPoint();
            this.Bezier1.Point3 = this.Data.Bezier1Point3.ToPoint();

            this.TargetTextBlock.Text = text;
            this.IsInit = true;

            this.DismissRectangle.Visibility = Visibility.Visible;
            this.TargetBorder.Visibility = Visibility.Visible;
            this.Path.Visibility = Visibility.Visible;
            this.ContentPresenter.Visibility = Visibility.Visible;
        }
    }
}