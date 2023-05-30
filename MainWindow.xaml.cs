using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;

namespace Timer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddTimer_Click(object sender, RoutedEventArgs e)
        {
            AddTimerModal addTimerModal = new AddTimerModal();
            if(addTimerModal.ShowDialog() == true)
            {
                Close();
            }
        }
        private void ExitProgram_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(File.Exists("SavedTimers.xml"))
            {

            } 
            else
            {
                TextBlock notSavedTimers = new TextBlock {
                    FontFamily = new FontFamily("Cascadia Mono SemiBold"),
                    FontSize = 36,
                    Foreground = Brushes.Gray,
                    Margin = new Thickness(0, 350, 0, 0),
                    Text = "Вы ещё не сохранили ни одного таймера!" };
                SavedTimers.HorizontalAlignment = HorizontalAlignment.Center;
                SavedTimers.VerticalAlignment = VerticalAlignment.Center;
                SavedTimers.Children.Add(notSavedTimers);
            }
        }
    }
}
