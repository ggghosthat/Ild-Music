namespace Ild_Music.Core.Services.PagedList;

public class MetaData
{
    public int CurrentPage {get; set;}
    public int PageSize {get; set;}
    public int TotalPages => (int)Math.Floor((decimal)(TotalCount / PageSize));
    public int TotalCount {get; set;}

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
}
