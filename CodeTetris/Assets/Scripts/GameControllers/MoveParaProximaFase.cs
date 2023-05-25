using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveParaProximaFase : MonoBehaviour
{
    public int nextSceneLoad;

    SoundConfig gSoundConfig;
    public AudioClip vitoriaSound;

    // Start is called before the first frame update
    void Start()
    {
        gSoundConfig = GetComponent<SoundConfig>();
        vitoriaSound = gSoundConfig.vitoriaSound;
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }

    public void proximaCena()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            Debug.LogError("VOCE GANHOU !!!");
        }
        else
        {
            // Atraso de 2 segundos antes de chamar a função de carregamento da próxima cena
            PlaySFX();
            float delay = 1.5f;
            Invoke("CarregarProximaCena", delay);

        }
    }

    private void CarregarProximaCena()
    {
        // Movendo para a próxima fase
        // TODO: Mudar para avançar para a próxima cena apenas após clicar no botão no menu de vitória
        SceneManager.LoadScene(nextSceneLoad);

        // Definindo o valor do índice
        if (nextSceneLoad > PlayerPrefs.GetInt("levelAt"))
        {
            PlayerPrefs.SetInt("levelAt", nextSceneLoad);
        }
    }

    public void PlaySFX()
    {
        vitoriaSound = gSoundConfig.limpaLinhaSound;
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.7f; // Definir a altura do som
        audioSource.priority = 255; // Definir uma prioridade alta
        audioSource.PlayOneShot(vitoriaSound);
    }
}
