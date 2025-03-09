namespace PaperGate.Shared.ReturnTypes;
public class TaskResult
{
    #region Constructor
    public TaskResult() => Errors = [];
    #endregion

    #region Properties
    public bool Succeeded { get; set; }
    public List<string> Errors { get; set; }
    public object? Result { get; set; }

    #endregion
    #region Methods
    public void AddError(string message) => Errors.Add(message);
    #endregion
}