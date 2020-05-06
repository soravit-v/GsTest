using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CurrencyPanel : MonoBehaviour
{
    public string currencyId;
    public TMP_Text textMesh;
    private void Awake()
    {
        textMesh.text = $"{PlayerData.Get<PlayerInventory>().GetCurrencyAmount(currencyId)}";
        PlayerData.Get<PlayerInventory>().onCurrencyUpdate += UpdateCurrency;
    }
    public void UpdateCurrency(Dictionary<string, int> currency)
    {
        textMesh.text = $"{currency[currencyId]}";
    }
}
