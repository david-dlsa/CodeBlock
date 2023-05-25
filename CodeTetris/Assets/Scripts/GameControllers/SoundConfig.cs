using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundConfig : MonoBehaviour
{
    public static SoundConfig instance; // Inst�ncia do objeto de configura��o

    public AudioClip conectadoSound; // Arquivo de �udio para quando estiver conectado
    public AudioClip erradoSound; // Arquivo de �udio para quando estiver errado
    public AudioClip limpaLinhaSound; // Arquivo de �udio para quando limpar linha
    public AudioClip vitoriaSound; // Arquivo de �udio para quando ganhar

}