using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementoMatriz
{
    public string nome;
    public int valor; //index do elemento na linha
    public int indexLinha;
    public bool spawnDisponivel;
    public bool foiUtilizado;

    public ElementoMatriz(string nome, int valor, bool spawnDisponivel, int indexLinha, bool foiUtilizado)
    {
        this.nome = nome;
        this.valor = valor;
        this.spawnDisponivel = spawnDisponivel;
        this.indexLinha = indexLinha;
        this.foiUtilizado = foiUtilizado;
    }
}
