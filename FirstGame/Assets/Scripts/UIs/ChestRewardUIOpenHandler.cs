using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Random = UnityEngine.Random;

public class ChestRewardUIOpenHandler : UIBase
{
    public class Key
    {
        public Image imgMain;
        public bool isComsume;
        public void Consume()
        {
            imgMain.enabled = false;
            isComsume = true;
        }
        public void Reset()
        {
            imgMain.enabled = true;
            isComsume = false;
        }
    }
    [SerializeField] Camera targetCam;
    [SerializeField] LayerMask targetMask;
    [SerializeField] float timeBetweenChestOpen = 0.8f;
    [SerializeField] Transform chestHolder;
    [SerializeField] Transform keyHolder;
    [SerializeField] GameObject keyPreb;
    [SerializeField] TextMeshProUGUI textReward;
    [SerializeField] TextMeshProUGUI textDoubleReward;
    [SerializeField] Sprite sprCoin;
    [SerializeField] Sprite sprBestItem;
    [SerializeField] List<ChestRewardUIController> lstChestController = new List<ChestRewardUIController>();
    [Header("Canvas group item")]
    [SerializeField] CanvasGroup keysBtnGroup;
    [SerializeField] CanvasGroup claimBtnGroup;
    [Header("Fx Prefab")]
    public GameObject fx_pickingChest;

    Key[] keyArr;
    ShopItem currentUnlockItem;
    bool isUnlockItemEarn;
    bool canOpenChest = true;
    int pricePool = 0;
    int totalKey = 0;
    int openChestCount = 0;

    float lastOpenTimer = 1;
    public static ChestRewardUIOpenHandler Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    protected override void Start()
    {
        base.Start();
        //InitializeChest();
        UpdateRewardUI();
    }
    public void Initilized()
    {
        GameManager.Instance.userData._currentKeyCount = 0;
        ResetChest();
        InitializeChest();
        InitializeKeys();
    }
    private void ResetChest()
    {
        if (keyArr != null)
        {
            for (int i = 0; i < keyArr.Length; i++)
            {
                Destroy(keyArr[i].imgMain?.gameObject);
            }
        }
        if (lstChestController != null)
        {
            for (int i = 0; i < lstChestController.Count; i++)
            {
                lstChestController[i].OnReset();
            }
        }
        lastOpenTimer = 1;
        canOpenChest = true;
        isUnlockItemEarn = false;
        openChestCount = 0;
        keysBtnGroup.interactable = true;
        claimBtnGroup.interactable = true;
        keysBtnGroup.gameObject.SetActive(false);
        claimBtnGroup.gameObject.SetActive(false);
    }
    public void InitializeChest()
    {
        currentUnlockItem = ShopManager.Instance.GetUnlockItem();
        Debug.Log("Chest Reward Current Item: " + currentUnlockItem);
        //currentUnlockItem = GameplayUIController.Instance.CurrentUnlockItem;
        int specialItemIndex = -1;
        if (currentUnlockItem != null)
        {
            specialItemIndex = Random.Range(0, 9);
        }
        for(int i = 0; i < lstChestController.Count; i++)
        {
            if (i == specialItemIndex)
            {
                lstChestController[i].isSpecialItem = true;
            }
            else
            {
                lstChestController[i].value = GetRewardValue(100, 1);
            }
            lstChestController[i].id = i;
        }
        int count = 0;
        foreach(Transform tr in chestHolder)
        {
            GameObject uiObj = tr.GetChild(2).gameObject;
            Sprite _itemSpr = sprCoin;
            if(count == specialItemIndex)
            {
                _itemSpr = sprBestItem;
            }
            lstChestController[count].SetUIObject(uiObj, _itemSpr);
            count++;
        }
    }
    public void InitializeKeys()
    {
        totalKey = 3;
        keyArr = new Key[totalKey];
        for(int i = 0; i < keyArr.Length; i++)
        {
            keyArr[i] = new Key();
            keyArr[i].imgMain = Instantiate(keyPreb, keyHolder).GetComponent<Image>();
            keyArr[i].imgMain.gameObject.SetActive(true);
        }
    }
    
    int GetRewardValue(int baseValue, float bonusRatio = 0.5f)
    {
        int maxValue = (int)((float)baseValue * (1f + bonusRatio));
        int value = Random.Range(baseValue, maxValue);
        value = FormatValue(value,5);
        return value;
    }
    int FormatValue(int value,int modulo)
    {
        value = Mathf.CeilToInt(value / modulo) * modulo;
        return value;
    }
    void Update()
    {
        if (!canOpenChest) return;
        if(totalKey <= 0)
        {
            if (!keysBtnGroup.gameObject.activeInHierarchy)
            {
                keysBtnGroup.gameObject.SetActive(true);
                keysBtnGroup.alpha = 0;
                keysBtnGroup.DOFade(1, 0.6f);
            }
            return;
        }

        lastOpenTimer += Time.deltaTime;
        if (lastOpenTimer < timeBetweenChestOpen) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            //Vector3 origin = targetCam.ScreenToWorldPoint(touch.position);
            //RaycastHit hit;
            //if (Physics.Raycast(origin, targetCam.transform.forward, out hit, Mathf.Infinity, 5))
            //{
            //    Debug.Log(hit.collider.name + " " + hit.collider.gameObject.layer);
            //    //Debug.DrawLine(origin, hit.point, Color.red);
            //    Debug.DrawLine(origin, origin + targetCam.transform.forward * 1000, Color.red);


            //}
            //else
            //{
            //    Debug.DrawLine(origin, origin + targetCam.transform.forward * 1000,Color.red);
            //}
            Ray ray = targetCam.ScreenPointToRay(touch.position);
            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, targetMask, QueryTriggerInteraction.Collide);
            for (int i = 0; i < hits.Length; i++)
            {
                //if (hits[i].collider.name.Equals("UI_Chest"))
                //{
                //    Debug.Log(hits[i].collider.name);
                //}
                ChestRewardUIController chestRewardUI = hits[i].collider.GetComponent<ChestRewardUIController>();
                if (chestRewardUI != null)
                {
                    lastOpenTimer = 0;
                    OpenChest(chestRewardUI);
                    OnConsumeKey();
                    openChestCount++;
                    if(openChestCount == 9)
                    {
                        FinishOpenChest();
                    }
                }
            }
        }
    }
    void AddPriceToPool(int value)
    {
        pricePool += value;
        UpdateRewardUI();
    }
    void UpdateRewardUI()
    {
        textReward.text = pricePool.ToString();
        textDoubleReward.text = (pricePool*2).ToString();
    }
    void OpenChest(ChestRewardUIController chest)
    {
        chest.OnOpen();
        if (chest.isSpecialItem)
        {
            isUnlockItemEarn = true;
            //ShopManager.Instance.UpdateUnlockStatus(currentUnlockItem.id);
            //GameManager.Instance.userData._unlockedGloves = GameManager.Instance.userData._unlockedGloves.AddItemToArray<int>(currentUnlockItem.id);
            //GameUtils.SavePlayerData(GameManager.Instance.userData);
        }
        else
        {
            AddPriceToPool(chest.value);
        }
    }
    public void OnNoMoreKeyBtnClick()
    {
        //StartCoroutine(_OnNoMoreKeyBtnClick());
        FinishOpenChest();
    }
    void FinishOpenChest()
    {
        canOpenChest = false;
        keysBtnGroup.DOFade(0, 0.65f).onComplete += () =>
        {
            keysBtnGroup.interactable = false;
            claimBtnGroup.gameObject.SetActive(true);
            claimBtnGroup.alpha = 0;
            claimBtnGroup.DOFade(1, 0.65f);
            keysBtnGroup.gameObject.SetActive(false);
        };
    }
    public void OnClaimReward()
    {
        GameManager.Instance.userData._currentCoin += pricePool;
        GameUtils.SavePlayerData(GameManager.Instance.userData);
        if (isUnlockItemEarn)
        {
            GameplayUIController.Instance.InitializeItemUnlockScene(currentUnlockItem);
        }
        else
        {
            GameManager.Instance.NextLevel();
        }
    }
    public void OnConsumeKey()
    {
        keyArr[totalKey - 1].Consume();
        totalKey--;
    }
    public void OnMoreKey()
    {
        totalKey = 3;
        for(int i = 0; i < keyArr.Length; i++)
        {
            keyArr[i].Reset();
        }
        keysBtnGroup.gameObject.SetActive(false);
    }
    //IEnumerator _OnNoMoreKeyBtnClick()
    //{
    //    keysBtnGroup.DOFade(0, 0.65f);
    //}
}
