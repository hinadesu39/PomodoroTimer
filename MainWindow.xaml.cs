using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PomodoroTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            TitleZone.MouseDown += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            };
            DragZone.MouseDown += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            };
            btn_Min.Click += (s, e) =>
            {
                this.WindowState = WindowState.Minimized;
            };
        }
        WindowAccentCompositor wac = null;
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            wac = new(this, false, (c) =>
            {
                //没有可用的模糊特效
                c.A = 255;
                Background = new SolidColorBrush(c);
            });
            //wac.Color = (bool)DarkModeCheck.IsChecked ? Color.FromArgb(180, 0, 0, 0) : Color.FromArgb(180, 255, 255, 255);
            wac.Color = Color.FromArgb(180, 0, 0, 0);
            wac.IsEnabled = true;
        }

        
    }
}