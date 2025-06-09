using System;
public class Participant
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool IsConfirmed { get; set; }
    public override string ToString()
    {
        return $"{Name}                     ({Email})";
    }

}
