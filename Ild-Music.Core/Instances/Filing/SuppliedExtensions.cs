namespace Ild_Music.Core.Instances.Filing;

public class SuppliedExtensions
{
    private static List<string> _fileExtensions = new List<string>();

    public static List<string> FileExtensions => _fileExtensions;

    public static void SupplySingleExtension(string fileExtension)
    {
        _fileExtensions.Add(fileExtension);
    }

    public static void SupplyMultipleExtensions(IEnumerable<string> fileExtensions)
    {
        _fileExtensions.AddRange(fileExtensions);
    }

    public static void UnsupplyExtension(string fileExtension)
    {
        if (_fileExtensions.Contains(fileExtension))
            _fileExtensions.Remove(fileExtension);
    }

    public static bool CheckFile(string file)
    {
        var ext = Path.GetExtension(file.ToString());
        return _fileExtensions.Contains(ext);
    }
}