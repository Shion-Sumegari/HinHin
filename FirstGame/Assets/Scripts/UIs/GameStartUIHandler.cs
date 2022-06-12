using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameStartUIHandler : MonoBehaviour
{
    [Header("UI Object")]
    [SerializeField] Button settingBtn;
    [SerializeField] Button shopBtn;
    [SerializeField] Button upgradePowerBtn;
    [Header("Canvas Group")]
    [SerializeField] CanvasGroup processBarGroup;
    [SerializeField] CanvasGroup moneyGroup;
    [Header("Animation variables")]
    [SerializeField] float settingBtnInPosX;
    [SerializeField] float settingBtnOutPosX;
    [SerializeField] float shopBtnInPosX;
    [SerializeField] float shopBtnOutPosX;
    [SerializeField] float upgradePowerBtnInPosX;
    [SerializeField] float upgradePowerBtnOutPosX;

    protected  void Start()
    {
        
    }

    public void TurnOn()
    {

    }
    public void TurnOff()
    {

    }
    IEnumerator _OnTurnOn()
    {

        yield return null;
    }
    IEnumerator _OnTurnOff()
    {
        yield return null;
    }
}
