# KoreDefenceGodot
Porting my university group project game to be playable on the Web!

Try playing the current release here: https://mozarellaman.github.io/KoreDefenceGame/
# How To Build
## Requirements
Microsoft Build Tools 2019 (https://visualstudio.microsoft.com/2c8e2887-17be-4989-b644-2c267c9965be)
Nuget (https://docs.microsoft.com/en-us/nuget/install-nuget-client-tools) Download the exe, put it in a convenient place, add to PATH

## Instructions
Clone the repository

Go here, download the mono version 64 bit
https://godotengine.org/download/windows

Open the executable, import a project, navigate to the cloned repository

I suggest JetBrains Rider as the IDE to use to develop it, here's a video on getting that set up:  
https://www.youtube.com/watch?v=N4M5eV982n0  
(this includes setting up C# to work on your computer, slightly lengthy process but not too bad)
Disclaimer: JetBrains Rider is not free, but can be used with a student email or if you contribute to an OSS (Open Source Project)

If you won't code in C#, then you can use GDScript and code in the editor. It's pretty similar to Python.


# Screenshots



# Useful Things to know

The components of the game are designed to be object oriented - but it might not look that way because of the lack of constructors.  
This is because of the way Godot works with C#.  
The method I use as a sort of "constructor" is called "Setup()".
