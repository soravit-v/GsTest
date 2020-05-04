using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;

public class GameStarter : MonoBehaviour
{
    public PlayfabLogin playfabLogin;
    public GameObject purchasingPanel;
    void Start()
    {
        playfabLogin.gameObject.SetActive(true);
        purchasingPanel.gameObject.SetActive(false);
        playfabLogin.InitializeCallback(OnLoginSuccess, OnLoginFail);
    }
    public async void OnLoginSuccess(LoginResult loginResult)
    {
        await PlayerData.OnPlayfabConnected();
        playfabLogin.gameObject.SetActive(false);
        purchasingPanel.gameObject.SetActive(true);
    }
    public void OnLoginFail(PlayFabError onLoginFail)
    {

    }
}
