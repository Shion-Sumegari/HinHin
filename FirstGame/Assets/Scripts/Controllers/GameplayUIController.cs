using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayUIController : MonoBehaviour
{
    [SerializeField] GameObject gameOverHolder, ShopHolder, SettingsHolder, ChestHolder, ItemUnlockHolder, DiamondHolder, PunchingPowerBar, GameStartHolder;

    Animator animator;

    [SerializeField]
    Image powerBar;

    private bool m_IsPowerBarRolling;
    public bool isPowerBarRolling { get { return m_IsPowerBarRolling; } set { m_IsPowerBarRolling = value; RollingPowerBar(); } }
    [SerializeField] [Range(0.1f, 5f)] float rollingSpeed;

    public float currentPowerbarValue;
    Coroutine powerbarRollingCoroutine;

    // Unlock item GameOverUI
    ShopItem currentUnlockItem;
    public ShopItem CurrentUnlockItem => currentUnlockItem;
    [SerializeField] Image currentUnlockArtwork, currentFillFrontground;
    [SerializeField] TextMeshProUGUI txtUnlockProcess;
    [SerializeField] float countTime = 1.5f;


    //Unlock ItemPreview
    [SerializeField] Image unlockedItemArtwork;
    [Header("ControllerUIs")]
    public LevelListControllerUI LevelListController;
    public ItemUnlockControllerUI itemUnlockController;
    public ChestRewardUIOpenHandler chestRewardOpenUIController;
    #region Singleton

    public static GameplayUIController Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    #endregion


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Claim()
    {
        if (GameManager.Instance.isChestReady)
        {
            chestRewardOpenUIController.Initilized();
            //chestRewardOpenUIController.TurnOn();
            animator.SetTrigger("ChestUnlockShow");
        } else
        {
            GameManager.Instance.NextLevel();
            HideGameoverHolder();
        }
    }

    public void StartGameUI()
    {
        if(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "UIGameStartIOut")
        {
            LogUtils.Log("already begin");
        }
        animator.SetTrigger("isStartUIShow");
    }

    public void GameOver(bool isWin, float delay)
    {
        Invoke("DisplayGameOverUI", delay);

        if (isWin)
        {
            GameManager.Instance.userData._level++;
            GameUtils.SavePlayerData(GameManager.Instance.userData);
            Invoke("UpdateCurrentUnlockItemProcess", 1f + delay);
        }
        else
        {
            GameManager.Instance.userData._currentKeyCount = GameManager.Instance.lastKeyCount;
            GameManager.Instance.isChestReady = false;
        }
    }

    public void HideGameoverHolder()
    {
        animator.SetTrigger("isGameOverHide");
    }

    public void GameStart()
    {
        animator.SetTrigger("isStartUIHide");
    }

    void DisplayGameOverUI()
    {
        animator.SetTrigger("isGameOverShow");
        
    }

    public void ShopOpen()
    {
        animator.SetTrigger("isShopShow");
    }

    public void ShopClose()
    {
        animator.SetTrigger("isShopHide");
    }

    public void UpgradePower()
    {
        LogUtils.Log("Upgrade Power");
    }

    public void SettingsOpen()
    {
        animator.SetTrigger("isSettingShow");
    }

    public void SettingsClose()
    {
        animator.SetTrigger("isSettingHide");
    }

    public void ShowPowerBar()
    {
        animator.SetTrigger("PowerBarShow");
        isPowerBarRolling = true;
    }
    public void HidePowerBar()
    {
        animator.SetTrigger("PowerBarHide");
    }

    public void HideItemUnlockHolder()
    {
        GameManager.Instance.NextLevel();
        animator.SetTrigger("ItemUnlockedHide");
    }

    void RollingPowerBar()
    {
        if (powerbarRollingCoroutine != null) StopCoroutine(powerbarRollingCoroutine);
        powerbarRollingCoroutine = StartCoroutine(StartRollingPowerbar());
    }

    IEnumerator StartRollingPowerbar()
    {
        if (!m_IsPowerBarRolling)
        {
            currentPowerbarValue = 1 - powerBar.fillAmount;
            LogUtils.Log("PowerValue: " + currentPowerbarValue);
        }
        float changeSpeed = rollingSpeed;
        while (m_IsPowerBarRolling)
        {
            if (powerBar.fillAmount >= 1)
            {
                changeSpeed = -rollingSpeed;
            } else if(powerBar.fillAmount <= 0)
            {
                changeSpeed = rollingSpeed;
            }
            powerBar.fillAmount += changeSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    #region Unlock_Item
    public void SetCurrentUnlockItem(ShopItem shopItem)
    {
        currentUnlockItem = shopItem;
        currentUnlockArtwork.sprite = currentUnlockItem.artwork;
        txtUnlockProcess.text = PlayerPrefs.GetInt(GameConfigs.UNLOCK_ITEM_PROCESS_KEY, 0) + "%";
        float currentFill = 1f - PlayerPrefs.GetInt(GameConfigs.UNLOCK_ITEM_PROCESS_KEY, 0) / 100;
        LogUtils.Log("Fill: " + currentFill);
        currentFillFrontground.fillAmount = currentFill;
    }

    public void UpdateCurrentUnlockItemProcess()
    {
        currentUnlockItem = ShopManager.Instance.GetUnlockItem();
        if (currentUnlockItem == null) return;
        int extra = LevelManager.Instance.levelController.rewardUnlock;
        int process = PlayerPrefs.GetInt(GameConfigs.UNLOCK_ITEM_PROCESS_KEY, 0);
        int newProcess = process + extra;
        if(newProcess >= 100)
        {
            newProcess = 100;
        }
        StartCoroutine(ChangeUnlockProcess(process, newProcess));
    }

    IEnumerator ChangeUnlockProcess(int oldProcess, int newProcess)
    {
        txtUnlockProcess.text = oldProcess + "%";
        float old = oldProcess;
        float elapsedTime = 0f;
        while(elapsedTime < countTime)
        {
            old = Mathf.Lerp(oldProcess, newProcess, elapsedTime / countTime);
            txtUnlockProcess.text = (int) old + "%";
            currentFillFrontground.fillAmount = 1 - (float)old / 100;
            //LogUtils.Log("Fill: " + (1 - (float)old / 100));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        txtUnlockProcess.text = newProcess + "%";

        if (newProcess >= 100)
        {
            //Play Unlock FX Here
            LogUtils.Log("Hey! Unlock Item: " + PlayerPrefs.GetInt(GameConfigs.UNLOCK_ITEM_PROCESS_KEY));
            ObjectPooler.Instance.SpawnFormPool("FX_Confetti_Unlock_Main", currentUnlockArtwork.transform.parent, new Vector3(0.7f, 1f, -1),
                currentUnlockArtwork.transform.position, Quaternion.identity);
            newProcess = 0;
            ShopManager.Instance.UpdateUnlockStatus(currentUnlockItem.id);
            unlockedItemArtwork.sprite = currentUnlockItem.artwork;
            GameManager.Instance.userData._unlockedGloves = GameManager.Instance.userData._unlockedGloves.AddItemToArray<int>(currentUnlockItem.id);
            GameUtils.SavePlayerData(GameManager.Instance.userData);

            GameManager.ChangeView changeToUnlockItemView = LoadItemUnlockScene;

            GameManager.Instance.ChangeViewInsideScene(changeToUnlockItemView, 0.5f);
        }
        PlayerPrefs.SetInt(GameConfigs.UNLOCK_ITEM_PROCESS_KEY, newProcess);
    }

    void LoadItemUnlockScene()
    {
        animator.SetTrigger("ItemUnlocked");
        //var nextUnlockItem = ShopManager.Instance.FindGloveById(currentUnlockItem.id + 1);
        //SetCurrentUnlockItem(nextUnlockItem);
        //if (nextUnlockItem != null)
        //{
            
        //} else
        //{
        //    //questionable
        //    GameUtils.SavePlayerData(new UserData());
        //}
        
    }
    public void InitializeItemUnlockScene(ShopItem unlockItem)
    {
        animator.SetTrigger("ItemUnlocked");
        currentUnlockItem = unlockItem;
        unlockedItemArtwork.sprite = unlockItem.artwork;
    }
    public void OnClaimUnlockRewardItem()
    {
        ShopManager.Instance.UpdateUnlockStatus(currentUnlockItem.id);
        GameManager.Instance.userData._unlockedGloves = GameManager.Instance.userData._unlockedGloves.AddItemToArray<int>(currentUnlockItem.id);
        GameManager.Instance.userData._unlockedGloves = GameManager.Instance.userData._purchasedGloves.AddItemToArray<int>(currentUnlockItem.id);
        GameUtils.SavePlayerData(GameManager.Instance.userData);
    }
    public void TestFX()
    {
        // ObjectPooler.Instance.SpawnFormPool("FX_Confetti_Unlock_Main", currentUnlockArtwork.transform.parent, currentUnlockArtwork.transform.position + new Vector3(0,1f,-1), Quaternion.identity);
    }
    #endregion;
}
