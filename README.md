# Description

Ild-Music is music player to listen local mp3 files.

# From scratch build requirement:

  - dotnet sdk 6.0.4

# Supported operating systems: 

  - Windows 10 -x64 (beta release only)
  - Linux-x64 (test mode)

# Dependencies:

if you want to build this client from sourec code or rebuild, first you need to build 2 nessessory dependencies:
  1) synchronization block dependency [Ild-Music.SynchronizationBlock](https://github.com/ggghosthat/Ild-Music.SynchronizationBlock)
  2) player instance dependency for Windows [Ild-Music.NAudioCore](https://github.com/ggghosthat/Ild-Music.NAudioPlayerCore)
  3) player instance dependency for Linux (Manjaro tested) [Ild-Music.VLCSharpPlayer](https://github.com/ggghosthat/Ild-Music.VLCSharpPlayer) (still testing)
