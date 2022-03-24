using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game6Controler : MonoBehaviour
{
    public void Pre()
    {
        Application.LoadLevel("Game5");
    }
    public void Next()
    {
        Application.LoadLevel("Game7");
    }
    public void Reload()
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
