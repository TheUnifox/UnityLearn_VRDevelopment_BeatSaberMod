# Dev log
### Oct. 21, 2023   2:40pm - 11:30pm
Initial project creation, starting out with a project set up for vr. Decided to recreate Beat Saber, and implement the most popular mods directly into the game. As well as create an easier to use modding framework, while keeping current mods still working with this version. All existing and future maps will work with this version as well. \
BeatmapCore.dll deconstructed into scripts

### Oct. 22, 2023    4:00pm - 10:00pm
GameplayCore.dll deconstructed into scripts \
Gaga.dll deconstructed into scripts \
Zenject-usage.dll deconstructed into scripts \
Zenject.dll scripts created 

### Oct. 23, 2023    12:00pm - 11:00pm
Zenject.dll deconstructed into scripts \
Colors.dll deconstructed into scripts \
MockCore.dll deconstructed into scripts 

### Oct. 24, 2023    3:30pm - 11:00pm
BGNet.dll deconstructed into scripts \
BeatmapEditor3D.dll deconstructed into scripts \
HMLib.dll deconstructed into scripts \
HMLibAttributes.dll deconstructed into scripts \
HMRendering.dll deconstructed into scripts \
Main.dll deconstructed into scripts, just error fixing to do 

Some lambda functions may not have decompiled properly, so need to be rewritten. \
Most errors currently will be easy to fix, it'll just take a while to work through them all. \
Other errors, like the lambda functions that didn't decompile correctly, will take more work to fix.

### Oct. 25, 2023    2:00pm - 12:00am
HMUI.dll deconstructed into scripts \
Rendering.dll deconstructed into scripts \
VRUI.dll deconstructed into scripts \
ALL ERRORS FIXED!!!

Tomorrow will be the start of scene reconstruction. \
Using a tool while the game is running to inspect each scene, and see all the GameObjects and what scripts are on them, to reconstruct the game. \
Once reconstruction is done, I can implement the most popular mods into the game code, while testing to make sure things work.

### Oct. 26, 2023    3:00pm - 9:000pm
Attempts at Deconstructing assets.

### Oct. 31, 2023    2:30pm - 11pm
A lot of work reconstructing UI. \
Mostly settings menu stuff that I have re-added the scripts onto.

Gonna be at least a few days of this adding scripts onto objects, there's a lot to do. \
After that will be possibly fixing shaders, then *maybe* a first test build.
