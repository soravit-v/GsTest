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
    Task<List<CatalogItem>> GetCatalogAsync()
    {
        var task = new TaskCompletionSource<List<CatalogItem>>();
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), GetCatalogSuccess(task), GetCatalogFail);
        return task.Task;
    }

    private Action<GetCatalogItemsResult> GetCatalogSuccess(TaskCompletionSource<List<CatalogItem>> task)
    {
        return result =>
        {
            Debug.Log("Get catalog success");
            task.SetResult(result.Catalog);
            itemCatalog = result.Catalog;
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
