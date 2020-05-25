# An interpretation of a Tetris game
## Constant refresh rate, no drawing libraries, no drawing at all, custom figures (think of tetromino) beside default tetrominos and custom transformation

This project was created when .NET Core 3.0 was released, with added support for WPF and Winforms. I wanted to see how the .NET core WPF performs compared to existing .NET Framework. There are so many Tetris projects today, for example 32 799 public github repositories and counting (I guess when I created the project year ago the number was comparable), so why yet another one? The answer is pure egoism, pure selfishness. That was my proper motivation. I wanted a game that I will like, a game that I will enjoy. Contrary to most of other (Tetris projects) developers I actually played Tetris. I played different implementations over different hardware. I still enjoy playing it, even for short time.
So my motivation was not to make a Tetris game under 40 minutes. Because later when I play it, it will feel like a game made under 40 minutes ;) Or to make it via fancy language. My motivation was to make a stable, responsive, simple game. Project where it is easy to implement new figures. In github repository of project there is only one new figure, working name is dotsquare, but you can follow the idea and easily create more figures. A project where it is easy to tune the way fugires rotate. In fact, it is not necessary even to rotate them. You can have a figure that at one moment is one square, at other moment two, and then three, and then back one. You don't rotate it, you transform it.

## Gameplay and key bindings

The game that is fast, very responsive, it is a game having simple, but appealing user interface. Figures are colored, single piece of figure is made as an octagon, just one idea more fancy than a square, but that's all. No gradients, no other complications. Buttons for start/stop and pause are styled, so that they are in theme with the game. No visual states are used for styling, but few simple triggers. Game is typically played with keyboard arrows, instant down with space and pause/unpause with Esc key. Few alternating sounds are played when a figure is placed, there are two sounds effects for row clearance and one for level up. Usual sound for less than four lines cleared and slighly more "powerfull" ;) sound when a proper Tetris (clearing of four lines at once) is made. When rows are cleared, they blink for some time in red and blue. The current score, the score made with this particular move, blinks as well. They all blink in sync. Blink blink blink...

## Technical

Project is dot net core WPF. I have made it when .NET Core 3.0 was released, so that I can see how the .NET Core WPF performs. WPF was chosed as a framework, because it talks well with Windows, timers are very stable in it, and binding is great in WPF - it uses different thread, many other goodies. IMHO with some tuning the project can easily be done in Blazor or any framework, supporting binding. There is a timer ticking every 10 milliseconds. Method tied to it try to refresh the user interface is something had changed compared to previous state, and then it tries to process eventual user input. User input or no, figures go down at some point. When enough score is made, figures start to go down faster.

## How so no drawing libraries

For simple figures all transformation looks and feel like rotation. But there are no complicated calcultions for rotation made easy, there are no easy way to draw a rotation etc. Game do not use drawing. There are 200 cells (think of squares, octagons to be precise) and their background is dynamically changed. When this is done dynamically enough, it appears like animaition.

## Model OO in practice

Classes are designed in a way that their object closely resemble the actual non-game object. A single cell cares about its color - Background, border color, opacity etc. It does not care where is on the board, it does not care how it moves from one position to other. The board cares about all cells. The board knows how many they are, how to clear them, how to move them. Board does not know about figures. It does not know how many cells a figure have, it does not know how many transformation a figure have. Abstract figire specify some variables that all other figures need, like Cells to be drawn and Cells to be cleared, what state a figure have, and defines few methods to be implemented. All figures who inherit the main one provide their own implementation of methods for moving left, moving right, down, rotating etc. For more complicated figures rotation is actually a transformation - we have one set of visible cells and we move to another set of visible cells.

## Figures design

There are odf diagram files, made with Libre Office(r) draw. For every diagram there is a screen-shot added. For example, the figure commonly known as L tetromino, described in this project in Right Winkel class, have the following diagram:

![L tetrimono](https://github.com/MahmudOnWeb/TetrisDotNetCoreWpf/blob/master/RightWinkel%20-%20L.PNG?raw=true)

It has four main positions, and when it rotates, the new position is with blue color, and the old is with gray. Blue are displayed, gray are removed.

## Start game, middle game and end game

This is how the game starts:

![Start game](https://github.com/MahmudOnWeb/TetrisDotNetCoreWpf/blob/master/StartGame.gif?raw=true)

This is a view in the middle of the game. Score depends heavily on level:

![Middle game](https://github.com/MahmudOnWeb/TetrisDotNetCoreWpf/blob/master/MiddleGame.gif?raw=true)

And this is how the game ends. Short end game animation. Another game perhaps?

![End game](https://github.com/MahmudOnWeb/TetrisDotNetCoreWpf/blob/master/EndGame.gif?raw=true)