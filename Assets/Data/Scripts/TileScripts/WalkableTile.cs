using UnityEngine;
using System.Collections.Generic;

public class WalkableTile : TerrainTile
{
    protected List<TerrainTile> CardShapeTiles { get; set; } = new List<TerrainTile>();
    protected List<TerrainTile> path = new List<TerrainTile>();
    protected UnitManager unitManager = UnitManager.Instance;
    protected Building buildingPreview;

    protected override void OnMouseOver()
    {
        base.OnMouseOver();

        //Highlight selected unit path on mouse over
        if(unitManager.unitSelected)
        {
            Unit unit = (Unit)unitManager.selectedInstance;
            if (unitManager.selectedInstance != null && ((Unit)unitManager.selectedInstance).StatDir["Movement"].CurrentStatValue != 0)
            {
                if(unit.pathFinder.FindPath(unit.TileBelow, this, unit.TilesInRange).Count <= unit.StatDir["Movement"].CurrentStatValue)
                {
                    path = unit.pathFinder.FindPath(unit.TileBelow, this, unit.TilesInRange);

                    foreach (TerrainTile tile in path)
                    {
                        tile.highlightingObject.SetActive(true);
                    }
                }
            }
        }

        //Highlight spell shape if spell card is being held
        if (unitManager.selectedInstance == null && HandManager.Instance.isCardSelected)
        {
            if(HandManager.Instance.selectedCard != null)
                if(HandManager.Instance.selectedCard.GetType() == typeof(SpellCard))
                {
                    SpellCardDataSO spellSO = (SpellCardDataSO)HandManager.Instance.selectedCard.GetCardSO();

                    CardShapeTiles = TileManager.Instance.GetShape(this, spellSO.spellEffect.shape, spellSO.spellEffect.range);

                    HighlightTiles(CardShapeTiles, new Color(.8f, 0, 0, .5f));
                }
        }

        //Preview of building
        if(HandManager.Instance.selectedCard != null && HandManager.Instance.selectedCard.GetCardSO().GetType() == typeof(BuildingCardDataSO))
        {
            //Move building preview to highlighted tile position
            if(UnitManager.Instance.buildingPreview != null)
            {
                Building building = UnitManager.Instance.buildingPreview;

                building.transform.position = transform.position;
                building.gameObject.SetActive(true);
               
                BuildingCardDataSO buildingSO = (BuildingCardDataSO)HandManager.Instance.selectedCard.GetCardSO();
                CardShapeTiles = TileManager.Instance.GetShape(this, buildingSO.buildingShape, buildingSO.size);
                HighlightTiles(CardShapeTiles, Color.grey);

                foreach(TerrainTile tile in CardShapeTiles)
                {
                    if(!tile.walkable)
                    {
                        building.GetComponent<SpriteRenderer>().color = Color.red;
                        return;
                    }
                    else
                    {
                        building.GetComponent<SpriteRenderer>().color = Color.green;
                    }
                }
            }
        }

        //Preview of unit
        if (HandManager.Instance.selectedCard != null && HandManager.Instance.selectedCard.GetCardSO().GetType() == typeof(CreatureCardDataSO))
        {
            //Move unit preview to highlighted tile position
            if (UnitManager.Instance.unitPreview != null)
            {
                Unit unit = UnitManager.Instance.unitPreview;

                unit.transform.position = transform.position;
                unit.gameObject.SetActive(true);

                if(walkable)
                {
                    unit.GetComponent<SpriteRenderer>().color = Color.green;
                }
                else
                {
                    unit.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            //Selecting tile to play creature card
            if (unitManager.selectedInstance == null && HandManager.Instance.isCardSelected && walkable)
            {
                HandManager.Instance.clickedTile = this;
                ClearHighlightedTiles();
            }

            //Selecting tile to play spell card
            if (unitManager.selectedInstance == null && HandManager.Instance.IsSpellSelected())
            {
                HandManager.Instance.clickedTile = this;
                ClearHighlightedTiles();
            }

            //Selecting instance on clicked tile
            if (unitManager.selectedInstance == null && someoneOnTop != null && !HandManager.Instance.isCardSelected && ((IHaveFaction)someoneOnTop).Faction == Factions.Player)
            {
                unitManager.SelectInstance(someoneOnTop);
            }

            //Move unit to clicked tile
            if (unitManager.selectedInstance != null && someoneOnTop == null && walkable)
            {
                if(unitManager.unitSelected)
                {
                    Unit unit = (Unit)unitManager.selectedInstance;
                    ClearPath();
                    //If path created
                    if (unit.TrySetDistanation(this))
                    {
                        unitManager.UnSelectInstance(unit);
                    }
                }
            }

            //Clicking tile with another instance while instance selected
            if (unitManager.selectedInstance != null && someoneOnTop != null)
            {
                //Clicked on friendly instance
                if(((IHaveFaction)someoneOnTop).Faction == Factions.Player)
                {
                    unitManager.UnSelectInstance(unitManager.selectedInstance);
                    unitManager.SelectInstance(someoneOnTop);
                }

                //Clicked on enemy instance
                if (((IHaveFaction)someoneOnTop).Faction == Factions.Enemy)
                {
                    if(unitManager.unitSelected)
                    {
                        ((Unit)unitManager.selectedInstance).TryAttack(someoneOnTop, true);
                        unitManager.UnSelectInstance(unitManager.selectedInstance);
                    }
                }
            }
        }
    }

    protected override void OnMouseExit()
    {
        base.OnMouseExit();

        ClearPath();
        ClearHighlightedTiles();
    }

    protected void HighlightTiles(List<TerrainTile> tilesToHighlight, Color color)
    {
        foreach (TerrainTile currentTile in tilesToHighlight)
        {
            currentTile.SetColor(color);
        }
    }

    protected void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            highlightingObject.SetActive(false);
            ClearHighlightedTiles();
        }

        if (TileManager.Instance.map.grid[gridLocation.x, gridLocation.y] == 0)
            walkable = someoneOnTop == null ? true : false;
    }

    protected void ClearPath()
    {
        foreach (TerrainTile tile in path)
        {
            tile.highlightingObject.SetActive(false);
        }
        path.Clear();
    }

    public void ClearHighlightedTiles()
    {
        foreach(TerrainTile tile in CardShapeTiles)
        {
            tile.SetColor(baseColor);
        }    
        CardShapeTiles.Clear();
    }
}