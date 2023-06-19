using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveParaProximaFase : MonoBehaviour
{
    public int nextSceneLoad;
    public GameObject loadingScreen;
    public Slider loadingBarFill;
    private Gabarito gGabarito;

    private void Start()
    {
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
        gGabarito = GameObject.FindObjectOfType<Gabarito>();
    }

    public void proximaCena()
    {
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            Debug.LogError("VOCÊ GANHOU!!!");
        }
        else
        {
            StartCoroutine(CarregarProximaCena());
        }
    }

    private IEnumerator CarregarProximaCena()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneLoad);
        loadingScreen.SetActive(true);

        // Definindo o valor do índice
        if (nextSceneLoad > PlayerPrefs.GetInt("levelAt"))
        {
            PlayerPrefs.SetInt("levelAt", nextSceneLoad);
        }

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            loadingBarFill.value = progressValue;

            yield return null;
        }

    }
}