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
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private void LoadTimer_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
