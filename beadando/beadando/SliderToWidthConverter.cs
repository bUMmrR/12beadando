using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace beadando
{
    public class SliderToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double sliderValue = (double)value;

            // Scale the value from 20 to 80 pixels
            double minWidth = 50;
            double maxWidth = 200;

            // Assuming the slider maximum value is 100 for this example
            double scaledWidth = minWidth + (sliderValue / 100) * (maxWidth - minWidth);
            return scaledWidth;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
