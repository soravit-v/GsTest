using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    public GameObject panel;
    public Button nextStateButton;
    void Start()
    {
        OnStateChange(GameStateManager.CurrentState);
        GameStateManager.onStateChange += OnStateChange;
        nextStateButton.onClick.AddListener(()=> GameStateManager.GameEnd());
    }
    void OnStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.ShowingResult:
                panel.SetActive(true);
                break;
            default:
                panel.SetActive(false);
                break;
        }
    }
}
