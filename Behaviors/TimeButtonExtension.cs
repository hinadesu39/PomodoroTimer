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
    public static class TimeButtonExtension
    {
        #region Scale
        public static int GetScale(DependencyObject obj)
        {
            return (int)obj.GetValue(ScaleProperty);
        }

        public static void SetScale(DependencyObject obj, int value)
        {
            obj.SetValue(ScaleProperty, value);
        }

        // Using a DependencyProperty as the backing store for Scale.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.RegisterAttached(
                "Scale",
                typeof(int),
                typeof(TimeButtonExtension),
                new PropertyMetadata(5)
            );

        #endregion

        #region IncreaseButton
        public static bool GetIsIncreaseTimeButton(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsIncreaseTimeButtonProperty);
        }

        public static void SetIsIncreaseTimeButton(DependencyObject obj, bool value)
        {
            obj.SetValue(IsIncreaseTimeButtonProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsIncreaseTimeButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsIncreaseTimeButtonProperty =
            DependencyProperty.RegisterAttached(
                "IsIncreaseTimeButton",
                typeof(bool),
                typeof(TimeButtonExtension),
                new PropertyMetadata(false, OnIsIncreaseTimeButtonChanged)
            );

        private static void OnIsIncreaseTimeButtonChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e
        )
        {
            var button = d as Button;
            if (button != null)
            {
                button.Click += TimeIncrease;
            }
        }
        #endregion

        #region DecreaseButton
        public static bool GetIsDecreaseTimeButton(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDecreaseTimeButtonProperty);
        }

        public static void SetIsDecreaseTimeButton(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDecreaseTimeButtonProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsIncreaseTimeButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDecreaseTimeButtonProperty =
            DependencyProperty.RegisterAttached(
                "IsDecreaseTimeButton",
                typeof(bool),
                typeof(TimeButtonExtension),
                new PropertyMetadata(false, OnIsDecreaseTimeButtonChanged)
            );

        private static void OnIsDecreaseTimeButtonChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e
        )
        {
            var button = d as Button;
            if (button != null)
            {
                button.Click += TimeDecrease;
            }
        } 
        #endregion

        private static void TimeDecrease(object sender, RoutedEventArgs e)
        {
            var textBox = GetParentObject<TextBox>((Button)sender);
            int currentTime;
            if (textBox != null && int.TryParse(textBox.Text, out currentTime))
            {
                currentTime += GetScale((Button)sender);
                textBox.Text = currentTime.ToString();
            }
        }

        private static void TimeIncrease(object sender, RoutedEventArgs e)
        {
            var textBox = GetParentObject<TextBox>((Button)sender);
            int currentTime;
            if (textBox != null && int.TryParse(textBox.Text, out currentTime))
            {
                currentTime -= GetScale((Button)sender);
                textBox.Text = currentTime.ToString();
            }
        }

        public static T GetParentObject<T>(DependencyObject child)
            where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            while (parent != null)
            {
                if (parent is T && parent != null)
                {
                    return (T)parent;
                }
                // 在上一级父控件中没有找到指定的控件，就再往上一级找
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }
    }
}
