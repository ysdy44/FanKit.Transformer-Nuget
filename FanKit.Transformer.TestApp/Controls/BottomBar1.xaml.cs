using FanKit.Transformer.Sample;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class BottomBar1 : UserControl
    {
        public event RoutedEventHandler HideToggled
        {
            remove => this.HideButton.Toggled -= value;
            add => this.HideButton.Toggled += value;
        }
        public event RoutedEventHandler EventsUnchecked
        {
            remove => this.EventsButton.Unchecked -= value;
            add => this.EventsButton.Unchecked += value;
        }
        public event RoutedEventHandler EventsChecked
        {
            remove => this.EventsButton.Checked -= value;
            add => this.EventsButton.Checked += value;
        }

        public bool IsHidden => this.HideButton.IsOn;

        public BottomBar1()
        {
            this.InitializeComponent();
        }
    }
}