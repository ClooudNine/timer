using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Timer
{
    /// <summary>
    /// Логика взаимодействия для StopwatchControl.xaml
    /// </summary>
    public partial class StopwatchControl : UserControl, IUpdater
    {
        DispatcherTimer dispatcherTimer;
        List<IUpdater> stopwatchesAndTimers;
        string taskName;
        Stopwatch stopwatch = new Stopwatch();

        public StopwatchControl(DispatcherTimer dispatcherTimer, List<IUpdater> stopwatchesAndTimers, string taskName)
        {
            InitializeComponent();
            this.dispatcherTimer = dispatcherTimer;
            this.stopwatchesAndTimers = stopwatchesAndTimers;
            this.taskName = taskName;
            TaskNameTextBox.Text = taskName;
            StopStopwatchButton.IsEnabled = false;
        }
        private void StartStopwatchButton_Click(object sender, RoutedEventArgs e)
        {
            if(stopwatchesAndTimers.Count == 0)
            {
                dispatcherTimer.Start();
            }
            stopwatchesAndTimers.Add(this);
            stopwatch.Start();
            StartStopwatchButton.IsEnabled = false;
            StopStopwatchButton.IsEnabled = true;
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
            StopwatchStatTextBox.Text = "00:00:00";
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
    }
}