using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;

public class PlayfabLogin : MonoBehaviour
{
    public TMP_InputField customIdInputField;
    public Button loginButton;
    private System.Action<LoginResult> onLoginSuccess;
    private System.Action<PlayFabError> onLoginFail;
    private void Start()
    {
        loginButton.onClick.AddListener(Login);
    }
    public void Login()
    {
        loginButton.interactable = false;
        var request = new LoginWithCustomIDRequest { CustomId = customIdInputField.text, CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, onLoginSuccess, onLoginFail);
    }
    public void InitializeCallback(System.Action<LoginResult> onLoginSuccess, System.Action<PlayFabError> onLoginFail)
    {
        this.onLoginSuccess = onLoginSuccess;
        this.onLoginSuccess += OnLoginSuccessLog;
        this.onLoginFail = onLoginFail;
        this.onLoginFail += OnLoginFailureLog;
    }
    private void OnLoginSuccessLog(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        loginButton.interactable = false;
    }

    private void OnLoginFailureLog(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
        loginButton.interactable = false;
    }
}
