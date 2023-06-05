using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Timer
{
    /// <summary>
    /// Логика взаимодействия для CompetitorControl.xaml
    /// </summary>
    public partial class CompetitorControl : UserControl
    {
        DispatcherTimer dispatcherTimer;
        List<IUpdater> stopwatchesAndTimers;
        public string competitorName { get; set; }
        public CompetitorControl(DispatcherTimer dispatcherTimer, List<IUpdater> stopwatchesAndTimers, string competitorName)
        {
            InitializeComponent();
            this.dispatcherTimer = dispatcherTimer;
            this.stopwatchesAndTimers = stopwatchesAndTimers;
            this.competitorName = competitorName;
            CompetitorExpander.Header = competitorName;
        }
        private void AddTaskImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            AddCompetitorTask addCompetitorTask = new AddCompetitorTask();
            if (addCompetitorTask.ShowDialog() == true)
            {
                if (addCompetitorTask.IsStopwatchRadio.IsChecked == true)
                {
                    StopwatchControl stopwatchControl = new StopwatchControl(dispatcherTimer, stopwatchesAndTimers, addCompetitorTask.TaskNameTextBox.Text);
                    CompetitorTasksListBox.Items.Add(stopwatchControl);
                }
                else
                {
                    int fullSeconds = int.Parse(addCompetitorTask.HoursTextBox.Text) * 3600 +
                                      int.Parse(addCompetitorTask.MinutesTextBox.Text) * 60 +
                                      int.Parse(addCompetitorTask.SecondsTextBox.Text);
                    TimerControl timerControl = new TimerControl(dispatcherTimer, stopwatchesAndTimers, competitorName, addCompetitorTask.TaskNameTextBox.Text, fullSeconds);
                    CompetitorTasksListBox.Items.Add(timerControl);
                }
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            foreach (IUpdater stopwatchOrTimer in CompetitorTasksListBox.Items) 
            {
                if(stopwatchesAndTimers.Contains(stopwatchOrTimer))
                {
                    stopwatchesAndTimers.Remove(stopwatchOrTimer);
                }
            }
        }
    }
}
