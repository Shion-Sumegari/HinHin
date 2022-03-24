using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game2Controler : MonoBehaviour
{
    public void PreButton()
    {
        Application.LoadLevel("Game Play 1");
    }
    public void NextButton()
    {
        Application.LoadLevel("GamePlay3");
    }
    public void ReloadButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
