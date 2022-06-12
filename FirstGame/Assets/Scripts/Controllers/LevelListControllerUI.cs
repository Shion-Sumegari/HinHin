using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RotaryHeart.Lib.SerializableDictionary;
[System.Serializable]
public class LevelNodeDictionary : SerializableDictionaryBase<LevelEnd, GameObject> { }
public class LevelListControllerUI : MonoBehaviour
{
    [SerializeField] Image imgLvProgressBar;
    [SerializeField] Image imgLvProgressBarFloor;
    [SerializeField] LevelNodeDictionary levelNodeDic = new LevelNodeDictionary();
    [SerializeField] Transform levelListHolder;
    float itemWidth = 70;
    float itemPadding = 40;
    float barheight = 17.12f;
    int _itemCount = 0;
    void Start()
    {
        
    }
    /// <summary>
    /// Khởi tạo level list UI
    /// </summary>
    /// <param name="levelIndex"></param>
    /// <param name="levelControllers">level controller list</param>
    public void InitializeLevelListUI(int levelIndex, List<LevelController> levelControllers)
    {
        OnReset();
        int startIndex = 0;
        int count = 5;
        // int totalLevelLeft = levelControllers.Count - startIndex;
        // count = Mathf.Min(count, totalLevelLeft);
        int nextRightMax = 0;
        Debug.Log(levelControllers.Count);
        if(levelControllers.Count <= 5 || levelIndex < 3){
            nextRightMax = count-1;
            startIndex = 0;
        }else
        {
            nextRightMax = levelIndex + 2;
            Debug.Log(nextRightMax+" "+levelControllers.Count);
            nextRightMax = Mathf.Min(nextRightMax, levelControllers.Count-1);
            startIndex = nextRightMax + 1 - count;
        }
        count = nextRightMax - startIndex;
        //Initialize level node
        for(int i = startIndex; i < nextRightMax + 1; i++)
        {
            LevelEnd levelType = levelControllers[i].levelEndType;
            LevelListNodeUI levelNode = Instantiate(levelNodeDic[levelType], levelListHolder).GetComponent<LevelListNodeUI>();
            levelNode.gameObject.SetActive(true);
            levelNode.name = levelControllers[i].name;
            levelNode.OnReset();
            levelNode.OnInitialize(i);
            if(i <= levelIndex)
            {
                levelNode.SetHighLight();
            }
            if(i == levelIndex)
            {
                levelNode.OnSelected();
            }
        }
        InitializeProgressBar(count);
        int index = levelIndex - startIndex;
        SetBarProgress(index);
    }
    void OnReset()
    {
        foreach(Transform tr in levelListHolder)
        {
            Destroy(tr.gameObject);
        }
    } 
    /// <summary>
    /// Khởi tạo thanh progress
    /// </summary>
    /// <param name="itemCount"></param>
    void InitializeProgressBar(int itemCount)
    {
        _itemCount = itemCount;
        Vector2 newSize = new Vector2(itemWidth * (itemCount) + itemPadding * (Mathf.Max(0, itemCount - 1)), barheight);
        imgLvProgressBar.GetComponent<RectTransform>().sizeDelta = newSize;
        imgLvProgressBarFloor.GetComponent<RectTransform>().sizeDelta = newSize;
    }
    /// <summary>
    /// Set gia trị thanh Progress dựa trên index level hiện tại
    /// </summary>
    /// <param name="index"></param>
    void SetBarProgress(int index)
    {
        Debug.Log(index);
        float barLength = itemWidth * (_itemCount) + itemPadding * (Mathf.Max(0, _itemCount - 1));
        float progress = (itemWidth * (index) + itemPadding * (Mathf.Max(0, index - 1))) / barLength;
        imgLvProgressBar.fillAmount = progress;
    }
}
