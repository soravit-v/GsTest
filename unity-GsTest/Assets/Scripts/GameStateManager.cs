using System;
using UnityEngine;

public static class GameStateManager
{
    public static GameState CurrentState { get; private set; } = GameState.Connecting;
    private static Action<GameState> onStateChange;
    public static void Next()
    {
        if (CurrentState != GameState.ShowingResult)
            CurrentState += 1;
        Debug.Log($"Goto next state {CurrentState}");
        onStateChange?.Invoke(CurrentState);
    }
    public static void Subscribe(Action<GameState> action)
    {
        onStateChange += action;
    }
    public static void Back()
    {
        if (CurrentState != GameState.Connecting)
            CurrentState -= 1;
        onStateChange?.Invoke(CurrentState);
    }
    public static void GameEnd()
    {
        CurrentState = GameState.Preparing;
        onStateChange?.Invoke(CurrentState);
    }
}
public enum GameState
{
    Connecting,
    Preparing,
    FindingMatch,
    Playing,
    ShowingResult,
}
