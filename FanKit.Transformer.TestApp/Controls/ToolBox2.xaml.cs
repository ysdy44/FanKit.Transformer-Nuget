using FanKit.Transformer.Controllers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class ToolBox2 : UserControl
    {
        public event RoutedEventHandler RemoveClick
        {
            remove => this.RemoveButton.Click -= value;
            add => this.RemoveButton.Click += value;
        }
        public event RoutedEventHandler DeselectAllClick
        {
            remove => this.DeselectAllButton.Click -= value;
            add => this.DeselectAllButton.Click += value;
        }
        public event RoutedEventHandler SelectAllClick
        {
            remove => this.SelectAllButton.Click -= value;
            add => this.SelectAllButton.Click += value;
        }
        public event RoutedEventHandler SelectNextClick
        {
            remove => this.SelectNextButton.Click -= value;
            add => this.SelectNextButton.Click += value;
        }

        public bool CenteredScaling => this.CenterButton.IsOn;
        public bool KeepRatio => this.RatioButton.IsOn;

        public string Title
        {
            get => this.TitleTextBlock.Text;
            set => this.TitleTextBlock.Text = value;
        }

        public ToolBox2()
        {
            this.InitializeComponent();
        }
    }
}