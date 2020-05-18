using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayfabLogin : MonoBehaviour
{
    public GameObject panel;
    public TMP_InputField customIdInputField;
    public Button loginButton;
    public bool autoLogin;
    private System.Action onLoginSuccess;
    private System.Action<PlayFabError> onLoginFail;
    private string _playFabPlayerIdCache;
    private void Start()
    {
        loginButton.onClick.AddListener(Login);
        customIdInputField.text = PlayerPrefs.GetString("customId");
        OnStateChange(GameStateManager.CurrentState);
        GameStateManager.Subscribe(OnStateChange);
    }
    public void Login()
    {
        loginButton.interactable = false;
        var request = new LoginWithCustomIDRequest { CustomId = customIdInputField.text, CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, RequestPhotonToken, onLoginFail);
    }
    public void InitializeCallback(System.Action onLoginSuccess, System.Action<PlayFabError> onLoginFail)
    {
        this.onLoginSuccess = onLoginSuccess;
        this.onLoginFail = onLoginFail;
        this.onLoginFail += OnLoginFailureLog;
        var customId = PlayerPrefs.GetString("customId");
        if (!string.IsNullOrEmpty(customId) && autoLogin)
            Login();

    }
    private void OnLoginSuccessLog(GetPhotonAuthenticationTokenResult result)
    {
        PlayerPrefs.SetString("customId", customIdInputField.text);
        Debug.Log($"Logged in");
        loginButton.interactable = false;
        onLoginSuccess?.Invoke();
    }


    private void RequestPhotonToken(LoginResult obj)
    {
        Debug.Log("PlayFab authenticated. Requesting photon token...");
        //We can player PlayFabId. This will come in handy during next step
        _playFabPlayerIdCache = obj.PlayFabId;

        PlayFabClientAPI.GetPhotonAuthenticationToken(new GetPhotonAuthenticationTokenRequest()
        {
            PhotonApplicationId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime
        }, OnLoginSuccessLog, OnLoginFailureLog);
    }
    private void AuthenticateWithPhoton(GetPhotonAuthenticationTokenResult obj)
    {
        Debug.Log("Photon token acquired: " + obj.PhotonCustomAuthenticationToken + "  Authentication complete.");

        //We set AuthType to custom, meaning we bring our own, PlayFab authentication procedure.
        var customAuth = new AuthenticationValues { AuthType = CustomAuthenticationType.Custom };
        //We add "username" parameter. Do not let it confuse you: PlayFab is expecting this parameter to contain player PlayFab ID (!) and not username.
        customAuth.AddAuthParameter("username", _playFabPlayerIdCache);    // expected by PlayFab custom auth service

        //We add "token" parameter. PlayFab expects it to contain Photon Authentication Token issues to your during previous step.
        customAuth.AddAuthParameter("token", obj.PhotonCustomAuthenticationToken);

        //We finally tell Photon to use this authentication parameters throughout the entire application.
        PhotonNetwork.AuthValues = customAuth;
    }
    private void OnLoginFailureLog(PlayFabError error)
    {
        Debug.LogError("OnLoginFailureLog: " + error.GenerateErrorReport());
        loginButton.interactable = false;
    }
    private void OnStateChange(GameState state)
    {
        panel.SetActive(state == GameState.Connecting);
    }

}
