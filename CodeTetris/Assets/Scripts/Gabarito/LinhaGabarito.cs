using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class LinhaGabarito : MonoBehaviour
{
    public Transform[] elementos;

    private int indexElementoAtual = 0;
    private int countElementoAtual = 0;

    Gabarito gGabarito;

    // Use this for initialization
    void Start()
    {
        gGabarito = GameObject.FindObjectOfType<Gabarito>();

        int cloneID = 0;
        // Cria uma lista auxiliar com os elementos da linha atual
        countElementoAtual = elementos.Length;
        CriarPeca(indexElementoAtual, gGabarito.matriz[0][0].nome, gGabarito.transform);
    }

    public void CriarPeca(int indexElementoAtual, string elementoNome, Transform transformGabarito)
    {
            // Instancia uma peça a partir do elemento selecionado
            GameObject clone = Instantiate(elementos[indexElementoAtual].gameObject, transformGabarito.position, Quaternion.identity);
            clone.name = elementoNome;

    }
}
