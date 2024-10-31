using System.Windows;

namespace CRMClientApp.Views
{
    public partial class EditOrderControlWindow : Window
    {
        public EditOrderControlWindow()
        {
            InitializeComponent();
        }

        private void SaveFieldValuesBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
