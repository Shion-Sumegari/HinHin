using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageIndicatorUI : MonoBehaviour
{
    [SerializeField] GameObject templatePreb;
    List<GameObject> lstIndicator = new List<GameObject>();
    Image currentIndicatorImg;
    private void Start()
    {
        
    }
    public void InitializeInidcator(int pageCount, int startIndex = 0)
    {
        for(int i = 0; i < pageCount; i++)
        {
            GameObject indicator = Instantiate(templatePreb, transform);
            lstIndicator.Add(indicator);
            indicator.SetActive(true);
        }
        currentIndicatorImg = lstIndicator[startIndex].GetComponent<Image>();
        DisableAllIndicator();
        SetCurrentActive(startIndex);
    }
    void DisableAllIndicator()
    {
        for(int i = 0; i < lstIndicator.Count; i++)
        {
            SetIndicatorActive(lstIndicator[i].GetComponent<Image>(),false);
        }
    }
    public void SetCurrentActive(int index)
    {
        SetIndicatorActive(currentIndicatorImg, false);
        currentIndicatorImg = lstIndicator[index].GetComponent<Image>();
        SetIndicatorActive(currentIndicatorImg, true);
        
    }
    public void SetIndicatorActive(Image img, bool active)
    {
        if (active)
        {
            img.color = SetColorOpacity(img.color, 0.75f);
        }
        else
        {
            img.color = SetColorOpacity(img.color, 0.3f);

        }
    }
    Color SetColorOpacity(Color color, float value)
    {
        color.a = value;
        return color;
    }
}
