namespace Ild_Music.Core.Contracts.Plagination;

public abstract class PlaginationTag
{
    public abstract long PlaginationTagId { get; }
    
    public abstract string PlaginationTagName { get; }

    public abstract Guid SourcePlugin { get; }
}

