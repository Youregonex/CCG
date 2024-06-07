using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PathFinder
{
    public List<TerrainTile> FindPath(TerrainTile start, TerrainTile end, List<TerrainTile> searchableTiles)
    {
        List<TerrainTile> openList = new List<TerrainTile>();
        List<TerrainTile> closeList = new List<TerrainTile>();

        openList.Add(start);

        while (openList.Count > 0)
        {
            TerrainTile currentTile = openList.OrderBy(x => x.F).First();

            openList.Remove(currentTile);
            closeList.Add(currentTile);

            if (currentTile == end)
            {
                return GetFinishedList(start, end);
            }

            var neighbourTiles = TileManager.Instance.GetNeighbourTiles(currentTile, searchableTiles);

            foreach (var neighbour in neighbourTiles)
            {
                if (!neighbour.walkable || closeList.Contains(neighbour))
                {
                    continue;
                }

                neighbour.G = GetManhattenDistance(start, neighbour);
                neighbour.H = GetManhattenDistance(end, neighbour);

                neighbour.previousTile = currentTile;

                if(!openList.Contains(neighbour))
                {
                    openList.Add(neighbour);
                }
            }
        }

        return new List<TerrainTile>();
    }

    private List<TerrainTile> GetFinishedList(TerrainTile start, TerrainTile end)
    {
        List<TerrainTile> finishedList = new List<TerrainTile>();

        TerrainTile currentTile = end;

        while(currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.previousTile;
        }

        finishedList.Reverse();

        return finishedList;
    }

    private int GetManhattenDistance(TerrainTile start, TerrainTile neighbour)
    {
        return Mathf.Abs(start.gridLocation.x - neighbour.gridLocation.x) + Mathf.Abs(start.gridLocation.y - neighbour.gridLocation.y);
    }

}
