using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayfabItemCatalog : IPlayfabData
{
    public List<CatalogItem> itemCatalog;

    public async Task OnPlayfabConnect()
    {
        await GetCatalogAsync();
    }
    public async Task<List<CatalogItem>> GetCatalogAsync()
    {
        while (!PlayFabClientAPI.IsClientLoggedIn())
           await Task.Delay(100);
        var task = new TaskCompletionSource<List<CatalogItem>>();
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), GetCatalogSuccess(task), GetCatalogFail);
        return await task.Task;
    }

    private Action<GetCatalogItemsResult> GetCatalogSuccess(TaskCompletionSource<List<CatalogItem>> task)
    {
        return result =>
        {
            Debug.Log("Get catalog success");
            itemCatalog = result.Catalog;
            task.SetResult(result.Catalog);
        };
    }

    public CatalogItem GetCatalogItemData(string itemId)
    {
        return itemCatalog.Find(catalogItem => catalogItem.ItemId.Equals(itemId));
    }
    void GetCatalogFail(PlayFabError error)
    {
        Debug.LogError($"GetCatalogFail {error.Error} {error.ErrorMessage}");
    }

}
