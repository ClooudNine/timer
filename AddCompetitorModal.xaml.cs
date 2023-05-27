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
    /// Логика взаимодействия для AddCompetitorModal.xaml
    /// </summary>
    public partial class AddCompetitorModal : Window
    {
        public AddCompetitorModal()
        {
            InitializeComponent();
        }

        private void AddCompetitor_Click(object sender, RoutedEventArgs e)
        {
            if(CaptionCompetitorTextBox.Text.Length == 0)
            {
                MessageBox.Show("Наименование участника не может быть пустым!");
            } else
            {
                DialogResult = true;
            }
        }
    }
}
