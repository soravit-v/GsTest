using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WeaponData
{
    public float damage;
    public float stamina;
    public float mana;
    public float speed;
    public float size;

    public WeaponData(ItemInstance itemInstance)
    {
        if (itemInstance.CustomData.ContainsKey("damage"))
        {
            damage = 0;
            float.TryParse(itemInstance.CustomData["damage"], out damage);
        }
        if (itemInstance.CustomData.ContainsKey("stamina"))
        {
            stamina = 0;
            float.TryParse(itemInstance.CustomData["stamina"], out stamina);
        }
        if (itemInstance.CustomData.ContainsKey("mana"))
        {
            mana = 0;
            float.TryParse(itemInstance.CustomData["mana"], out mana);
        }
        if (itemInstance.CustomData.ContainsKey("speed"))
        {
            speed = 0;
            float.TryParse(itemInstance.CustomData["speed"], out speed);
        }
        if (itemInstance.CustomData.ContainsKey("size"))
        {
            size = 1;
            float.TryParse(itemInstance.CustomData["size"], out size);
        }
    }
}
