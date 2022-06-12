using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopManager : UIBase
{
    public Color32 unlockedOutline, unlockedBackground, purchasedOutline, purchasedBackground, lockedOutline, lockedBackground, equippedOutline, equippedBackground;

    public Material grayScaleMat;
    [SerializeField] PageUIController pageSkinController;

    [SerializeField] Transform glovesParent, skinsParent;

    [SerializeField] GameObject m_ShopItemGlovePrefab, m_ShopSkinItemPrefab;

    ShopItemController m_EquippedGlove;
    public ShopItemController equippedGlove { get { return m_EquippedGlove; } set { OnEquipGlove(value); } }
    
    ShopSkinItemController m_EquippedSkin;
    public ShopSkinItemController equippedSkin { get { return m_EquippedSkin; } set { OnEquipSkin(value); } }

    public List<GameObject> glovesItemList;

    List<ShopItem> m_ShopItemsDataBuffer;
    List<ShopSkinItem> m_ShopSkinItemsDataBuffer;
    #region Singleton

    public static ShopManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            // Application.targetFrameRate = 300;
            Instance = this;
            // Make sure that the server is connected
        }
        else
        {
            Destroy(gameObject);
        }
        m_ShopItemsDataBuffer = new List<ShopItem>();
        m_ShopSkinItemsDataBuffer = new List<ShopSkinItem>();
    }

    #endregion

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void TurnOn(float duration = 0.65F)
    {
        base.TurnOn(duration);
    }
    public override void TurnOff(float duration = 0.65F)
    {
        base.TurnOff(duration);
    }
    public void LoadData()
    {
        m_ShopItemsDataBuffer.Clear();
        m_ShopSkinItemsDataBuffer.Clear();

        List<int> playerPurchasedGloves = GameManager.Instance.userData._purchasedGloves.ToList<int>();
        List<int> playerUnlockedGloves = GameManager.Instance.userData._unlockedGloves.ToList<int>();
        List<int> playerPurchasedSkins = GameManager.Instance.userData._purchasedSkins.ToList<int>();

        UnityEngine.Object[] glovesItems = Resources.LoadAll("ShopItems/Gloves", typeof(ShopItem));
        UnityEngine.Object[] skinItems = Resources.LoadAll("ShopItems/Skins", typeof(ShopSkinItem));

        // Gloves Items
        foreach (var gloveItem in glovesItems)
        {
            var shopItem = (ShopItem)gloveItem;
            shopItem.isUnlocked = playerUnlockedGloves.Contains(shopItem.id);
            shopItem.isPurchased = playerPurchasedGloves.Contains(shopItem.id);
            m_ShopItemsDataBuffer.Add(shopItem);
        }
        
        
        m_ShopItemsDataBuffer = m_ShopItemsDataBuffer.OrderBy(glove => !playerUnlockedGloves.Contains(glove.id)).ToList<ShopItem>();
        //--------------------
        //int currentUnlockId = 0;

        //foreach (var glove in m_ShopItemsDataBuffer)
        //{
        //    var shopItem = Instantiate(m_ShopItemGlovePrefab, glovesParent);
        //    glovesItemList.Add(shopItem);
        //    var shopItemController = shopItem.GetComponent<ShopItemController>();
        //    shopItemController.shopItem = glove;
        //    if (GameManager.Instance.userData._currentGloveId == glove.id)
        //    {
        //        shopItemController.isEquiped = true;
        //        m_EquippedGlove = shopItemController;
        //    }

        //    if (shopItemController.shopItem.isUnlocked && shopItemController.shopItem.id > currentUnlockId)
        //    {
        //        currentUnlockId = shopItemController.shopItem.id;
        //    }
        //}
        //currentUnlockId++;
        //foreach (var glove in m_ShopItemsDataBuffer)
        //{
        //    if (glove.id == currentUnlockId)
        //    {
        //        GameplayUIController.Instance.SetCurrentUnlockItem(glove);
        //        break;
        //    }
        //}
        //------------------------
        glovesItemList = pageSkinController.InitializeGloveSkin(m_ShopItemsDataBuffer, m_ShopItemGlovePrefab);


        // Skin Items
        foreach (var skinItem in skinItems)
        {
            var shopSkinItem = (ShopSkinItem)skinItem;
            shopSkinItem.isUnlocked = true;
            shopSkinItem.isPurchased = playerPurchasedSkins.Contains(shopSkinItem.id);
            m_ShopSkinItemsDataBuffer.Add(shopSkinItem);
        }

        m_ShopSkinItemsDataBuffer = m_ShopSkinItemsDataBuffer.OrderBy(skin => !playerPurchasedSkins.Contains(skin.id)).ToList<ShopSkinItem>();
        //pageSkinController.InitializeSkin(m_ShopSkinItemsDataBuffer, m_ShopSkinItemPrefab);
        foreach (var skin in m_ShopSkinItemsDataBuffer)
        {
            var shopItem = Instantiate(m_ShopSkinItemPrefab, skinsParent);
            var shopSkinController = shopItem.GetComponent<ShopSkinItemController>();
            shopSkinController.shopItem = skin;
            if (GameManager.Instance.userData._currentSkinId == skin.id)
            {
                shopSkinController.isEquiped = true;
                m_EquippedSkin = shopSkinController;
            }
        }
    }
    public ShopItem GetUnlockItem()
    {
        foreach (var glove in m_ShopItemsDataBuffer)
        {
            if (glove.isUnlocked == false)
            {
                return glove;
            }
        }
        return null;
    }
    void OnEquipGlove(ShopItemController value)
    {
        m_EquippedGlove.isEquiped = false;
        m_EquippedGlove = value;
    }

    void OnEquipSkin(ShopSkinItemController value)
    {
        m_EquippedSkin.isEquiped = false;
        m_EquippedSkin = value;
    }
    public void SetEquipGlove(ShopItemController value)
    {
        m_EquippedGlove = value;
    }
    public ShopItem FindGloveById(int id)
    {
        LogUtils.Log("New ID: " + id);
        foreach (var glove in m_ShopItemsDataBuffer)
        {
            LogUtils.Log("Search New ID: " + glove.id);
            if (glove.id == id)
            {
                return glove;
            }
        }
        LogUtils.Log("Final New ID: NULL");
        return null;
    }

    public void UpdateUnlockStatus(int unlockedID)
    {
        foreach (var glove in glovesItemList)
        {
            ShopItem shopItem = glove.GetComponent<ShopItemController>().shopItem;
            if (shopItem.id == unlockedID)
            {
                shopItem.isUnlocked = true;
                glove.GetComponent<ShopItemController>().shopItem = shopItem;
            }
        }
    }
}
