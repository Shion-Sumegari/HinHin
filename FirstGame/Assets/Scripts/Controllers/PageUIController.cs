using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PageUIController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] GameObject m_pagePrefab;
    [SerializeField] PageIndicatorUI pageIndicator;
    [SerializeField] int pageCount = 1;
    [SerializeField] float pageWidth = 500;
    [SerializeField] float pageThreshold = 0.2f;
    [SerializeField]float dragSmooth = 10;
    PointerEventData currentEvent;
    RectTransform mainRect;
    int currentPage = 0;
    float distanceThreshold;
    bool isTrasiting;
    bool isDragging;
    Vector2 panelPostion;
    float panelPosY;
    void Start()
    {
        mainRect = GetComponent<RectTransform>();
        panelPosY = mainRect.anchoredPosition.y;
        distanceThreshold = pageThreshold * pageWidth;
        panelPostion = GetPagePosition(0);
        pageIndicator.InitializeInidcator(pageCount);
    }
    public List<GameObject> InitializeGloveSkin(List<ShopItem> m_ShopItemsDataBuffer, GameObject m_ShopItemGlovePrefab)
    {
        List<GameObject> glovesItemList = new List<GameObject>();
        GameObject[] pages = new GameObject[Mathf.CeilToInt((float)m_ShopItemsDataBuffer.Count / 9f)];
        for(int i = 0; i < pages.Length; i++)
        {
            pages[i] = Instantiate(m_pagePrefab, transform);
            pages[i].SetActive(true);
        }
        pageCount = pages.Length;

        int count = 0;
        int currentUnlockId = 0;
        foreach (var glove in m_ShopItemsDataBuffer)
        {
            int pageIndex = (int)(count / 9);
            var shopItem = Instantiate(m_ShopItemGlovePrefab, pages[pageIndex].transform);
            var shopItemController = shopItem.GetComponent<ShopItemController>();
            shopItemController.shopItem = glove;
            glovesItemList.Add(shopItem);
            if (GameManager.Instance.userData._currentGloveId == glove.id)
            {
                shopItemController.isEquiped = true;
                ShopManager.Instance.SetEquipGlove(shopItemController);
            }

            if (shopItemController.shopItem.isUnlocked && shopItemController.shopItem.id > currentUnlockId)
            {
                currentUnlockId = shopItemController.shopItem.id;
            }
            count++;
        }
        currentUnlockId++;
        foreach (var glove in m_ShopItemsDataBuffer)
        {
            if (glove.id == currentUnlockId)
            {
                GameplayUIController.Instance.SetCurrentUnlockItem(glove);
                break;
            }
        }
        return glovesItemList;
    }
    private void Update()
    {
        //Check if can do dragging
        if (isTrasiting) return;

        if (isDragging)
        {
            Dragging();
        }
    }


    /// <summary>
    /// Handle keo page
    /// </summary>
    public void Dragging()
    {
        float dragDistance = currentEvent.pressPosition.x - currentEvent.position.x;
        //Set page postion
        Vector2 newPos = panelPostion - new Vector2(dragDistance, 0);
        newPos = DragClampElastic(newPos);
        mainRect.anchoredPosition = Vector2.Lerp(mainRect.anchoredPosition, newPos, Time.deltaTime * dragSmooth);
    }

    /// <summary>
    /// Xu ly su kien bat dau keo
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isTrasiting) return;
        isDragging = true;
        if (currentEvent == null) currentEvent = eventData;
    }

    /// <summary>
    /// Xu ly su kien dung keo tha
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDragging) return;
        isDragging = false;
        currentEvent = null;
        float dragDistance = eventData.pressPosition.x - eventData.position.x;
        if (Mathf.Abs(dragDistance) > distanceThreshold)
        {
            float transitionSpeed = 1f;
            int overShoot = 0;
            if(dragDistance> pageWidth / 2)
            {
                overShoot = (int)((dragDistance-(pageWidth/2)) / pageWidth);
            }
            int pageIndex = dragDistance > 0 ? (currentPage + 1 + overShoot) : (currentPage - 1 + overShoot);
            pageIndex = Mathf.Clamp(pageIndex, 0, pageCount - 1);
            if (pageIndex == currentPage)
            {
                transitionSpeed = 3f;
            }

            StartCoroutine(DoPageTrasition(pageIndex, transitionSpeed));
        }
        else
        {
            StartCoroutine(DoPageTrasition(currentPage, 3f));
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
    IEnumerator DoPageTrasition(int pageIndex, float speed = 1f)
    {
        isTrasiting = true;
        
        panelPostion = mainRect.anchoredPosition;
        Vector2 nextPage = new Vector2(-pageIndex * pageWidth, panelPosY);

        float step = 0.05f;
        int division = 50;
        WaitForSeconds wait = new WaitForSeconds(1f/(division*speed));

        float t = 0;
        while (t <= 1)
        {
            t += step;
            mainRect.anchoredPosition = Vector2.Lerp(panelPostion, nextPage, t);
            yield return wait;
        }
        OnFinishPageTransition(pageIndex);
        
    }
    public void OnFinishPageTransition(int pageIndex)
    {
        currentPage = pageIndex;
        panelPostion = GetPagePosition(currentPage);
        pageIndicator.SetCurrentActive(pageIndex);
        isTrasiting = false;
    }
    Vector2 DragClampElastic(Vector2 target)
    {
        float persistance = 0.3f;
        float overShootDistance = 0;
        float borderX = 0;
        if(target.x > 0)
        {
            overShootDistance = target.x - PageStartX;
            borderX = PageStartX;
            target.x = borderX + (overShootDistance) * (persistance);
        }
        else if(target.x < PageEndX)
        {
            overShootDistance = target.x - PageEndX;
            borderX = PageEndX;
            target.x = borderX + (overShootDistance) * (persistance);
        }

        
        return target;
    }
    public int GetPageIndex(Vector2 target)
    {
        if (target.x >= 0) return 0;
        if (target.x <= PageEndX) return (pageCount - 1);
        int index = (int)(target.x / (-pageWidth));
        return index;
    }
    public Vector2 GetPagePosition(int pageIndex)
    {
        return new Vector2(-pageIndex * pageWidth, panelPosY);
    }

    

    public float PageStartX
    {
        get
        {
            return 0;
        }
        
    }
    public float PageEndX
    {
        get
        {
            return (-(pageCount - 1) * pageWidth);
        }
    }
}
