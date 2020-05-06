using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingPanel : MonoBehaviour
{
    void Start()
    {
        OnStateChange(GameStateManager.CurrentState);
        GameStateManager.onStateChange += OnStateChange;
    }
    void OnStateChange(GameState state)
    {

    }
}
