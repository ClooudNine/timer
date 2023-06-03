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
        private DispatcherTimer DispatcherTimer = new DispatcherTimer();
        public List<IStopwatchAndTimer> StopwatchesAndTimers = new List<IStopwatchAndTimer>();
        public CustomizationTimer()
        {
            InitializeComponent();
            DispatcherTimer.Tick += DispatcherTimer_Tick;
            DispatcherTimer.Interval = TimeSpan.FromSeconds(1);
        }
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            foreach(IStopwatchAndTimer stopwatch in StopwatchesAndTimers.ToArray())
            {
                stopwatch.UpdateStat();
            }
        }
        private void AddPeople_Click(object sender, RoutedEventArgs e)
        {
            AddCompetitorModal addCompetitorModal = new AddCompetitorModal();
            if (addCompetitorModal.ShowDialog() == true)
            {
                StackPanel competitorPanel = new StackPanel();

                StackPanel controlsPanel = new StackPanel { 
                    Orientation = Orientation.Horizontal,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                Border controlsBorder = new Border
                {
                    Child = controlsPanel,
                    BorderBrush = Brushes.White,
                    BorderThickness = new Thickness(0, 4, 0, 4)
                };

                Image addTask = new Image { 
                    Source = new BitmapImage(new Uri("Assets/addTask.png", UriKind.Relative)),
                    ToolTip = new ToolTip { Content = "Добавить задачу" }
                };
                addTask.MouseLeftButtonDown += AddTask_MouseLeftButtonDown;
                controlsPanel.Children.Add(addTask);

                Image deleteTask = new Image {
                    Source = new BitmapImage(new Uri("Assets/deleteTask.png", UriKind.Relative)),
                    ToolTip = new ToolTip { Content = "Удалить задачу" }
                };
                deleteTask.MouseLeftButtonDown += DeleteTask_MouseLeftButtonDown;
                controlsPanel.Children.Add(deleteTask);

                ListBox tasksList = new ListBox
                {
                    BorderBrush = Brushes.Transparent,
                    Background = Brushes.Transparent
                };
                

                competitorPanel.Children.Add(controlsBorder);
                competitorPanel.Children.Add(tasksList);

                Expander expander = new Expander
                {
                    Header = new TextBlock { 
                        Text = addCompetitorModal.CaptionCompetitorTextBox.Text,
                        FontSize = 20,
                        Foreground = Brushes.White
                    },
                    MaxHeight = 300,
                    Content = competitorPanel,
                    IsExpanded = true
                };

                Border expanderBorder = new Border
                {
                    Child = expander,
                    BorderBrush = Brushes.White,
                    BorderThickness = new Thickness(5),
                    CornerRadius = new CornerRadius(20),
                    Margin = new Thickness(20, 20, 0, 0)
                };
                Competitors.Children.Add(expanderBorder);
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
        private void AddTask_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AddCompetitorTask addCompetitorTask = new AddCompetitorTask();
            if (addCompetitorTask.ShowDialog() == true)
            {
                ListBox tasksList = GetTaskListFromButton(sender);
                if(addCompetitorTask.IsStopwatchRadio.IsChecked == true)
                {
                    StopwatchControl stopwatchControl = new StopwatchControl(DispatcherTimer, StopwatchesAndTimers, addCompetitorTask.TaskNameTextBox.Text);
                    tasksList.Items.Add(stopwatchControl);
                } 
                else
                {
                    int fullSeconds = int.Parse(addCompetitorTask.HoursTextBox.Text) * 3600 +
                                      int.Parse(addCompetitorTask.MinutesTextBox.Text) * 60 +
                                      int.Parse(addCompetitorTask.SecondsTextBox.Text);
                    TimerControl timerControl = new TimerControl(DispatcherTimer, StopwatchesAndTimers, addCompetitorTask.TaskNameTextBox.Text, fullSeconds);
                    tasksList.Items.Add(timerControl);
                }
            }
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