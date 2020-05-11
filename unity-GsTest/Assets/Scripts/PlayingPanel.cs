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
                attributesDisplay.playerAttributes = PhotonMatchMaker.Instance.myPlayer.GetComponent<PlayerAttributes>();
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
