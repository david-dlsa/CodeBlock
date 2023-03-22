using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnTetro : MonoBehaviour {

    public int mostraProximaPeca;

    public Transform[] criaPecas;
    public List<GameObject> mostraPecas;

     private int cloneID = 0;


        // Use this for initialization
        void Start () {

        mostraProximaPeca = Random.Range(0, criaPecas.Length);
        Debug.Log("Tamanho array: "+criaPecas.Length + "Resultado: "+mostraProximaPeca);
        proximaPeca();
    }
        /*TODO 
        Fazer com que criar uma lista dos 7 tetronimos aleatoriamente, sem se repetir nesse lista. Apos isso cria uma nv lista
        com os mesmos 7 sem repetir...*/
    public void proximaPeca()
    {
        cloneID++;
        Debug.Log("Pos Spawn: "+transform.position);
        GameObject clone = Instantiate(criaPecas[mostraProximaPeca].gameObject, transform.position, Quaternion.identity);
        clone.name = clone.name + cloneID;

        mostraProximaPeca = Random.Range(0, criaPecas.Length);
       
        // for (int i = 0; i < mostraPecas.Count; i++)
        // {
        //     mostraPecas[i].SetActive(false);
        // }

        // mostraPecas[mostraProximaPeca].SetActive(true);
    }
}
