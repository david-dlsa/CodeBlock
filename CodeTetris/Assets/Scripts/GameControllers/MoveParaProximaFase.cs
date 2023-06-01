using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveParaProximaFase : MonoBehaviour
{
    public int nextSceneLoad;

    Gabarito gGabarito;

    // Start is called before the first frame update
    void Start()
    {
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;

        gGabarito = GameObject.FindObjectOfType<Gabarito>();
    }

    public void proximaCena()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            Debug.LogError("VOCE GANHOU !!!");
        }
        else
        {
            // Atraso de 2 segundos antes de chamar a fun��o de carregamento da pr�xima cena
            //float delay = 1.5f;
            //Invoke("CarregarProximaCena", delay);
            CarregarProximaCena();

        }
    }

    private void CarregarProximaCena()
    {
        // Movendo para a pr�xima fase
        // TODO: Mudar para avan�ar para a pr�xima cena apenas ap�s clicar no bot�o no menu de vit�ria
        SceneManager.LoadScene(nextSceneLoad);

        // Definindo o valor do �ndice
        if (nextSceneLoad > PlayerPrefs.GetInt("levelAt"))
        {
            PlayerPrefs.SetInt("levelAt", nextSceneLoad);
        }
    }

}
