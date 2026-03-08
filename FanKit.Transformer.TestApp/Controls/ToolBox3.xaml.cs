using FanKit.Transformer.Controllers;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class ToolBox3 : UserControl
    {
        public bool KeepConvex => this.ConvexButton.IsOn;

        public string Title
        {
            get => this.TitleTextBlock.Text;
            set => this.TitleTextBlock.Text = value;
        }

        public ToolBox3()
        {
            this.InitializeComponent();
        }
    }
}