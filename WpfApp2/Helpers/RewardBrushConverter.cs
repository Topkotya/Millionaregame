using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfApp2
{
    public class RewardBrushConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = (ERewardState)value;
            switch (state)
            {
                case ERewardState.Current:
                    return new SolidColorBrush(System.Windows.Media.Color.FromRgb(250,176,5));
                case ERewardState.Completed:
                    return new SolidColorBrush(System.Windows.Media.Color.FromRgb(55, 195, 55));
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
            //конвертирование обратно из колорбраша в еревордстейт
        }
    }
}
