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

        public ToolType0 ToolType
        {
            get
            {
                switch (this.ToolListBox.SelectedIndex)
                {
                    case 0: return ToolType0.Transform;
                    case 1: return ToolType0.CreateNew;
                    default: return ToolType0.Transform;
                }
            }
        }

        public TopBar0()
        {
            this.InitializeComponent();
        }
    }
}