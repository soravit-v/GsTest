using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryItemButton : MonoBehaviour
{
    public TMP_Text itemIdTextMesh;
    public TMP_Text itemCountTextMesh;
    public Button button;
    public string itemId;

    public void Initialize(string itemId)
    {
        this.itemId = itemId;
        UpdateText();
    }
    public void UpdateText()
    {
        itemIdTextMesh.text = itemId;
        itemCountTextMesh.text = PlayerData.Get<PlayerInventory>().GetItemAmount(itemId).ToString();
    }
}
