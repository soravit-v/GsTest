using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingPanel : MonoBehaviour
{
    public GameObject panel;
    public PlayerAttributesDisplay attributesDisplay;
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
                attributesDisplay.Init(PhotonMatchMaker.Instance.myPlayer.GetComponent<PlayerAttributes>(), false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
                break;
            case GameState.ShowingResult:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
            default:
                panel.SetActive(false);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
        }
    }
}
