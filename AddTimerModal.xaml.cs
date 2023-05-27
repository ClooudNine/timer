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
    /// Логика взаимодействия для AddTimerModal.xaml
    /// </summary>
    public partial class AddTimerModal : Window
    {
        public AddTimerModal()
        {
            InitializeComponent();
        }
        private void Close_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if(EnterCaptionTimer.Text.Length < 3)
            {
                MessageBox.Show("Длина названия таймера не может быть меньше 3!");
            } else
            {
                CustomizationTimer customizationTimer = new CustomizationTimer();
                DialogResult = true;
                customizationTimer.Show();
                customizationTimer.CaptionTimer.Content = "Таймер " + EnterCaptionTimer.Text;
                Close();
            }
        }
    }
}
