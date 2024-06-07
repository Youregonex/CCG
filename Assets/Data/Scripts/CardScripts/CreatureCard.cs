using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

public class CreatureCard : Card
{
    [SerializeField] protected string cardHealth;
    [SerializeField] protected string cardAttack;
    [SerializeField] protected string cardMovement;
    [SerializeField] protected string cardAttackRange;
    [SerializeField] protected string attacksPerTurn;
    [SerializeField] protected DamageType damageType;

    [SerializeField] protected TMP_Text healthText;
    [SerializeField] protected TMP_Text attackText;
    [SerializeField] protected TMP_Text movementText;

    [SerializeField] protected Unit unitPrefab;
    [SerializeField] protected Sprite unitSprite;
    [SerializeField] protected List<StatusEffect> statusEffects;

    protected override void CopySOData(BaseCardDataSO cardSO)
    {
        base.CopySOData(cardSO);

        cardHealth      = ((CreatureCardDataSO)cardSO).health.ToString();
        cardAttack      = ((CreatureCardDataSO)cardSO).attack.ToString();
        cardMovement    = ((CreatureCardDataSO)cardSO).movement.ToString();
        attacksPerTurn  = ((CreatureCardDataSO)cardSO).attacksPerTurn.ToString();
        cardAttackRange = ((CreatureCardDataSO)cardSO).attackRange.ToString();
        unitPrefab      = ((CreatureCardDataSO)cardSO).unitPrefab;
        unitSprite      = ((CreatureCardDataSO)cardSO).unitSprite;
        damageType      = ((CreatureCardDataSO)cardSO).damageType;

        if (((CreatureCardDataSO)cardSO).statusEffects.Count > 0)
            foreach (StatusEffect statusEffect in ((CreatureCardDataSO)cardSO).statusEffects)
            {
                statusEffects.Add(statusEffect);
            }
    }

    public override void InitializeCardVisual(BaseCardDataSO cardSO)
    {
        CopySOData(cardSO);

        titleText.text = cardTitle;
        typeText.text = cardType;
        descriptionText.text = cardDescription;
        costText.text = cardCost;
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

                    UnitManager.Instance.unitPreview = CreatePreview();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            HandManager.Instance.UnSelectCard();
            isSelected = false;
            isHighlighted = false;

            if (UnitManager.Instance.unitPreview != null)
                DeletePreview(UnitManager.Instance.unitPreview);
        }
    }

    public override void PlayCard(TerrainTile tile)
    {
        if (ResourceManager.Instance.TrySpendResource(resourceRequired, Convert.ToInt32(cardCost)))
        {
            Unit unit = Instantiate(unitPrefab, tile.transform.position, Quaternion.identity).GetComponent<Unit>();

            unit.InitializeUnit(cardTitle, Convert.ToInt32(cardAttack), Convert.ToInt32(attacksPerTurn), Convert.ToInt32(cardHealth),
                                Convert.ToInt32(cardMovement), unitSprite, Convert.ToInt32(cardAttackRange),
                                damageType, Factions.Player, ((CreatureCardDataSO)cardSO).unitAnimController, statusEffects);

            tile.someoneOnTop = unit;
            unit.TileBelow = tile;
            isSelected = false;
            OnCardPlayed();
            Destroy(gameObject);
        }
        else
        {
            string str = "Not enough resources";
            TextManager.Instance.ShowWarningText(str);
            isSelected = false;
        }

        DeletePreview(UnitManager.Instance.unitPreview);
        UnitManager.Instance.unitPreview = null;
    }

    public Unit CreatePreview()
    {
        Unit unit = Instantiate(unitPrefab, transform.position, Quaternion.identity);

        unit.GetComponent<SpriteRenderer>().sprite = unitSprite;
        unit.GetComponent<SpriteRenderer>().sortingOrder = 2;
        unit.gameObject.SetActive(false);

        return unit;
    }

    public void DeletePreview(Unit unit)
    {
        Destroy(unit.gameObject);
    }
}
