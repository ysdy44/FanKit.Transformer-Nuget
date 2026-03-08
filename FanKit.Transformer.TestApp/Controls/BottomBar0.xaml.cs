using FanKit.Transformer.Sample;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class BottomBar0 : UserControl
    {
        public event RoutedEventHandler HideToggled
        {
            remove => this.HideButton.Toggled -= value;
            add => this.HideButton.Toggled += value;
        }

        public bool IsHidden => this.HideButton.IsOn;

        public BottomBar0()
        {
            this.InitializeComponent();
        }
    }
}