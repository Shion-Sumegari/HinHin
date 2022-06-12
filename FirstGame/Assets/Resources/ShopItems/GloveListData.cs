using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GloveList")]
public class GloveListData : ScriptableObject
{
    public List<ShopItemNew> gloveList = new List<ShopItemNew>();
}

