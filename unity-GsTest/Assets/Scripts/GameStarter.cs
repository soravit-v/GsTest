using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;
using PlayFab.Json;

public class GameStarter : MonoBehaviour
{
    public PlayfabLogin playfabLogin;
    void Start()
    {
        playfabLogin.InitializeCallback(OnLoginSuccess, OnLoginFail);
    }
    public async void OnLoginSuccess(LoginResult loginResult)
    {
        await PlayerData.OnPlayfabConnected();
        GiveStartingCurrency();
        GameStateManager.GotoNextState();
    }
    private void GiveStartingCurrency()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "AddLoginCurrency",
            FunctionParameter = new {  },
            GeneratePlayStreamEvent = true,
        },
        result => 
        {
            var jsonObj = (JsonObject)result.FunctionResult;
            Debug.Log("Login success "+ jsonObj["messageValue"]);
            PlayerData.Get<PlayerInventory>().UpdateInventoryAsync();
        },
        error => 
        {
            Debug.LogError("AddLoginCurrency Fail");
        });
    }
    public void OnLoginFail(PlayFabError onLoginFail)
    {

    }
}
