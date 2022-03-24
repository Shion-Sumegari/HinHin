using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game5Controler : MonoBehaviour
{
    public void Pre()
    {
        Application.LoadLevel("Game4");
    }
    public void Next()
    {
        Application.LoadLevel("Game6");
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
