using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PomodoroTimer.Behaviors
{
    class GetTextFocusedBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.GotKeyboardFocus += SelectContents;
        }
        private void SelectContents(object sender, RoutedEventArgs e)
        {
            TextBox tb = (sender as TextBox);
            if (tb != null)
            {
                tb.SelectionBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#a94dc1"));
                tb.SelectAll();
            }
        }
    }
}
