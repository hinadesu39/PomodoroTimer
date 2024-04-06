using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PomodoroTimer.Behaviors
{
    class TimeInOrDeBehavior : Behavior<Button>
    {
        public int Scale { get; set; } = 5;
        public int MaxTime { get; set; } = 100;
        public int MinTime { get; set; } = 0;
        public bool IsIncrease { get; set; }

        public TextBox Target
        {
            get { return (TextBox)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Target.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target", typeof(TextBox), typeof(TimeInOrDeBehavior), new PropertyMetadata(null));


        protected override void OnAttached()
        {
            if (IsIncrease)
            {
                AssociatedObject.Click += TimeIncrease;
            }
            else
            {
                AssociatedObject.Click += TimeDecrease;
            }

        }

        private void TimeDecrease(object sender, RoutedEventArgs e)
        {
            int currentTime;
            if (Target != null && int.TryParse(Target.Text,out currentTime)) 
            {
                currentTime += Scale;
                if(currentTime > MaxTime)
                {
                    currentTime = MaxTime;
                }
                else if (currentTime < MinTime)
                {
                    currentTime = MinTime;
                }
                Target.Text = currentTime.ToString();
            }
        }

        private void TimeIncrease(object sender, RoutedEventArgs e)
        {
            int currentTime;
            if (Target != null && int.TryParse(Target.Text, out currentTime))
            {
                currentTime -= Scale;
                if (currentTime > MaxTime)
                {
                    currentTime = MaxTime;
                }
                else if (currentTime < MinTime)
                {
                    currentTime = MinTime;
                }
                Target.Text = currentTime.ToString();
            }
        }
    }
}
