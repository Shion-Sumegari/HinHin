using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class LevelListNodeUI : MonoBehaviour
{
    //[SerializeField] Image imgOutline;
    [SerializeField] Image imgBG;
    [SerializeField] LevelEnd levelType;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Sprite grayBG;
    [SerializeField] Sprite finishBG;
    [SerializeField] Sprite selectBG;
    [SerializeField] Vector3 orginalScale = Vector3.one;

    bool isSelected;
    Coroutine onSelectedCoroutine;

    void Start()
    {
        
    }
    private void OnEnable()
    {
        CheckSelected();
    }
    public void OnReset()
    {
        KillCoroutine(onSelectedCoroutine);
        transform.localScale = orginalScale;
        //imgOutline.gameObject.SetActive(false);
        imgBG.sprite = grayBG;
        isSelected = false;
        if(levelText!=null) levelText.text = string.Empty;
    }
    public void OnInitialize(int index = -1)
    {
        if (index != -1 && levelText!=null)
        {
            levelText.text = index.ToString();
        }
    } 
    /// <summary>
    /// Change Icon BG color if level is finished
    /// </summary>
    public void SetHighLight()
    {
        imgBG.sprite = finishBG;
    }
    public void OnSelected()
    {
        // Debug.Log(transform.parent.gameObject.activeInHierarchy);
        // Debug.Log(transform.parent.parent.gameObject.activeInHierarchy);
        // Debug.Log(transform.parent.parent.parent.gameObject.activeInHierarchy);
        isSelected = true;
        if(gameObject.activeInHierarchy){
            CheckSelected();
        }
    }
    void CheckSelected(){
        if(isSelected){
            //imgOutline?.gameObject.SetActive(true);
            imgBG.sprite = selectBG;
            if(onSelectedCoroutine != null)
            {
                StopCoroutine(onSelectedCoroutine);
            }
            onSelectedCoroutine = StartCoroutine(_OnSelected());
        }
    }
    IEnumerator _OnSelected()
    {
        while (true)
        {
            transform.DOKill();
            transform.DOScale(1.15f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            transform.DOKill();
            transform.DOScale(1f, 0.6f);
            yield return new WaitForSeconds(0.6f);
        }
    }
    public void KillCoroutine(Coroutine coroutine)
    {
        if (coroutine != null)
        {
            Debug.Log(gameObject+" Kill Coroutine: " + coroutine.ToString());
            StopCoroutine(coroutine);
        }
    }
}
