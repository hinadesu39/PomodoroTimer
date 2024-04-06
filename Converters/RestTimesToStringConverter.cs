using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Windows.Web.Syndication;

namespace PomodoroTimer.Converters
{
    public class RestTimesToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int times;
            if(value != null && int.TryParse(value.ToString(),out times))
            {
                if(times == 0)
                {
                    return "您将没有休息时间。";
                }
                else
                {
                    return $"您将有{times}次休息时间。";
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
