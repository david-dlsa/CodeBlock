using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadroResultado : MonoBehaviour
{
    public void AtivarFilhoPorIndex(int indexResultado)
    {
        int childCount = transform.childCount;

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
}