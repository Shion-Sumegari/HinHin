using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game5Controler : MonoBehaviour
{
    public void PreButton()
    {
        Application.LoadLevel("GamePlay4");
    }
    public void NextButton()
    {
        Application.LoadLevel("Scene Menu");
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
