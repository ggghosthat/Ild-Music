#!/bin/bash
comp1="../../src/Ild-Music.Repository/"
dotnet publish $comp1 -c Release --framework=net8.0 --runtime=linux-x64 --self-contained

comp2="../../src/Ild-Music.VlcPlayer.Linux/"
dotnet publish $comp2 -c Release --framework=net8.0 --runtime=linux-x64 --self-contained
