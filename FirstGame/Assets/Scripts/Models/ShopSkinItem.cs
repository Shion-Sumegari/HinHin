using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "ShopSkinItem", menuName = "ScriptedObjects/ShopSkinItem")]
public class ShopSkinItem : ShopItem
{
    [SerializeField] TextMeshProUGUI txtAdsCount;
}
