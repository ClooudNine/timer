using System;
using System.Collections.Generic;
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
        private DispatcherTimer DispatcherTimer = new DispatcherTimer();
        private List<Tuple<Stopwatch, TextBlock>> stopwatches = new List<Tuple<Stopwatch, TextBlock>>();
        private List<Tuple<Stopwatch, TextBlock, ProgressBar>> timers = new List<Tuple<Stopwatch, TextBlock, ProgressBar>>();
        public CustomizationTimer()
        {
            InitializeComponent();
            DispatcherTimer.Tick += Dt_Tick;
            DispatcherTimer.Interval = new TimeSpan(0, 0, 1);
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
            ListBox tasksList = GetTaskListFromButton(sender);
            if(tasksList.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить эту задачу?", "Удаление задачи", MessageBoxButton.YesNo);
                if(result == MessageBoxResult.Yes) 
                {
                    StackPanel task = (StackPanel)tasksList.SelectedItem;
                    TextBlock stopwatchStat = (TextBlock)task.Children[1];
                    if (stopwatchStat.Tag != null) {
                        Stopwatch sw = (Stopwatch)stopwatchStat.Tag;
                        if (sw.IsRunning == true)
                        {
                            foreach (var stopwatch in stopwatches)
                            {
                                if (stopwatch.Item1 == sw)
                                {
                                    stopwatches.Remove(stopwatch);
                                    break;
                                }
                            }
                            if (stopwatches.Count == 0)
                            {
                                DispatcherTimer.Stop();
                            }
                        }
                        sw.Stop();
                    }
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
                TextBlock taskCaption = new TextBlock 
                {
                    Text = addCompetitorTask.TaskNameTextBox.Text,
                    FontFamily = new FontFamily("Cascadia Mono SemiBold"),
                    Foreground = Brushes.White,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 16,
                    Margin = new Thickness(20, 0, 0, 0)
                };
                task.Children.Add(taskCaption);

                TextBlock stopwatch = new TextBlock
                {
                    Text = addCompetitorTask.IsStopwatchRadio.IsChecked == true ? "00:00:00" :
                           string.Format("{0:00}:{1:00}:{2:00}", addCompetitorTask.HoursTextBox.Text,
                                                                 addCompetitorTask.MinutesTextBox.Text,
                                                                 addCompetitorTask.SecondsTextBox.Text),
                    FontFamily = new FontFamily("Cascadia Mono SemiBold"),
                    Foreground = Brushes.White,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 16,
                    Margin = new Thickness(30, 0, 0, 0)
                };
                task.Children.Add(stopwatch);

                Image startStopwatchImage = new Image
                {
                    Source = new BitmapImage(new Uri("Assets/start.png", UriKind.Relative)),
                    ToolTip = new ToolTip { Content = "Старт" }
                };

                Button startStopwatch = new Button
                {
                    Content = startStopwatchImage,
                    BorderBrush = Brushes.Transparent,
                    Background = Brushes.Transparent
                };

                Image pauseStopwatchImage = new Image
                {
                    Source = new BitmapImage(new Uri("Assets/pause.png", UriKind.Relative)),
                    ToolTip = new ToolTip { Content = "Пауза" }
                };

                Button stopStopwatch = new Button 
                {   
                    Content = pauseStopwatchImage,
                    BorderBrush = Brushes.Transparent,
                    Background = Brushes.Transparent,
                    IsEnabled = false 
                };

                Image resetStopwatchImage = new Image
                {
                    Source = new BitmapImage(new Uri("Assets/reset.png", UriKind.Relative)),
                    ToolTip = new ToolTip { Content = "Сброс" }
                };

                Button resetStopwatch = new Button {
                    Content = resetStopwatchImage,
                    BorderBrush = Brushes.Transparent,
                    Background = Brushes.Transparent,
                    IsEnabled = false };

                if (addCompetitorTask.IsTimerRadio.IsChecked == true)
                {
                    int fullSeconds = int.Parse(addCompetitorTask.HoursTextBox.Text) * 3600 +
                        int.Parse(addCompetitorTask.MinutesTextBox.Text) * 60 +
                        int.Parse(addCompetitorTask.SecondsTextBox.Text);
                    ProgressBar timerProgress = new ProgressBar
                    {
                        Minimum = 0,
                        Maximum = fullSeconds,
                        Value = 0,
                        Width = 150,
                        Height = 20
                    };
                    StackPanel competitorPanel = (StackPanel)tasksList.Parent;
                    Expander competitorExapnder = (Expander)competitorPanel.Parent;
                    TextBlock competitorName = (TextBlock)competitorExapnder.Header;
                    stopwatch.Tag = new Tuple<Stopwatch, int, string, string> ( null, fullSeconds, addCompetitorTask.TaskNameTextBox.Text, competitorName.Text);
                    task.Children.Add(timerProgress);
                }
                startStopwatch.Click += StartStopwatch_Click;
                stopStopwatch.Click += PauseStopwatch_Click;
                resetStopwatch.Click += ResetStopwatch_Click;
                task.Children.Add(startStopwatch);
                task.Children.Add(stopStopwatch);
                task.Children.Add(resetStopwatch);
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
            StackPanel task = (StackPanel)stopButton.Parent;
            TextBlock stopwatchStat = (TextBlock)task.Children[1];
            Tuple<Stopwatch, int, string, string> timerInfo = null;
            Button startButton = null;
            Stopwatch sw = null;
            if(task.Children.Count == 6)
            {
                startButton = (Button)task.Children[3];
                timerInfo = (Tuple<Stopwatch, int, string, string>)stopwatchStat.Tag;
                sw = timerInfo.Item1;
                foreach (var timer in timers)
                {
                    if (timer.Item1 == sw)
                    {
                        timers.Remove(timer);
                        break;
                    }
                }
            } 
            else
            {
                startButton = (Button)task.Children[2];
                sw = (Stopwatch)stopwatchStat.Tag;
                foreach (var stopwatch in stopwatches)
                {
                    if (stopwatch.Item1 == sw)
                    {
                        stopwatches.Remove(stopwatch);
                        break;
                    }
                }
            }
            if (stopwatches.Count == 0 && timers.Count == 0)
            {
                DispatcherTimer.Stop();
            }
            sw.Stop();
            stopButton.IsEnabled = false;
            if(isReset)
            {
                Button otherButton = null;
                if(task.Children.Count == 6)
                {
                    stopwatchStat.Tag = new Tuple<Stopwatch, int, string, string>(null, timerInfo.Item2, timerInfo.Item3, timerInfo.Item4);
                    int hours = timerInfo.Item2 / 3600;
                    int minutes = (timerInfo.Item2 - hours * 3600) / 60;
                    int seconds = (timerInfo.Item2 - hours * 3600) - minutes * 60;
                    stopwatchStat.Text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
                    otherButton = (Button)task.Children[4];
                    ProgressBar timerProgress = (ProgressBar)task.Children[2];
                    timerProgress.Value = 0;
                } 
                else
                {
                    stopwatchStat.Tag = null;
                    stopwatchStat.Text = "00:00:00";
                    otherButton = (Button)task.Children[3];
                }
                otherButton.IsEnabled = false;
            }
            startButton.IsEnabled = true;
        }

        private void StartStopwatch_Click(object sender, RoutedEventArgs e)
        {
            Button startButton = (Button)sender;
            StackPanel task = (StackPanel)VisualTreeHelper.GetParent(startButton);
            TextBlock stopwatchStat = (TextBlock)task.Children[1];
            Button pauseButton = (Button)task.Children[3];
            Button resetButton = (Button)task.Children[4];
            Tuple<Stopwatch, int, string, string> timerInfo = new Tuple<Stopwatch, int, string, string>(null, 0, null, null);
            ProgressBar timerProgress = null;
            if (task.Children.Count == 6)
            {
                pauseButton = (Button)task.Children[4];
                resetButton = (Button)task.Children[5];
                timerProgress = (ProgressBar)task.Children[2];
                timerInfo = (Tuple<Stopwatch, int, string, string>)stopwatchStat.Tag;
            }
            Stopwatch sw = new Stopwatch();
            if (stopwatchStat.Tag != null && timerInfo.Item1 != null)
            {
                if(task.Children.Count == 6)
                {
                    sw = timerInfo.Item1;
                } 
                else
                {
                    sw = (Stopwatch)stopwatchStat.Tag;
                }
            }
            else
            {
                if (task.Children.Count == 6)
                {
                    stopwatchStat.Tag = new Tuple<Stopwatch, int, string, string>(sw, timerInfo.Item2, timerInfo.Item3, timerInfo.Item4);
                }
                else
                {
                    stopwatchStat.Tag = sw;
                }
            }
            if(task.Children.Count == 6)
            {
                timers.Add(new Tuple<Stopwatch, TextBlock, ProgressBar>(sw, stopwatchStat, timerProgress));
            } 
            else
            {
                stopwatches.Add(new Tuple<Stopwatch, TextBlock>(sw, stopwatchStat));
            }
            sw.Start();
            if(DispatcherTimer.IsEnabled == false)
            {
                DispatcherTimer.Start();
            }
            startButton.IsEnabled = false;
            pauseButton.IsEnabled = true;
            resetButton.IsEnabled = true;
        }

        private void Dt_Tick(object sender, EventArgs e)
        {
            foreach (var stopwatch in stopwatches)
            {
                TimeSpan ts = stopwatch.Item1.Elapsed;
                stopwatch.Item2.Text = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
            }
            foreach (var timer in timers.ToList())
            {
                TimeSpan ts = timer.Item1.Elapsed;
                Tuple<Stopwatch, int, string, string> timerInfo = (Tuple<Stopwatch, int, string, string>)timer.Item2.Tag;
                int elapseSeconds = ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds;
                int remainingFullSeconds = timerInfo.Item2 - elapseSeconds;
                int remainingHours = remainingFullSeconds / 3600;
                int remainingMinutes = (remainingFullSeconds - remainingHours * 3600) / 60;
                int remainingSeconds = (remainingFullSeconds - remainingHours * 3600) - remainingMinutes * 60;
                timer.Item2.Text = string.Format("{0:00}:{1:00}:{2:00}", remainingHours, remainingMinutes, remainingSeconds);
                timer.Item3.Value = elapseSeconds;
                if (elapseSeconds == timerInfo.Item2)
                {
                    MessageBox.Show("Время на задание " + timerInfo.Item3 + " у участника " + timerInfo.Item4 + " вышло!");
                    timer.Item1.Stop();
                    timers.Remove(timer);
                    continue;
                }
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void RunFirstTask_Click(object sender, RoutedEventArgs e)
        {
            if(Competitors.Children.Count == 0)
            {
                MessageBox.Show("Участники отсутствуют! Добавьте участников и их задачи!", "Участники отсутствуют");
            } 
            else
            {

            }
        }
    }
}