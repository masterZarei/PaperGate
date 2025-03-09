namespace PaperGate.Shared.ReturnTypes;
public class CDateTime
{
    public CDateTime() { }
    public CDateTime(int year, byte month, byte day, byte hour, byte minute, byte second)
    {
        Year = year;
        Month = month;
        Day = day;
        Hour = hour;
        Minute = minute;
        Second = second;
    }
    public int Year { get; set; }
    public byte Month { get; set; }
    public byte Day { get; set; }
    public byte Hour { get; set; }
    public byte Minute { get; set; }
    public byte Second { get; set; }
}