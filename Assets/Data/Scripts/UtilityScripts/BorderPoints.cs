using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BorderPoints
{
    public static void GetBorderPoints(int x, int y, int[,] grid, List<Vector2Int> borderTiles)
    {
        if (x + 1 > grid.GetLength(0) || x - 1 < grid.GetLength(0) || y + 1 > grid.GetLength(1) || y - 1 < grid.GetLength(1))
            return;

        if (grid[x + 1, y] == 1 || grid[x - 1, y] == 1 || grid[x, y + 1] == 1 || grid[x, y - 1] == 1)
            borderTiles.Add(new Vector2Int(x, y));
    }
}
