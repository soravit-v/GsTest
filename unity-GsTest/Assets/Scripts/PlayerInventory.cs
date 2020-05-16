using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.AuthenticationModels;
using System.Threading.Tasks;
using System;

public class PlayerInventory : IPlayfabData
{
    public List<ItemInstance> itemInstances;
    public Dictionary<string, int> virtualCurrency;
    public Dictionary<string, VirtualCurrencyRechargeTime> virtualCurrencyRechargeTimes;
    public Action<Dictionary<string, int>> onCurrencyUpdate;
    public Action<List<ItemInstance>> onItemUpdate;
    public Action onEquippedItemChange;
    public ItemInstance MeleeWeapon { get; private set; }
    public ItemInstance RangeWeapon { get; private set; }
    public bool Connected { get; private set; } = false;
    public async Task OnPlayfabConnect()
    {
        await UpdateInventoryAsync();
        
    }
    public void EquipItem(ItemInstance item)
    {
        if (item.ItemClass.Equals("Equipment_melee"))
        {
            MeleeWeapon = item;
            Debug.Log("Equip melee weapon" + MeleeWeapon.DisplayName);
        }
        else if (item.ItemClass.Equals("Equipment_range"))
        {
            RangeWeapon = item;
            Debug.Log("Equip range weapon " + RangeWeapon.DisplayName);
        }
        else
            Debug.LogError("Cannot equip as weapon");
        SaveEquippedItem();
    }
    public bool IsEquipped(string instanceId)
    {
        if (MeleeWeapon != null && MeleeWeapon.ItemInstanceId == instanceId)
            return true;
        if (RangeWeapon != null && RangeWeapon.ItemInstanceId == instanceId)
            return true;
        return false;
    }
    private void LoadEquippedItem()
    {
        var meleeWeaponId = PlayerPrefs.GetString("equippedMelee");
        var rangeWeaponId = PlayerPrefs.GetString("equippedRange");
        if (!string.IsNullOrEmpty(meleeWeaponId))
        {
            MeleeWeapon = itemInstances.Find(item => item.ItemInstanceId == meleeWeaponId);
        }
        if (!string.IsNullOrEmpty(rangeWeaponId))
        {
            RangeWeapon = itemInstances.Find(item => item.ItemInstanceId == rangeWeaponId);
        }
        Debug.Log("Load cached equipped item");
        onEquippedItemChange?.Invoke();
    }
    private void SaveEquippedItem()
    {
        if (MeleeWeapon != null)
            PlayerPrefs.SetString("equippedMelee", MeleeWeapon.ItemInstanceId);
        if (RangeWeapon != null)
            PlayerPrefs.SetString("equippedRange", RangeWeapon.ItemInstanceId);
        onEquippedItemChange?.Invoke();
    }
    public Task UpdateInventoryAsync()
    {
        var task = new TaskCompletionSource<GetUserInventoryResult>();
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), GetInventorySuccess(task), GetInventoryFail);
        return task.Task;
    }

    private Action<GetUserInventoryResult> GetInventorySuccess(TaskCompletionSource<GetUserInventoryResult> task)
    {
        return result =>
        {
            task.SetResult(result);
            itemInstances = result.Inventory;
            virtualCurrency = result.VirtualCurrency;
            virtualCurrencyRechargeTimes = result.VirtualCurrencyRechargeTimes;
            Connected = true;
            LoadEquippedItem();
            onItemUpdate?.Invoke(itemInstances);
            onCurrencyUpdate?.Invoke(virtualCurrency);
        };
    }
    void GetInventoryFail(PlayFabError error)
    {
        Debug.LogError($"GetInventoryFail {error.Error} {error.ErrorMessage}");
    }

    public void PurchaseItem(string itemId, string currencyId = "CO")
    {
        var itemData = PlayerData.Get<PlayfabItemCatalog>().GetCatalogItemData(itemId);
        var request = new PurchaseItemRequest()
        {
            CatalogVersion = "0.0.1",
            VirtualCurrency = currencyId,
            Price = (int)itemData.VirtualCurrencyPrices[currencyId],
            ItemId = itemId
        };
        PlayFabClientAPI.PurchaseItem(request, PurchaseSuccess, PurchaseFail);
    }
    public void PurchaseItem(PurchaseItemRequest request)
    {
        PlayFabClientAPI.PurchaseItem(request, PurchaseSuccess, PurchaseFail);
    }
    void PurchaseSuccess(PurchaseItemResult purchaseItemResult)
    {
        string itemList = "";
        foreach (var item in purchaseItemResult.Items)
            itemList += "\n" + item;
        Debug.Log("PurchaseSuccess" + itemList);
        UpdateInventoryAsync();
    }
    void PurchaseFail(PlayFabError error)
    {
        Debug.LogError($"PurchaseFail {error.Error} {error.ErrorMessage}");
    }
    public int GetItemAmount(string itemId)
    {
        var allInstances = itemInstances.FindAll(item => item.ItemId == itemId);
        return allInstances.Count;
    }
    public int GetCurrencyAmount(string currencyId)
    {
        return virtualCurrency != null ? virtualCurrency[currencyId] : 0;
    }
}
