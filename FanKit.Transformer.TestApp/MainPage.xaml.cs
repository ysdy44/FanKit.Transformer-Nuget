using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace FanKit.Transformer.TestApp
{
    internal sealed class Kvp
    {
        public readonly string Key;
        public readonly string Text;
        public readonly Type PageType;

        public Kvp(string key, string text, Type value)
        {
            this.Key = key;
            this.Text = text;
            this.PageType = value;
        }
    }

    public sealed partial class MainPage : Page, ICommand
    {
        //@Instance
        private readonly Lazy<SystemNavigationManager> ManagerLazy = new Lazy<SystemNavigationManager>(() => SystemNavigationManager.GetForCurrentView());
        private readonly Lazy<ApplicationView> ViewLazy = new Lazy<ApplicationView>(() => ApplicationView.GetForCurrentView());
        private SystemNavigationManager Manager => this.ManagerLazy.Value;
        private ApplicationView View => this.ViewLazy.Value;

        readonly IDictionary<string, Kvp> DictionaryKey = (
            from item in Tree
            from kvp in item.Value
            where kvp != null
            select kvp).ToDictionary(c => c.Key);
        readonly IDictionary<string, Kvp> DictionaryText = (
            from item in Tree
            from kvp in item.Value
            where kvp != null
            select kvp).ToDictionary(c => c.Text);

        public MainPage()
        {
            this.InitializeComponent();
            foreach (KeyValuePair<string, Kvp[]> item in Tree)
            {
                MenuFlyoutSubItem subItem = new MenuFlyoutSubItem
                {
                    Text = item.Key,
                };

                foreach (Kvp type in item.Value)
                {
                    if (type == null)
                        subItem.Items.Add(new MenuFlyoutSeparator());
                    else
                        subItem.Items.Add(new MenuFlyoutItem
                        {
                            Text = type.Text,
                            CommandParameter = type.Key,
                            Command = this,
                        });
                }

                this.MenuFlyout.Items.Add(subItem);
            }

            // Register a handler for BackRequested events
            base.Unloaded += delegate { this.Manager.BackRequested -= this.OnBackRequested; };
            base.Loaded += delegate
            {
                this.Manager.BackRequested += this.OnBackRequested;
                this.AutoSuggestBox.Focus(FocusState.Keyboard);
            };

            this.Hyperlink0.Inlines.Add(new Run { Text = nameof(Transform0Page) });
            this.Hyperlink0.Click += delegate { this.NavigateKey("Transform0"); };

            this.Hyperlink1.Inlines.Add(new Run { Text = nameof(Transforms0Page) });
            this.Hyperlink1.Click += delegate { this.NavigateKey("Transforms0"); };

            this.Hyperlink2.Inlines.Add(new Run { Text = nameof(FreeTransformPage) });
            this.Hyperlink2.Click += delegate { this.NavigateKey("FreeTransform"); };

            this.Hyperlink3.Inlines.Add(new Run { Text = nameof(BoundsPage) });
            this.Hyperlink3.Click += delegate { this.NavigateKey("Bounds"); };

            this.Hyperlink4.Inlines.Add(new Run { Text = nameof(MoveCurvePage) });
            this.Hyperlink4.Click += delegate { this.NavigateKey("MoveCurve"); };

            this.ListView.ItemsSource = this.Overlay.Children.Select(c => ((FrameworkElement)c).Tag).ToArray();
            this.ListView.SelectionChanged += delegate
            {
                int index = this.ListView.SelectedIndex;

                for (int i = 0; i < this.Overlay.Children.Count; i++)
                {
                    UIElement item = this.Overlay.Children[i];

                    item.Visibility = index == i ? Visibility.Visible : Visibility.Collapsed;
                }
                this.Overlay.Visibility = index < 0 ? Visibility.Collapsed : Visibility.Visible;
            };

            this.AutoSuggestBox.SuggestionChosen += (s, e) => this.NavigateText($"{e.SelectedItem}");
            this.AutoSuggestBox.TextChanged += (sender, args) =>
            {
                switch (args.Reason)
                {
                    case AutoSuggestionBoxTextChangeReason.ProgrammaticChange:
                    case AutoSuggestionBoxTextChangeReason.SuggestionChosen:
                        break;
                    default:
                        if (string.IsNullOrEmpty(sender.Text))
                        {
                            sender.ItemsSource = null;
                            this.View.Title = "Pages";
                        }
                        else
                        {
                            string text = sender.Text.ToLower();
                            IEnumerable<string> suitableItems = this.DictionaryText.Keys.Where(x => x.ToLower().Contains(text));

                            int count = suitableItems.Count();
                            if (count is 0)
                            {
                                sender.ItemsSource = null;
                                this.View.Title = "No results found";
                            }
                            else
                            {
                                sender.ItemsSource = suitableItems;
                                this.View.Title = $"{count} results";
                            }
                        }
                        break;
                }
            };
        }

        // Command
        public ICommand Command => this;
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => this.NavigateKey($"{parameter}");

        private void NavigateKey(string key)
        {
            if (this.DictionaryKey.ContainsKey(key))
            {
                Kvp item = this.DictionaryKey[key];

                this.ListView.SelectedIndex = -1;
                this.ContentFrame.Navigate(item.PageType);

                this.Manager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                this.View.Title = item.Text;
            }
        }
        private void NavigateText(string text)
        {
            if (this.DictionaryText.ContainsKey(text))
            {
                Kvp item = this.DictionaryText[text];

                this.ListView.SelectedIndex = -1;
                this.ContentFrame.Navigate(item.PageType);

                this.Manager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                this.View.Title = item.Text;
            }
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;

            if (this.ContentFrame.CanGoBack)
            {
                this.ListView.SelectedIndex = -1;
                this.ContentFrame.GoBack();

                this.Manager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                this.View.Title = this.ContentFrame.Content.GetType().Name;
            }
            else
            {
                this.ListView.SelectedIndex = -1;
                this.ContentFrame.Content = this.ContentPage;

                this.Manager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                this.View.Title = string.Empty;
            }
        }

        static readonly IDictionary<string, Kvp[]> Tree = new Dictionary<string, Kvp[]>
        {
            ["Map"] = new Kvp[]
            {
                new Kvp("Crop", "Crop", typeof(CropPage)),
                default,
                new Kvp("CanvasCrop0", "Canvas Crop 0", typeof(CanvasCrop0Page)),
                new Kvp("CanvasCrop1", "Canvas Crop 1", typeof(CanvasCrop1Page)),
                new Kvp("CanvasCrop2", "Canvas Crop 2", typeof(CanvasCrop2Page)),
                new Kvp("CanvasCrop3", "Canvas Crop 3", typeof(CanvasCrop3Page)),
                default,
                new Kvp("Crops", "Crops", typeof(CropsPage)),
                default,
                new Kvp("CanvasCrops0", "Canvas Crops 0", typeof(CanvasCrops0Page)),
                new Kvp("CanvasCrops1", "Canvas Crops 1", typeof(CanvasCrops1Page)),
                new Kvp("CanvasCrops2", "Canvas Crops 2", typeof(CanvasCrops2Page)),
                new Kvp("CanvasCrops3", "Canvas Crops 3", typeof(CanvasCrops3Page)),
            },
            ["Affine"] = new Kvp[]
            {
                new Kvp("Transform0", "Transform 0", typeof(Transform0Page)),
                new Kvp("Transform1", "Transform 1", typeof(Transform1Page)),
                new Kvp("Transform2", "Transform 2", typeof(Transform2Page)),
                new Kvp("Transform3", "Transform 3", typeof(Transform3Page)),
                default,
                new Kvp("CanvasTransform0", "Canvas Transform 0", typeof(CanvasTransform0Page)),
                new Kvp("CanvasTransform1", "Canvas Transform 1", typeof(CanvasTransform1Page)),
                new Kvp("CanvasTransform2", "Canvas Transform 2", typeof(CanvasTransform2Page)),
                new Kvp("CanvasTransform3", "Canvas Transform 3", typeof(CanvasTransform3Page)),
                default,
                new Kvp("Transforms0", "Transforms 0", typeof(Transforms0Page)),
                new Kvp("Transforms1", "Transforms 1", typeof(Transforms1Page)),
                new Kvp("Transforms2", "Transforms 2", typeof(Transforms2Page)),
                new Kvp("Transforms3", "Transforms 3", typeof(Transforms3Page)),
                default,
                new Kvp("CanvasTransforms0", "Canvas Transforms 0", typeof(CanvasTransforms0Page)),
                new Kvp("CanvasTransforms1", "Canvas Transforms 1", typeof(CanvasTransforms1Page)),
                new Kvp("CanvasTransforms2", "Canvas Transforms 2", typeof(CanvasTransforms2Page)),
                new Kvp("CanvasTransforms3", "Canvas Transforms 3", typeof(CanvasTransforms3Page)),
            },
            ["Perspective"] = new Kvp[]
            {
                new Kvp("FreeTransform", "Free Transform", typeof(FreeTransformPage)),
                default,
                new Kvp("CanvasFreeTransform0", "Canvas Free Transform 0", typeof(CanvasFreeTransform0Page)),
                new Kvp("CanvasFreeTransform1", "Canvas Free Transform 1", typeof(CanvasFreeTransform1Page)),
                new Kvp("CanvasFreeTransform2", "Canvas Free Transform 2", typeof(CanvasFreeTransform2Page)),
                new Kvp("CanvasFreeTransform3", "Canvas Free Transform 3", typeof(CanvasFreeTransform3Page)),
                default,
                new Kvp("PerspectiveCrop", "Perspective Crop", typeof(PerspectiveCropPage)),
                default,
                new Kvp("CanvasPerspectiveCrop0", "Canvas Perspective Crop 0", typeof(CanvasPerspectiveCrop0Page)),
                new Kvp("CanvasPerspectiveCrop1", "Canvas Perspective Crop 1", typeof(CanvasPerspectiveCrop1Page)),
                new Kvp("CanvasPerspectiveCrop2", "Canvas Perspective Crop 2", typeof(CanvasPerspectiveCrop2Page)),
                new Kvp("CanvasPerspectiveCrop3", "Canvas Perspective Crop 3", typeof(CanvasPerspectiveCrop3Page)),
            },
            ["Lines"] = new Kvp[]
            {
                new Kvp("Line0", "Line 0", typeof(Line0Page)),
                new Kvp("Line1", "Line 1", typeof(Line1Page)),
                new Kvp("Line2", "Line 2", typeof(Line2Page)),
                new Kvp("Line3", "Line 3", typeof(Line3Page)),
                default,
                new Kvp("CanvasLine0", "Canvas Line 0", typeof(CanvasLine0Page)),
                new Kvp("CanvasLine1", "Canvas Line 1", typeof(CanvasLine1Page)),
                new Kvp("CanvasLine2", "Canvas Line 2", typeof(CanvasLine2Page)),
                new Kvp("CanvasLine3", "Canvas Line 3", typeof(CanvasLine3Page)),
            },
            ["Polylines"] = new Kvp[]
            {
                new Kvp("Polyline0", "Polyline 0", typeof(Polyline0Page)),
                new Kvp("Polyline1", "Polyline 1", typeof(Polyline1Page)),
                new Kvp("Polyline2", "Polyline 2", typeof(Polyline2Page)),
                new Kvp("Polyline3", "Polyline 3", typeof(Polyline3Page)),
                default,
                new Kvp("CanvasPolyline0", "Canvas Polyline 0", typeof(CanvasPolyline0Page)),
                new Kvp("CanvasPolyline1", "Canvas Polyline 1", typeof(CanvasPolyline1Page)),
                new Kvp("CanvasPolyline2", "Canvas Polyline 2", typeof(CanvasPolyline2Page)),
                new Kvp("CanvasPolyline3", "Canvas Polyline 3", typeof(CanvasPolyline3Page)),
                default,
                new Kvp("Polylines0", "Polylines 0", typeof(Polylines0Page)),
                new Kvp("Polylines1", "Polylines 1", typeof(Polylines1Page)),
                new Kvp("Polylines2", "Polylines 2", typeof(Polylines2Page)),
                new Kvp("Polylines3", "Polylines 3", typeof(Polylines3Page)),
                default,
                new Kvp("CanvasPolylines0", "Canvas Polylines 0", typeof(CanvasPolylines0Page)),
                new Kvp("CanvasPolylines1", "Canvas Polylines 1", typeof(CanvasPolylines1Page)),
                new Kvp("CanvasPolylines2", "Canvas Polylines 2", typeof(CanvasPolylines2Page)),
                new Kvp("CanvasPolylines3", "Canvas Polylines 3", typeof(CanvasPolylines3Page)),
            },
            ["Curves"] = new Kvp[]
            {
                new Kvp("Curve0", "Curve 0", typeof(Curve0Page)),
                new Kvp("Curve1", "Curve 1", typeof(Curve1Page)),
                new Kvp("Curve2", "Curve 2", typeof(Curve2Page)),
                new Kvp("Curve3", "Curve 3", typeof(Curve3Page)),
                default,
                new Kvp("CanvasCurve0", "Canvas Curve 0", typeof(CanvasCurve0Page)),
                new Kvp("CanvasCurve1", "Canvas Curve 1", typeof(CanvasCurve1Page)),
                new Kvp("CanvasCurve2", "Canvas Curve 2", typeof(CanvasCurve2Page)),
                new Kvp("CanvasCurve3", "Canvas Curve 3", typeof(CanvasCurve3Page)),
                default,
                new Kvp("Curves0", "Curves 0", typeof(Curves0Page)),
                new Kvp("Curves1", "Curves 1", typeof(Curves1Page)),
                new Kvp("Curves2", "Curves 2", typeof(Curves2Page)),
                new Kvp("Curves3", "Curves 3", typeof(Curves3Page)),
                default,
                new Kvp("CanvasCurves0", "Canvas Curves 0", typeof(CanvasCurves0Page)),
                new Kvp("CanvasCurves1", "Canvas Curves 1", typeof(CanvasCurves1Page)),
                new Kvp("CanvasCurves2", "Canvas Curves 2", typeof(CanvasCurves2Page)),
                new Kvp("CanvasCurves3", "Canvas Curves 3", typeof(CanvasCurves3Page)),
            },
            ["Canvas"] = new Kvp[]
            {
                new Kvp("CanvasSingle0", "Single Canvas 0", typeof(CanvasSingle0Page)),
                new Kvp("CanvasSingle1", "Single Canvas 1", typeof(CanvasSingle1Page)),
                new Kvp("CanvasSingle2", "Single Canvas 2", typeof(CanvasSingle2Page)),
                new Kvp("CanvasSingle3", "Single Canvas 3", typeof(CanvasSingle3Page)),
                default,
                new Kvp("CanvasDouble0", "Double Canvas 0", typeof(CanvasDouble0Page)),
                new Kvp("CanvasDouble1", "Double Canvas 1", typeof(CanvasDouble1Page)),
                new Kvp("CanvasDouble2", "Double Canvas 2", typeof(CanvasDouble2Page)),
                new Kvp("CanvasDouble3", "Double Canvas 3", typeof(CanvasDouble3Page)),
            },
            ["Homography"] = new Kvp[]
            {
                new Kvp("DirectAffine", "Affine", typeof(DirectAffinePage)),
                new Kvp("BidirectionalAffine", "Bidirectional Affine", typeof(BidirectionalAffinePage)),
                new Kvp("InverseAffine", "Inverse Affine", typeof(InverseAffinePage)),
                default,
                new Kvp("DirectPerspective", "Perspective", typeof(DirectPerspectivePage)),
                new Kvp("BidirectionalPerspective", "Bidirectional Perspective", typeof(BidirectionalPerspectivePage)),
                new Kvp("InversePerspective", "Inverse Perspective", typeof(InversePerspectivePage)),
            },
            ["Shapes"] = new Kvp[]
            {
                new Kvp("Rectangle", "Rectangle", typeof(RectanglePage)),
                new Kvp("Bounds", "Bounds", typeof(BoundsPage)),
                new Kvp("Triangle", "Triangle", typeof(TrianglePage)),
                new Kvp("Quadrilateral", "Quadrilateral", typeof(QuadrilateralPage)),
                default,
                new Kvp("BoundsBox0", "Bounds & Box 0", typeof(BoundsBox0Page)),
                new Kvp("BoundsBox1", "Bounds & Box 1", typeof(BoundsBox1Page)),
                default,
                new Kvp("TriangleBox0", "Triangle & Box 0", typeof(TriangleBox0Page)),
                new Kvp("TriangleBox1", "Triangle & Box 1", typeof(TriangleBox1Page)),
                new Kvp("TriangleBox2", "Triangle & Box 2", typeof(TriangleBox2Page)),
                new Kvp("TriangleBox3", "Triangle & Box 3", typeof(TriangleBox3Page)),
                default,
                new Kvp("LineLine0", "Line & Line 0", typeof(LineLine0Page)),
                new Kvp("LineLine1", "Line & Line 1", typeof(LineLine1Page)),
                new Kvp("LineLine2", "Line & Line 2", typeof(LineLine2Page)),
                new Kvp("LineLine3", "Line & Line 3", typeof(LineLine3Page)),
            },
            ["Controls"] = new Kvp[]
            {
                new Kvp("Convert", "Convert", typeof(ConvertPage)),
                new Kvp("Box", "Box", typeof(BoxPage)),
                default,
                new Kvp("CombineBounds", "Combine Bounds", typeof(CombineBoundsPage)),
                new Kvp("ContainsBounds", "Contains Bounds", typeof(ContainsBoundsPage)),
                new Kvp("CombinePath", "Combine Path", typeof(CombinePathPage)),
                default,
                new Kvp("Carousel", "Carousel", typeof(CarouselPage)),
                new Kvp("Carousels", "Carousels", typeof(CarouselsPage)),
                new Kvp("Scroller", "Scroller", typeof(ScrollerPage)),
                default,
                new Kvp("Flyout", "Flyout", typeof(FlyoutPage)),
                new Kvp("FlyoutPlacement", "Flyout Placement", typeof(FlyoutPlacementPage)),
            },
            ["Beziers"] = new Kvp[]
            {
                new Kvp("MovePolyline", "Move Polyline", typeof(MovePolylinePage)),
                new Kvp("MoveCurve", "Move Curve", typeof(MoveCurvePage)),
                default,
                new Kvp("InsertPolyline", "Insert Polyline", typeof(InsertPolylinePage)),
                new Kvp("InsertCurve", "Insert Curve", typeof(InsertCurvePage)),
                default,
                new Kvp("ClosestCubicBezier", "Closest Cubic Bezier", typeof(ClosestCubicBezierPage)),
                new Kvp("ClosestQuadraticBezier", "Closest Quadratic Bezier", typeof(ClosestQuadraticBezierPage)),
                new Kvp("ClosestLinearBezier", "Closest Linear Bezier", typeof(ClosestLinearBezierPage)),
                new Kvp("ClosestAllBezier", "Closest All Bezier", typeof(ClosestAllBezierPage)),
                default,
                new Kvp("SplitCubicBezier", "Split Cubic Bezier", typeof(SplitCubicBezierPage)),
                new Kvp("SplitQuadraticBezier", "Split Quadratic Bezier", typeof(SplitQuadraticBezierPage)),
                new Kvp("SplitLinearBezier", "Split Linear Bezier", typeof(SplitLinearBezierPage)),
                new Kvp("SplitAllBezier", "Split All Bezier", typeof(SplitAllBezierPage)),
            },
        };
    }
}