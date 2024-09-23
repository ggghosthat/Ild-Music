#!/bin/bash

#release build name
out_header="Ild-Music.linux"

#prepare allocation directory
function allocate_out_dir () {
    mkdir "../out/"
    mkdir "../out/$out_header/";
    mkdir "../out/$out_header/components/";
}

#publish component instance
function process_component () {
    local component=$1
    local component_path=$2
    local component_out_directory="../out/$out_header/components/$component/"

    mkdir $component_out_directory
    dotnet publish $component_path -c Release --framework=net8.0 --runtime=linux-x64 --self-contained -o=$component_out_directory
}

#publish app instance
function process_app() {
    local app_path=$1
    local app_out_directory="../out/$out_header"

    dotnet publish $app_path -c Release --framework=net8.0 --runtime=linux-x64 --self-contained -p PublishSingleFile=True -o=$app_out_directory
    
    cp "./config.json" "$app_out_directory/config.json"
    cp "./config.json" "$app_out_directory/config.json.copy"

    tar -czvf "$app_out_directory.tar.gz" $app_out_directory
}

function main () {
    allocate_out_dir

    #publish 'Repository' component 
    local comp1="Ild-Music.Repository"
    local comp_path1="../src/Ild-Music.Repository/Ild-Music.Repository.csproj"
    process_component $comp1 $comp_path1;
    
    #publish 'LinuxPlayer' component
    local comp2="Ild-Music.VlcPlayer.Linux"
    local comp_path2="../src/Ild-Music.VlcPlayer.Linux/Ild-Music.VlcPlayer.Linux.csproj"
    process_component $comp2 $comp_path2;

    #publish 'app' component
    local app="../src/Ild-Music/Ild-Music.csproj"
    process_app $app;
}

main
