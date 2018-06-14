using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public class Minesweeper
    {
        //default setups : 16x16 board with 40 mines (medium difficulty)
        // 9x9 with 10 mines (easy)
        //16x30 with 99 mines (hard)

        //variables
        private sbyte[,] board; // represents the board
        private sbyte size;//the size of the board, our board is nxn, i.e. matrix
        //The logic behind the values in the board[,] : 
        // -1 - for mine
        // 0 to 8 - shows the number of neighbouring fields with mines
        // 0 should be the "empty" field

        private sbyte mines; // shows the number of mines int he game

        private char[,] gameBoard; // used for drawing the board when the game starts
        private bool[,] visited;//shows the visited fields, usefull for the recursive function later
        private bool gameOver;
        //end of variables

        //properties
        public sbyte Size
        {
            get
            {
                return size;
            }
            set
            {
                if (value > 0 && value < 17)
                //if (value == 16)
                {
                    size = value;
                }
                else size = 16;
            }
        }

        public sbyte Mines
        {
            get
            {
                return mines;
            }
            set
            {
                if (value > 0 && value < 41)
                {
                    mines = value;
                }
                else mines = 40;
            }
        }


        //properties


        //constructors

        public Minesweeper(sbyte size = 16, sbyte mines = 40)//default with params, meant to be called with no args
        {
            Size = size;
            Mines = mines;
            gameOver = false;
            board = new sbyte[size, size];
            gameBoard = new char[size, size];
            visited = new bool[Size, Size];


            for (int i = 0; i < size; i++)//fills the board with 0 ~~ "empty" spaces
            {
                for (int j = 0; j < size; j++)
                {
                    board[i, j] = 0;
                    gameBoard[i, j] = '_';
                    visited[i, j] = false;
                }
            }

        }

        public Minesweeper(Minesweeper A)//copy ctor
        {
            Size = A.Size;
            Mines = A.mines;
            gameOver = A.gameOver;

            board = new sbyte[Size, Size];
            gameBoard = new char[Size, Size];
            visited = new bool[Size, Size];

            for (sbyte i = 0; i < Size; i++)
            {
                for (sbyte j = 0; j < Size; j++)
                {
                    board[i, j] = A.board[i, j];
                    gameBoard[i, j] = A.gameBoard[i, j];
                    visited[i, j] = A.visited[i, j];
                }
            }
        }
        //end of constructors

        //methods

        private sbyte CalculatePosition(int i, int j)//calculates the nearby mines for one position
        {
            if (board[i, j] == -1) return -1;//we are on a mine
            sbyte br = 0;//else we check if the neighbouring fields are valid and if they are a mine
            if (i - 1 >= 0 && board[i - 1, j] == -1) br++;
            if (i - 1 >= 0 && j - 1 >= 0 && board[i - 1, j - 1] == -1) br++;
            if (i - 1 >= 0 && j + 1 < Size && board[i - 1, j + 1] == -1) br++;
            if (j - 1 >= 0 && board[i, j - 1] == -1) br++;
            if (j + 1 < Size && board[i, j + 1] == -1) br++;
            if (i + 1 < Size && j - 1 >= 0 && board[i + 1, j - 1] == -1) br++;
            if (i + 1 < Size && board[i + 1, j] == -1) br++;
            if (i + 1 < Size && j + 1 < Size && board[i + 1, j + 1] == -1) br++;
            return br;
        }

        public void GenerateBoard(sbyte firstX, sbyte firstY)//generates the mines, calculates neighbours, the first "click" on a position will not be a mine
        {
            Random generator = new Random();//we use it to generate the places of the mines


            int placedMines = 0;

            while (placedMines < mines)//will loop until all the mines have been placed
            {
                sbyte mineI = 0, mineJ = 0;
                do
                {
                    mineI = (sbyte)generator.Next(0, size);//randomly generates the positions 
                    mineJ = (sbyte)generator.Next(0, size);

                } while ((mineI == firstX && mineJ == firstY) || (board[mineI, mineJ] != 0));//wont generate a mine on the starting position

                board[mineI, mineJ] = -1;//sets the mine
                placedMines++;
            }

            for (int i = 0; i < size; i++)//fills the other fields
            {
                for (int j = 0; j < size; j++)
                {
                    board[i, j] = CalculatePosition(i, j);//gets the number of the neighbouring mines
                }
            }
            UncoverField(firstX, firstY);//we uncover the first move

        }

        private bool IsMine(int x, int y)//checks if the position is a mine
        {
            return (board[x, y] == -1) ? true : false;
        }

        private bool IsValidMove(int x, int y)//checks if the move is valid - the selected field is covered
        {
            return visited[x, y] == false; 
        }

        private void UncoverField(int x, int y)//uncovers the given field and all nearby "empty" fields
        {
            if (x >= 0 && x < size && y >= 0 && y < size) //If (x,y) is a position on the board
            {
                if (visited[x, y]) return; //if we have visited this field we exit the recursion

                if (IsMine(x, y))//If we stepped on a mine we loose,
                {
                    gameBoard[x, y] = 'X';
                    visited[x, y] = true;
                    gameOver = true;

                    return;//we exit the recursion
                }
                else//we uncover the field
                {
                    if (board[x, y] > 0) //we didn't step on an empty field
                    {
                        visited[x, y] = true;
                        gameBoard[x, y] = (char)('0' + board[x, y]);//we convert the digit on the board to it's character
                    }
                    else //we are on an empty field
                    {
                        visited[x, y] = true;
                        gameBoard[x, y] = '0';
                        //we will check if the neighboring fields have been visited
                        //if a field has not been visited, then we will call this function for that field (recursive call)
                        if ((x + 1 < size) && !visited[x + 1, y]) UncoverField(x + 1, y);
                        if ((x + 1 < size && y + 1 < size) && !visited[x + 1, y + 1]) UncoverField(x + 1, y + 1);
                        if ((x + 1 < size && y - 1 >= 0) && !visited[x + 1, y - 1]) UncoverField(x + 1, y - 1);
                        if ((y + 1 < size) && !visited[x, y + 1]) UncoverField(x, y + 1);
                        if ((y - 1 >= 0) && !visited[x, y - 1]) UncoverField(x, y - 1);
                        if ((x - 1 >= 0 && y + 1 < size) && !visited[x - 1, y + 1]) UncoverField(x - 1, y + 1);
                        if ((x - 1 >= 0 && y - 1 >= 0) && !visited[x - 1, y - 1]) UncoverField(x - 1, y - 1);
                        if ((x - 1 >= 0) && !visited[x - 1, y]) UncoverField(x - 1, y);
                    }
                }
            }
            else return;//not a valid position
        }

        private bool DoWeWin()//checks if we have uncovered all fields that are not mines,
                              //i.e. the number of uncovered fields is equal to the number of mines
        {
            int br = 0;//this will show the number of unvisited fields

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (!visited[i, j])//checks if the field is uncovered
                        br++;

                }
            }


            return br == Mines;//if this is true, we have only mines left, so we win
        }

        public void Game()//the main loop of the game,
                          //0)we print the field to the console
                          // 1)user inputs coordinates for his nect move
                          // 2)if the move is valid, i.e. it's on a covered field, the field is uncovered
                          // 2.1) if the move is invalid, the user must retype the coordinates of his move
                          // 3) if the user stepped on a mine , he looses the game, gets GAME OVER msg and the board with all positions uncovered
                          // 3.1) if the only left covered fields are mines, the user Wins and gets YOU WIN msg
                          // 3.2) else he continues the game
        {
            Console.WriteLine("You need to uncover all fields that are not mines.\n" +
                               "There are {0} mines on the field.\n"+ 
                               "The input from the coordinates is in the <0;{1}> Integer range.\n Good Luck!\n",Mines,Size-1);
            PrintBoard();//we print the board
            //we must make the first move
            sbyte x, y;//user input coords
            MakeMove(out x, out y);
            GenerateBoard(x, y);

            //
            while (!DoWeWin() && !gameOver)
            {

                do
                {
                    PrintBoard();
                    MakeMove(out x, out y);

                } while (!IsValidMove(x, y));//loops until x and y are the coordinates of a covered field

                UncoverField(x, y);//the user makes his move

            }

            if (gameOver)
            {
                PrintFinal();
                Console.WriteLine("YOU STEPPED ON A MINE!! GAME OVER!!\n");
            }
            else
            {
                PrintBoard();
                Console.WriteLine("You Evaded all mines!! YOU WIN!!\n");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();



        }

        private void PrintBoard()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Console.Write("{0} ", gameBoard[i, j]);
                }
                Console.WriteLine();
            }
            //Console.WriteLine(); //theese nested loops print the mines and the "numbers" fields, usefull for debugging
          //  for (int i = 0; i < Size; i++)
           // {
            //    for (int j = 0; j < Size; j++)
            //    {
            //        Console.Write("{0} ", board[i, j]);
            //    }
           //     Console.WriteLine();
           // }
        }

        private void PrintFinal() // prints the board when the user has stepped on a mine
        {
            Console.WriteLine();
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (board[i, j] == -1) Console.Write("X ");
                    else Console.Write("{0} ", 48 + board[i, j] - '0');
                }
                Console.WriteLine();
            }
        }

        private void MakeMove(out sbyte x, out sbyte y)//gets user input and checks if it's OK
        {

            do
            {
                Console.WriteLine("\nEnter coordinate x: ");
                
                string input = Console.ReadLine();
                x = (sbyte)(input[0] - '0');//gets the number from the char
                Console.WriteLine("\nEnter coordinate y: ");
                input = Console.ReadLine();
                y = (sbyte)(input[0] - '0');//reads y;
                Console.WriteLine();
            } while (x < 0 || y < 0 || x >= Size || y >= Size);
        }
        //end of methods
    }
}
