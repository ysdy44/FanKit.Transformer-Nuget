using FanKit.Transformer.Sample;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class TopBar0 : UserControl
    {
        public event SelectionChangedEventHandler ToolTypeChanged
        {
            remove => this.ToolListBox.SelectionChanged -= value;
            add => this.ToolListBox.SelectionChanged += value;
        }

        public ToolType0 ToolType => (ToolType0)this.ToolListBox.SelectedIndex;

        public TopBar0()
        {
            this.InitializeComponent();
        }
    }
}