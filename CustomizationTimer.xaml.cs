using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace Timer
{
    /// <summary>
    /// Логика взаимодействия для CustomizationTimer.xaml
    /// </summary>
    public partial class CustomizationTimer : Window
    {
        public DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public List<IUpdater> stopwatchesAndTimers = new List<IUpdater>();
        public List<CompetitorControl> competitors  = new List<CompetitorControl>();
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
                CompetitorControl competitorControl = new CompetitorControl(dispatcherTimer, stopwatchesAndTimers, addCompetitorModal.CompetitorNameTextBox.Text);
                Competitors.Children.Add(competitorControl);
                competitors.Add(competitorControl);
            }
        }
        private void DeletePeople_Click(object sender, RoutedEventArgs e)
        {
            DeleteCompetitorModal deleteCompetitorModal = new DeleteCompetitorModal(competitors, Competitors);
            deleteCompetitorModal.ShowDialog();
        }
        private void RunFirstTask_Click(object sender, RoutedEventArgs e)
        {
            if (Competitors.Children.Count == 0)
            {
                MessageBox.Show("Участники отсутствуют! Добавьте участников и их задачи!", "Участники отсутствуют");
            }
            else
            {
                foreach (CompetitorControl competitor in competitors)
                {
                    competitor.RunFirstTask();
                }
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}