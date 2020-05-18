using Photon.Realtime;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LobbyManager : MonoBehaviour
{
    [Inject]
    IMatchMaker matchMaker;
    public GameObject panel;
    public Button startMatchingButton;
    public EquippedInventory equippedInventory;
    public AlertPopup popup;
    void Start()
    {
        OnStateChange(GameStateManager.CurrentState);
        GameStateManager.Subscribe(OnStateChange);
        startMatchingButton.onClick.AddListener(FindDeathMatch);
    }
    private void OnStateChange(GameState state)
    {
        panel.SetActive(state == GameState.Preparing || state == GameState.FindingMatch);
        if (state == GameState.Preparing)
            SetButtonInteractable(true);
    }
    private bool IsReadyToFindMatch(out string errorMessage)
    {
        errorMessage = "";
        var inventory = PlayerData.Get<PlayerInventory>();
        if (inventory.MeleeWeapon == null || inventory.RangeWeapon == null)
        {
            errorMessage = "Weapon is not equipped";
            return false;
        }
        return true;
    }
    public void ShowPopup(string message)
    {
        popup.ShowPopup(message);
    }
    #region MatchMaking
    private void FindDeathMatch()
    {
        if (matchMaker.IsConnected)
        {
            if (IsReadyToFindMatch(out string error))
            {
                SetButtonInteractable(false);
                GameStateManager.Next();
                var roomOption = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 12 };
                matchMaker.JoinRoom("DeathMatch", roomOption, OnJoinSuccess, OnJoinFailed);
            }
            else
                ShowPopup(error);
        }
        else
            ShowPopup("Waiting for photon connection");
    }
    private void OnJoinSuccess()
    {
        var inventory = PlayerData.Get<PlayerInventory>();
        var player = matchMaker.PlayerObject.GetComponent<PhotonPlayer>();
        player.SetEquipment(inventory.MeleeWeapon, inventory.RangeWeapon);
        GameStateManager.Next();
    }
    private void OnJoinFailed()
    {
        Debug.Log("Join failed");
        GameStateManager.Back();
        SetButtonInteractable(true);
    }
    private void SetButtonInteractable(bool isInteractable)
    {
        startMatchingButton.interactable = isInteractable;
    }
    #endregion
}
public class EquippedInventory
{
    public ItemInstance meleeWeapon;
    public ItemInstance rangeWeapon;
}