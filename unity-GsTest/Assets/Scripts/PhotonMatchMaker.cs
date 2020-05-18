using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using Zenject;

public class PhotonMatchMaker : MonoBehaviourPunCallbacks, IMatchMaker
{
    public GameObject PlayerObject { get; private set; }
    private System.Action onJoinedSuccess;
    private System.Action onJoinedFail;
    List<Player> players;
    private void Start()
    {
        GameStateManager.Subscribe(OnStateChange);
        PhotonNetwork.ConnectUsingSettings();
    }
    public bool IsConnected => PhotonNetwork.IsConnectedAndReady;

    public List<Player> Players => players;

    public override void OnConnectedToMaster()
    {
        Debug.Log($"OnConnect to master <{PhotonNetwork.LocalPlayer.UserId}>");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError("JoinRandom room failed " + returnCode + " " + message);
        onJoinedFail?.Invoke();
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Room joined");
        players = new List<Player>(PhotonNetwork.PlayerList);
        foreach (var player in players)
            Debug.Log("Player " + player.UserId);
        var prefabPath = "Player";
        var spawnPoints = FindObjectOfType<SceneSpawnPoints>();
        PlayerObject = PhotonNetwork.Instantiate(prefabPath, spawnPoints.GetRandomSpawnPosition(), Quaternion.identity);
        onJoinedSuccess?.Invoke();
    }
    public void JoinRoom(string roomName, RoomOptions roomOptions, Action onSuccess, Action onFail)
    {
        onJoinedSuccess = onSuccess;
        onJoinedFail = onFail;
        var typeLobby = new TypedLobby(roomName, LobbyType.Default);
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typeLobby);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        players.Add(newPlayer);
        Debug.Log($"Player <{newPlayer.UserId}> joined");
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        players.Remove(otherPlayer);
        Debug.Log($"Player <{otherPlayer.UserId}> left");
    }
    public void JoinOrCreateRoom(string roomName, System.Action onSuccess, System.Action onFail)
    {
        onJoinedSuccess = onSuccess;
        onJoinedFail = onFail;
        var roomOption = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 12 };
        var typeLobby = new TypedLobby(roomName, LobbyType.Default);
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOption, typeLobby);
    }
    public void LeaveRoom()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.NetworkingClient.Server != ServerConnection.MasterServer)
            PhotonNetwork.LeaveRoom();
    }
    void OnStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.Preparing:
                LeaveRoom();
                break;
        }
    }
}
