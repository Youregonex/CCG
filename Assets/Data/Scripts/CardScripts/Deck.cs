using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Deck : MonoBehaviour
{

    [SerializeField] private List<BaseCardDataSO> deckOfCards;
    [SerializeField] private BaseCardDataSO[] cardVariations;

    [SerializeField] private int maxCards;
    [SerializeField] private int cardsLeft;

    [SerializeField] private TMP_Text maxCardsText;
    [SerializeField] private TMP_Text cardsLeftText;

    private void Awake()
    {
        InitializeDeck(GenerateRandomDeck(30));
    }

    private void Start()
    {
        UpdateCardText();
    }

    private List<BaseCardDataSO> GenerateRandomDeck(int size)
    {
        List<BaseCardDataSO> newList = new List<BaseCardDataSO>();

        for (int i = 0; i < size; i++)
        {
            newList.Add(cardVariations[Random.Range(0, cardVariations.Length)]);
        }

        return newList;
    }

    private void InitializeDeck(List<BaseCardDataSO> deck)
    {
        deckOfCards = deck;
    }

    public void DrawCard()
    {
        if (deckOfCards.Count <= 0)
            return;
        
        deckOfCards.RemoveAt(0);
        Debug.Log($"В колоде осталось {deckOfCards.Count} карт");

        UpdateCardText();
    }

    public List<BaseCardDataSO> GetCurrentDeck() => deckOfCards;

    private void UpdateCardText()
    {
        cardsLeftText.text = deckOfCards.Count.ToString();
    }

}
