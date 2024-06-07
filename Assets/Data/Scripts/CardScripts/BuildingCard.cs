using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

public class BuildingCard : Card
{
    [SerializeField] protected TMP_Text healthText;
    [SerializeField] protected string maxHealth;
    [SerializeField] protected Building buildingPrefab;
    [SerializeField] protected Sprite buildingSprite;

    protected override void CopySOData(BaseCardDataSO cardSO)
    {
        base.CopySOData(cardSO);

        maxHealth = ((BuildingCardDataSO)cardSO).health.ToString();
        buildingPrefab = ((BuildingCardDataSO)cardSO).buildingPrefab;
        buildingSprite = ((BuildingCardDataSO)cardSO).buildingSprite;
    }

    public override void InitializeCardVisual(BaseCardDataSO cardSO)
    {
        base.InitializeCardVisual(cardSO);
        healthText.text = maxHealth;
    }

    protected override void Update()
    {
        ManageColor();

        if (isHighlighted)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (!HandManager.Instance.isCardSelected && UnitManager.Instance.selectedInstance == null)
                {
                    HandManager.Instance.SelectCard(this);
                    isHighlighted = false;
                    isSelected = true;
                    UnitManager.Instance.buildingPreview = CreatePreview();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            HandManager.Instance.UnSelectCard();
            isSelected = false;
            isHighlighted = false;

            if(UnitManager.Instance.buildingPreview != null)
            DeletePreview(UnitManager.Instance.buildingPreview);
        }
    }

    public override void PlayCard(TerrainTile tile)
    {
        if (ResourceManager.Instance.TrySpendResource(resourceRequired, Convert.ToInt32(cardCost)))
        {
            Building building = Instantiate(buildingPrefab, tile.transform.position, Quaternion.identity).GetComponent<Building>();
            building.InitializeBuilding(cardTitle, Convert.ToInt32(maxHealth), buildingSprite, Factions.Player);
            building.transform.localScale *= .8f;
            List<TerrainTile> tilesBelow = TileManager.Instance.GetShape(tile, ((BuildingCardDataSO)cardSO).buildingShape, ((BuildingCardDataSO)cardSO).size);

            foreach(TerrainTile currentTile in tilesBelow)
            {
                currentTile.someoneOnTop = building;
            }

            building.tilesBelow = tilesBelow;

            isSelected = false;
            Destroy(gameObject);
            OnCardPlayed();
        }
        else
        {
            string str = "Not enough resources";
            TextManager.Instance.ShowWarningText(str);
            isSelected = false;
        }

        DeletePreview(UnitManager.Instance.buildingPreview);
        UnitManager.Instance.buildingPreview = null;
    }

    public Building CreatePreview()
    {
        Building building = Instantiate(buildingPrefab, transform.position, Quaternion.identity);

        building.GetComponent<SpriteRenderer>().sprite = buildingSprite;
        building.GetComponent<SpriteRenderer>().sortingOrder = 2;
        building.transform.localScale *= .8f;
        building.gameObject.SetActive(false);

        return building;
    }

    public void DeletePreview(Building building)
    {
        Destroy(building.gameObject);
    }
}
