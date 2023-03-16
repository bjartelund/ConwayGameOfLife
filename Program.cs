namespace GameOfLife
{
    static class Program
    {
        // Define the size of the grid
        const int Rows = 28;
        const int Cols = 115;
        static int changes = 0;

        static int generation = 0;
        // Create a random number generator
        static Random random = new Random();

        // Create a boolean array to store the state of each cell
        static bool[,] grid = new bool[Rows, Cols];

        static void Main(string[] args)
        {
            // Initialize the grid with random values
            InitializeGrid();

            // Start an infinite loop
            while (true)
            {
                // Display the grid on the console
                DisplayGrid();

                // Update the grid according to the rules
                UpdateGrid();

                // Wait for some time before repeating
                Thread.Sleep(1000);
                generation++;
            }
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
            Console.CursorVisible = false; // Hide the cursor
            Console.SetCursorPosition(0, 0);
            string outputLine = string.Format("Generation {0,5} - Changed cells {1,5}", generation, changes);
            Console.WriteLine(outputLine);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    if (grid[i, j])
                    {
                        Console.Write("*");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
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
                        if (liveNeighbours < 2 || liveNeighbours > 3)
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

