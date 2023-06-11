using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class LinhaGabarito : MonoBehaviour
{
    public Transform[] elementos;

    public int indexElementoAtual = 0;
    public int countElementoAtual = 0;

    private int cloneID = 0;

    // Use this for initialization
    void Start()
    {

        // Cria uma lista auxiliar com os elementos da linha atual
        countElementoAtual = elementos.Length;
        CriarPeca(indexElementoAtual);
    }

    public void CriarPeca(int indexElementoAtual)
    {
            // Instancia uma peça a partir do elemento selecionado
            cloneID++;
            GameObject clone = Instantiate(elementos[indexElementoAtual].gameObject, transform.position, Quaternion.identity);
            clone.name = clone.name + cloneID;

       
    }
}
