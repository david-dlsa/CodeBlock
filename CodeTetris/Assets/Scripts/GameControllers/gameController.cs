using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SeletorDeFase;

public class gameController : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public GameObject optionsPanel;
    private int indexScene;

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

    public void ShowPanel(GameObject panel)
    {
        if (gManagerGrade != null)
        {
            gManagerGrade.pausarJogo();
            Time.timeScale = 1;
        }
        panel.SetActive(true);
    }


    public void ClosePanel(GameObject panel)
    {
        if (gManagerGrade != null)
        {
            gManagerGrade.pausarJogo();
            Time.timeScale = 1;
        }
        panel.SetActive(false);
    }

    public void RestartGame(){
        SceneManager.LoadScene(indexScene);
    }

    public void FecharJogo()
    {
        Application.Quit();
    }

    public void OpenScene(int index)
    {
        if(gGabarito != null){
            gGabarito.LimparGabaritoComparativo();
        }
        SceneManager.LoadScene(index);
    }
}
