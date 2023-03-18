using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour
{
    public GameObject gameOverPanel;
    public int indexScene;

    public static gameController instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Time.timeScale = 1;
    }

    public void ShowGameOver(){
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame(){
        SceneManager.LoadScene(indexScene);
    }
}
