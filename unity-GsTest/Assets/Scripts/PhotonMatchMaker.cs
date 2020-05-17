using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonMatchMaker : MonoBehaviourPunCallbacks
{
    public static PhotonMatchMaker Instance;
    public GameObject myPlayer;
    private System.Action onJoinedSuccess;
    private System.Action onJoinedFail;
    List<Player> players;
    private void Start()
    {
        Instance = this;
        GameStateManager.onStateChange += OnStateChange;
        PhotonNetwork.ConnectUsingSettings();
    }
    public bool IsConnected => PhotonNetwork.IsConnectedAndReady;
    public override void OnConnectedToMaster()
    {
        Debug.Log($"OnConnect to master <{PhotonNetwork.LocalPlayer.UserId}>");
    }

    public void JoinRandomRoom(System.Action onSuccess, System.Action onFail)
    {
        onJoinedSuccess = onSuccess;
        onJoinedFail = onFail;
        PhotonNetwork.JoinRandomRoom();
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
        myPlayer = PhotonNetwork.Instantiate(prefabPath, spawnPoints.GetRandomSpawnPosition(), Quaternion.identity);
        onJoinedSuccess?.Invoke();
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
            case GameState.Waiting:
                LeaveRoom();
                break;
        }
    }
}
