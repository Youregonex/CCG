using UnityEngine;
using TMPro;
using System;

public class TurnManager : MonoBehaviour
{
    private static TurnManager instance;
    public static TurnManager Instance { get { return instance; } }

    [SerializeField] private int turnNumber = 1;
    [SerializeField] private TMP_Text turnText;

    public Action OnPlayerOneTurnStart;
    public Action OnPlayerTwoTurnStart;


    private void Awake()
    {
        instance = this;
    }

    public void StartPlayerOneTurn()
    {
        OnPlayerOneTurnStart?.Invoke();
        turnText.text = turnNumber.ToString();
    }

    public void StartPlayerTwoTurn()
    {
        OnPlayerTwoTurnStart?.Invoke();
        NextTurn();
    }

    private void NextTurn()
    {
        turnNumber++;
        UpdateText();
    }

    public void PlayerOneEndTurn()
    {
        OnPlayerOneTurnStart?.Invoke();

        if(UnitManager.Instance.selectedInstance != null)
            UnitManager.Instance.UnSelectInstance(UnitManager.Instance.selectedInstance);

        if (HandManager.Instance.selectedCard != null)
            HandManager.Instance.UnSelectCard();

        NextTurn();
    }

    private void UpdateText()
    {
        turnText.text = turnNumber.ToString();
    }

}
