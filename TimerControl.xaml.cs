using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Timer
{
    /// <summary>
    /// Логика взаимодействия для TimerControl.xaml
    /// </summary>
    public partial class TimerControl : UserControl, IUpdater
    {
        DispatcherTimer dispatcherTimer;
        ObservableCollection<IUpdater> stopwatchesAndTimers;
        string competitorName;
        string taskName;
        bool isSelected;
        int fullSeconds;
        Stopwatch stopwatch = new Stopwatch();

        public TimerControl(DispatcherTimer dispatcherTimer, ObservableCollection<IUpdater> stopwatchesAndTimers, string competitorName, string taskName, int fullSeconds)
        {
            InitializeComponent();
            this.dispatcherTimer = dispatcherTimer;
            this.stopwatchesAndTimers = stopwatchesAndTimers;
            this.competitorName = competitorName;
            this.taskName = taskName;
            this.fullSeconds = fullSeconds;

            TaskNameTextBox.Text =this.taskName;
            TaskNameTextBox.ToolTip = this.taskName;
            StopwatchStatTextBox.Text = SecondsToHMS(this.fullSeconds);
            TimerProgressBar.Maximum = this.fullSeconds;
            StopStopwatchButton.IsEnabled = false;
            TimerBorder.Background = new SolidColorBrush(Color.FromRgb(255, 87, 87));
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
            StopwatchStatTextBox.Text = SecondsToHMS(fullSeconds);
            TimerProgressBar.Value = 0;
            StartStopwatchButton.IsEnabled = true;
            StopStopwatchButton.IsEnabled = false;
        }
        private string SecondsToHMS(int seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return time.ToString(@"hh\:mm\:ss");
        }
        public void SelectItem()
        {
            isSelected = true;
            TimerBorder.Background = new SolidColorBrush(Color.FromRgb(87, 188, 255));
        }
        public void UnselectItem()
        {
            isSelected = false;
            if (stopwatch.IsRunning)
            {
                TimerBorder.Background = new SolidColorBrush(Color.FromRgb(87, 255, 95));
            }
            else
            {
                TimerBorder.Background = new SolidColorBrush(Color.FromRgb(255, 87, 87));
            }
        }
        public void UpdateStat()
        {
            TimerProgressBar.Value = stopwatch.Elapsed.TotalSeconds;
            StopwatchStatTextBox.Text = SecondsToHMS(fullSeconds - (int)Math.Round(stopwatch.Elapsed.TotalSeconds));
            if((int)Math.Round(stopwatch.Elapsed.TotalSeconds) == fullSeconds) 
            {
                MessageBox.Show("Время на задание " + taskName + " у участника вышло!");
                stopwatchesAndTimers.Remove(this);
                stopwatch.Reset();
                if (stopwatchesAndTimers.Count == 0)
                {
                    dispatcherTimer.Stop();
                }
                TimerProgressBar.Value = 0;
                StopwatchStatTextBox.Text = SecondsToHMS(fullSeconds);
                StartStopwatchButton.IsEnabled = true;
                StopStopwatchButton.IsEnabled = false;
            }
        }
        public void RunTask()
        {
            if (stopwatchesAndTimers.Count == 0)
            {
                dispatcherTimer.Start();
            }
            stopwatchesAndTimers.Add(this);
            stopwatch.Start();
            StartStopwatchButton.IsEnabled = false;
            StopStopwatchButton.IsEnabled = true;
        }
    }
}
