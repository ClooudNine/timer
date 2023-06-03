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
        private void CloseImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DialogResult = false;
        }
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if(EnterCaptionTimer.Text.Length < 3)
            {
                MessageBox.Show("Длина названия таймера не может быть меньше 3!");
            } else
            {
                DialogResult = true;
                CustomizationTimer customizationTimer = new CustomizationTimer();
                customizationTimer.Show();
                customizationTimer.CaptionTimer.Content = "Таймер " + EnterCaptionTimer.Text;
            }
        }
    }
}
