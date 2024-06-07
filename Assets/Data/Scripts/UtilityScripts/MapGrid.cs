
public class MapGrid
{

    private bool obstaclesFilled;
    private int ammountOfWallsToChangeTile = 4;

    public int width, height;
    public string seed;
    public int[,] grid;


    public MapGrid(int width, int height, int wallFillPercent, int smootheGenerations, int treeFillPercent, string seed, bool useRandomSeed, bool generateIslands)
    {
        this.width = width;
        this.height = height;
        this.seed = seed;

        grid = GenerateGrid(wallFillPercent, smootheGenerations, treeFillPercent, useRandomSeed, generateIslands);
    }


    private int[,] GenerateGrid(int wallFillPercent, int smootheGenerations, int treeFillPercent, bool useRandomSeed, bool generateIslands)
    {
        grid = new int[width, height];
        obstaclesFilled = false;
        RandomFillGrid(useRandomSeed, wallFillPercent);
        SmootheGrid(smootheGenerations, generateIslands, treeFillPercent);

        return grid;
    }

    private void RandomFillGrid(bool useRandomSeed, int wallFillPercent)
    {
        if (useRandomSeed)
        {
            seed = UnityEngine.Random.Range(float.MinValue, float.MaxValue).ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x, y] = (pseudoRandom.Next(0, 100) < wallFillPercent) ? 1 : 0;
            }
        }
    }

    private void SmootheGrid(int smootheGenerations, bool generateIslands, int treeFillPercent)
    {
        for (int i = 0; i < smootheGenerations; i++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    int neighbourWallTiles = GetSurroundingWallsCount(x, y, generateIslands);

                    if (neighbourWallTiles > ammountOfWallsToChangeTile)
                    {
                        grid[x, y] = 1;
                    }
                    else if (neighbourWallTiles < ammountOfWallsToChangeTile)
                    {
                        grid[x, y] = 0;
                    }
                }
            }
        }

        obstaclesFilled = false;
        RandomTreeFill(treeFillPercent);
    }

    private int GetSurroundingWallsCount(int x, int y, bool generateIslands)
    {
        int wallCount = 0;

        for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++)
        {
            for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < grid.GetLength(0) && neighbourY >= 0 && neighbourY < grid.GetLength(1))
                {
                    if (neighbourX != x || neighbourY != y)
                    {
                        if (grid[neighbourX, neighbourY] == 1)
                            wallCount += grid[neighbourX, neighbourY];
                    }
                }
                else
                {
                    if (generateIslands)
                        wallCount++;
                }
            }
        }

        return wallCount;
    }

    private void RandomTreeFill(int treeFillPercent)
    {
        if (obstaclesFilled)
            return;

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y] == 0)
                {
                    grid[x, y] = (pseudoRandom.Next(0, 100) < treeFillPercent) ? 2 : 0;
                }
            }
        }

        obstaclesFilled = true;
    }
}
