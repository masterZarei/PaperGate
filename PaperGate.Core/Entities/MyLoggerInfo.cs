namespace PaperGate.Core.Entities;
public class MyLoggerInfo : BaseEntity
{
    public string Section { get; set; }
    public LoggingLevel Level { get; set; }
    public string Description { get; set; }
}
public enum LoggingLevel
{
    Information,
    Warning,
    Fatal
}