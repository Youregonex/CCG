using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HighlightedCard : MonoBehaviour
{
    [SerializeField] private Image sprite;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text typeText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text movementText;

    [SerializeField] private Image attackImage;
    [SerializeField] private Image healthImage;
    [SerializeField] private Image movementImage;

    public void InitializeCardVisual(BaseCardDataSO cardSO, Color color)
    {
        GetComponent<Image>().color = color;

        if(cardSO.GetType() == typeof(CreatureCardDataSO))
        {
            attackImage.gameObject.SetActive(true);
            healthImage.gameObject.SetActive(true);
            movementImage.gameObject.SetActive(true);

            titleText.text = cardSO.title;
            typeText.text = cardSO.type;
            descriptionText.text = cardSO.description;
            costText.text = cardSO.cost.ToString();
            sprite.sprite = cardSO.cardSprite;

            CreatureCardDataSO creatureSO = (CreatureCardDataSO)cardSO;

            healthText.text = creatureSO.health.ToString();
            attackText.text = creatureSO.attack.ToString();
            movementText.text = creatureSO.movement.ToString();
        }

        if(cardSO.GetType() == typeof(SpellCardDataSO))
        {
            attackImage.gameObject.SetActive(false);
            healthImage.gameObject.SetActive(false);
            movementImage.gameObject.SetActive(false);

            titleText.text = cardSO.title;
            typeText.text = cardSO.type;
            descriptionText.text = cardSO.description;
            costText.text = cardSO.cost.ToString();
            sprite.sprite = cardSO.cardSprite;

        }

        if (cardSO.GetType() == typeof(BuildingCardDataSO))
        {
            attackImage.gameObject.SetActive(false);
            movementImage.gameObject.SetActive(false);
            healthImage.gameObject.SetActive(true);

            titleText.text = cardSO.title;
            typeText.text = cardSO.type;
            descriptionText.text = cardSO.description;
            costText.text = cardSO.cost.ToString();
            sprite.sprite = cardSO.cardSprite;
            BuildingCardDataSO buildSO = (BuildingCardDataSO)cardSO;

            healthText.text = buildSO.health.ToString();

        }
    }

    protected void Awake()
    {
        gameObject.SetActive(false);
    }

}
