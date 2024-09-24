# Contributing
Thanks for your interest in contributing to Ild-Music! I am very welcome and appreciate contributions.

# Dev directories
  - `src` - codebase directory
  - `build` - build scripts directory.
  - `out` (optional) - artifact directory for building and publishing. DO NOT INCLUDE IN REPOSITORY!
  - `img` - images for markdow files (README, etc)

## Source code directory structure
  - Ild-Music - main app
  - Ild-Music.Core - share functionality across whole app and components
  - Ild-Music.Repository - data saving component
  - Ild-Music.NAudio.Windows - player component for Windows
  - Ild-Music.VlcPlayer.Linux - player component for Linux
    
## Build directory rules
  - Each subfolder named as supported operating system
  - Subfolder must contains scripts just for building or publishing process no more no less.

### Examples
  - `windows` subfolder stores build or publish script for Linux and `linux` subfodler stores both for Linux and WIndows. (NOT ACCEPTABLE)
  - `window` stores for Windows, `linux` stores for Linux, `macos` stores for Mac OS, `bsd` stores for BSD. (SINGLE ACCEPTABLE VARIANT)
    
# Contribution algorithm
  - fork repository
  - configure git
  - make changes,test (make sure you won't break whole project before pull request)
  - commit to your fork
  - pull request to main repository
