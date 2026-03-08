using FanKit.Transformer.Controllers;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class ToolBox5 : UserControl
    {
        public bool Disconnected => this.NodeMenu.Disconnected;
        public SelfControlPointMode Mode1 => this.NodeMenu.Mode1;
        public EachControlPointLengthMode Mode2 => this.NodeMenu.Mode2;

        public string Title
        {
            get => this.TitleTextBlock.Text;
            set => this.TitleTextBlock.Text = value;
        }

        public ToolBox5()
        {
            this.InitializeComponent();
        }
    }
}