using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoomManager : MonoBehaviour
{
    public GameObject panel;
    public PhotonMatchMaker matchMaker;
    public Button gameMode1Button;
    public Button gameMode2Button;
    public EquippedInventory equippedInventory;
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

        if (matchMaker.IsConnected)
        {
            SetButtonInteractable(false);
            GameStateManager.Next();
            matchMaker.JoinOrCreateRoom("DeathMatch", OnJoinSuccess, OnJoinFailed);
        }
        else
            Debug.Log("Waiting for photon connection");
    }
    private void FindTeamMatch()
    {
        if (matchMaker.IsConnected)
        {
            SetButtonInteractable(false);
            GameStateManager.Next();
            matchMaker.JoinOrCreateRoom("TeamMatch", OnJoinSuccess, OnJoinFailed);
        }
        else
            Debug.Log("Waiting for photon connection");
    }
    private void OnJoinSuccess()
    {
        var inventory = PlayerData.Get<PlayerInventory>();
        var player = PhotonMatchMaker.Instance.myPlayer.GetComponent<PhotonPlayer>();
        player.SetEquipment(inventory.MeleeWeapon, inventory.RangeWeapon);
        GameStateManager.Next();
        Debug.Log("GameStateManager.GotoNextState " + GameStateManager.CurrentState);
    }
    private void OnJoinFailed()
    {
        Debug.Log("Join failed");
        GameStateManager.Back();
        SetButtonInteractable(true);
    }

    private void SetButtonInteractable(bool isInteractable)
    {
        gameMode1Button.interactable = isInteractable;
        gameMode2Button.interactable = isInteractable;
    }
    #endregion
}
public class EquippedInventory
{
    public ItemInstance meleeWeapon;
    public ItemInstance rangeWeapon;
}