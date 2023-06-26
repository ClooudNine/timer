using System.Collections.ObjectModel;
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
        ObservableCollection<IUpdater> stopwatchesAndTimers;
        ObservableCollection<IUpdater> competitorTasks = new ObservableCollection<IUpdater>();
        public string competitorName { get; set; }
        public CompetitorControl(DispatcherTimer dispatcherTimer, ObservableCollection<IUpdater> stopwatchesAndTimers, string competitorName)
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
            CompetitorTasksListBox.ItemsSource = competitorTasks;
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
                }
                else
                {
                    int fullSeconds = int.Parse(addCompetitorTask.HoursTextBox.Text) * 3600 +
                                      int.Parse(addCompetitorTask.MinutesTextBox.Text) * 60 +
                                      int.Parse(addCompetitorTask.SecondsTextBox.Text);
                    TimerControl timerControl = new TimerControl(dispatcherTimer, stopwatchesAndTimers, competitorName, addCompetitorTask.TaskNameTextBox.Text, fullSeconds);
                    competitorTasks.Add(timerControl);
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
                    stopwatchesAndTimers.Remove((IUpdater)CompetitorTasksListBox.SelectedItems[0]);
                    competitorTasks.Remove((IUpdater)CompetitorTasksListBox.SelectedItems[0]);
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
            if(CompetitorTasksListBox.SelectedItems.Count == 1)
            {
                TaskUpImage.IsEnabled = true;
                TaskUpImage.Opacity = 1;
                TaskDownImage.IsEnabled = true;
                TaskDownImage.Opacity = 1;
            } 
            else
            {
                TaskUpImage.IsEnabled = false;
                TaskUpImage.Opacity = 0.5;
                TaskDownImage.IsEnabled = false;
                TaskDownImage.Opacity = 0.5;
            }
        }
        private void TaskUpImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int index = CompetitorTasksListBox.SelectedIndex;
            if (index != 0)
            {
                (competitorTasks[index], competitorTasks[index - 1]) = (competitorTasks[index - 1], competitorTasks[index]);
                CompetitorTasksListBox.SelectedIndex = index - 1;
            }
        }
        private void TaskDownImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int index = CompetitorTasksListBox.SelectedIndex;
            if (index != competitorTasks.Count - 1)
            {
                (competitorTasks[index], competitorTasks[index + 1]) = (competitorTasks[index + 1], competitorTasks[index]);
                CompetitorTasksListBox.SelectedIndex = index + 1;
            }
        }
        private void Competitor_Unloaded(object sender, RoutedEventArgs e)
        {
            foreach (IUpdater stopwatchOrTimer in competitorTasks)
            {
                stopwatchesAndTimers.Remove(stopwatchOrTimer);
            }
        }
    }
}
