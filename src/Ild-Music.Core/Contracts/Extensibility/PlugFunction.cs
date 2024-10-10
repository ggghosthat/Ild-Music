using System.Linq.Expressions;

namespace Ild_Music.Core.Contracts.Plagination;

public class PlugFunction
{
    public Guid FunctionId { get; set; }

    public string FunctionName { get; set; }

    public Expression Function { get; set; } 
}