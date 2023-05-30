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
                MessageBox.Show("Выберите тип таймера!");
            } else
            {
                DialogResult = true;
            }
        }
    }
}
