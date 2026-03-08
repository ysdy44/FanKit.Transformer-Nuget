using FanKit.Transformer.Operators;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class SettingsDialog3 : UserControl
    {
        // Delegate
        public event System.EventHandler<TouchMode> TouchModeChanged;

        public bool WheelScale => this.WheelScaleButton.IsChecked.Value;

        public bool RightMove => this.RightMoveButton.IsChecked.Value;
        public bool RightScale => this.RightScaleButton.IsChecked.Value;
        public bool RightRotate => this.RightRotateButton.IsChecked.Value;

        public bool DoubleScale => this.DoubleScaleButton.IsChecked.Value;
        public bool DoubleRotate => this.DoubleRotateButton.IsChecked.Value;

        public TouchMode TouchMode
        {
            get
            {
                switch (this.TouchModeComboBox.SelectedIndex)
                {
                    case 0: return TouchMode.Disable;
                    case 1: return TouchMode.SingleFinger;
                    case 2: return TouchMode.RightButton;
                    default: return default;
                }
            }
            set
            {
                this.TouchModeComboBox.SelectedIndex = (int)value;
            }
        }

        public SettingsDialog3()
        {
            this.InitializeComponent();
            this.TouchModeComboBox.SelectionChanged += delegate
            {
                this.TouchModeChanged?.Invoke(this, this.TouchMode);
            };
        }
    }
}