using System;
using System.Collections.Generic;
using System.IO;

namespace Maze
{
    class Program
    {
        public int[,] getArray()
        {
            String line = "";
            try
            {
                using (StreamReader sr = new StreamReader("RPAMaze.txt"))
                {
                    line = sr.ReadToEnd();
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("error");
                Console.WriteLine(e.Message);
            }
            line = line.Replace(Environment.NewLine, " ");
            String[] line2 = line.Split(" ");
            int[] dimensions = new int[2];
            dimensions[0] = Convert.ToInt32(line2[0]);
            dimensions[1] = Convert.ToInt32(line2[1]);
            int[,] arr = new int[dimensions[0], dimensions[1]];
            int k = 2;
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    arr[i, j] = Convert.ToInt32(line2[k]);
                    k++;
                }
            }
            return arr;
        }

        public void setStartingPoint(int[,] arr)
        {
            try
            {
                Console.WriteLine("Row = ");
                int row = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Column = ");
                int col = Convert.ToInt32(Console.ReadLine());
                for (int i = 0; i < arr.GetLength(0); i++)
                {
                    for (int j = 0; j < arr.GetLength(1); j++)
                    {
                        if (arr[i, j] == 2) arr[i, j] = 1;
                    }
                }
                arr[row, col] = 2;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        public void getStartingPoint(int[,] arr, ref int row, ref int col)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (arr[i, j] == 2)
                    {
                        row = i;
                        col = j;
                    }
                }
            }
        }

        public void printMaze(int[,] arr)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    Console.Write(arr[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void getBack(List<string> moves, List<int> times, ref int row, ref int col, ref string allMoves)
        {
            while(times[times.Count - 1] > 0)
            {
                if(moves[moves.Count - 1] == "Up")
                {
                    Console.Write("Down, ");
                    allMoves += "Down, ";
                    row++;
                }
                if (moves[moves.Count - 1] == "Left")
                {
                    Console.Write("Right, ");
                    allMoves += "Right, ";
                    col++;
                }
                if (moves[moves.Count - 1] == "Down")
                {
                    Console.Write("Up, ");
                    allMoves += "Up, ";
                    row--;
                }
                if (moves[moves.Count - 1] == "Right")
                {
                    Console.Write("Left, ");
                    allMoves += "Left, ";
                    col--;
                }
                times[times.Count - 1]--;
            }
            moves.RemoveAt(moves.Count - 1);
        }
        static void Main(string[] args)
        {
            Program functions = new Program();
            int[,] arr = functions.getArray();
            int curCol = 0, curRow = 0;
            Console.WriteLine("If you want to change starting point, type 1.");
            String answer = Console.ReadLine();
            if(answer == "1")
            {
                functions.setStartingPoint(arr);
            }
            functions.getStartingPoint(arr, ref curRow, ref curCol);
            functions.printMaze(arr);

            List<int> map = new List<int>();
            List<int> times = new List<int>();
            List<string> moves = new List<string>();
            string allMoves = "";

            bool con = true;

            while(con)
            {
                int ways = 0;
                if (curCol == 0 || curCol == arr.GetLength(1) - 1 || curRow == 0 || curRow == arr.GetLength(0) - 1)
                {
                    con = false;
                    if (curCol == 0)
                    {
                        allMoves += "Left.";
                        Console.Write("Left.");
                        break;
                    }
                    if (curCol == arr.GetLength(1) - 1)
                    {
                        allMoves += "Right.";
                        Console.Write("Right.");
                        break;
                    }
                    if (curRow == 0)
                    {
                        allMoves += "Up.";
                        Console.Write("Up.");
                        break;
                    }
                    if (curRow == arr.GetLength(0) - 1)
                    {
                        allMoves += "Down.";
                        Console.Write("Down.");
                        break;
                    }
                }
                else
                {
                    arr[curRow, curCol] = 2;
                    if (arr[curRow, curCol + 1] == 0)
                    {
                        map.Add(4);
                        ways++;
                    }
                    if (arr[curRow + 1, curCol] == 0)
                    {
                        map.Add(3);
                        ways++;
                    }
                    if (arr[curRow, curCol - 1] == 0)
                    {
                        map.Add(2);
                        ways++;
                    }
                    if (arr[curRow - 1, curCol] == 0)
                    {
                        map.Add(1);
                        ways++;
                    }
                    if (ways == 0)
                    {
                        if (map.Count == 0)
                        {
                            allMoves += " Impossible to escape from the maze.";
                            Console.WriteLine(" Impossible to escape from the maze.");
                            con = false;
                            break;
                        }
                        else functions.getBack(moves, times, ref curRow, ref curCol, ref allMoves);
                    }
                    if (ways > 1)
                    {
                        times.Add(0);
                    }
                    switch (map[map.Count - 1])
                    {
                        case 1:
                            Console.Write("Up, ");
                            allMoves += "Up, ";
                            curRow--;
                            moves.Add("Up");
                            break;
                        case 2:
                            Console.Write("Left, ");
                            allMoves += "Left, ";
                            curCol--;
                            moves.Add("Left");
                            break;
                        case 3:
                            Console.Write("Down, ");
                            allMoves += "Down, ";
                            curRow++;
                            moves.Add("Down");
                            break;
                        case 4:
                            Console.Write("Right, ");
                            allMoves += "Right, ";
                            curCol++;
                            moves.Add("Right");
                            break;
                    }
                    map.RemoveAt(map.Count - 1);
                    if (times.Count > 0)
                    {
                        times[times.Count - 1]++;
                    }
                }
            }
            File.WriteAllText(@"Log.txt", allMoves);
            Console.ReadLine();
        }
    }
}
