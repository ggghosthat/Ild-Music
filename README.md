![](title.jpg)

# Description

Local music player for mp3 files and my desktop player. 

# From scratch build requirement:

  - dotnet sdk 6.0.4

# Supported operating systems: 

  - Windows 10 -x64 (beta release only)
  - Linux-x64 (test mode)

# Dependencies:

## Totally from scratch 

1) Build and include [Ild-Music.ShareInstances](https://github.com/ggghosthat/Ild-Music.ShareInstances)
2) Also download and build synchronization block dependency [Ild-Music.SynchronizationBlock](https://github.com/ggghosthat/Ild-Music.SynchronizationBlock),
   which perfom dile system conversations.   
4) Define operating system (Windows or Linux) and select it's own player components.

    - [Ild-Music.NAudioCore](https://github.com/ggghosthat/Ild-Music.NAudioPlayerCore) for Windows
    - [Ild-Music.VLCSharpPlayer](https://github.com/ggghosthat/Ild-Music.VLCSharpPlayer) for Linux

# Dependencies description

  1) synchronization block dependency [Ild-Music.SynchronizationBlock](https://github.com/ggghosthat/Ild-Music.SynchronizationBlock) - using for file system conversations.
  2) player instance dependency for Windows [Ild-Music.NAudioCore](https://github.com/ggghosthat/Ild-Music.NAudioPlayerCore) - player component for Windows.\n Based on [NAudio lib](https://github.com/naudio/NAudio)
  3) player instance dependency for Linux (Manjaro tested) [Ild-Music.VLCSharpPlayer](https://github.com/ggghosthat/Ild-Music.VLCSharpPlayer) - player for Linux. \n Based on [LibVLCSharp lib](https://github.com/videolan/libvlcsharp)
