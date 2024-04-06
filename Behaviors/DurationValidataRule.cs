using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PomodoroTimer.Behaviors
{
    public class DurationValidataRule : ValidationRule
    {
        public int MaxDuration { get; set; } = 100;
        public int MinDuration { get; set; } = 0;
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int currentDuration;

            if (int.TryParse(value.ToString(), out currentDuration))
            {
                if (currentDuration > MaxDuration)
                {
                    return new ValidationResult(false, "DurationTooLong");
                }
                else if (currentDuration < MinDuration)
                {
                    return new ValidationResult(false, "DurationTooShort");
                }
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, "Duration Must be a int");
            }
        }
    }
}
