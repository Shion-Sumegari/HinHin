using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayControl : MonoBehaviour
{
    public static PlayControl instance;
    [SerializeField]
    private Button intro;
    public Sprite zero,one,two,three,four,five,six,seven,eight,night;

    [SerializeField]
    private GameObject score1,score2;
    [SerializeField]
    private Text endScore, highScore;
    [SerializeField]
    private GameObject gameover,pausePannel;
    private void Awake()
    {
        Time.timeScale = 0;
        _MakeInsstance();
    }
    void _MakeInsstance()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public void _IntroButton()
    {
        Time.timeScale = 1;
        intro.gameObject.SetActive(false);
    }
    public void _SetScore(int score)
    {
        Sprite[] arr = { zero, one, two, three, four, five, six, seven, eight, night };
        score1.gameObject.GetComponent<SpriteRenderer>().sprite = arr[score / 10];
        score2.gameObject.GetComponent<SpriteRenderer>().sprite = arr[score % 10];
    }
    public void _GameOver(int score)
    {
        gameover.SetActive(true);
        endScore.text = score.ToString();
        if(score > NewBehaviourScript.instance.GetHighScore())
        {
            NewBehaviourScript.instance._SetHighScore(score);
        }
        highScore.text = NewBehaviourScript.instance.GetHighScore().ToString();
    }
    public void _MenuButton()
    {
        SceneManager.LoadScene("MenuScene");
    }
    public void _RestartLevel()
    {
        SceneManager.LoadScene("PlayScene");
    }
    public void _Pausebutton()
    {
        Time.timeScale = 0;
        pausePannel.SetActive(true);
        if (Scroll.instance != null)
        {
            Scroll.instance.speed = 0f;
        }
    }
    public void _ResumeButton()
    {
        Time.timeScale = 1;
        pausePannel.SetActive(false);
        if(Scroll.instance != null)
        {
            Scroll.instance.speed = 0.001f;
        }
    }
}
