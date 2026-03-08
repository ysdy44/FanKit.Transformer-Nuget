using FanKit.Transformer.Sample;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class TopBar1 : UserControl
    {
        public event SelectionChangedEventHandler ToolTypeChanged
        {
            remove => this.ToolListBox.SelectionChanged -= value;
            add => this.ToolListBox.SelectionChanged += value;
        }

        public ToolType1 ToolType => (ToolType1)this.ToolListBox.SelectedIndex;

        public TopBar1()
        {
            this.InitializeComponent();
        }
    }
}