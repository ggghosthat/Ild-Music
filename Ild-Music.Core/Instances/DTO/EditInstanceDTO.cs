namespace Ild_Music.Core.Instances.DTO;
public struct EditInstanceDTO
{
    public Guid Id { get; }

    private IDictionary<string, string> fields;
    private IDictionary<string, IEnumerable<Guid>> relationships; 

    public EditInstanceDTO(Guid id)
    {
        Id = id;
        fields = new Dictionary<string, string>();
        relationships = new Dictionary<string, IEnumerable<Guid>>();
    }

    public void AppendPair(string propName,
                      string propValue) =>
        fields[propName] = propValue;

    public void AppendRelationships(string relateTag,
                                    IEnumerable<Guid> relates) =>
        relationships[relateTag] = relates;
   
    public void Trim(IEnumerable<string> fieldsTrimVector,
                     IEnumerable<string> relationshipsTrimVector)
    {
        var fieldsKeysToRemove = fields.Keys
                                    .Except(fieldsTrimVector)
                                    .ToList();
        var relationshipsKeysToRemove = relationships.Keys
                                                    .Except(relationshipsTrimVector)
                                                    .ToList();

        foreach(var key in fieldsKeysToRemove)
            fields.Remove(key);

        foreach(var key in relationshipsTrimVector)
            relationships.Remove(key);
    }

    public void FlushOut()
    {
        fields.Clear();
    }
}
