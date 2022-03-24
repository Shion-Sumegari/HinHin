using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game1Controler : MonoBehaviour
{
    // Start is called before the first frame update
    public void PreButton()
    {
        Application.LoadLevel("Scene Intro");
    }
    public void NextButton()
    {
        Application.LoadLevel("GamePlay2");
    }
    public void ReloadButton()
    {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
