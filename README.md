# KoreDefenceGodot
Porting my university group project game to be playable on the Web!

# How To Build

Clone the repository

Go here, download the mono version 64 bit
https://godotengine.org/download/windows

Open the executable, import a project, navigate to the cloned repository

I suggest JetBrains Rider as the IDE to use to develop it, here's a video on getting that set up:  
https://www.youtube.com/watch?v=N4M5eV982n0  
(this includes setting up C# to work on your computer, slightly lengthy process but not too bad)

If you won't code in C#, then you can use GDScript and code in the editor. It's pretty similar to Python.

# Things to know

The components of the game are designed to be object oriented - but it might not look that way because of the lack of constructors.  
This is because of the way Godot works with C#.  
The method I use as a sort of "constructor" is called "Setup()".
