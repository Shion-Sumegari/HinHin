using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ShopItemNew
{
    public enum PurchaseType
    {
        GLOVE, SKIN
    }

    public int id;
    public string name;
    public PurchaseType purchaseType;
    public int cost;
    public Sprite artwork;
    public Sprite demoArtwork;
    public bool isUnlocked;
    public bool isPurchased;
}
