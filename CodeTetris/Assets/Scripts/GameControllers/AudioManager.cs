using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("--- Audio Source ---")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("--- Audio Source ---")]
    public AudioClip GameBackground;
    public AudioClip MenuBackground;

    public static AudioManager instance;

    int indiceCenaAtual;
    AudioClip musicaAtual;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Start is called before the first frame update
    void Start()
    {
        indiceCenaAtual = SceneManager.GetActiveScene().buildIndex;
        SelecionaMusica();
    }

    private void SelecionaMusica()
    {
        if (indiceCenaAtual == 0 || indiceCenaAtual == 1)
        {
            musicSource.clip = MenuBackground;
        }
        else
        {
            musicSource.clip = GameBackground;
        }

        if (musicSource.clip != musicaAtual)
        {
            musicaAtual = musicSource.clip;
            musicSource.Play();
        }

        musicSource.loop = false;
        StartCoroutine(CheckMusicEnd());
    }

    private IEnumerator CheckMusicEnd()
    {
        while (true)
        {
            if (!musicSource.isPlaying)
            {
                musicSource.Play();
            }

            yield return null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        indiceCenaAtual = scene.buildIndex;
        SelecionaMusica();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}