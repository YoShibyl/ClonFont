# ClonFont
A BepInEx plugin for Clone Hero that changes fonts of various in-game texts.

Requires [BepInEx and BiendeoCHLib](https://github.com/Biendeo/My-Clone-Hero-Tweaks).  This is a work-in-progress mod.

Upon launching the game with this installed, you should navigate to `[Clone Hero folder]/BepInEx/plugins/ClonFont`.  From here, make a folder called `Fonts` and place any TrueType Font files there.

Once you have your desired fonts installed, edit the `fonts.ini` and change the font settings from `default` to the exact filename of one of the fonts you placed in `Fonts` (without the `.ttf` extension).  Once you save the `fonts.ini`, the fonts will be loaded into the game upon starting/restarting a song, or quitting to the menu.
