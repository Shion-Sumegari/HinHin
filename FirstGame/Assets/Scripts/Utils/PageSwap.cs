using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageSwap : MonoBehaviour
{
    public List<GameObject> pages;

    public List<Button> tabView;

    public Sprite sprEnableTab, sprDisableTab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwapPage(int tabID)
    {
        for(int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(i == tabID);
            SwapTab(tabView[i], i == tabID);
        }
    }

    public void SwapTab(Button button, bool isEnable) 
    {
        button.GetComponent<Image>().sprite = isEnable ? sprEnableTab : sprDisableTab;
    }
}
