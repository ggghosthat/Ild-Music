using Ild_Music.Core.Instances.DTO;

namespace Ild_Music.Core.Services.PagedList;

public class PagedList<T> : List<T>
{
    private IEnumerable<CommonInstanceDTO> _source;
    
    public PagedList( 
        MetaData metaData,
        IList<T> items = null) 
    {
        MetaData = metaData;

        if (items != null)
            AddRange(items);
    }

    public MetaData MetaData {get; set;}

    public bool MoveForward()
    {
        if (MetaData.HasNext)
            MetaData.CurrentPage++;

        return MetaData.HasNext;
    }

    public bool MoveBack()
    {
        if (MetaData.HasPrevious)
            MetaData.CurrentPage--;

        return MetaData.HasPrevious;
    }

    public void PageItems(IEnumerable<T> source)
    {
        Clear();
        AddRange(source);
    }
}
