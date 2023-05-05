using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour
{
    public GameObject gameOverPanel;
    public int indexScene;

    public static gameController instance;
    gameManagerGrade gManagerGrade;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Time.timeScale = 1;

        gManagerGrade = GameObject.FindObjectOfType<gameManagerGrade>();
    }

    public void ShowGameOver(){
        gManagerGrade.pausarJogo();
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame(){
        SceneManager.LoadScene(indexScene);
    }
}
