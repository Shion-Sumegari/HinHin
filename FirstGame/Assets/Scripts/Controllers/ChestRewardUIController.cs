using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ChestRewardUIController : MonoBehaviour
{
    [SerializeField] Animator animController;
    [SerializeField] Transform rigTransform;
    GameObject rewardUIObject;
    AnimationEvent endEvent = new AnimationEvent();
    AnimationEvent chestDieFinish = new AnimationEvent();
    Vector3 orginaUIObjlScale;
    bool isOpen;
    public int id;
    public bool isSpecialItem;
    public int value;
    public void OnOpen()
    {
        if (isOpen) return;
        PlayOpenAnim();
    }
    public void SetUIObject(GameObject obj, Sprite itemSpr)
    {
        rewardUIObject = obj;
        rewardUIObject.SetActive(false);
        orginaUIObjlScale = rewardUIObject.transform.localScale;
        obj.GetComponent<Image>().sprite = itemSpr;
        obj.transform.GetComponentInChildren<TextMeshProUGUI>().text = value.ToString();
    }
    void Start()
    { 
        InitializeAnimEvent();
    }
    
    void InitializeAnimEvent()
    {
        AnimationClip openChestClip = GetAnim("Chest_Open");
        endEvent.time = openChestClip.length;
        endEvent.functionName = "OnFinishOpening";
        openChestClip.AddEvent(endEvent);
        AnimationClip chestDieClip = GetAnim("Chest_Die");
        chestDieFinish.time = chestDieClip.length;
        chestDieFinish.functionName = "OnFinishChestDie";
        chestDieClip.AddEvent(chestDieFinish);
    }
    void PlayOpenAnim()
    {
        isOpen = true;
        animController.SetTrigger("Hit");
        animController.SetTrigger("DieByCombat");
        GameObject fx = Instantiate(ChestRewardUIOpenHandler.Instance.fx_pickingChest,transform.parent);
        fx.transform.localPosition = new Vector3(0,16,-90);
        fx.transform.eulerAngles = new Vector3(-80,0,0);
        fx.SetActive(true);
    }
    void OnFinishChestDie()
    {
        rewardUIObject.SetActive(true);
        rewardUIObject.transform.localScale = Vector3.zero;
        rewardUIObject.transform.DOScale(orginaUIObjlScale.x * 1.4f, 0.4f).onComplete += ()=> { rewardUIObject.transform.DOScale(orginaUIObjlScale, 0.25f); };
    }
    void OnFinishOpening()
    {
        StartCoroutine(ActionHelper.StartAction(() => { 
            rigTransform.DOScale(0, 0.5f);
            transform.parent.GetChild(1).gameObject.SetActive(true);
        },0.6f));
        GetComponent<BoxCollider>().enabled = false;
    }
    public void OnReset()
    {
        isOpen = false;
        GetComponent<BoxCollider>().enabled = true;
        transform.parent.GetChild(1).gameObject.SetActive(false);
    }
    public AnimationClip GetAnim(string animName)
    {
        for(int i = 0; i < animController.runtimeAnimatorController.animationClips.Length; i++)
        {
            if(animController.runtimeAnimatorController.animationClips[i].name == animName)
            {
                return animController.runtimeAnimatorController.animationClips[i];
            }
        }
        return null;
    }
    public void ListAnimation()
    {
        for (int i = 0; i < animController.runtimeAnimatorController.animationClips.Length; i++)
        {
            Debug.Log(animController.runtimeAnimatorController.animationClips[i].name);
        }
    }
}
