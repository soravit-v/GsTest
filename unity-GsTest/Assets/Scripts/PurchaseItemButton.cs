using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PlayFab.ClientModels;

public class PurchaseItemButton : MonoBehaviour
{
    public TMP_Text itemIdTextMesh;
    public TMP_Text currencyIdTextMesh;
    public Button button;
    public CatalogItem catalogItem;
    public string currencyId;
    public int price;

    public void Initialize(CatalogItem catalogItem, string currencyId)
    {
        this.catalogItem = catalogItem;
        this.currencyId = currencyId;
        //var catalogData = PlayerData.Get<PlayfabItemCatalog>().GetCatalogItemData(itemId);
        price = (int)catalogItem.VirtualCurrencyPrices[currencyId];
        UpdateText();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(PurchaseItem);
    }
    void PurchaseItem()
    {
        var request = new PurchaseItemRequest()
        {
            ItemId = catalogItem.ItemId,
            VirtualCurrency = currencyId,
            Price = price,
            CatalogVersion = "0.0.1",
        };
        PlayerData.Get<PlayerInventory>().PurchaseItem(request);
    }
    void UpdateText()
    {
        itemIdTextMesh.text = catalogItem.DisplayName;
        currencyIdTextMesh.text = price.ToString();
    }
}
