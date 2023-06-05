using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Timer
{
    /// <summary>
    /// Логика взаимодействия для DeleteCompetitorModal.xaml
    /// </summary>
    public partial class DeleteCompetitorModal : Window
    {
        List<CompetitorControl> competitors;
        WrapPanel competitorsPanel;
        public DeleteCompetitorModal(List<CompetitorControl> competitors, WrapPanel competitorsPanel)
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
                competitors.Remove(CompetitorsListBox.SelectedItem as CompetitorControl);
                competitorsPanel.Children.Remove(CompetitorsListBox.SelectedItem as CompetitorControl);
                CompetitorsListBox.Items.Refresh();
            }
        }

        private void CloseImage_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DialogResult = true;
        }
    }
}
