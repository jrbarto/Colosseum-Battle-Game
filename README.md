## GIT LFS
This project uses git lfs to store large files like .fbx and .mp3 files. You must install git lfs (https://git-lfs.com/) and run `git lfs pull` to pull down the large files.

### Local storage
In addition to using git lfs, this project also does not use a traditional LFS server (such as Github). It stores the lfs files in a local directory using the following project: https://github.com/sinbad/lfs-folderstore. 

The local directory being used stores the files in a directory in Dropbox. Here is how I am doing that (you need to do the same if pulling down this repo fresh):
```
git config --add lfs.customtransfer.lfs-folder.path lfs-folderstore
git config --add lfs.customtransfer.lfs-folder.args "/path/to/your/dropbox/folder"
git config --add lfs.standalonetransferagent lfs-folder
git config lfs.url "https://localhost"
```
(Note that `lfs-folderstore` needs to be on the system path as explained in the lfs-folderstore project. Replace `/path/to/your/dropbox/folder` with the actual local path to your dropbox shared folder on the file system.)

### LFS Cleanup
Since this is using local storage, we have complete control over the files (unlike if we were using an LFS server like Github, who does not allow you to clean up old versions of files). So, I created a script that when run only keeps the latest versions of the stored LFS files. When running out of LFS storage, run the `remove-old-lfs-files.sh` script and it will remove past versions of the stored files and only keep the current versions. 

WARNING: This script is still in testing. I may need to change some things... I had issues after I ran it just doing a git push. Git started showing errors that I was referencing files that do not exist. I had to clone the repo into a new directory to fix it. So, it may have just been some old hidden references in my local file system in the old directory that existed from when I was using GitHub's LFS server. Still testing if that fix is working. 

## Music Credits
The music files used in this project are licensed under a Creative Commons license and provided by https://incompetech.com/music/royalty-free/. Individual file credits:
- "Ibn Al-Noor" Kevin MacLeod (incompetech.com)
Licensed under Creative Commons: By Attribution 4.0 License
http://creativecommons.org/licenses/by/4.0/
- "Volatile Reaction" Kevin MacLeod (incompetech.com)
Licensed under Creative Commons: By Attribution 4.0 License
http://creativecommons.org/licenses/by/4.0/
- "Heavy Heart" Kevin MacLeod (incompetech.com)
Licensed under Creative Commons: By Attribution 4.0 License
http://creativecommons.org/licenses/by/4.0/
- "Hiding Your Reality" Kevin MacLeod (incompetech.com)
Licensed under Creative Commons: By Attribution 4.0 License
http://creativecommons.org/licenses/by/4.0/