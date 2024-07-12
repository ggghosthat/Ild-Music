namespace Ild_Music.Core.Instances.Pagging;

public class MetaData
{
    public int CurrentPage {get; set;}
    public int PageSize {get; set;}
    public int TotalPages => (int)Math.Floor((decimal)(TotalCount / PageSize));
    public int TotalCount {get; set;}
    public EntityTag EntityTag {get; set;}
    
    public bool Skip = false;
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage <= TotalPages + 1;
}
