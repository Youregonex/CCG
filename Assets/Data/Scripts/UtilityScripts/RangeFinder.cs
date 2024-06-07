using System.Collections.Generic;
using System.Linq;

public class RangeFinder
{
    //PathFinder pf = new PathFinder();

    public List<TerrainTile> GetTilesInRange(TerrainTile startingTile, int range)
    {
        if (range <= 0) return null;

        List<TerrainTile> tilesInRange = new List<TerrainTile>();
        int stepCount = 0;

        tilesInRange.Add(startingTile);

        List<TerrainTile> tileForPreviousStep = new List<TerrainTile>();

        tileForPreviousStep.Add(startingTile);

        while(stepCount < range)
        {
            List<TerrainTile> surroundingTiles = new List<TerrainTile>();

            foreach(TerrainTile tile in tileForPreviousStep)
            {
                surroundingTiles.AddRange(TileManager.Instance.GetNeighbourTiles(tile, new List<TerrainTile>()));
            }

            tilesInRange.AddRange(surroundingTiles);
            tileForPreviousStep = surroundingTiles.Distinct().ToList();
            stepCount++;
        }

        return tilesInRange.Distinct().ToList();
        
    }

}
