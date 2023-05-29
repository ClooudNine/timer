using System;
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
                    HorizontalAlignment = HorizontalAlignment.Center};

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
                controlsPanel.Children.Add(deleteTask);

                ScrollViewer scrollViewerForTasks = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
                ListBox tasksList = new ListBox
                {
                    BorderBrush = Brushes.White,
                    BorderThickness = new Thickness(5)
                };
                scrollViewerForTasks.Content = tasksList;
                competitorPanel.Children.Add(controlsPanel);
                competitorPanel.Children.Add(scrollViewerForTasks);
                Expander expander = new Expander
                {
                    Header = new Label { 
                        Content = addCompetitorModal.CaptionCompetitorTextBox.Text,
                        FontSize = 16,
                        Foreground = Brushes.Wheat
                    },
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
        private void AddTask_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AddCompetitorTask addCompetitorTask = new AddCompetitorTask();
            UIElement element = (UIElement)sender;
            StackPanel controls = (StackPanel)VisualTreeHelper.GetParent(element);
            StackPanel competitor = (StackPanel)VisualTreeHelper.GetParent(controls);
            ScrollViewer scrollViewerForTasks = competitor.Children.OfType<ScrollViewer>().FirstOrDefault();
            ListBox tasksList = (ListBox)scrollViewerForTasks.Content;
            if (addCompetitorTask.ShowDialog() == true)
            {
                StackPanel task = new StackPanel { Orientation = Orientation.Horizontal };
                Label taskCaption = new Label { Content = addCompetitorTask.TaskNameTextBox.Text };
                task.Children.Add(taskCaption);
                if(addCompetitorTask.IsStopwatchRadio.IsChecked == true)
                {
                    TextBlock stopwatch = new TextBlock { Text = "00:00:00", Name = addCompetitorTask.TaskNameTextBox.Text };
                    Button startStopwatch = new Button { Content = "Старт!" };
                    Button stopStopwatch = new Button { Content = "Стоп!" };
                    startStopwatch.Click += StartStopwatch_Click;
                    stopStopwatch.Click += StopStopwatch_Click;
                    task.Children.Add(stopwatch);
                    task.Children.Add(startStopwatch);
                    task.Children.Add(stopStopwatch);
                } else if (addCompetitorTask.IsTimerRadio.IsChecked == true)
                {

                } else
                {
                    MessageBox.Show("Выберите тип таймера!");
                } 
                tasksList.Items.Add(task);
            }
        }

        private void StopStopwatch_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void StartStopwatch_Click(object sender, RoutedEventArgs e)
        {
            DispatcherTimer dt = new DispatcherTimer();
            dt.Tick += Dt_Tick;
            dt.Interval = new TimeSpan(0, 0, 1);
            dt.Start();
        }

        private void Dt_Tick(object sender, EventArgs e)
        {
            UIElement element = (UIElement)sender;
            StackPanel task = (StackPanel)VisualTreeHelper.GetParent(element);
            TextBlock stopwatchStat = task.Children.OfType<TextBlock>().FirstOrDefault();
            stopwatchStat.Text = "TimerWork";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
