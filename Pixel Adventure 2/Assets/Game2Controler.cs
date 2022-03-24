using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game2Controler : MonoBehaviour
{
    public void Pre()
    {
        Application.LoadLevel("Game1");
    }
    public void Next()
    {
        Application.LoadLevel("Game3");
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
