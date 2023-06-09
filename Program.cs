﻿using System.Text;

namespace GameOfLife
{
    static class Program
    {
        // Define the size of the grid
        static int Rows = 20;
        static int Cols = 80;
        static int changes = 0;
        static int seed = 42;

        static int generation = 0;
        // Create a random number generator
        static Random random;

        // Create a boolean array to store the state of each cell
        static bool[,] grid ;

        static void Main(string[] args)
        {
            // Initialize the grid with random values
            if (args.Length> 0) {
                int.TryParse(args[0],out seed);
                random = new Random(seed);
            } 
            else {
                random = new Random();
                seed  = 0;
                }
            PrepareGrid();
            InitializeGrid();

            PrepareDisplay();
            // Start an infinite loop
            while (true)
            {
                // Display the grid on the console
                DisplayGrid();

                // Update the grid according to the rules
                UpdateGrid();

                // Wait for some time before repeating
                Thread.Sleep(10);
                generation++;
            }
        }

        private static void PrepareGrid()
        {
            Rows=Console.WindowHeight -2;
            Cols = Console.WindowWidth;
            grid = new bool[Rows,Cols];
        }

        static void PrepareDisplay()
        {
            Console.CursorVisible = false; // Hide the cursor
            Console.Clear();
        }
        static void InitializeGrid()
        {
            
            // Loop through every cell in the grid
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    // Assign a random value (true or false) to each cell
                    grid[i, j] = random.Next(2) == 0;
                }
            }

        }

        static void DisplayGrid()
        {
            Console.SetCursorPosition(0, 0);
            string outputLine = string.Format("Generation {0,5} - Changed cells {1,5} - Seed {2,5}", generation, changes, seed);
            Console.WriteLine(outputLine);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    sb.Append(grid[i, j] ? "*" : " ");
                }
            }
            Console.WriteLine(sb.ToString());
        }


        static void UpdateGrid()
        {
            // Create a temporary array to store the new state of each cell
            bool[,] newGrid = new bool[Rows, Cols];

            // Loop through every cell in the grid
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    // Count how many live neighbours the current cell has
                    int liveNeighbours = CountLiveNeighbours(i, j);

                    // Apply the rules of life to determine if the current cell lives or dies

                    if (grid[i, j])
                    {
                        if (liveNeighbours is < 2 or > 3)
                        {
                            newGrid[i, j] = false;
                        }
                        else
                        {
                            newGrid[i, j] = true;
                        }
                    }

                    else
                    {
                        if (liveNeighbours == 3)
                        {
                            newGrid[i, j] = true;
                        }
                        else
                        {
                            newGrid[i, j] = false;
                        }
                    }
                }
            }

            changes=CountChanges(grid, newGrid);
            // Copy the temporary array to the original grid
            Array.Copy(newGrid, grid, Rows * Cols);
        }

        // A helper method to count how many live neighbours a given cell has
        static int CountLiveNeighbours(int x, int y)
        {
            // Initialize a counter variable
            int count = 0;

            // Loop through all adjacent cells in a 3x3 square around the given cell
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    // Skip the current cell itself
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    // Calculate the coordinates of the neighbour cell
                    int neighbourX = x + i;
                    int neighbourY = y + j;

                    // Wrap around the edges of the grid if necessary
                    if (neighbourX < 0)
                    {
                        neighbourX += Rows;
                    }

                    if (neighbourY < 0)
                    {
                        neighbourY += Cols;
                    }

                    if (neighbourX >= Rows)
                    {
                        neighbourX -= Rows;
                    }

                    if (neighbourY >= Cols)
                    {
                        neighbourY -= Cols;
                    }

                    // If the neighbour cell is alive, increment the counter
                    if (grid[neighbourX, neighbourY])
                    {
                        count++;
                    }
                }
            }

            // Return the final count of live neighbours
            return count;
        }
        // A helper method to count how many cells change state between two generations
        static int CountChanges(bool[,] oldGrid, bool[,] newGrid)
        {
            // Initialize a counter variable
            int count = 0;

            // Loop through every cell in the grid
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    // If the state of the cell is different in the old and new grid, increment the counter
                    if (oldGrid[i, j] != newGrid[i, j])
                    {
                        count++;
                    }
                }
            }

            // Return the final count of changes
            return count;
        }
    }
}

