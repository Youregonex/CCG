using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public abstract class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectable
{
    [SerializeField] protected string cardTitle;
    [SerializeField] protected string cardType;
    [SerializeField] protected string cardDescription;
    [SerializeField] protected string cardCost;

    [SerializeField] protected BaseCardDataSO cardSO;

    [SerializeField] protected Image cardSprite;
    [SerializeField] protected TMP_Text titleText;
    [SerializeField] protected TMP_Text typeText;
    [SerializeField] protected TMP_Text descriptionText;
    [SerializeField] protected TMP_Text costText;
    [SerializeField] protected Resources resourceRequired;

    [SerializeField] protected bool isHighlighted = false;
    [SerializeField] protected bool isSelected = false;

    protected Color baseColor;
    protected Color highlitColor = Color.white;
    protected Color selectColor = Color.yellow;

    public static Action OnCardPlayed;

    protected virtual void Awake()
    {
        baseColor = GetComponent<Image>().color;
    }

    protected virtual void Update()
    {
        ManageColor();

        if (isHighlighted)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(!HandManager.Instance.isCardSelected && UnitManager.Instance.selectedInstance == null)
                {
                    SetSelected();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            SetUnSelected();
        }
    }

    protected void ManageColor()
    {
        if (isSelected)
        {
            SetColor(selectColor);
        }
        else if (isHighlighted)
        {
            SetColor(highlitColor);
        }
        else SetColor(baseColor);
    }


    protected virtual void CopySOData(BaseCardDataSO cardSO)
    {
        cardTitle = cardSO.title;
        cardSprite.sprite = cardSO.cardSprite;
        cardType = cardSO.type;
        cardDescription = cardSO.description;
        cardCost = cardSO.cost.ToString();
        resourceRequired = cardSO.resourceRequired;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (HandManager.Instance.selectedCard == null)
        {
            isHighlighted = true;
            HandManager.Instance.ShowHighlightedCard(this);
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected)
        {
            isHighlighted = false;
            HandManager.Instance.HideHighlightedCard();
        }
    }

    public virtual void InitializeCardVisual(BaseCardDataSO cardSO)
    {
        CopySOData(cardSO);
        titleText.text = cardTitle;
        typeText.text = cardType;
        descriptionText.text = cardDescription;
        costText.text = cardCost;
    }

    public virtual BaseCardDataSO GetCardSO() => this.cardSO;


    public virtual void SetCardSO(BaseCardDataSO cardSO)
    {
        this.cardSO = cardSO;
    }

    public string GetCardType() => cardType;


    public void SetColor(Color color)
    {
        GetComponent<Image>().color = color;
    }

    public abstract void PlayCard(TerrainTile tile);

    public void SetSelected()
    {
        HandManager.Instance.SelectCard(this);
        isHighlighted = false;
        isSelected = true;
    }

    public void SetUnSelected()
    {
        HandManager.Instance.UnSelectCard();
        isSelected = false;
        isHighlighted = false;
    }
}
