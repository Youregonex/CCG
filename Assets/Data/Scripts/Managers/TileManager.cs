using UnityEngine;
using System;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    private static TileManager instance;
    public static TileManager Instance { get { return instance; } }

    [Range(36, 70)]
    [SerializeField] private int wallFillPercent = 45;
    
    [Range(1, 10)]
    [SerializeField] private int smootheGenerations = 3;

    [Range(0, 100)]
    [SerializeField] private int treeFillPercent = 4;

    [SerializeField] private int width; //100 pref
    [SerializeField] private int height; //80 pref
    [SerializeField] private float cellSize = 1f;

    [SerializeField] private string seed;
    [SerializeField] private bool useRandomSeed = true;

    [SerializeField] private bool generateIslands = true;

    [SerializeField] private Sprite[] grassSprites;
    [SerializeField] private Sprite[] waterSprites;
    [SerializeField] private GameObject[] treePrefabs;
    [SerializeField] private GameObject waterPrefab;
    [SerializeField] private GameObject groundGrassPrefab;

    private bool tilesFilled;
    private RangeFinder rangeFinder = new();

    public Dictionary<Vector2Int, TerrainTile> tileGameObjects;
    public MapGrid map;
    public List<Vector2Int> borderTiles = new();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        DestroyTiles();
        if (tileGameObjects != null)
            tileGameObjects.Clear();

        tileGameObjects = new Dictionary<Vector2Int, TerrainTile>();

        map = new MapGrid(width, height, wallFillPercent, smootheGenerations, treeFillPercent, seed, useRandomSeed, generateIslands);
        FillTiles();

        //for(int x = 0; x < map.grid.GetLength(0); x++)
        //{
        //    for(int y = 0; y < map.grid.GetLength(1); y++)
        //    {
        //        borderTiles = BorderPoints.GetBorderPoints();
        //    }
        //}

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GenerateMap();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            FillTiles();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            DestroyTiles();
        }
    }

    private void FillTiles()
    {
        if(!tilesFilled)
        {
            GameObject parent = new("GroundTiles");

            parent.transform.position = new Vector3(0, 0);

            Sprite tileSprite;
            GameObject prefab;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    switch (map.grid[x, y])
                    {
                        case 0:
                            tileSprite = grassSprites[UnityEngine.Random.Range(0, grassSprites.Length)];
                            tileGameObjects[new Vector2Int(x, y)] = PlaceTile(groundGrassPrefab, GetWorldPosition(x, y), parent, tileSprite);
                            tileGameObjects[new Vector2Int(x, y)].gridLocation = new Vector2Int(x, y);
                            break;

                        case 1:
                            tileSprite = waterSprites[UnityEngine.Random.Range(0, waterSprites.Length)];
                            tileGameObjects[new Vector2Int(x, y)] = PlaceTile(waterPrefab, GetWorldPosition(x, y), parent, tileSprite);
                            tileGameObjects[new Vector2Int(x, y)].gridLocation = new Vector2Int(x, y);
                            break;

                        case 2:
                            tileSprite = grassSprites[UnityEngine.Random.Range(0, grassSprites.Length)];
                            tileGameObjects[new Vector2Int(x, y)] = PlaceTile(groundGrassPrefab, GetWorldPosition(x, y), parent, tileSprite);
                            tileGameObjects[new Vector2Int(x, y)].gridLocation = new Vector2Int(x, y);
                            WalkableTile walkabletile = tileGameObjects[new Vector2Int(x, y)].GetComponent<WalkableTile>();
                            walkabletile.walkable = false;

                            prefab = treePrefabs[UnityEngine.Random.Range(0, treePrefabs.Length)];
                            PlaceObject(prefab, GetWorldPosition(x, y), parent);
                            break;

                        default:
                            break;
                    }
                }
            }
        }
        tilesFilled = true;
    }

    private void DestroyTiles()
    {
        GameObject parent = GameObject.Find("GroundTiles");

        if (parent != null)
            Destroy(parent);

        tilesFilled = false;
    }

    private void OnDrawGizmos()
    {
        if (map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (map.grid[x, y] == 0) Gizmos.color = Color.yellow;
                    if (map.grid[x, y] == 1) Gizmos.color = Color.blue;
                    if (map.grid[x, y] == 2) Gizmos.color = Color.green;

                    Gizmos.DrawCube(GetWorldPosition(x, y), new Vector3(cellSize, cellSize));
                }
            }
        }
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + new Vector3(cellSize, cellSize)/2;
    }

    public void SetMapValue(int x, int y, int value)
    {
        if (x >= 0 && x <= width && y >= 0 && y <= height && value >= 0)
            map.grid[x, y] = value;
    }

    public void SetMapValue(Vector3 position, int value)
    {
        Vector3Int result = GetXY(position);
        SetMapValue(result.x, result.y, value);
    }

    private Vector3Int GetXY(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / cellSize);
        int y = Mathf.FloorToInt(position.y / cellSize);

        return new Vector3Int(x, y);
    }

    public int GetMapValue(int x, int y)
    {
        if (x >= 0 && x <= width && y >= 0 && y <= height)
            return map.grid[x, y];
        else return -1;
    }

    public int GetMapValue(Vector3 position)
    {
        Vector3Int result = GetXY(position);

        return GetMapValue(result.x, result.y);
    }

    public Vector3Int GetMapPosition(Vector3 position) => GetXY(position);

    public TerrainTile PlaceTile(GameObject prefab, Vector3 position, GameObject parent, Sprite sprite)
    {
        TerrainTile tile = Instantiate(prefab, position, Quaternion.identity).GetComponent<TerrainTile>();
        tile.gameObject.name = $"{position.x} | {position.y}";
        tile.transform.parent = parent.transform;
        SpriteRenderer spriteRenderer = tile.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;

        return tile;
    }

    public void PlaceObject(GameObject prefab, Vector3 position, GameObject parent)
    {
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        obj.transform.parent = parent.transform;
    }

    public Vector2 GetSizeOfTheMap() => new Vector2(width, height);


    public List<TerrainTile> GetNeighbourTiles(TerrainTile currentTile, List<TerrainTile> searchableTiles)
    {

        Dictionary<Vector2Int, TerrainTile> tilesToSearch = new();

        if(searchableTiles.Count  > 0)
        {
            foreach (TerrainTile tile in searchableTiles)
            {
                tilesToSearch.Add(tile.gridLocation, tile);
            }
        }
        else
        {
            tilesToSearch = tileGameObjects;
        }

        List<TerrainTile> neighbours = new();

        //Top neighbour
        Vector2Int locationToCheck = new(currentTile.gridLocation.x,
                                         currentTile.gridLocation.y + 1);

        if (tilesToSearch.ContainsKey(locationToCheck))
        {
            neighbours.Add(tilesToSearch[locationToCheck]);
        }

        //Bottom neighbour
        locationToCheck = new Vector2Int(currentTile.gridLocation.x,
                                         currentTile.gridLocation.y - 1);

        if (tilesToSearch.ContainsKey(locationToCheck))
        {
            neighbours.Add(tilesToSearch[locationToCheck]);
        }

        //Right neighbour
        locationToCheck = new Vector2Int(currentTile.gridLocation.x + 1,
                                         currentTile.gridLocation.y);

        if (tilesToSearch.ContainsKey(locationToCheck))
        {
            neighbours.Add(tilesToSearch[locationToCheck]);
        }

        //Left neighbour
        locationToCheck = new Vector2Int(currentTile.gridLocation.x - 1,
                                         currentTile.gridLocation.y);

        if (tilesToSearch.ContainsKey(locationToCheck))
        {
            neighbours.Add(tilesToSearch[locationToCheck]);
        }

        return neighbours;
    }


    public List<TerrainTile> GetNeighboursIncludingCorners(TerrainTile originTile, int range, bool includeInitialTile = false)
    {
        List<TerrainTile> neighbours = new();
        Vector2Int tileOnGrid = originTile.gridLocation;

        for (int x = tileOnGrid.x - range; x <= tileOnGrid.x + range; x++)
        {
            for (int y = tileOnGrid.y - range; y <= tileOnGrid.y + range; y++)
            {
                if (x == tileOnGrid.x && y == tileOnGrid.y && includeInitialTile == false)
                    continue;

                if (tileGameObjects.ContainsKey(new Vector2Int(x, y)))
                    neighbours.Add(tileGameObjects[new Vector2Int(x, y)]);
            }
        }

        return neighbours;
    }

    public List<TerrainTile> GetShape(TerrainTile startingTile, SpellShapes shape, int range)
    {
        List<TerrainTile> resultShape = new();

        switch(shape)
        {
            case SpellShapes.None:
                resultShape.Add(startingTile);
                break;

            case SpellShapes.HorizontalLine:
               resultShape = GetHorizontalLineShape(startingTile, range);
                break;

            case SpellShapes.VerticalLine:
                resultShape = GetVerticalLineShape(startingTile, range);
                break;

            case SpellShapes.Square:
                resultShape = GetNeighboursIncludingCorners(startingTile, range, true);
                break;
            case SpellShapes.Diamond:
                resultShape = rangeFinder.GetTilesInRange(startingTile, range);
                break;
        }

        return resultShape;
    }

    private List<TerrainTile> GetHorizontalLineShape(TerrainTile startingTile, int range)
    {
        range /= 2;
        List<TerrainTile> resultShape = new();
        int y = startingTile.gridLocation.y;

        for (int x = startingTile.gridLocation.x - range; x <= startingTile.gridLocation.x + range; x++)
        {
            if(tileGameObjects.ContainsKey(new Vector2Int(x, y)))
            {
                resultShape.Add(tileGameObjects[new Vector2Int(x, y)]);
            }
        }

        return resultShape;
    }

    private List<TerrainTile> GetVerticalLineShape(TerrainTile startingTile, int range)
    {
        range /= 2;
        List<TerrainTile> resultShape = new();
        int x = startingTile.gridLocation.x;

        for (int y = startingTile.gridLocation.y - range; y <= startingTile.gridLocation.y + range; y++)
        {
            if (tileGameObjects.ContainsKey(new Vector2Int(x, y)))
            {
                resultShape.Add(tileGameObjects[new Vector2Int(x, y)]);
            }
        }

        return resultShape;
    }
}
