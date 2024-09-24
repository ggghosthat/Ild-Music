#release build name
$out_header="Ild-Music.windows"

#prepare allocation directory
function allocate_out_dir () {
    & "mkdir" "../../out/"
    & "mkdir" "../../out/$out_header/";
    & "mkdir" "../../out/$out_header/components/";
}

#publish component instance
function process_component () {
    param (
        [string]$component,
        [string]$component_path
    )

    $component_out_directory="../../out/$out_header/components/$component/"

    & "mkdir" "$component_out_directory"
    & "dotnet" publish "$component_path" -c Release --framework=net8.0 --runtime=win-x64 --self-contained -o="$component_out_directory"
}

#publish app instance
function process_app() {
    param (
        [string]$app_path
    )

    $app_out_directory="../../out/$out_header"
    $app_executable="$app_out_directory/Ild-Music"

    & "dotnet" publish "$app_path" -c Release --framework=net8.0 --runtime=win-x64 --self-contained -p PublishSingleFile=True -o="$app_out_directory"
    
    & "cp" "./config.json" "$app_out_directory/config.json"
    & "cp" "./config.json" "$app_out_directory/config.json.copy"

    & "Compress-Archive" -LiteralPath "$app_out_directory" -DestinationPath "$app_out_directory.zip"
}

function main () {
    allocate_out_dir

    #publish 'Repository' component 
    $comp1="Ild-Music.Repository"
    $comp_path1="../../src/Ild-Music.Repository/Ild-Music.Repository.csproj"
    process_component $comp1 $comp_path1;
    
    #publish 'LinuxPlayer' component
    $comp2="Ild-Music.NAudio.Windows"
    $comp_path2="../../src/Ild-Music.NAudio.Windows/Ild-Music.NAudio.Windows.csproj"
    process_component $comp2 $comp_path2;

    #publish 'app' component
    $app="../../src/Ild-Music/Ild-Music.csproj"
    process_app $app;

}

main