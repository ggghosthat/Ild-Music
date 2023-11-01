namespace Cube.Mapper.Entities;
public record struct ApPair(string AID, string PID);

public record struct AtPair(string AID, string TID);

public record struct PtPair(string PID, string TID);

public record struct TagPair(string TagId, string IID);
