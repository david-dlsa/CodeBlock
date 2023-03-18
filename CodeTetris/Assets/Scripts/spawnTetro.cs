using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnTetro : MonoBehaviour {

    public int mostraProximaPeca;

    public Transform[] criaPecas;
    public List<GameObject> mostraPecas;

        // Use this for initialization
        void Start () {

        // mostraProximaPeca = Random.Range(0, 10);
        proximaPeca();
    }
        /*TODO 
        Fazer com que criar uma lista dos 7 tetronimos aleatoriamente, sem se repetir nesse lista. Apos isso cria uma nv lista
        com os mesmos 7 sem repetir...*/
    public void proximaPeca()
    {
        Instantiate(criaPecas[mostraProximaPeca], transform.position, Quaternion.identity);

        //mostraProximaPeca = Random.Range(0, 10);

        for (int i = 0; i < mostraPecas.Count; i++)
        {
            mostraPecas[i].SetActive(false);
        }

        //mostraPecas[mostraProximaPeca].SetActive(true);
    }
}
