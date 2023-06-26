using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Timer
{
    /// <summary>
    /// Логика взаимодействия для StopwatchControl.xaml
    /// </summary>
    public partial class StopwatchControl : UserControl, IUpdater
    {
        DispatcherTimer dispatcherTimer;
        ObservableCollection<IUpdater> stopwatchesAndTimers;
        string taskName;
        bool isSelected;
        Stopwatch stopwatch = new Stopwatch();

        public StopwatchControl(DispatcherTimer dispatcherTimer, ObservableCollection<IUpdater> stopwatchesAndTimers, string taskName)
        {
            InitializeComponent();
            this.dispatcherTimer = dispatcherTimer;
            this.stopwatchesAndTimers = stopwatchesAndTimers;
            this.taskName = taskName;
            TaskNameTextBox.Text = taskName;
            TaskNameTextBox.ToolTip = taskName;
            StopStopwatchButton.IsEnabled = false;
            if (!isSelected)
            {
                TimerBorder.Background = new SolidColorBrush(Color.FromRgb(255, 87, 87));
            }
        }
        private void StartStopwatchButton_Click(object sender, RoutedEventArgs e)
        {
            RunTask();
        }
        private void StopStopwatchButton_Click(object sender, RoutedEventArgs e)
        {
            stopwatchesAndTimers.Remove(this);
            stopwatch.Stop();
            if (stopwatchesAndTimers.Count == 0)
            {
                dispatcherTimer.Stop();
            }
            if (!isSelected)
            {
                TimerBorder.Background = new SolidColorBrush(Color.FromRgb(255, 87, 87));
            }
            StartStopwatchButton.IsEnabled = true;
            StopStopwatchButton.IsEnabled = false;
        }
        private void ResetStopwatchButton_Click(object sender, RoutedEventArgs e)
        {
            stopwatchesAndTimers.Remove(this);
            stopwatch.Reset();
            if (stopwatchesAndTimers.Count == 0)
            {
                dispatcherTimer.Stop();
            }
            StopwatchStatTextBox.Text = "00:00:00";
            if (!isSelected)
            {
                TimerBorder.Background = new SolidColorBrush(Color.FromRgb(255, 87, 87));
            }
            StartStopwatchButton.IsEnabled = true;
            StopStopwatchButton.IsEnabled = false;
        }
        public void UpdateStat()
        {
            int hours = stopwatch.Elapsed.Hours;
            int minutes = stopwatch.Elapsed.Minutes;
            int seconds = stopwatch.Elapsed.Seconds;
            StopwatchStatTextBox.Text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }
        public void RunTask()
        {
            if (stopwatchesAndTimers.Count == 0)
            {
                dispatcherTimer.Start();
            }
            stopwatchesAndTimers.Add(this);
            stopwatch.Start();
            if (!isSelected)
            {
                TimerBorder.Background = new SolidColorBrush(Color.FromRgb(87, 255, 95));
            }
            StartStopwatchButton.IsEnabled = false;
            StopStopwatchButton.IsEnabled = true;
        }

        public void SelectItem()
        {
            isSelected = true;
            TimerBorder.Background  = new SolidColorBrush(Color.FromRgb(87, 188, 255));
        }
        public void UnselectItem()
        {
            isSelected = false;
            if(stopwatch.IsRunning)
            {
                TimerBorder.Background = new SolidColorBrush(Color.FromRgb(87, 255, 95));
            } else
            {
                TimerBorder.Background = new SolidColorBrush(Color.FromRgb(255, 87, 87));
            }
        }
    }
}