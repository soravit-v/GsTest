using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStateManager
{
    public static GameState CurrentState { get; private set; } = GameState.Connecting;
    public static Action<GameState> onStateChange;
    public static void Next()
    {
        if (CurrentState != GameState.ShowingResult)
            CurrentState += 1;
        Debug.Log($"Goto next state {CurrentState}");
        onStateChange?.Invoke(CurrentState);
    }
    public static void Back()
    {
        if (CurrentState != GameState.Connecting)
            CurrentState -= 1;
        onStateChange?.Invoke(CurrentState);
    }
    public static void GameEnd()
    {
        CurrentState = GameState.Waiting;
        onStateChange?.Invoke(CurrentState);
    }
}
public enum GameState
{
    Connecting,
    Waiting,
    FindingMatch,
    Playing,
    ShowingResult,
}
