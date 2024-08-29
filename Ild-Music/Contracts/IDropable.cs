using System.Collections.Generic;

namespace Ild_Music.Contracts;

internal interface IFileDropable
{
    public void DropFile(string filePath);
    public void DropFiles(IEnumerable<string> filePaths);
}