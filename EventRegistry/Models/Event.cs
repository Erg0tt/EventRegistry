using System;
namespace EventRegistrationApp
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Format { get; set; }

        public override string ToString()
        {
            return $"{Title}     |     {Date:dd.MM.yyyy}     |     {Format}";
        }
    }

}