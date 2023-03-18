using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectBehaviour : MonoBehaviour
{
    public string codePart;
    private Animator anim;
    public bool selecionado;
    protected void Start()
    {
        setConfiguracoesIniciais();
    }

    // Update é chamado uma vez por frame 
    protected void Update()
    {
        // verificando  se o usuario clickou na tela 
        if (Input.GetMouseButtonDown(0))
            //caso clicke executa a funcao clickObjeto() que verifica se o usuario clickou na caixa
            clickObjeto();
        //defineAnimacaoAtual();
    }



    /*
   * Quando ha um click na tela do jogo é chamado essa função que verifica se houve um click em um objeto , e toma medidas caso haja um click em qualquer objeto do jogo 
   */
    private void clickObjeto()
    {
       
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject == gameObject)
            {
                GameObjectBehaviour objetoAtual = hit.collider.gameObject.GetComponent<GameObjectBehaviour>();
                if (objetoAtual.selecionado)
                    objetoAtual.setSelecionado(false);
                else
                {	
                    objetoAtual.setSelecionado(true);
                }
                 
            }
        }else
		{
			this.setSelecionado(false);
		}
    }



    /*
     * pega o animator do objeto associado a instancia dessa classe e o define para ser usado durante a execução
     */
    private void setAnimator()
    {
        this.anim = this.gameObject.GetComponent<Animator>();
    }
    /*
     * define inicialmente os objetos como nao selecionados
     */
    private void setConfiguracoesIniciais()
    {
        this.selecionado = false;
        setAnimator();
    }
    
    /*
     * define a animação do objeto de acordo com a seleção do usuario
     */
    private void defineAnimacaoAtual()
    {
        if (this.selecionado)
            anim.SetBool("Click", true);
        else
            anim.SetBool("Click", false);
    }
    /*
     * retorna se o objeto esta selecionado ou não 
     Mudar pra protected de volta e arrumar pra chamar no Grade
     */
    public bool isSelecionado()
    {
        return this.selecionado;
    }

    /*
    * seta  o atributo selecionado desta instancia 
    */
    public void setSelecionado(bool selecionado)
    {
        this.selecionado = selecionado;
    }
}
