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
using System.Windows.Shapes;

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
                StackPanel competitorPanel = new StackPanel { Width = 500, Height = 300};
                Border stackPanelBorder = new Border
                {
                    Child = competitorPanel,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(5),
                    CornerRadius = new CornerRadius(20)
                };

                StackPanel controlsPanel = new StackPanel { Orientation = Orientation.Horizontal };

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
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(5)
                };
                scrollViewerForTasks.Content = tasksList;
                competitorPanel.Children.Add(controlsPanel);
                competitorPanel.Children.Add(scrollViewerForTasks);
                Expander expander = new Expander
                {
                    Header = addCompetitorModal.CaptionCompetitorTextBox.Text,
                    Content = stackPanelBorder,
                    IsExpanded = true,
                    Margin = new Thickness(20, 0, 0, 0)
                };
                Competitors.Children.Add(expander);
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
                Label taskCaption = new Label { Content = addCompetitorTask.TaskNameTextBox.Text };
                tasksList.Items.Add(taskCaption);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
