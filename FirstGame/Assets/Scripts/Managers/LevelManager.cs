using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public event System.Action OnLevelLoaded;

    public Vector3 endPoint;
    public Vector3 startPoint;

    public GameObject finalChallenge;

    public LevelEnd levelEndType;

    public GameObject levelLoader;

    public List<GameObject> LevelList;

    public GameObject currentLevel { get; private set; }

    public LevelController levelController;

    public int currentPickupDiamond;
    

    Coroutine loadLevelCoroutine;

    #region TESTING 
    public Button yourButton;

    void TaskOnClick()
    {
        GameManager.Instance.userData._level++;
        GameUtils.SavePlayerData(GameManager.Instance.userData);
        
        GameManager.Instance.NextLevel();
    }

    #endregion

    #region Singleton

    public static LevelManager Instance;

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
    }

    #endregion

    private void Start()
    {
        LoadLevel(GameManager.Instance.userData._level);
        StartCoroutine(LoadShopData());
        
        // TEST
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    public void LoadLevel(int index)
    {
        GameManager.Instance.m_LevelLoader = levelLoader;
        GameManager.Instance.lastKeyCount = GameManager.Instance.userData._currentKeyCount;
        
        currentPickupDiamond = 0;
        if (index < LevelList.Count)
        {
            if (loadLevelCoroutine != null) StopCoroutine(loadLevelCoroutine);
            loadLevelCoroutine = StartCoroutine(LoadLevelByIndex(index));
        } else
        {
            GameManager.Instance.ResetAllData();
            LoadLevel(0);
            return;
        }
        
    }

    IEnumerator LoadShopData()
    {
        while(ShopManager.Instance == null)
        {
            yield return new WaitForEndOfFrame();
        }

        ShopManager.Instance.LoadData();
        
    }

    IEnumerator LoadLevelByIndex(int index)
    {
        if (currentLevel != null) Destroy(currentLevel.gameObject);

        currentLevel = Instantiate(LevelList[index], transform);
        levelController = currentLevel.GetComponent<LevelController>();
        List<LevelController> levelControllers = new List<LevelController>();
        for (int i = 0; i < LevelList.Count; i++)
        {
            levelControllers.Add(LevelList[i].GetComponent<LevelController>());
        }
        GameplayUIController.Instance.LevelListController.InitializeLevelListUI(index, levelControllers);
        yield return null;
        if (currentLevel != null)
        {
            ResetGameStatusToNewLevel();
        }
    }

    void ResetGameStatusToNewLevel()
    {
        startPoint = levelController.startPoint;
        endPoint = levelController.endPoint;
        finalChallenge = levelController.endChallenge;
        levelEndType = levelController.levelEndType;
        PlayerController.Instance.BeginLevel();
        CameraController.Instance.MoveToStartScene(CameraController.SceneType.RACING);
    }
}
