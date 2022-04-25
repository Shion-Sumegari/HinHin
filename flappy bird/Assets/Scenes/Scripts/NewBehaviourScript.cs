using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public static NewBehaviourScript instance;
    private const string HIGH_SCORE = "High Score";
    // Start is called before the first frame update
    void Awake()
    {
        _MakeSingle();
        _isGameStartFirst();
    }

    void _isGameStartFirst()
    {
        if (!PlayerPrefs.HasKey("isGameStartFirst")){
            PlayerPrefs.SetInt(HIGH_SCORE, 0);
            PlayerPrefs.SetInt("isGameStartFirst", 0);
        }
    }
    void _MakeSingle()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void _SetHighScore(int score)
    {
        PlayerPrefs.SetInt(HIGH_SCORE, score);
    }
    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(HIGH_SCORE);
    }
}
