using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingPanel : MonoBehaviour
{
    public GameObject panel;
    void Start()
    {
        OnStateChange(GameStateManager.CurrentState);
        GameStateManager.onStateChange += OnStateChange;
    }
    void OnStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                panel.SetActive(true);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            default:
                panel.SetActive(false);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                break;
        }
    }
}
