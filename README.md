Ild-Music is a music player to liste local mp3 files. 
Now it can only support mp3 files on Windows OS.
In future planning extension of supporting mediaformats, enabling Linux support and architecture improvements.
This is a beta-preview of Prototype edition, so it can containning some logical and performance bugs.

In case you will find some of them, please write in issue.



Requirements:
- dotnet 6.0
- Windows OS

Dependencies:
if you want to build this client from sourec code or rebuild, first you need to build 2 nessessory dependencies:
  1) synchronization block dependency [Ild-Music.SynchronizationBlock](https://github.com/ggghosthat/Ild-Music.SynchronizationBlock)
  2) player instance dependency for Windows [Ild-Music.NAudioCore](https://github.com/ggghosthat/Ild-Music.NAudioPlayerCore)
  3) player instance dependency for Linux (Manjaro tested) [Ild-Music.VLCSharpPlayer](https://github.com/ggghosthat/Ild-Music.VLCSharpPlayer) (still testing)
