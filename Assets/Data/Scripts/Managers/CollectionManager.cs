using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    private static CollectionManager instance;
    public static CollectionManager Instance { get { return instance; } }

    public List<CreatureCardDataSO> creatureCards;
    public List<SpellCardDataSO> spellCards;
    public List<BuildingCardDataSO> buildingCards;

    [SerializeField]
    private List<HighlightedCard> cardPreviews;

    [SerializeField]
    private Color creatureColor = new Color(255, 0, 4);
    [SerializeField]
    private Color spellColor = new Color(159, 7, 156);
    [SerializeField]
    private Color buildingColor = new Color(106, 101, 101);

    private void Awake()
    {
        instance = this;
    }

    public void ShowCreatureCards()
    {
        HideAllCards();
        for (int i = 0; i < creatureCards.Count; i++)
        {
            cardPreviews[i].gameObject.SetActive(true);
            cardPreviews[i].InitializeCardVisual(creatureCards[i], creatureColor);
        }
    }

    public void ShowSpellCards()
    {
        HideAllCards();
        for (int i = 0; i < spellCards.Count; i++)
        {
            cardPreviews[i].gameObject.SetActive(true);
            cardPreviews[i].InitializeCardVisual(spellCards[i], spellColor);
        }
    }

    public void ShowBuildingCards()
    {
        HideAllCards();
        for (int i = 0; i < buildingCards.Count; i++)
        {
            cardPreviews[i].gameObject.SetActive(true);
            cardPreviews[i].InitializeCardVisual(buildingCards[i], buildingColor);
        }
    }

    public void HideAllCards()
    {
        for (int i = 0; i < cardPreviews.Count; i++)
        {
            cardPreviews[i].gameObject.SetActive(false);
        }
    }
}
