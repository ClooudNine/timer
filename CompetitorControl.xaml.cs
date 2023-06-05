using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
        string competitorName;
        public CompetitorControl(DispatcherTimer dispatcherTimer, List<IUpdater> stopwatchesAndTimers, string competitorName)
        {
            InitializeComponent();
            this.dispatcherTimer = dispatcherTimer;
            this.stopwatchesAndTimers = stopwatchesAndTimers;
            this.competitorName = competitorName;
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
    }
}
