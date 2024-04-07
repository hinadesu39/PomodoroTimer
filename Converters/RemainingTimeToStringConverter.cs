using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PomodoroTimer.Converters
{
    public class RemainingTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var remainingTime = (TimeSpan)value;
                if (remainingTime == TimeSpan.FromMinutes(0))
                {
                    return "完成";
                }
                else if(remainingTime < TimeSpan.FromMinutes(1))
                {
                    return remainingTime.ToString("ss'秒'");
                }
                else
                {
                    return remainingTime.ToString("mm'分'");
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
