using System;
using UnityEngine;

public class TurnManager : MonoBehaviour  
{
    public static TurnManager Instance { get; private set; }

    public enum PlayerTurns
    {
        Player1Turn = 0,
        Player2Turn
    }

    public PlayerTurns CurrentTurn { get; private set; } = PlayerTurns.Player1Turn;

    public event Action<PlayerTurns> OnTurnChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Destroyed instance");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void EndTurn()
    {
        if (CurrentTurn == PlayerTurns.Player1Turn)
            CurrentTurn = PlayerTurns.Player2Turn;
        else
            CurrentTurn = PlayerTurns.Player1Turn;

        OnTurnChanged?.Invoke(CurrentTurn);
    }

    public bool IsPlayerTurn(PlayerTurns playerTurn)
    {
        return CurrentTurn == playerTurn;
    }

}


