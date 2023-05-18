using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class LinhaGabarito : MonoBehaviour
{
    public Transform[] elementos;
    public Transform[] elementosAux; 

    public int indexElementoAtual = 0;
    public int countElementoAtual = 0;
    private int indexLinhaAtual = 0;

    private int cloneID = 0;

    // Use this for initialization
    void Start()
    {

        // Cria uma lista auxiliar com os elementos da linha atual
        countElementoAtual = elementos.Length;
        fazCopiaElementos();
        CriarPeca(indexElementoAtual);
    }

    public void CriarPeca(int indexElementoAtual)
    {
        // Verifica se ainda h� elementos na lista auxiliar
        if (indexElementoAtual < (elementosAux.Length))
        {
            // Seleciona aleatoriamente um elemento da lista auxiliar
            //int index = Random.Range(0, elementosAux.Count);
            GameObject elementoAtual = elementosAux[indexElementoAtual].gameObject;

            // Instancia uma pe�a a partir do elemento selecionado
            cloneID++;
            GameObject clone = Instantiate(elementoAtual, transform.position, Quaternion.identity);
            clone.name = clone.name + cloneID;

            // Remove o elemento selecionado da lista auxiliar
            countElementoAtual++;
            return;
        }
        //Se n�o tiver mais elementos reinicia o indice de elementos
        else
        {
            Debug.LogWarning("Lista de elementos da linha atual est� vazia! Avan�ando para a pr�xima linha...");
            indexElementoAtual = 0;
            countElementoAtual = 0;
            return;
        }
    }

    public void fazCopiaElementos()
    {
        if(elementosAux.Length == 0)
        {
            if(elementosAux.Length != elementos.Length)
            {
                elementosAux = new Transform[elementos.Length];
            }
            countElementoAtual = elementos.Length;
            Array.Copy(elementos, elementosAux, elementos.Length);
        }
    }
}
