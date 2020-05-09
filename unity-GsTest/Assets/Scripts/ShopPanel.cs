using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    public PurchaseItemButton itemButtonPrefab;
    public Transform panelParent;
    private async void Start()
    {
        var catalog = await PlayerData.Get<PlayfabItemCatalog>().GetCatalogAsync();
        GenerateCatalogItemButton(catalog);
    }
    private void GenerateCatalogItemButton(List<CatalogItem> catalog)
    {
        ClearChilds();
        for (int i = 0; i < catalog.Count; i++)
        {
            var button = Instantiate(itemButtonPrefab, panelParent);
            button.Initialize(catalog[i].ItemId, "CO");
            button.gameObject.SetActive(true);
        }
    }
    private void ClearChilds()
    {
        for (int i = panelParent.childCount; i > 0; i--)
            Destroy(panelParent.GetChild(i));
    }
}
