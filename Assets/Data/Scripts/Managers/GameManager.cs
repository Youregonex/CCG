using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance;} }

    public GameStates GameState;

    public static event Action<GameStates> OnGameStateChange;

    private void Awake()
    {
        instance= this;
    }

    private void Start()
    {
        GameState = GameStates.None;
    }

    public void UpdateGameState(GameStates newGameState)
    {
        GameState = newGameState;

        //switch (newGameState)
        //{
        //    case GameStates.None:

        //        break;

        //    case GameStates.BoardGeneration:
        //        GenerateBoard();
        //        break;

        //    case GameStates.SetupGame:

        //        break;

        //    case GameStates.PlayerTurn:

        //        break;

        //    case GameStates.SecondPlayerTurn:

        //        break;

        //    case GameStates.Victory:

        //        break;

        //    case GameStates.Loss:

        //        break;

        //    default:
        //        break;
        //}

        OnGameStateChange?.Invoke(newGameState);

    }

    private void GenerateBoard()
    {
    }

}


public enum GameStates
{
    None,
    BoardGeneration,
    SetupGame,
    PlayerTurn,
    SecondPlayerTurn,
    Victory,
    Loss
}
