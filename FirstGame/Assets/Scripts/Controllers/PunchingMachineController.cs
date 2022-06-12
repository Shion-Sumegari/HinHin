using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PunchingMachineController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textHighscore, textScore;

    [SerializeField] int currentPoint = 0;

    [SerializeField] float timeToCount = 3f;
    int highScore;
    static string Score_Format = "D3";

    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.Instance != null)
        {
            highScore = GameManager.Instance.userData._highPunchPoint;
            DisplayScore(textHighscore, highScore);
        }
        DisplayScore(textScore, currentPoint);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PunchHit(int newScore)
    {
        StartCoroutine(RollingPoint(newScore));
    }

    void DisplayScore(TextMeshProUGUI text, int score)
    {
        text.text = score.ToString(Score_Format);
    }

    IEnumerator RollingPoint(int newScore)
    {
        float current = 0f;
        GameplayUIController.Instance.HidePowerBar();
        while (currentPoint < newScore)
        {
            //current += rollSpeed;
            //currentPoint = (int) current;

            currentPoint++;

            DisplayScore(textScore, currentPoint);
            yield return new WaitForSeconds(timeToCount / newScore);
        }
        DisplayScore(textScore, newScore);

        if (newScore > highScore)
        {
            GameManager.Instance.userData._highPunchPoint = newScore;
            GameUtils.SavePlayerData(GameManager.Instance.userData);
            DisplayScore(textHighscore, newScore);
        }

        GameManager.Instance.ScoreCountFinish();

        currentPoint = 0;
    }
}
