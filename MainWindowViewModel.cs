using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Toolkit.Uwp.Notifications;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Windows.Foundation.Collections;

namespace PomodoroTimer
{
    public partial class MainWindowViewModel : ObservableObject
    {
        #region Fields
        private TimeSpan elapsedTime;
        private DispatcherTimer timer;
        private int restStatusCount;
        private int focusStatusCount;
        private int currentRestStatusCount = 1;
        private int currentFocusStatusCount = 1;
        private bool isFocusTime = true;
        #endregion

        #region Ctor
        public MainWindowViewModel()
        {
            GenerateHourMarks();
            RestTimesTootip = Duration / 25;
            //订阅Toast交互信息
            ToastNotificationManagerCompat.OnActivated += toastArgs =>
            {
                // Obtain the arguments from the notification
                ToastArguments args = ToastArguments.Parse(toastArgs.Argument);
            };
        }
        #endregion

        #region Properties
        [ObservableProperty]
        public ObservableCollection<HourMark> hourMarks;

        [ObservableProperty]
        public string titleBar;

        [ObservableProperty]
        public string statusBar;

        [ObservableProperty]
        public bool isPlayOrPause = true;

        private TimeSpan remainingTime;

        public TimeSpan RemainingTime
        {
            get { return remainingTime; }
            set
            {
                remainingTime = value;
                OnPropertyChanged();
            }
        }

        private int duration = 30;

        public int Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                RestTimesTootip = duration / 25;
                OnPropertyChanged();
            }
        }

        private int restTimesTootip;

        public int RestTimesTootip
        {
            get { return restTimesTootip; }
            set
            {
                restTimesTootip = value;
                OnPropertyChanged();
            }
        }

        private bool skipRest;

        public bool SkipRest
        {
            get { return skipRest; }
            set
            {
                skipRest = value;
                if (skipRest)
                    RestTimesTootip = 0;
                else
                    RestTimesTootip = duration / 25;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Command
        [RelayCommand]
        public void StartFocusSession()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            currentFocusStatusCount = 1;
            currentRestStatusCount = 1;
            focusStatusCount = (Duration - 1) / 25 + 1;
            restStatusCount = Duration / 25;

            if (SkipRest)
            {
                TitleBar = $"专注时间段(第1个/共1个)";
                StatusBar = "下一个: 无";
                RemainingTime = TimeSpan.FromMinutes(Duration);
            }
            else
            {
                TitleBar = $"专注时间段(第1个/共{focusStatusCount}个)";
                StatusBar = "下一个: 5分钟休息";
                RemainingTime = TimeSpan.FromMinutes(25);
            }
        }

        [RelayCommand]
        public void PlayOrPause(bool isStart)
        {
            if (!isStart)
            {
                timer.Stop();
                StatusBar = "已暂停";
                return;
            }
            else
            {
                timer.Start();
            }
            if (isFocusTime)
            {
                if (currentRestStatusCount <= restStatusCount)
                {
                    StatusBar = "下一个: 5分钟休息";
                }
                else
                {
                    StatusBar = "下一个: 无";
                }
            }
            else
            {
                if (currentFocusStatusCount <= focusStatusCount)
                {
                    StatusBar = $"下一个: 专注时间段(第{currentFocusStatusCount}个)";
                }
                else
                {
                    StatusBar = "下一个: 无";
                }
            }
        }

        [RelayCommand]
        public void Reset()
        {
            elapsedTime = TimeSpan.Zero;
            foreach (var item in HourMarks)
            {
                item.IsFill = false;
            }
        }

        private void GenerateHourMarks()
        {
            HourMarks = new();
            for (int i = 0; i < 24; i++)
            {
                double angle = i * Math.PI / 12;
                double outerRadius = 90;
                double innerRadius = outerRadius - 12;
                double outerX = 100 + outerRadius * Math.Sin(angle);
                double outerY = 100 - outerRadius * Math.Cos(angle);
                double innerX = 100 + innerRadius * Math.Sin(angle);
                double innerY = 100 - innerRadius * Math.Cos(angle);
                HourMarks.Add(
                    new HourMark()
                    {
                        Data = new LineGeometry(
                            new Point(innerX, innerY),
                            new Point(outerX, outerY)
                        )
                    }
                );
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            elapsedTime += TimeSpan.FromSeconds(1);
            //用户选择不休息
            if (skipRest)
            {
                RemainingTime = TimeSpan.FromMinutes(Duration) - elapsedTime;
                //完成了专注时段
                if (elapsedTime >= TimeSpan.FromMinutes(Duration))
                {
                    ConstructToastAndUpdateView(Status.Completed);
                    return;
                }
                var perTime = TimeSpan.FromMinutes(Duration) / 24;
                HourMarks[(int)(elapsedTime / perTime)].IsFill = true;
            }
            //专注时段，并且还未完成所有专注时段
            else if (isFocusTime && currentFocusStatusCount <= focusStatusCount)
            {
                RemainingTime = GetCurrentFocusDuration() - elapsedTime;
                //完成了专注时段
                if (elapsedTime >= GetCurrentFocusDuration())
                {
                    if (currentRestStatusCount <= restStatusCount)
                    {
                        ConstructToastAndUpdateView(Status.Rest);
                    }
                    else
                    {
                        ConstructToastAndUpdateView(Status.Completed);
                    }
                    Reset();
                    currentFocusStatusCount++;
                    isFocusTime = false;
                    return;
                }
                var perTime = GetCurrentFocusDuration() / 24;
                HourMarks[(int)(elapsedTime / perTime)].IsFill = true;
            }
            //休息时段，并且还未完成所有休息时段
            else if (!isFocusTime && currentRestStatusCount <= restStatusCount)
            {
                RemainingTime = TimeSpan.FromMinutes(5) - elapsedTime;
                //休息时段结束
                if (elapsedTime >= TimeSpan.FromMinutes(5))
                {
                    if (currentFocusStatusCount <= focusStatusCount)
                    {
                        ConstructToastAndUpdateView(Status.Foucs);
                    }
                    else
                    {
                        ConstructToastAndUpdateView(Status.Completed);
                    }
                    Reset();
                    currentRestStatusCount++;
                    isFocusTime = true;
                    return;
                }
                var perTime = TimeSpan.FromMinutes(5) / 24;
                HourMarks[(int)(elapsedTime / perTime)].IsFill = true;
            }
        }

        private void ConstructToastAndUpdateView(Status status)
        {
            switch (status)
            {
                case Status.Completed:
                    // 构建和显示通知
                    new ToastContentBuilder()
                        .AddArgument("action", "viewConversation")
                        .AddArgument("conversationId", 9813)
                        .AddText("非常好!")
                        .AddText("你已经完成了所有专注时段!")
                        // Buttons
                        .AddButton(
                            new ToastButton()
                                .SetContent("继续")
                                .AddArgument("action", "continue")
                                .SetBackgroundActivation()
                        )
                        .Show();
                    StatusBar = "下一个: 无";
                    IsPlayOrPause = false;
                    timer.Stop();
                    break;
                case Status.Rest:
                    new ToastContentBuilder()
                        .AddArgument("action", "viewConversation")
                        .AddArgument("conversationId", 9813)
                        .AddText("非常不错!")
                        .AddText("你已经完成了专注时段, 接下来请休息5分钟!")
                        // Buttons
                        .AddButton(
                            new ToastButton()
                                .SetContent("跳过休息")
                                .AddArgument("action", "skipRest")
                                .SetBackgroundActivation()
                        )
                        .AddButton(
                            new ToastButton()
                                .SetContent("继续")
                                .AddArgument("action", "continue")
                                .SetBackgroundActivation()
                        )
                        .Show();
                    TitleBar = $"休息时间段(第{currentRestStatusCount}个/共{restStatusCount}个)";
                    if (currentFocusStatusCount + 1 <= focusStatusCount)
                    {
                        StatusBar = $"下一个: 专注时间段(第{currentFocusStatusCount + 1}个)";
                    }
                    else
                    {
                        StatusBar = "下一个: 无";
                    }
                    break;
                case Status.Foucs:
                    new ToastContentBuilder()
                        .AddArgument("action", "viewConversation")
                        .AddArgument("conversationId", 9813)
                        .AddText("时间到!")
                        .AddText("休息时间结束, 接下来请继续专注!")
                        // Buttons
                        .AddButton(
                            new ToastButton()
                                .SetContent("暂停")
                                .AddArgument("action", "pause")
                                .SetBackgroundActivation()
                        )
                        .AddButton(
                            new ToastButton()
                                .SetContent("继续")
                                .AddArgument("action", "continue")
                                .SetBackgroundActivation()
                        )
                        .Show();
                    TitleBar = $"专注时间段(第{currentFocusStatusCount}个/共{focusStatusCount}个)";
                    if (currentRestStatusCount + 1 <= restStatusCount)
                    {
                        StatusBar = $"下一个: 休息时间段(第{currentRestStatusCount + 1}个)";
                    }
                    else
                    {
                        StatusBar = "下一个: 无";
                    }
                    break;
            }
        }

        private TimeSpan GetCurrentFocusDuration()
        {
            return currentFocusStatusCount < focusStatusCount
                ? TimeSpan.FromMinutes(25)
                : TimeSpan.FromMinutes(duration)
                    - TimeSpan.FromMinutes(25) * (focusStatusCount - 1);
        }
        #endregion

        private enum Status
        {
            Completed,
            Rest,
            Foucs
        }
    }
}
