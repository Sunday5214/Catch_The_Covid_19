using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace CatchTheCovid19_UWPClient.Converter
{
    public class TemperatureToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double data = System.Convert.ToDouble(value);
            if (data == 0.0) return "#000000";
            if(data >= 37.5)
            {
                return "#FF0000";
            }
            else
            {
                return "#00FF00";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
