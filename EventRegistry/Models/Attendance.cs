using EventRegistrationApp;
using System;

public class Attendance
{
    public int Id { get; set; }
    public Participant Participant { get; set; }
    public Event Event { get; set; }
    public bool IsPresent { get; set; } 
    public override string ToString()
    {
        return $"{Participant?.Name}    -    {DateTime.Now:dd.MM.yyyy}";
    }

}
