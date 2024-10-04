$comp1="../../src/Ild-Music.Repository/Ild-Music.Repository.csproj"
& "dotnet" publish $comp1 -c Release --framework=net8.0 --runtime=win-x64 --self-contained

$comp2="../../src/Ild-Music.NAudio.Windows/Ild-Music.NAudio.Windows.csproj"
& "dotnet" publish $comp2 -c Release --framework=net8.0 --runtime=win-x64 --self-contained