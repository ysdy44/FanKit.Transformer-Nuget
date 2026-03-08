using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace FanKit.Transformer.TestApp
{
    public class BoolToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            return value is bool item && item ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}