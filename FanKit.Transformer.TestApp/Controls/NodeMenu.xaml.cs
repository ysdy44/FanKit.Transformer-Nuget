using FanKit.Transformer.Controllers;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class NodeMenu : UserControl
    {
        public bool Disconnected => this.DisconnectedButton.IsChecked.Value;

        public SelfControlPointMode Mode1 => this.NoneButton.IsChecked.Value ?
            SelfControlPointMode.None :
            this.AngleButton.IsChecked.Value ?
            SelfControlPointMode.Angle :
            SelfControlPointMode.Length;

        public EachControlPointLengthMode Mode2 => this.AsymmetricButton.IsChecked.Value ?
            EachControlPointLengthMode.Ratio :
            EachControlPointLengthMode.Equal;

        public NodeMenu()
        {
            this.InitializeComponent();
            this.LengthButton.Unchecked += delegate
            {
                this.AsymmetricButton.IsEnabled = true;
            };
            this.LengthButton.Checked += delegate
            {
                this.AsymmetricButton.IsEnabled = false;
            };
        }
    }
}