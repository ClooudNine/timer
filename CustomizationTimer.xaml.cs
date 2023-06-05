using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Timer
{
    /// <summary>
    /// Логика взаимодействия для CustomizationTimer.xaml
    /// </summary>
    public partial class CustomizationTimer : Window
    {
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public List<IUpdater> stopwatchesAndTimers = new List<IUpdater>();
        public CustomizationTimer()
        {
            InitializeComponent();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
        }
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            foreach(IUpdater stopwatch in stopwatchesAndTimers.ToArray())
            {
                stopwatch.UpdateStat();
            }
        }
        private void AddPeople_Click(object sender, RoutedEventArgs e)
        {
            AddCompetitorModal addCompetitorModal = new AddCompetitorModal();
            if (addCompetitorModal.ShowDialog() == true)
            {
                CompetitorControl competitorControl = new CompetitorControl(dispatcherTimer, stopwatchesAndTimers, addCompetitorModal.CaptionCompetitorTextBox.Text);
                Competitors.Children.Add(competitorControl);
            }
        }
        private ListBox GetTaskListFromButton(object sender)
        {
            Image element = (Image)sender;
            StackPanel controls = (StackPanel)element.Parent;
            Border controlsBorder = (Border)controls.Parent;
            StackPanel competitor = (StackPanel)controlsBorder.Parent;
            ListBox tasksList = competitor.Children.OfType<ListBox>().FirstOrDefault();
            return tasksList;
        }
        private void DeleteTask_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
        }
        private void RunFirstTask_Click(object sender, RoutedEventArgs e)
        {
            if (Competitors.Children.Count == 0)
            {
                MessageBox.Show("Участники отсутствуют! Добавьте участников и их задачи!", "Участники отсутствуют");
            }
            else
            {

            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}