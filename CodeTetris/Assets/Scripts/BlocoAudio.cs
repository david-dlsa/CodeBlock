using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocoAudio : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip conectadoSound;
    public AudioClip erradoSound;
    public AudioClip LinhaCompletaSound;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(AudioClip sfx)  
    {
        audioSource.PlayOneShot(sfx);
    }
}
