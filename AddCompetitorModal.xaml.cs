using System.Windows;

namespace Timer
{
    /// <summary>
    /// Логика взаимодействия для AddCompetitorModal.xaml
    /// </summary>
    public partial class AddCompetitorModal : Window
    {
        public AddCompetitorModal()
        {
            InitializeComponent();
        }

        private void AddCompetitorButton_Click(object sender, RoutedEventArgs e)
        {
            if(CompetitorNameTextBox.Text.Length == 0)
            {
                MessageBox.Show("Наименование участника не может быть пустым!", "Пустое имя участника!");
            } else
            {
                DialogResult = true;
            }
        }

        private void CloseModalButton_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DialogResult = false;
        }
    }
}
