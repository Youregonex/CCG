using UnityEngine;
using UnityEngine.UI;

public class HandManager : MonoBehaviour
{
    private static HandManager instance;
    public static HandManager Instance { get { return instance; } }

    [SerializeField] private Deck deck;
    [SerializeField] private int maxHandSize;

    public HighlightedCard highlightedCard;
    public TerrainTile clickedTile;
    public Card selectedCard;
    public bool isCardSelected;

    private void Awake()
    {
        instance = this;
        TurnManager.Instance.OnPlayerOneTurnStart += AddFirstCardToHand;
    }

    private void OnDestroy()
    {
        TurnManager.Instance.OnPlayerOneTurnStart -= AddFirstCardToHand;
    }

    public void ShowHighlightedCard(Card card)
    {
        highlightedCard.gameObject.SetActive(true);
        highlightedCard.InitializeCardVisual(card.GetCardSO(), card.GetComponent<Image>().color);
    }

    public void HideHighlightedCard()
    {
        highlightedCard.gameObject.SetActive(false);
    }

    public void SelectCard(Card card)
    {
        HideHighlightedCard();

        if (card != null)
            selectedCard = card;
    }

    public void UnSelectCard()
    {
        selectedCard = null;
        clickedTile = null;
    }

    public void AddFirstCardToHand()
    {
        if (deck.GetCurrentDeck().Count <= 0 || transform.childCount >= maxHandSize)
            return;

        BaseCardDataSO cardSO = deck.GetCurrentDeck()[0];

        Card card = Instantiate(cardSO.cardPrefab, deck.gameObject.transform.position, Quaternion.identity).GetComponent<Card>();
        card.transform.SetParent(transform);
        card.transform.localScale = Vector3.one;
        card.SetCardSO(cardSO);
        card.InitializeCardVisual(cardSO);

        deck.DrawCard();
    }

    public void Update()
    {
        isCardSelected = selectedCard == null ? false : true;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddFirstCardToHand();
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            AddCardsToHand(3);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (selectedCard != null && clickedTile != null)
            {
                selectedCard.PlayCard(clickedTile);
                UnSelectCard();
                clickedTile = null;
            }
        }
    }

    public bool IsSpellSelected()
    {
        if (selectedCard != null)
            return selectedCard.GetCardSO().GetType() == typeof(SpellCardDataSO) ? true : false;

        return false;
    }

    public void AddCardsToHand(int numberOfCards)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            AddFirstCardToHand();
        }
    }

}
