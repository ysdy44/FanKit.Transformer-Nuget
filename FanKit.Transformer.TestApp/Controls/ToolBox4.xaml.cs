using FanKit.Transformer.Controllers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class ToolBox4 : UserControl
    {
        public event RoutedEventHandler EndClick
        {
            remove => this.EndButton.Click -= value;
            add => this.EndButton.Click += value;
        }

        public string Title
        {
            get => this.TitleTextBlock.Text;
            set => this.TitleTextBlock.Text = value;
        }

        public ToolBox4()
        {
            this.InitializeComponent();
        }
    }
}