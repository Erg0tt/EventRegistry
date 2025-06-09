using System.Windows;
using System.Windows.Input;

namespace EventRegistrationApp
{
    public partial class MainWindow : Window
    {
        string connectionString = "Data Source=DESKTOP-UFRBR18;Initial Catalog=EventRegistryDB;Integrated Security=True;";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OrganizerButton_Click(object sender, RoutedEventArgs e)
        {
            new OrganizerWindow().Show();
            Close();
        }

        private void ParticipantButton_Click(object sender, RoutedEventArgs e)
        {
            new ParticipantWindow().Show();
            Close();
        }

        private void RegistrarButton_Click(object sender, RoutedEventArgs e)
        {
            new RegistrarWindow().Show();
            Close();
        }
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}