using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadroResultado : MonoBehaviour
{

    public AudioClip limpaLinhaSound;

    SoundConfig gSoundConfig;

    void Start()
    {
        gSoundConfig = GameObject.FindObjectOfType<SoundConfig>();

        limpaLinhaSound = gSoundConfig.limpaLinhaSound;
    }

    public void AtivarFilhoPorIndex(int indexResultado)
    {
        int childCount = transform.childCount;
        //limpaLinhaSound = Resources.Load<AudioClip>("CasualGameSounds/DM-CGS-24");

        for (int i = 0; i < childCount; i++)
        {
            GameObject filho = transform.GetChild(i).gameObject;
            LinhaResultado linhaResultado = filho.GetComponent<LinhaResultado>();

            if (linhaResultado.pular == true)
            {
                // Se LinhaResultado.pular for verdadeiro, pule para a próxima iteração
                continue;
            }

            if (linhaResultado.index == indexResultado)
            {
                filho.SetActive(true);
            }
        }
    }

    public void DecrementarIndicesFilhos()
    {
        int childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            GameObject filho = transform.GetChild(i).gameObject;
            LinhaResultado LinhaResultado = filho.GetComponent<LinhaResultado>();
            LinhaResultado.index--;
        }
    }

    public void PlaySFX()
    {
        limpaLinhaSound = gSoundConfig.limpaLinhaSound;
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f; // Definir a altura do som
        audioSource.PlayOneShot(limpaLinhaSound);
        // StartCoroutine(StopSFXAfterDuration(audioSource, 0.5f));

    }

    private IEnumerator StopSFXAfterDuration(AudioSource audioSource, float duration)
    {
        yield return new WaitForSeconds(duration);
        audioSource.Stop();
    }
}