using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMatchMaker
{
    GameObject PlayerObject { get; }
    List<Player> Players { get; }
    bool IsConnected { get; }
    void JoinRoom(string roomName, RoomOptions roomOptions, System.Action onSuccess, System.Action onFail);
    void LeaveRoom();

}
