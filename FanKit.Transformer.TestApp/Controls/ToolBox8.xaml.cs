using FanKit.Transformer.Controllers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class ToolBox8 : UserControl
    {
        public event RoutedEventHandler CloseClick
        {
            remove => this.CloseButton.Click -= value;
            add => this.CloseButton.Click += value;
        }
        public event RoutedEventHandler OpenClick
        {
            remove => this.OpenButton.Click -= value;
            add => this.OpenButton.Click += value;
        }

        public event RoutedEventHandler SharpClick
        {
            remove => this.SharpButton.Click -= value;
            add => this.SharpButton.Click += value;
        }
        public event RoutedEventHandler SmoothClick
        {
            remove => this.SmoothButton.Click -= value;
            add => this.SmoothButton.Click += value;
        }

        public bool CenteredScaling => this.CenterButton.IsOn;
        public bool KeepRatio => this.RatioButton.IsOn;

        public string Title
        {
            get => this.TitleTextBlock.Text;
            set => this.TitleTextBlock.Text = value;
        }

        public ToolBox8()
        {
            this.InitializeComponent();
        }
    }
}