using EventRegistrationApp;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace EventRegistrationApp
{
    public partial class OrganizerWindow : Window
    {
        string connectionString = "Data Source=DESKTOP-UFRBR18;Initial Catalog=EventRegistryDB;Integrated Security=True;";
        private void LoadEvents()
        {
            Events.Clear();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Title, Date, Format FROM Events", con);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Events.Add(new Event
                        {
                            Title = reader.GetString(0),
                            Date = reader.GetDateTime(1),
                            Format = reader.GetString(2)
                        });
                    }
                }
            }
        }
        public ObservableCollection<Event> Events { get; set; } = new ObservableCollection<Event>();

        public OrganizerWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadEvents();
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


        private void AddEventButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text) || EventDatePicker.SelectedDate == null || string.IsNullOrWhiteSpace(FormatComboBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Events (Title, Date, Format) VALUES (@Title, @Date, @Format)", con);
                cmd.Parameters.AddWithValue("@Title", TitleTextBox.Text);
                cmd.Parameters.AddWithValue("@Date", EventDatePicker.SelectedDate ?? DateTime.Today);
                cmd.Parameters.AddWithValue("@Format", FormatComboBox.Text);
                cmd.ExecuteNonQuery();
            }

            Events.Add(new Event
            {
                Title = TitleTextBox.Text,
                Date = EventDatePicker.SelectedDate ?? DateTime.Today,
                Format = FormatComboBox.Text
            });
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void DeleteEvent_Click(object sender, RoutedEventArgs e)
        {
            var selected = (Event)EventsListBox.SelectedItem;
            if (selected == null)
            {
                MessageBox.Show("Выберите мероприятие для удаления.");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Events WHERE Title = @Title AND Date = @Date AND Format = @Format", con);
                cmd.Parameters.AddWithValue("@Title", selected.Title);
                cmd.Parameters.AddWithValue("@Date", selected.Date);
                cmd.Parameters.AddWithValue("@Format", selected.Format);
                cmd.ExecuteNonQuery();
            }

            Events.Remove(selected);
        }
    }
}
