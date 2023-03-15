using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TetrominoBehaviour : GameObjectBehaviour
{
	
	private GameObject[] tetrominos;
 
 	void Start()
    {
        base.Start();
    }
    // Update is called once per frame
    void Update()
    {	
        base.Update();
		verificaSelecao();
		//setConfiguracoesIniciais();	
    }
	//remove os compostos caso cheguem no limite da tela sem ser selecionado
    
	// movimenta os compostos 
   
    private void setConfiguracoesIniciais()
    {      
		
    }

	/*
	* pega todos os compostos 
	*/
	private void getCompostos()
    {
        this.tetrominos = GameObject.FindGameObjectsWithTag ("tetromino");
    }

    private void removeSelecao()
    {
      //	pega todos os tetrominos atuais e seta como não selecionado 
        getCompostos();
      	 for (int i = 0; i < this.tetrominos.Length; i++)
       	{
            this.tetrominos[i].GetComponent<TetrominoBehaviour>().selecionado = false;
        }
		// seta como selecionado apenas o elemento atual desta instancia 
       	this.selecionado = true;
    }

	/*
  	  * verifica se o composto  atual esta selecionado e executa um metodo corretivo da animaxao 
    */
	private void verificaSelecao()
	{
		if(isSelecionado())
			removeSelecao();
	}

    /*
    *  
    private void renomearFormula(){
        GameObject originalGameObject = this.gameObject;
        GameObject child = originalGameObject.transform.GetChild(0).gameObject;
        child.GetComponent<TMP_Text>().text = "pao";
        //Debug.Log(child.GetComponent<TMP_Text>());
    }
     */
}