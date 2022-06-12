using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopSkinItemController : ShopItemController
{
    [SerializeField] GameObject adCountContainer;
    [SerializeField] TextMeshProUGUI txtAdCount;

    protected override void DisplayShopItemData()
    {
        adCountContainer.SetActive(!m_ShopItem.isPurchased);
        base.DisplayShopItemData();
    }
}
