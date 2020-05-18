using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayingPanel : MonoBehaviour
{
    [Inject]
    IMatchMaker matchMaker;
    public GameObject panel;
    public PlayerAttributesDisplay attributesDisplay;
    void Start()
    {
        OnStateChange(GameStateManager.CurrentState);
        GameStateManager.Subscribe(OnStateChange);
    }
    void OnStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                panel.SetActive(true);
                attributesDisplay.Init(matchMaker.PlayerObject.GetComponent<PlayerAttributes>(), false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case GameState.ShowingResult:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                break;
            default:
                panel.SetActive(false);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                break;
        }
    }
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            OnStateChange(GameStateManager.CurrentState);
        }
    }
}
