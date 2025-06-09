using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace EventRegistrationApp
{
    public partial class RegistrarWindow : Window
    {
        string connectionString = "Data Source=DESKTOP-UFRBR18;Initial Catalog=EventRegistryDB;Integrated Security=True;";

        private void LoadAttendances()
        {
            Attendances.Clear();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"
            SELECT 
                a.Id, 
                p.Id, p.Name, p.Email, p.IsConfirmed, 
                e.Title, e.Date, e.Format, 
                a.IsPresent
            FROM Attendances a
            JOIN Participants p ON a.ParticipantId = p.Id
            JOIN Events e ON a.EventId = e.Id", con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Attendances.Add(new Attendance
                        {
                            Id = reader.GetInt32(0),
                            Participant = new Participant
                            {
                                Id = reader.GetInt32(1),
                                Name = reader.GetString(2),
                                Email = reader.GetString(3),
                                IsConfirmed = reader.GetBoolean(4)
                            },
                            Event = new Event
                            {
                                Title = reader.GetString(5),
                                Date = reader.GetDateTime(6),
                                Format = reader.GetString(7)
                            },
                            IsPresent = reader.GetBoolean(8)
                        });
                    }
                }
            }
        }
        public ObservableCollection<Attendance> Attendances { get; set; } = new ObservableCollection<Attendance>();

        public RegistrarWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadAttendances();
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


        private void MarkAttendanceButton_Click(object sender, RoutedEventArgs e)
        {
            int participantId = 1; 
            int eventId = 1;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Attendances (ParticipantId, EventId, IsPresent) VALUES (@Pid, @Eid, @Present)", con);
                cmd.Parameters.AddWithValue("@Pid", participantId);
                cmd.Parameters.AddWithValue("@Eid", eventId);
                cmd.Parameters.AddWithValue("@Present", true);
                cmd.ExecuteNonQuery();
            }

            Attendances.Add(new Attendance
            {
                Participant = new Participant { Name = "Иван", Email = "ivan@example.com" },
                Event = new Event { Title = "Семинар", Date = DateTime.Now, Format = "Офлайн" },
                IsPresent = true
            });
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void DeleteAttendance_Click(object sender, RoutedEventArgs e)
        {
            var selected = (Attendance)AttendanceListBox.SelectedItem;
            if (selected == null)
            {
                MessageBox.Show("Выберите запись для удаления.");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Attendances WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", selected.Id);
                cmd.ExecuteNonQuery();
            }

            Attendances.Remove(selected);
        }
    }
}