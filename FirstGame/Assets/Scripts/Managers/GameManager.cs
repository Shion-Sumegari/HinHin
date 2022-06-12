using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EffectsManager))]
public class GameManager : MonoBehaviour
{
    public EffectsManager effectsManager;
    public HomeUIController uIController;

    #region Singleton
    public static GameManager Instance;

    public float flyingSpeed = 30f;

    private void Awake()
    {
        effectsManager = GetComponent<EffectsManager>();
        uIController = GetComponent<HomeUIController>();
        isPlayerDataLoaded = false;
        if (Instance == null)
        {
            Application.targetFrameRate = 300;
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            // Make sure that the server is connected

            // Start Loading the PlayerData
        }
        else if (Instance != this)
        {
            //Instance is not the same as the one we have, destroy old one, and reset to newest one
            Destroy(Instance.gameObject);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region LOAD_USER_DATA

    public event System.Action OnPlayerDataLoaded;

    [SerializeField] UserData m_PlayerData;
    public UserData userData { get { return m_PlayerData; } set { m_PlayerData = value; } }

    bool m_IsDataLoaded;
    public bool isPlayerDataLoaded { get { return m_IsDataLoaded; } set { m_IsDataLoaded = value; LoadData(); } }

    public void LoadData()
    {
        if (!m_IsDataLoaded)
        {
            StartCoroutine(LoadUserData());
        }
        else
        {
            // When the data loaded => Syncronize all refs and Fire the event for others Instance
            QualitySettings.SetQualityLevel(m_PlayerData._qualitySettingsIndex);
            if (OnPlayerDataLoaded != null)
            {
                LogUtils.Log("Fire event current playerDataLoaded !!!! ");
                OnPlayerDataLoaded();
            }
        }
    }

    

    IEnumerator LoadUserData()
    {
        if (!isPlayerDataLoaded)
        {
            m_PlayerData = GameUtils.LoadPlayerData();
        }
        while (m_PlayerData == null)
        {
            LogUtils.Log("Waiting for Player Data! Show Loading... UI");
            
            yield return null;
        }
        UpdateUserSettingsData();
        m_IsDataLoaded = true;

        // Fire PlayerDataLoaded Event for the other Instances Objects
        if (OnPlayerDataLoaded != null)
        {
            LogUtils.Log("Fire event playerDataLoaded for other objects !!!! ");
            OnPlayerDataLoaded();
        }

    }

    void UpdateUserSettingsData()
    {
        QualitySettings.SetQualityLevel(m_PlayerData._qualitySettingsIndex);
        //if (AudioManager.Instance != null)
        //{
        //    AudioManager.Instance.musicVolume = m_PlayerData._musicVolumeSettings;
        //    AudioManager.Instance.soundVolume = m_PlayerData._soundFXVolumeSettings;
        //}
    }

    #endregion

    #region LOAD_SCENE_FX
    public GameObject m_LevelLoader;

    Coroutine levelTransCoroutine;

    int currentUnlockItemId;

    public void BeginTransition()
    {
        m_LevelLoader.GetComponent<Animator>().SetTrigger("Start");
    }

    public void NextLevel()
    {
        if (levelTransCoroutine != null) StopCoroutine(levelTransCoroutine);
        levelTransCoroutine = StartCoroutine(NextLevelTransition());
    }

    IEnumerator NextLevelTransition()
    {
        var animator = m_LevelLoader.GetComponent<Animator>();
        animator.speed = 2f;
        animator.Play("LevelLoadStart");
        yield return new WaitForSeconds(0.5f);
        LogUtils.Log("Load Level");
        LevelManager.Instance.LoadLevel(userData._level);
        while (LevelManager.Instance.currentLevel == null){
            LogUtils.Log("Current level: " + LevelManager.Instance.currentLevel != null);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        LogUtils.Log("Play Start Trans");
        if (!animator.isActiveAndEnabled)
        {
            animator.enabled = true;
        }
            animator.Play("LevelLoadEnd");

        animator.speed = 1f;
        yield return new WaitForSeconds(0.2f);
        if(!PlayerController.Instance.isRunning)
        GameplayUIController.Instance.StartGameUI();
    }

    #endregion

    public void ResetAllData()
    {
        userData = new UserData();
        GameUtils.SavePlayerData(userData);
    }

    #region UIs_RUNTMIE_GAMEPPLAY

    public void ScoreCountFinish()
    {
        // Play FX and Reward Scene
        // After user click button claim => load next level
        GameplayUIController.Instance.GameOver(true, 1.5f);
    }

    #endregion

    //public bool IsPointerOverUIObject()
    //{
    //    if (EventSystem.current.IsPointerOverGameObject())
    //        return true;

    //    for (int touchIndex = 0; touchIndex < Input.touchCount; touchIndex++)
    //    {
    //        Touch touch = Input.GetTouch(touchIndex);
    //        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
    //            return true;
    //    }

    //    return false;
    //}

    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    public static bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }
    ///Returns 'true' if we touched or hovering on Unity UI element.
    public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                return true;
        }
        return false;
    }
    ///Gets all event systen raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
    public delegate void ChangeView();
    public void ChangeViewInsideScene(ChangeView changeMethod, float delay)
    {
        StartCoroutine(LoadNewView(changeMethod, delay));
    }

    IEnumerator LoadNewView(ChangeView changeMethod, float delay)
    {
        var animator = m_LevelLoader.GetComponent<Animator>();
        animator.speed = 2f;
        yield return new WaitForSeconds(delay);
        animator.Play("LevelLoadStart");
        yield return new WaitForSeconds(0.5f);
        changeMethod();
        yield return new WaitForSeconds(0.5f);
        if (!animator.isActiveAndEnabled)
        {
            animator.enabled = true;
        }
        animator.Play("LevelLoadEnd");
        animator.speed = 1f;
    }

    public bool isChestReady;
    public int lastKeyCount;
    //public int currentKeycount;
    public void PickupKey()
    {
        int currentKeycount = GameManager.Instance.userData._currentKeyCount;
        currentKeycount++;
        isChestReady = currentKeycount >= 3;
        userData._currentKeyCount = currentKeycount >= 3 ? 3 : currentKeycount;
    }
}
