using FanKit.Transformer.Controllers;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class ToolBox1 : UserControl
    {
        public bool CenteredScaling => this.CenterButton.IsOn;
        public bool KeepRatio => this.RatioButton.IsOn;

        public string Title
        {
            get => this.TitleTextBlock.Text;
            set => this.TitleTextBlock.Text = value;
        }

        public ToolBox1()
        {
            this.InitializeComponent();
        }
    }
}