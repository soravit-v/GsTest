using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    public InventoryItemButton itemButtonPrefab;
    public Transform panelParent;
    private Dictionary<string, InventoryItemButton> buttonCollection;
    private void Start()
    {
        var inventory = PlayerData.Get<PlayerInventory>();
        buttonCollection = new Dictionary<string, InventoryItemButton>();
        if (inventory.Connected)
            GenerateCatalogItemButton(inventory.itemInstances);
        PlayerData.Get<PlayerInventory>().onItemUpdate += GenerateCatalogItemButton;
    }
    private void GenerateCatalogItemButton(List<ItemInstance> itemInstances)
    {
        var hashset = CombineItemInstances(itemInstances);
        foreach (var item in hashset)
        {
            if (buttonCollection.ContainsKey(item.ItemId))
            {
                buttonCollection[item.ItemId].UpdateText();
            }
            else
            {
                var button = Instantiate(itemButtonPrefab, panelParent);
                button.Initialize(item);
                button.gameObject.SetActive(true);
                buttonCollection.Add(item.ItemId, button);
            }
        }
    }
    private HashSet<ItemInstance> CombineItemInstances(List<ItemInstance> itemInstances)
    {
        var hashset = new HashSet<ItemInstance>();
        foreach (var item in itemInstances)
            hashset.Add(item);
        return hashset;
    }
    private void ClearChilds()
    {
        for (int i = panelParent.childCount; i > 0; i--)
            Destroy(panelParent.GetChild(i));
    }
}
