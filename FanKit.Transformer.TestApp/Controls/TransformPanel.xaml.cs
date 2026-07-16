using FanKit.Transformer.Indicators;
using FanKit.Transformer.Sample;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static FanKit.Transformer.Indicators.ParameterAvailability;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class TransformPanel : UserControl
    {
        public EventHandler<PanelAnchorMode> PanelAnchorModeChanged;
        public EventHandler<RowLineAnchorMode> RowLineAnchorModeChanged;
        public EventHandler<ColumnLineAnchorMode> ColumnLineAnchorModeChanged;
        public EventHandler<ParameterKind> Apply;

        public PanelAnchorMode PanelAnchorMode;
        public RowLineAnchorMode RowLineAnchorMode;
        public ColumnLineAnchorMode ColumnLineAnchorMode;
        public float Value;

        IndicatorSizeType OldValue;
        IndicatorSizeType NewValue;
        ParameterAvailability Availability = Unavailable;

        public TransformPanel()
        {
            this.InitializeComponent();
            this.ComboBox.ItemsSource = new string[]
            {
                "Left-Top",
                "Right-Top",
                "Left-Bottom",
                "Right-Bottom",

                "Center-Left",
                "Center-Top",
                "Center-Right",
                "Center-Bottom",

                "Center",
            };
            this.RowComboBox.ItemsSource = new string[]
            {
                "Left",
                "Center",
                "Right",
            };
            this.ColumnComboBox.ItemsSource = new string[]
            {
                "Top",
                "Center",
                "Bottom",
            };

            this.ComboBox.SelectedIndex = (byte)this.PanelAnchorMode;
            this.RowComboBox.SelectedIndex = (int)(byte)this.RowLineAnchorMode;
            this.ColumnComboBox.SelectedIndex = (int)(byte)this.ColumnLineAnchorMode;

            this.ComboBox.SelectionChanged += delegate
            {
                this.PanelAnchorMode = (PanelAnchorMode)(byte)this.ComboBox.SelectedIndex;
                this.PanelAnchorModeChanged?.Invoke(this, this.PanelAnchorMode);
            };
            this.RowComboBox.SelectionChanged += delegate
            {
                this.RowLineAnchorMode = (RowLineAnchorMode)(byte)this.RowComboBox.SelectedIndex;
                this.RowLineAnchorModeChanged?.Invoke(this, this.RowLineAnchorMode);
            };
            this.ColumnComboBox.SelectionChanged += delegate
            {
                this.ColumnLineAnchorMode = (ColumnLineAnchorMode)(byte)this.ColumnComboBox.SelectedIndex;
                this.ColumnLineAnchorModeChanged?.Invoke(this, this.ColumnLineAnchorMode);
            };

            this.XButton.Click += delegate
            {
                if (float.TryParse(XTextBox.Text, out this.Value))
                    this.Apply?.Invoke(this, ParameterKind.X);
            };
            this.YButton.Click += delegate
            {
                if (float.TryParse(YTextBox.Text, out this.Value))
                    this.Apply?.Invoke(this, ParameterKind.Y);
            };
            this.WButton.Click += delegate
            {
                if (float.TryParse(WTextBox.Text, out this.Value))
                    this.Apply?.Invoke(this, ParameterKind.Width);
            };
            this.HButton.Click += delegate
            {
                if (float.TryParse(HTextBox.Text, out this.Value))
                    this.Apply?.Invoke(this, ParameterKind.Height);
            };
            this.RButton.Click += delegate
            {
                if (float.TryParse(RTextBox.Text, out this.Value))
                    this.Apply?.Invoke(this, ParameterKind.Rotation);
            };
            this.SButton.Click += delegate
            {
                if (float.TryParse(STextBox.Text, out this.Value))
                    this.Apply?.Invoke(this, ParameterKind.Skew);
            };
        }

        private void UpdateComboBox()
        {
            switch (this.NewValue)
            {
                case IndicatorSizeType.Empty:
                case IndicatorSizeType.Point:
                    this.RowComboBox.Visibility = Visibility.Collapsed;
                    this.ColumnComboBox.Visibility = Visibility.Collapsed;
                    this.ComboBox.Visibility = Visibility.Collapsed;
                    break;
                case IndicatorSizeType.RowLine:
                    this.RowComboBox.Visibility = Visibility.Visible;
                    this.ColumnComboBox.Visibility = Visibility.Collapsed;
                    this.ComboBox.Visibility = Visibility.Collapsed;
                    break;
                case IndicatorSizeType.ColumnLine:
                    this.RowComboBox.Visibility = Visibility.Collapsed;
                    this.ColumnComboBox.Visibility = Visibility.Visible;
                    this.ComboBox.Visibility = Visibility.Collapsed;
                    break;
                case IndicatorSizeType.Cropper:
                case IndicatorSizeType.Transformer:
                    this.RowComboBox.Visibility = Visibility.Collapsed;
                    this.ColumnComboBox.Visibility = Visibility.Collapsed;
                    this.ComboBox.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        public void UpdateAll(IIndicator indicator)
        {
            IndicatorSizeType type = indicator.SizeType;

            this.OldValue = this.NewValue;
            this.NewValue = type;

            this.Availability = Indicator.ToAvailability(this.NewValue);

            this.UpdateComboBox();

            this.XTextBox.IsEnabled = this.XButton.IsEnabled = this.Availability.HasFlag(XValue);
            this.YTextBox.IsEnabled = this.YButton.IsEnabled = this.Availability.HasFlag(YValue);
            this.WTextBox.IsEnabled = this.WButton.IsEnabled = this.Availability.HasFlag(WidthValue);
            this.HTextBox.IsEnabled = this.HButton.IsEnabled = this.Availability.HasFlag(HeightValue);
            this.RTextBox.IsEnabled = this.RButton.IsEnabled = this.Availability.HasFlag(RotationValue);
            this.STextBox.IsEnabled = this.SButton.IsEnabled = this.Availability.HasFlag(SkewValue);

            this.UpdateX(indicator.X);
            this.UpdateY(indicator.Y);
            this.UpdateWidth(indicator.Width);
            this.UpdateHeight(indicator.Height);
            this.UpdateRotation(indicator.Rotation);
            this.UpdateSkew(indicator.Skew);
        }

        public void UpdateSizeType(IndicatorSizeType type)
        {
            this.OldValue = this.NewValue;
            this.NewValue = type;

            this.Availability = Indicator.ToAvailabilityChange(this.OldValue, this.NewValue);

            if (this.OldValue != this.NewValue)
            {
                this.UpdateComboBox();
            }

            if (this.Availability.HasFlag(XHasValue)) this.XTextBox.IsEnabled = this.XButton.IsEnabled = this.Availability.HasFlag(XValue);
            if (this.Availability.HasFlag(YHasValue)) this.YTextBox.IsEnabled = this.YButton.IsEnabled = this.Availability.HasFlag(YValue);
            if (this.Availability.HasFlag(WidthHasValue)) this.WTextBox.IsEnabled = this.WButton.IsEnabled = this.Availability.HasFlag(WidthValue);
            if (this.Availability.HasFlag(HeightHasValue)) this.HTextBox.IsEnabled = this.HButton.IsEnabled = this.Availability.HasFlag(HeightValue);
            if (this.Availability.HasFlag(RotationHasValue)) this.RTextBox.IsEnabled = this.RButton.IsEnabled = this.Availability.HasFlag(RotationValue);
            if (this.Availability.HasFlag(SkewHasValue)) this.STextBox.IsEnabled = this.SButton.IsEnabled = this.Availability.HasFlag(SkewValue);
        }

        public void UpdateX(float value) => this.XTextBox.Text = value.ToString("0.00");
        public void UpdateY(float value) => this.YTextBox.Text = value.ToString("0.00");
        public void UpdateWidth(float value) => this.WTextBox.Text = value.ToString("0.00");
        public void UpdateHeight(float value) => this.HTextBox.Text = value.ToString("0.00");
        public void UpdateRotation(float value) => this.RTextBox.Text = value.ToString("0.00");
        public void UpdateSkew(float value) => this.STextBox.Text = value.ToString("0.00");
    }
}