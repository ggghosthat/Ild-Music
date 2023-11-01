namespace Cube.Mapper.Entities;
public struct Store
{
    public int Tag {get; set;}
    public Guid Holder {get; set;} = Guid.Empty;
    public ICollection<Guid> Relates {get; set;} = new List<Guid>();
    public int Count => Relates.Count;
    
    public bool this[Guid item]
    {
        get => Relates.Contains(item);
    }

    public Store(int tag)
    {
        Tag = tag;
    }

    //set single first value for all pairs
    public void SetHolder(Guid holder)
    {
        Holder = holder;
    }
}
