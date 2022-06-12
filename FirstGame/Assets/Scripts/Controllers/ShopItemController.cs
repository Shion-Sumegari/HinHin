using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopItemController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image mainArtwork, lockedArtwork;
    [SerializeField] Image background;
    [SerializeField] GameObject purchasedTick;
    [SerializeField] Sprite sprSelectedBG;
    [SerializeField] Sprite sprUnselectedBG;

    private bool m_IsEquiped;
    protected ShopItem m_ShopItem;
    public ShopItem shopItem { get { return m_ShopItem; } set { m_ShopItem = value; DisplayShopItemData();} }
    public bool isEquiped { get { return m_IsEquiped; } set { m_IsEquiped = value; EquipedItem(); } }

    protected virtual void EquipedItem()
    {
        purchasedTick.gameObject.SetActive(m_IsEquiped);
        DisplayShopItemData();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual void DisplayShopItemData()
    {
        //mainArtwork.material = new Material(Shader.Find("Sprites/Outline"));
        //mainArtwork.material.SetFloat("_GrayscaleAmount", m_ShopItem.isPurchased ? 0f : 1f);

        if (lockedArtwork != null) lockedArtwork.gameObject.SetActive(!m_ShopItem.isUnlocked);
        mainArtwork.gameObject.SetActive(m_ShopItem.isUnlocked);
        mainArtwork.sprite = m_ShopItem.artwork;
        purchasedTick.gameObject.SetActive(isEquiped);
        if (m_ShopItem.isUnlocked)
        {
            if (isEquiped)
            {
                background.sprite = sprSelectedBG;
                //outline.color = ShopManager.Instance.equippedOutline;
            } else
            {
                background.sprite = sprUnselectedBG;
                //outline.color = m_ShopItem.isPurchased ? ShopManager.Instance.purchasedOutline : ShopManager.Instance.unlockedOutline;
            }

        } else
        {
            background.color = ShopManager.Instance.lockedBackground;
            //outline.color = ShopManager.Instance.lockedOutline;
        }
    }

    public void OnEquip(int type)
    {
        if(!isEquiped && shopItem != null && shopItem.isPurchased)
        {
            isEquiped = true;
            if(type == 0)
            {
                ShopManager.Instance.equippedGlove = this;
                GameManager.Instance.userData._currentGloveId = shopItem.id;
            } else
            {
                ShopManager.Instance.equippedSkin = (ShopSkinItemController)this;
                GameManager.Instance.userData._currentSkinId = shopItem.id;
            }
            GameUtils.SavePlayerData(GameManager.Instance.userData);
            background.sprite = sprSelectedBG;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnEquip(0);
    }
}
