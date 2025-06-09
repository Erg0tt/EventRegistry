using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace EventRegistrationApp
{
    public partial class ParticipantWindow : Window
    {
        string connectionString = "Data Source=DESKTOP-UFRBR18;Initial Catalog=EventRegistryDB;Integrated Security=True;";

        private void LoadParticipants()
        {
            Participants.Clear();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Id, Name, Email, IsConfirmed FROM Participants", con);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Participants.Add(new Participant
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),
                            IsConfirmed = reader.GetBoolean(3)
                        });
                    }
                }
            }
        }
        public ObservableCollection<Participant> Participants { get; set; } = new ObservableCollection<Participant>();

        public ParticipantWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadParticipants();
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


        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) || string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                MessageBox.Show("Имя и Email обязательны для заполнения.");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Participants (Name, Email, IsConfirmed) VALUES (@Name, @Email, @IsConfirmed)", con);
                cmd.Parameters.AddWithValue("@Name", NameTextBox.Text);
                cmd.Parameters.AddWithValue("@Email", EmailTextBox.Text);
                cmd.Parameters.AddWithValue("@IsConfirmed", ConfirmCheckBox.IsChecked == true);
                cmd.ExecuteNonQuery();
            }

            Participants.Add(new Participant
            {
                Name = NameTextBox.Text,
                Email = EmailTextBox.Text,
                IsConfirmed = ConfirmCheckBox.IsChecked == true
            });
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void DeleteParticipant_Click(object sender, RoutedEventArgs e)
        {
            var selected = (Participant)ParticipantsListBox.SelectedItem;
            if (selected == null)
            {
                MessageBox.Show("Выберите участника для удаления.");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Participants WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", selected.Id);
                cmd.ExecuteNonQuery();
            }

            Participants.Remove(selected);
        }
    }
}