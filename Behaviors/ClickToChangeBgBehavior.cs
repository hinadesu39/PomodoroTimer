using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PomodoroTimer.Behaviors
{
    public class ClickToChangeBgBehavior : Behavior<Image>
    {
        private int bgIndex = 0;
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += InitBg;
            AssociatedObject.MouseDown += ChangeBg;
        }

        private void InitBg(object sender, RoutedEventArgs e)
        {
            var bgList = AssociatedObject.Tag as ObservableCollection<string>;
            if (bgList != null)
            {
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri(bgList[bgIndex], UriKind.Relative);
                logo.EndInit();
                AssociatedObject.Source = logo;
            }
        }

        private void ChangeBg(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var bgList = AssociatedObject.Tag as ObservableCollection<string>;
            if (bgList != null)
            {
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                if (bgIndex == bgList.Count - 1)
                {
                    bgIndex = 0;
                }
                else
                {
                    bgIndex++;
                }
                logo.UriSource = new Uri(bgList[bgIndex], UriKind.Relative);
                logo.EndInit();
                AssociatedObject.Source = logo;
            }
        }
    }
}
