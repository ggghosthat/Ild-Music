namespace Ild_Music.Core.Exceptions.Flag;

public struct ErrorFlag
{
    public ErrorFlag(string handler, string step, string? message = null)
    {
        WarnTime = DateTime.Now;
        Handler = handler;
        Step = step;
        Message = message;
    }
    
    public DateTime WarnTime { get; }

    public string Handler {get; }

    public string Step { get; }

    public string? Message { get; }

    public override string ToString() => $"({WarnTime}) [{Handler}:{Step}] - {Message}";
}