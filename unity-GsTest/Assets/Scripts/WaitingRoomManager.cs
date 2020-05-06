using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoomManager : MonoBehaviour
{
    public GameObject panel;
    public Button gameMode1Button;
    public Button gameMode2Button;
    void Start()
    {
        OnStateChange(GameStateManager.CurrentState);
        GameStateManager.onStateChange += OnStateChange;
        gameMode1Button.onClick.AddListener(FindDeathMatch);
        gameMode2Button.onClick.AddListener(FindTeamMatch);
    }

    private void OnStateChange(GameState state)
    {
        panel.SetActive(state == GameState.Waiting || state == GameState.FindingMatch);
    }
    #region MatchMaking
    private void FindDeathMatch()
    {
        //photon baby
    }
    private void FindTeamMatch()
    {
        //photon baby
    }
    #endregion
}
