using System.Windows;
using System.Windows.Input;

namespace Timer
{
    /// <summary>
    /// Логика взаимодействия для AddCompetitorTask.xaml
    /// </summary>
    public partial class AddCompetitorTask : Window
    {
        public AddCompetitorTask()
        {
            InitializeComponent();
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if(IsStopwatchRadio.IsChecked == false && IsTimerRadio.IsChecked == false) 
            {
                MessageBox.Show("Выберите тип таймера!", "Не выбран тип таймера!");
            } else if (TaskNameTextBox.Text.Length == 0)
            {
                MessageBox.Show("Введите название задачи!", "Отсутствует название задачи!");
            } else
            {
                if(IsTimerRadio.IsChecked == true)
                {
                    if(int.TryParse(HoursTextBox.Text, out int hours) &&
                        int.TryParse(MinutesTextBox.Text, out int minutes) &&
                        int.TryParse(SecondsTextBox.Text, out int seconds))
                    {

                        if(hours < 0 || hours > 23)
                        {
                            MessageBox.Show("Часы должны находиться в диапазоне от 0 до 24!", "Неверный ввод данных!");
                            return;
                        }
                        if (minutes < 0 || minutes > 59)
                        {
                            MessageBox.Show("Минуты должны находиться в диапазоне от 0 до 59!", "Неверный ввод данных!");
                            return;
                        }
                        if (seconds < 0 || hours > 59)
                        {
                            MessageBox.Show("Секунды должны находиться в диапазоне от 0 до 59!", "Неверный ввод данных!");
                            return;
                        }
                        DialogResult = true;
                    } 
                    else
                    {
                        MessageBox.Show("Введите числовые данные!", "Неверный ввод данных!");
                    }
                } 
                else 
                {
                    DialogResult = true;
                }
            }
        }
        private void IsTimerRadio_Checked(object sender, RoutedEventArgs e)
        {
            SetTimer.Visibility = Visibility.Visible;
        }
        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DialogResult = false;
        }
        private void IsStopwatchRadio_Checked(object sender, RoutedEventArgs e)
        {
            SetTimer.Visibility = Visibility.Hidden;
        }
    }
}
