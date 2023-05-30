using System;
using System.Diagnostics;
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
        public CustomizationTimer()
        {
            InitializeComponent();
        }

        private void AddPeople_Click(object sender, RoutedEventArgs e)
        {
            AddCompetitorModal addCompetitorModal = new AddCompetitorModal();
            if (addCompetitorModal.ShowDialog() == true)
            {
                StackPanel competitorPanel = new StackPanel();
                Border stackPanelBorder = new Border
                {
                    Child = competitorPanel,
                    BorderBrush = Brushes.White,
                    BorderThickness = new Thickness(0, 5, 0, 0)
                };

                StackPanel controlsPanel = new StackPanel { 
                    Orientation = Orientation.Horizontal,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                Image addTask = new Image { 
                    Source = new BitmapImage(new Uri("Assets/addTask.png", UriKind.Relative)),
                    Width = 40,
                    Height = 40,
                    ToolTip = new ToolTip { Content = "Добавить задачу" }
                };
                addTask.MouseLeftButtonDown += AddTask_MouseLeftButtonDown;
                controlsPanel.Children.Add(addTask);

                Image deleteTask = new Image {
                    Source = new BitmapImage(new Uri("Assets/deleteTask.png", UriKind.Relative)),
                    Width = 40,
                    Height = 40,
                    ToolTip = new ToolTip { Content = "Удалить задачу" }
                };
                deleteTask.MouseLeftButtonDown += DeleteTask_MouseLeftButtonDown;
                controlsPanel.Children.Add(deleteTask);

                ListBox tasksList = new ListBox
                {
                    BorderBrush = Brushes.White,
                    BorderThickness = new Thickness(5)
                };
                competitorPanel.Children.Add(controlsPanel);
                competitorPanel.Children.Add(tasksList);

                Expander expander = new Expander
                {
                    Header = new TextBlock { 
                        Text = addCompetitorModal.CaptionCompetitorTextBox.Text,
                        FontSize = 16,
                        Foreground = Brushes.White
                    },
                    MaxHeight = 300,
                    Content = stackPanelBorder,
                    IsExpanded = true
                };

                Border expanderBorder = new Border
                {
                    Child = expander,
                    BorderBrush = Brushes.White,
                    BorderThickness = new Thickness(5),
                    CornerRadius = new CornerRadius(20),
                    Margin = new Thickness(0, 20, 0, 0)
                };
                Competitors.Children.Add(expanderBorder);
            }
        }

        private ListBox GetTaskListFromButton(object sender)
        {
            UIElement element = (UIElement)sender;
            StackPanel controls = (StackPanel)VisualTreeHelper.GetParent(element);
            StackPanel competitor = (StackPanel)VisualTreeHelper.GetParent(controls);
            ListBox tasksList = competitor.Children.OfType<ListBox>().FirstOrDefault();
            return tasksList;
        }

        private void DeleteTask_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBox tasksList = GetTaskListFromButton(sender);
            if(tasksList.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить эту задачу?", "Удаление задачи", MessageBoxButton.YesNo);
                if(result == MessageBoxResult.Yes) 
                {
                    tasksList.Items.Remove(tasksList.SelectedItem);
                }
            } else
            {
                MessageBox.Show("Выделите задачу для удаления!");
            }
        }

        private void AddTask_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AddCompetitorTask addCompetitorTask = new AddCompetitorTask();
            if (addCompetitorTask.ShowDialog() == true)
            {
                ListBox tasksList = GetTaskListFromButton(sender);
                StackPanel task = new StackPanel { Orientation = Orientation.Horizontal };
                TextBlock taskCaption = new TextBlock { Text = addCompetitorTask.TaskNameTextBox.Text };
                task.Children.Add(taskCaption);
                if(addCompetitorTask.IsStopwatchRadio.IsChecked == true)
                {
                    TextBlock stopwatch = new TextBlock { Text = "00:00:00" };
                    Button startStopwatch = new Button { Content = "Старт" };
                    Button stopStopwatch = new Button { Content = "Пауза", IsEnabled = false };
                    Button resetStopwatch = new Button { Content = "Сброс", IsEnabled = false };
                    startStopwatch.Click += StartStopwatch_Click;
                    stopStopwatch.Click += PauseStopwatch_Click;
                    resetStopwatch.Click += ResetStopwatch_Click;
                    task.Children.Add(stopwatch);
                    task.Children.Add(startStopwatch);
                    task.Children.Add(stopStopwatch);
                    task.Children.Add(resetStopwatch);
                } else 
                {

                }
                tasksList.Items.Add(task);
            }
        }

        private void ResetStopwatch_Click(object sender, RoutedEventArgs e)
        {
            StopStopwatch(sender, true);
        }

        private void PauseStopwatch_Click(object sender, RoutedEventArgs e)
        {
            StopStopwatch(sender, false);
        }

        private void StopStopwatch(object sender, bool isReset)
        {
            Button stopButton = (Button)sender;
            StackPanel task = (StackPanel)VisualTreeHelper.GetParent(stopButton);
            TextBlock stopwatchStat = (TextBlock)task.Children[1];
            Button startButton = (Button)task.Children[2];
            object[] relatedElements = (object[])stopwatchStat.Tag;
            DispatcherTimer dt = (DispatcherTimer)relatedElements[0];
            Stopwatch sw = (Stopwatch)relatedElements[1];
            dt.Stop();
            sw.Stop();
            stopButton.IsEnabled = false;
            startButton.IsEnabled = true;
            if(isReset)
            {
                Button pauseButton = (Button)task.Children[3];
                stopwatchStat.Tag = null;
                stopwatchStat.Text = "00:00:00";
                pauseButton.IsEnabled = false;
            }
        }

        private void StartStopwatch_Click(object sender, RoutedEventArgs e)
        {
            Button startButton = (Button)sender;
            StackPanel task = (StackPanel)VisualTreeHelper.GetParent(startButton);
            TextBlock stopwatchStat = (TextBlock)task.Children[1];
            Button pauseButton = (Button)task.Children[3];
            Button resetButton = (Button)task.Children[4];
            Stopwatch sw = new Stopwatch();
            object[] relatedElements = (object[])stopwatchStat.Tag;
            if (relatedElements != null)
            {
                sw = (Stopwatch)relatedElements[1];
            }
            DispatcherTimer dt = new DispatcherTimer
            {
                Tag = new object[] {sw, stopwatchStat}
            };
            stopwatchStat.Tag = new object[] { dt, sw };
            dt.Tick += Dt_Tick;
            dt.Interval = new TimeSpan(0, 0, 1);
            sw.Start();
            dt.Start();
            startButton.IsEnabled = false;
            pauseButton.IsEnabled = true;
            resetButton.IsEnabled = true;
        }

        private void Dt_Tick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            object[] relatedElements = (object[])timer.Tag;
            TextBlock stopwatch = (TextBlock)relatedElements[1];
            Stopwatch sw = (Stopwatch)relatedElements[0];
            TimeSpan ts = sw.Elapsed;
            stopwatch.Text = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
