using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PlayFab.ClientModels;

public class InventoryItemButton : MonoBehaviour
{
    public TMP_Text itemIdTextMesh;
    public TMP_Text itemCountTextMesh;
    public Button button;
    public ItemInstance itemInstance;
    public GameObject equippedPanel;

    private void Awake()
    {
        Initialize(itemInstance);
    }

    public void Initialize(ItemInstance itemInstance)
    {
        this.itemInstance = itemInstance;
        UpdateText();
        UpdateEquippedStatus();
        if (itemInstance == null)
            return;
        var inventory = PlayerData.Get<PlayerInventory>();
        if (inventory.IsEquipped(itemInstance.ItemInstanceId))
            inventory.onEquippedItemChange += UpdateEquippedStatus;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(EquipItem);
    }
    public void UpdateText()
    {
        if (itemInstance != null)
        {
            if (itemIdTextMesh)
                itemIdTextMesh.text = itemInstance.DisplayName;
            if (itemCountTextMesh)
                itemCountTextMesh.text = itemInstance.RemainingUses.ToString();
        }
        else
        {
            if (itemIdTextMesh)
                itemIdTextMesh.text = "NONE";
            if (itemCountTextMesh)
                itemCountTextMesh.text = "";
        }
        button.interactable = itemInstance != null;
    }
    public void UpdateEquippedStatus()
    {
        var inventory = PlayerData.Get<PlayerInventory>();
        if (itemInstance != null && inventory.IsEquipped(itemInstance.ItemInstanceId))
        {
            equippedPanel.SetActive(true);
            inventory.onEquippedItemChange -= UpdateEquippedStatus;
            inventory.onEquippedItemChange += UpdateEquippedStatus;
        }
        else
        {
            equippedPanel.SetActive(false);
            inventory.onEquippedItemChange -= UpdateEquippedStatus;
        }

    }
    public void EquipItem()
    {
        PlayerData.Get<PlayerInventory>().EquipItem(itemInstance);
        UpdateEquippedStatus();
    }
}
