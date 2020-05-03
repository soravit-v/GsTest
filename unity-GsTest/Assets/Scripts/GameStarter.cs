using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;

public class GameStarter : MonoBehaviour
{
    public PlayfabLogin playfabLogin;
    void Start()
    {
        playfabLogin.gameObject.SetActive(true);
        playfabLogin.InitializeCallback(OnLoginSuccess, OnLoginFail);
    }
    public void OnLoginSuccess(LoginResult loginResult)
    {
        //start game 
        playfabLogin.gameObject.SetActive(false);
    }
    public void OnLoginFail(PlayFabError onLoginFail)
    {

    }
}
