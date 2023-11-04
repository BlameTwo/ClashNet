using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;

namespace ClashNet.Styles.Converter;

internal class DelayConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is int val)
        {
            if(val >= 200 && val<400)
                return new SolidColorBrush(Colors.Green);
            if (val >= 400 && val < 600)
                return new SolidColorBrush(Colors.LightYellow); 
            if (val >= 600 && val < 800)
                return new SolidColorBrush(Colors.Yellow);
            if (val >= 800 || val ==-1)
                return new SolidColorBrush(Colors.Red);
            if (val == -1)
                return new SolidColorBrush(Colors.Red);
        }
        if(value is string str)
        {
            if(str == "Timeout")
                return new SolidColorBrush(Colors.Red);
            if (str.StartsWith("An error"))
                return new SolidColorBrush(Colors.Red);
        }
        return new SolidColorBrush(Colors.Black);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
