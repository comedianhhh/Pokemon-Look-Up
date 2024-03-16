using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;

using PlayFab.ClientModels;
using System.Data.Common;
using System.Linq;
using System;

public class PlayFabVirtualCurrency : MonoBehaviour
{
    public TMPro.TMP_Text GemText;
    public TMPro.TMP_Text BuxText;
    public TMPro.TMP_Text DucatsText;

    Dictionary<string, int> VC=new Dictionary<string, int>();


    public void SetInitialData()
    {
        PlayFabClientAPI.UpdateUserData(
            new UpdateUserDataRequest()
            {
                Data = new Dictionary<string, string>
                {
                    {"XP","300"},
                    {"Bullets","400" },
                    {"Elixir","2323" },
                    {"Health","1000" }
                }
            },
            result =>Debug.Log("successfully updated user data"),
            error=>
            {
                Debug.Log("failed to update user data");
                Debug.Log(error.GenerateErrorReport());
            }
            );
    }


    public void GEtVirtualCurrencies()
    {
        PlayFabClientAPI.GetUserInventory(
            new GetUserInventoryRequest { },
            GetResult=>
            {
                VC = GetResult.VirtualCurrency;
                var lines = VC.Select(kvp => kvp.Key + ": " + kvp.Value);
                print(string.Join(Environment.NewLine, lines));

                BuxText.text = "Super Bux:" + VC["SB"].ToString();
                GemText.text = "Gems:" + VC["GM"].ToString();
                DucatsText.text = "Ducats:" + VC["DC"].ToString();
            },
            null,
            null
            );
    }


    public void BuyGummies()
    {
        PlayFabClientAPI.PurchaseItem(
            new PurchaseItemRequest
            {
                CatalogVersion = "1.0",
                ItemId="1",
                Price = 3,
                VirtualCurrency="GM"
            },
            LogPurchSuccess,
            null
            );
    }
    public void BuyBullets()
    {
        PlayFabClientAPI.PurchaseItem(
            new PurchaseItemRequest
            {
                CatalogVersion = "1.0",
                ItemId = "2",
                Price = 7,
                VirtualCurrency = "DC"
            },
            LogPurchSuccess,
            null
            );
    }

    private void LogPurchSuccess(PurchaseItemResult result)
    {
        Debug.Log("Purchased Successfully");
    }
    public void GetInventory()
    {
        PlayFabClientAPI.GetUserInventory(
            new GetUserInventoryRequest(),
            LogInvSuccess, null);
    }


    private void LogInvSuccess(GetUserInventoryResult result)
    {
        foreach(var inv in result.Inventory)
        {
            Debug.Log("Inventory Retrieved"+inv.DisplayName+" : "+inv.RemainingUses);

        }

    }
}
