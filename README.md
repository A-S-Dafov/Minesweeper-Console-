# Minesweeper-Console-
This is a simple console version of the popular Minesweeper game written in C#.
By no means this code is optimised, free of bugs or is very secured and it is chaotic.
It was written as an exercise and it is published with one idea in mind,
that is to help other people who try to write their own Minesweeper.
The code should be easy to upgrade and integrate into a Windows Forms or WPF application 
and turned this way into complete game with UI.

HOW TO PLAY:
The game is meant to be played in the console terminal.
The user inputs the coordinates <X,Y> of his move in the console.
The input should be int he range [0;Size-1], where Size is the size of the board.
To play the game you need to create an object from the class Minesweeper and invoke the Game() method.
You win the game by revealing all fields that are not mines and you can't mark suspiciuos fields with a "flag" 
as in the original game.(Feel free to implement it.)


The class Minesweeper has a constructor that takes 2 arguments - size and mines.
By default the size will take a value of 16 (creating 16x16 board) and mines - a value of 40.
Because it is hard to keep track of the mines I recommend playing it on a small board with a few mines.
For example 5x5 with 5 mines, or 9x9 with 10 mines.
