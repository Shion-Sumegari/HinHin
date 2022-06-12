using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HomeUIController : MonoBehaviour
{
    public Canvas mainCanvas;

    public GameObject startGuideUI, EzBossGuideUI, HBossGuideUI;

    public TextMeshPro textPunchPointCount;

    public GameObject PlayerHealUI, BossHealUI;

    public void ToggleGuideUI(int index)
    {
        startGuideUI.SetActive(index == 0);
        EzBossGuideUI.SetActive(index == 1);
        HBossGuideUI.SetActive(index == 2);
    }

    public void ResetCombatUI()
    {
        var statsUIs = FindObjectsOfType<StatsUI>();
        if(statsUIs.Length > 0)
        {
            foreach(var statUI in statsUIs)
            {
                statUI.ResetFillCount();
            }
        }
    }
}
