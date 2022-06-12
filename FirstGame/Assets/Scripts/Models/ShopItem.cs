using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "ScriptedObjects/ShopItem")]
public class ShopItem : ScriptableObject
{
    // Start is called before the first frame update
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
