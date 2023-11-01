![](title.jpg)

Local desktop music player for Windows and Linux. 

# Supported operating systems: 

  - Windows 10 -x64 (beta release only)
  - Linux-x64 (test mode)

# From scratch build

⚠️ Please before building player from source make sure that on your machin installed .NET sdk v6.0.4⚠️

1) Build and include [Ild-Music.ShareInstances](https://github.com/ggghosthat/Ild-Music.ShareInstances) into your project with UI client
   (skip this step if you want to use Ild-Music player).
3) Also download and build synchronization block dependency [Ild-Music.SynchronizationBlock](https://github.com/ggghosthat/Ild-Music.SynchronizationBlock),
   which perfom dile system conversations.   
4) Define operating system (Windows or Linux) and select it's own player components.

    - [Ild-Music.NAudioCore](https://github.com/ggghosthat/Ild-Music.NAudioPlayerCore) for Windows
    - [Ild-Music.VLCSharpPlayer](https://github.com/ggghosthat/Ild-Music.VLCSharpPlayer) for Linux

# Dependencies
  All dependencies are holding inside this repository.
  Only a single dependency of player component for Windows has own repository for near time.

  - player instance dependency for Windows [Ild-Music.NAudioCore](https://github.com/ggghosthat/Ild-Music.NAudioPlayerCore) - player component for Windows.\n Based on [NAudio lib](https://github.com/naudio/NAudio)
