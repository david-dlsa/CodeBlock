using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundConfig : MonoBehaviour
{
    public static SoundConfig instance; // Instância do objeto de configuração

    public AudioClip conectadoSound; // Arquivo de áudio para quando estiver conectado
    public AudioClip erradoSound; // Arquivo de áudio para quando estiver errado
    public AudioClip limpaLinhaSound; // Arquivo de áudio para quando limpar linha
    public AudioClip vitoriaSound; // Arquivo de áudio para quando ganhar

}