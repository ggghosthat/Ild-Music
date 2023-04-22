![](title.jpg)
Local music player for mp3 files and my desktop player. 

# Supported operating systems: 

  - Windows 10 -x64 (beta release only)
  - Linux-x64 (test mode)

# From scratch build

⚠️ Please before building player from source make sure that on your machin installed .NET sdk v6.0.4⚠️

1) Build and include [Ild-Music.ShareInstances](https://github.com/ggghosthat/Ild-Music.ShareInstances)
2) Also download and build synchronization block dependency [Ild-Music.SynchronizationBlock](https://github.com/ggghosthat/Ild-Music.SynchronizationBlock),
   which perfom dile system conversations.   
4) Define operating system (Windows or Linux) and select it's own player components.

    - [Ild-Music.NAudioCore](https://github.com/ggghosthat/Ild-Music.NAudioPlayerCore) for Windows
    - [Ild-Music.VLCSharpPlayer](https://github.com/ggghosthat/Ild-Music.VLCSharpPlayer) for Linux

# Dependencies

  1) mainhold dependency [Ild-Music.ShareInstances](https://github.com/ggghosthat/Ild-Music.ShareInstances) - contain main instances, objects and interfaces
  2) synchronization block dependency [Ild-Music.SynchronizationBlock](https://github.com/ggghosthat/Ild-Music.SynchronizationBlock) - using for file system conversations.
  3) player instance dependency for Windows [Ild-Music.NAudioCore](https://github.com/ggghosthat/Ild-Music.NAudioPlayerCore) - player component for Windows.\n Based on [NAudio lib](https://github.com/naudio/NAudio)
  4) player instance dependency for Linux (Manjaro tested) [Ild-Music.VLCSharpPlayer](https://github.com/ggghosthat/Ild-Music.VLCSharpPlayer) - player for Linux. \n Based on [LibVLCSharp lib](https://github.com/videolan/libvlcsharp)
