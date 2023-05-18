using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tetroMov : MonoBehaviour
{

    public bool podeRodar;
    public bool roda360;

    public float queda;

    public float velocidade;
    public float timer;

    Transform child;
    gameManagerGrade gManagerGrade;

    gameController gController;

    spawnTetro gSpawner;

    Gabarito gGabarito;

    // Use this for initialization
    void Start()
    {
        timer = velocidade;
        child = transform.Find("partCode");

        gManagerGrade = GameObject.FindObjectOfType<gameManagerGrade>();
        gController = GameObject.FindObjectOfType<gameController>();
        gSpawner = GameObject.FindObjectOfType<spawnTetro>();
        gGabarito = GameObject.FindObjectOfType<Gabarito>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gManagerGrade.pause){
            if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.DownArrow))
                timer = velocidade;

            //Tecla para ir para direita
            if (Input.GetKey(KeyCode.RightArrow)){
                timer += Time.deltaTime;

                if (timer > velocidade){
                    transform.position += new Vector3(1, 0, 0);
                    timer = 0;
                }
                
                if (posicaoValida()){
                    gManagerGrade.atualizaGrade(this);
                }
                else{
                    transform.position += new Vector3(-1, 0, 0);
                }
            }
            //Tecla para ir para esquerda
            if (Input.GetKey(KeyCode.LeftArrow)){
                timer += Time.deltaTime;
                
                if(timer > velocidade){
                    transform.position += new Vector3(-1, 0, 0);
                    timer = 0;
                }

                if (posicaoValida()){
                    gManagerGrade.atualizaGrade(this);
                }
                else{
                    transform.position += new Vector3(1, 0, 0);
                }
            }

            //Tecla para ir para baixo  
            //TODO TEM QUE SER RETIRADO DEPOIS
            if (Input.GetKey(KeyCode.DownArrow)){ //|| Time.time - queda >= 1){
            timer += Time.deltaTime;
            if(timer > velocidade){
                    transform.position += new Vector3(0, -1, 0);
                    timer = 0;
            }

                if (posicaoValida()){
                    gManagerGrade.atualizaGrade(this);
                }
                else{
                    transform.position += new Vector3(0, 1, 0);
                    //gManagerGrade.apagaLinha();

                    if(gManagerGrade.abaixoGrade(this)){
                        Debug.Log("GAME OVER (down):" + gManagerGrade.abaixoGrade(this));
                        //gController.ShowGameOver();
                    }

                    gManagerGrade.score += 10;
                    enabled = false;
                }
                //queda = Time.time;
            }

            //Tecla para ir para cima
            if (Input.GetKey(KeyCode.UpArrow)){ //|| Time.time - queda >= 1){
            timer += Time.deltaTime;
            if(timer > velocidade){
                    transform.position += new Vector3(0, 1, 0);
                    timer = 0;
            }

                if (posicaoValida()){
                    gManagerGrade.atualizaGrade(this);
                }
                else{
                    transform.position += new Vector3(0, -1, 0);
                    gManagerGrade.apagaLinha(this);

                    if(gManagerGrade.abaixoGrade(this)){
                        Debug.Log("GAME OVER (up):" + gManagerGrade.abaixoGrade(this));
                        //gController.ShowGameOver();
                    }
                    else{
                    gManagerGrade.score += 10;
                    enabled = false;
                    gGabarito.gerarBlocoLinha();
                    //gSpawner.proximaPeca();
                    }
                }
                //queda = Time.time;
            }

            //Sobe automatica da peça
            if(Time.time - queda >= 1 && !Input.GetKey(KeyCode.DownArrow)){
                transform.position += new Vector3(0, 1, 0);
                if (posicaoValida()){
                    gManagerGrade.atualizaGrade(this);
                }
                else{
                    transform.position += new Vector3(0, -1, 0);  
                    gManagerGrade.apagaLinha(this);

                    if(gManagerGrade.abaixoGrade(this)){
                        Debug.Log("GAME OVER (auto):" + gManagerGrade.abaixoGrade(this));
                        //gController.ShowGameOver();
                    }
                    else{
                    gManagerGrade.score += 10;
                    enabled = false;
                    gGabarito.gerarBlocoLinha();
                    //gSpawner.proximaPeca();
                    }
                }
                queda = Time.time;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow)){
                checaRoda();
            }
        }
    }

    void checaRoda()
    {
        if (podeRodar){
            if (!roda360){
                if (transform.rotation.z < 0){
                    
                    transform.Rotate(0, 0, 90);
                    
                  
                    if (posicaoValida()){
                        gManagerGrade.atualizaGrade(this);
                    }
                    else{
                        transform.Rotate(0, 0, -90);
                    }
                }
                else{
                    transform.Rotate(0, 0, -90);

                    if (posicaoValida()){
                        gManagerGrade.atualizaGrade(this);
                    }
                    else{
                        transform.Rotate(0, 0, 90);
                    }
                }
            }
            else{
                transform.Rotate(0, 0, -90);

                if (posicaoValida()){
                    gManagerGrade.atualizaGrade(this);
                }
                else{
                    transform.Rotate(0, 0, 90);
                }
            }
        }
    }

    bool posicaoValida()
    {
        foreach (Transform child in transform)
        {
            Vector2 posBloco = gManagerGrade.arredonda(child.position);

            if (gManagerGrade.dentroGrade(posBloco) == false)
            {
                Debug.Log("FORA da grade 1 com pos: "+posBloco);
                return false;
            } 

            if(gManagerGrade.posicaoTransformGrade(posBloco) != null && 
                gManagerGrade.posicaoTransformGrade(posBloco).parent != transform){
                Debug.Log("FORA da grade 2 com pos: "+posBloco);
                return false;
            }
        }
        //Debug.Log("DENTRO da grade");
        return true;
    }
}