﻿using UnityEngine;
public class EquipmentPanel : MonoBehaviour
{
    public InventoryItemButton meleeWeaponButton;
    public InventoryItemButton rangeWeaponButton;
    void Start()
    {
        var inventory = PlayerData.Get<PlayerInventory>();
        if (inventory.Connected)
        {
            UpdateEquippedItem();
        }
        inventory.onEquippedItemChange += UpdateEquippedItem;
    }
    private void UpdateEquippedItem()
    {
        var inventory = PlayerData.Get<PlayerInventory>();
        meleeWeaponButton.Initialize(inventory.MeleeWeapon);
        rangeWeaponButton.Initialize(inventory.RangeWeapon);
    }
}
