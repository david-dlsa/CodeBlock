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
    Gabarito gGabarito;

    // Start is called before the first frame update
    void Start()
    {
        indexScene = SceneManager.GetActiveScene().buildIndex;

        instance = this;
        Time.timeScale = 1;

        gManagerGrade = GameObject.FindObjectOfType<gameManagerGrade>();
        gGabarito = GameObject.FindObjectOfType<Gabarito>();
    }

    public void ShowGameOver(){
        gManagerGrade.pausarJogo();
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame(){
        SceneManager.LoadScene(indexScene);
    }

    public void OpenScene(int index)
    {
        if(gGabarito != null){
            gGabarito.LimparGabaritoComparativo();
        }
        SceneManager.LoadScene(index);
    }
}
