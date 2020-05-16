using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributesPanel : MonoBehaviour
{
    public static PlayerAttributesPanel Instance { get; private set; }
    public Transform parent;
    public PlayerAttributesDisplay mainPlayerAttributeDisplay;
    PlayerAttributesDisplay prefab;

    private void Awake()
    {
        Instance = this;
    }
    public void AddMainPlayerAttribute(PlayerAttributes playerAttributes, Player player)
    {
        mainPlayerAttributeDisplay.SetName(player.UserId);
        mainPlayerAttributeDisplay.Init(playerAttributes, false);
    }
    public void AddOtherPlayerAttribute(PlayerAttributes playerAttributes, Player player)
    {
        if (prefab == null)
            prefab = Resources.Load<PlayerAttributesDisplay>("PlayerAttributesDisplay");
        var display = Instantiate(prefab, parent);
        display.SetName(player.UserId);
        display.Init(playerAttributes, true);
    }
}
