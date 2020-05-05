using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class PlayerData
{
    private static List<IPlayfabData> playfabDatas = new List<IPlayfabData>()
    {
        new PlayerInventory(),
        new PlayfabItemCatalog(),
    };
    public static T Get<T>() where T : IPlayfabData
    {
        return (T)playfabDatas.Find(data => data is T);
    }
    public static async Task OnPlayfabConnected()
    {
        foreach (var data in playfabDatas)
            await data.OnPlayfabConnect();
    }
}
