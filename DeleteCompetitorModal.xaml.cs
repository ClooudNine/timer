using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Timer
{
    /// <summary>
    /// Логика взаимодействия для DeleteCompetitorModal.xaml
    /// </summary>
    public partial class DeleteCompetitorModal : Window
    {
        ObservableCollection<CompetitorControl> competitors;
        WrapPanel competitorsPanel;
        public DeleteCompetitorModal(ObservableCollection<CompetitorControl> competitors, WrapPanel competitorsPanel)
        {
            InitializeComponent();
            this.competitors = competitors;
            this.competitorsPanel = competitorsPanel;
            CompetitorsListBox.ItemsSource = competitors;
            CompetitorsListBox.DisplayMemberPath = "competitorName";
        }

        private void DeleteCompetitorButton_Click(object sender, RoutedEventArgs e)
        {
            if(CompetitorsListBox.SelectedItem != null)
            {
                competitorsPanel.Children.Remove(CompetitorsListBox.SelectedItem as CompetitorControl);
                competitors.Remove(CompetitorsListBox.SelectedItem as CompetitorControl);
            }
        }

        private void CloseImage_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DialogResult = true;
        }
    }
}
