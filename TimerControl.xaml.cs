using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Timer
{
    /// <summary>
    /// Логика взаимодействия для TimerControl.xaml
    /// </summary>
    public partial class TimerControl : UserControl, IUpdater
    {
        DispatcherTimer dispatcherTimer;
        List<IUpdater> stopwatchesAndTimers;
        string competitorName;
        string taskName;
        int fullSeconds;
        Stopwatch stopwatch = new Stopwatch();

        public TimerControl(DispatcherTimer dispatcherTimer, List<IUpdater> stopwatchesAndTimers, string competitorName, string taskName, int fullSeconds)
        {
            InitializeComponent();
            this.dispatcherTimer = dispatcherTimer;
            this.stopwatchesAndTimers = stopwatchesAndTimers;
            this.competitorName = competitorName;
            this.taskName = taskName;
            this.fullSeconds = fullSeconds;

            TaskNameTextBox.Text =this.taskName;
            StopwatchStatTextBox.Text = SecondsToHMS(this.fullSeconds);
            TimerProgressBar.Maximum = this.fullSeconds;
            StopStopwatchButton.IsEnabled = false;
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
