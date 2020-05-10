using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributesPanel : MonoBehaviour
{
    public Transform parent;
    PlayerAttributesDisplay prefab;
    public void AddPlayerAttribute(PlayerAttributes playerAttributes, Player player)
    {
        if (prefab == null)
            prefab = Resources.Load<PlayerAttributesDisplay>("PlayerAttributesDisplay");
        var display = Instantiate(prefab, parent);
        display.SetName(player.UserId);
        display.playerAttributes = playerAttributes;
    }
}
