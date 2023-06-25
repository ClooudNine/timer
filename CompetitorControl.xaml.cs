using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
        List<IUpdater> competitorTasks = new List<IUpdater>();
        public string competitorName { get; set; }
        public CompetitorControl(DispatcherTimer dispatcherTimer, List<IUpdater> stopwatchesAndTimers, string competitorName)
        {
            InitializeComponent();
            this.dispatcherTimer = dispatcherTimer;
            this.stopwatchesAndTimers = stopwatchesAndTimers;
            this.competitorName = competitorName;
            CompetitorExpander.Header = new TextBlock
            {
                Text = competitorName,
                FontFamily = new FontFamily("Cascadia Mono SemiBold"),
                FontSize = 22,
                Foreground = new SolidColorBrush(Colors.White)
            };
        }
        private void AddTaskImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            AddCompetitorTask addCompetitorTask = new AddCompetitorTask();
            if (addCompetitorTask.ShowDialog() == true)
            {
                if (addCompetitorTask.IsStopwatchRadio.IsChecked == true)
                {
                    StopwatchControl stopwatchControl = new StopwatchControl(dispatcherTimer, stopwatchesAndTimers, addCompetitorTask.TaskNameTextBox.Text);
                    competitorTasks.Add(stopwatchControl);
                    CompetitorTasksListBox.Items.Add(stopwatchControl);
                }
                else
                {
                    int fullSeconds = int.Parse(addCompetitorTask.HoursTextBox.Text) * 3600 +
                                      int.Parse(addCompetitorTask.MinutesTextBox.Text) * 60 +
                                      int.Parse(addCompetitorTask.SecondsTextBox.Text);
                    TimerControl timerControl = new TimerControl(dispatcherTimer, stopwatchesAndTimers, competitorName, addCompetitorTask.TaskNameTextBox.Text, fullSeconds);
                    competitorTasks.Add(timerControl);
                    CompetitorTasksListBox.Items.Add(timerControl);
                }
            }
        }
        private void DeleteTaskImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(CompetitorTasksListBox.SelectedItem == null)
            {
                MessageBox.Show("Выделите задачи для удаления!", "Выделите задачи!");
            } 
            else
            {
                while (CompetitorTasksListBox.SelectedItems.Count > 0)
                {
                    competitorTasks.Remove((IUpdater)CompetitorTasksListBox.SelectedItems[0]);
                    CompetitorTasksListBox.Items.Remove(CompetitorTasksListBox.SelectedItems[0]);
                }
            }
        }
        public void RunFirstTask()
        {
            if (competitorTasks.Count > 0)
            {
                competitorTasks[0].RunTask();
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

        private void CompetitorTasksListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.RemovedItems.Count > 0)
            {
                ((IUpdater)e.RemovedItems[0]).UnselectItem();
            }
            if(e.AddedItems.Count > 0)
            {
                ((IUpdater)e.AddedItems[0]).SelectItem();
            }
        }
    }
}
