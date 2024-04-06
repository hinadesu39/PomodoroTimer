using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace PomodoroTimer
{
    public partial class HourMark : ObservableObject
    {
        private LineGeometry data;

        public LineGeometry Data
        {
            get { return data; }
            set { data = value; }
        }

        [ObservableProperty]
        private bool isFill = false;

    }
}
