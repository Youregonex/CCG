using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    private static ResourceManager instance;
    public static ResourceManager Instance { get { return instance; } }

    [SerializeField] private TMP_Text currentGoldAmmountText;
    [SerializeField] private TMP_Text maxGoldAmmountText;
    [SerializeField] private TMP_Text currentCrystalAmmountText;
    [SerializeField] private TMP_Text maxCrystalAmmountText;
    [SerializeField] private int maxGold = 1;
    [SerializeField] private int maxCrystals = 1;

    private int _goldLimit = 10;
    private int _crystalsLimit = 10;

    public int CurrentCrystals { get; private set; }
    public int CurrentGold { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        TurnManager.Instance.OnPlayerOneTurnStart += OnPlayerTurnStart;
        Card.OnCardPlayed += UpdateResources;

        CurrentGold = maxGold;
        CurrentCrystals = maxCrystals;

        UpdateResources();
    }

    private void OnDestroy()
    {
        TurnManager.Instance.OnPlayerOneTurnStart -= OnPlayerTurnStart;
        Card.OnCardPlayed -= UpdateResources;
    }

    private void OnPlayerTurnStart()
    {
        if(maxGold + 1 <= _goldLimit)
            maxGold++;

        if (maxCrystals + 1 <= _crystalsLimit)
            maxCrystals++;

        CurrentGold = maxGold;
        CurrentCrystals = maxCrystals;

        UpdateResources();
    }

    private void UpdateResources()
    {
        UpdateGold();
        UpdateCrystals();
    }

    private void UpdateGold()
    {
        currentGoldAmmountText.text = CurrentGold.ToString();
        maxGoldAmmountText.text = maxGold.ToString();
    }

    private void UpdateCrystals()
    {
        currentCrystalAmmountText.text = CurrentCrystals.ToString();
        maxCrystalAmmountText.text = maxCrystals.ToString();
    }

    public bool TrySpendResource(Resources resource, int cardCost)
    {
        bool check = false;

        switch(resource)
        {
            case Resources.Crystal:
                check = CurrentCrystals >= cardCost ? true : false;
                if(check)
                {
                    CurrentCrystals -= cardCost;
                }

                break;

            case Resources.Gold:
                check = CurrentGold >= cardCost ? true : false;
                if (check)
                {
                    CurrentGold -= cardCost;
                }
                break;
        }

        return check;
    }

}
