# GUILib 

This project is about fixing one of the biggest problems in OpenGL development: GUI. 

I have tried to find a good .NET GUI Library for years but never found something that I really liked. Many libraries required .NET Wrappers that were outdated or just not working, others were expensive - either financially or in terms of performance. A few were pretty cool but not very flexible in their look and feel which made them great tools during development but nothing you would want to release a product with - admittedly most of them were also not intended for that purpose.  

Project after project I implemented provisory GUI Systems that got permanent as development went on. But there were always flaws that could not be fixed without a full rework, mainly these three: Not enough customizability (or high effort to achieve it), bad or no reaction to resizes and bad or no animation systems (again: high effort or just not possible).  

This project was meant to fix all these three key requirements while still remaining light weight and easy extensible.  

**You can do what ever you want with the library. You can sell or modify it, use it privately or commercially. You can treat it as if this is your code.** The exception is the Resource folder. There are a few files in there that I have not created myself. If you want to use those please contact the original creators (listed in the creatorList.txt file). If you have made any useful extension to the library feel free to share it with everyone. Just get in contact with me via Email: GuiLib@outlook.de. You can send it to me for a merge or merge it yourself and create a merge request. 

# Examples 

Here you can see a few examples created with this library. 

Modern Game Menu: 

[![](http://img.youtube.com/vi/sBSMbJIkFvs/0.jpg)](http://www.youtube.com/watch?v=sBSMbJIkFvs "") 

# Current State 

The project is in a very early state. There are many features that still need to be implemented but it is possible to be used productively. You might need to dig around a bit in the code to figure out what is implemented exactly but here a basic overview: 

- Keyframe based animations 
- Round Edges 
- Gradients 
- A few [Gui Elements](https://github.com/JeremyFunk/GUILib/tree/master/GUILib/GUI/GuiElements) 
- Mouse and Keyboard Input 
- Event System 
- Easy customizability using the Theme class (default materials) 
- Basic Logger System  

As you can see this library is far from being feature complete, in fact it isn't even a library. It is a single executable. This way early development is a lot easier, but almost every single class would be in the library so making the cut yourself should not be a hard task. You might want to copy over every class over to your project and replace the engine core with your window class.
