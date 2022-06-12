using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupController : MonoBehaviour
{
    [SerializeField] Sprite m_DisableBackground;
    [SerializeField] Sprite m_BuyBackground;
    [SerializeField] Sprite m_AdsBackground;

    public Button btnClose, btnBuy, btnAds, btnConfirm, btnStarBonus, btnMagnetBonus, btnRate, btnRateCancel;
    [SerializeField] TextMeshProUGUI txtPrice, txtMessage, txtTitle;
    public int price { get { return Int32.Parse(txtPrice.text); } set { txtPrice.SetText(value + ""); } }
    public string message { get { return txtMessage.text; } set { txtMessage.SetText(value); } }
    public string title { get { return txtTitle.text; } set { txtTitle.SetText(value); } }

    [SerializeField] GameObject m_PopupContent;

    public GameUtils.PopupType type;

    bool m_IsPurchaseable;
    public bool isPurchaseable { get { return m_IsPurchaseable; } set { m_IsPurchaseable = value; UpdateUI(); } }

    bool m_IsRewardVideoReady;
    public bool isRewardVideoReady { get { return m_IsRewardVideoReady; } set { m_IsRewardVideoReady = value; UpdateUI(); } }

    public System.Action OnPurchaseConfirm;
    public System.Action<GameUtils.PopupType, int> OnBuyClick;
    public System.Action<GameUtils.PopupType> OnAdClick;

    public bool isGift;
    // Start is called before the first frame update
    void Start()
    {
        //GameUtils.ScaleIn(m_PopupContent);
        UpdateUI();
        //m_IsRewardVideoReady = AdsController.Instance.isExtraRewardReady();
    }

    private void UpdateUI()
    {
        //if (type == GameUtils.PopupType.CONFIRM_PURCHASED)
        //{
        //    btnBuy.gameObject.SetActive(false);
        //    btnAds.gameObject.SetActive(false);
        //    btnConfirm.gameObject.SetActive(true);
        //}
        //else if (type == GameUtils.PopupType.LACK_OF_RESOURCE)
        //{
        //    btnBuy.gameObject.SetActive(price != 0);
        //    btnAds.gameObject.SetActive(true);
        //    btnConfirm.gameObject.SetActive(false);
        //    btnBuy.GetComponent<Image>().sprite = m_IsPurchaseable ? m_BuyBackground : m_DisableBackground;
        //    btnBuy.interactable = m_IsPurchaseable;
        //    btnAds.GetComponent<Image>().sprite = m_IsRewardVideoReady ? m_AdsBackground : m_DisableBackground;
        //    btnAds.interactable = m_IsRewardVideoReady;
        //}
        //else if (type == GameUtils.PopupType.BONUS)
        //{
        //    txtTitle.SetText(title);
        //    btnConfirm.gameObject.SetActive(false);
        //    btnAds.gameObject.SetActive(m_IsRewardVideoReady);

        //    btnStarBonus.transform.GetChild(4).gameObject.SetActive(false);
        //    btnMagnetBonus.transform.GetChild(4).gameObject.SetActive(false);

        //    //int magnetCost = GameManager.Instance.magnetCost;
        //    //int starCost = GameManager.Instance.starCost;

        //    if (isGift)
        //    {
        //        btnStarBonus.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("FREE"); ;
        //        btnMagnetBonus.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("FREE");

        //        magnetCost = 0;
        //        starCost = -1;
        //    }
        //    Debug.Log("123");
        //    btnStarBonus.interactable = GameManager.Instance.userData._currentDiamond >= starCost;
        //    btnMagnetBonus.interactable = GameManager.Instance.userData._currentDiamond >= magnetCost;

        //    btnMagnetBonus.onClick.AddListener(() => OnBonusButtonClick(btnMagnetBonus, magnetCost));
        //    btnStarBonus.onClick.AddListener(delegate
        //    {
        //        OnBonusButtonClick(btnStarBonus, starCost);
        //    });
        //}
        //else if (type == GameUtils.PopupType.RATE)
        //{
        //    btnBuy.gameObject.SetActive(false);
        //    btnAds.gameObject.SetActive(false);
        //    btnRate.gameObject.SetActive(true);
        //    btnRateCancel.gameObject.SetActive(true);

        //    btnRateCancel.onClick.AddListener(delegate
        //    {
        //        OnRateDisable();
        //    });

        //    btnRate.onClick.AddListener(delegate
        //    {
        //        OnRateClick();
        //    });
        //}

    }

    public void OnBonusButtonClick(Button button, int cost)
    {
        if (GameManager.Instance.userData._currentDiamond >= cost)
        {
            button.interactable = false;
            button.transform.GetChild(4).gameObject.SetActive(true);
            Bonus(cost);
        }
    }

    public void Close()
    {
        Destroy(gameObject);
    }

    public void Buy()
    {
        if (OnBuyClick != null)
        {
            OnBuyClick(type, price);
        }
        Destroy(gameObject);
    }

    public void Bonus(int cost)
    {
        if (OnBuyClick != null)
        {
            OnBuyClick(type, cost);
        }
    }

    public void Ads()
    {
        btnAds.interactable = false;
        if (OnAdClick != null)
        {
            OnAdClick(type);
        }
        Destroy(gameObject);
    }

    public void Confirm()
    {
        if (OnPurchaseConfirm != null) OnPurchaseConfirm();
        Destroy(gameObject);
    }

    public void OnRateClick()
    {
        string store_url;
#if UNITY_ANDROID
        store_url = "market://details?id=" + Application.identifier;
#elif UNITY_IPHONE
            store_url = PlayerPrefs.GetString(PlayerPrefsConfig.ADMOB_REWARD_AD_ID_KEY, ADMOB_IOS_REWARDED_ID);
#else
            store_url = "";
#endif

        Application.OpenURL(store_url);
        Destroy(gameObject);
    }

    public void OnRateDisable()
    {
        PlayerPrefs.SetInt(GameConfigs.RATE_DISABLE_KEY, 0);
        Destroy(gameObject);
    }
}
