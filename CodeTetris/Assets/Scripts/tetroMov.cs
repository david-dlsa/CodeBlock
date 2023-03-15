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
    gameManager gManager;
    spawnTetro gSpawner;

    // Use this for initialization
    void Start()
    {
        timer = velocidade;
        child = transform.Find("partCode");

        gManager = GameObject.FindObjectOfType<gameManager>();
        gSpawner = GameObject.FindObjectOfType<spawnTetro>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gManager.pause){
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
                    gManager.atualizaGrade(this);
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
                    gManager.atualizaGrade(this);
                }
                else{
                    transform.position += new Vector3(1, 0, 0);
                }
            }
            //Tecla para ir para baixo
            if (Input.GetKey(KeyCode.DownArrow)){ //|| Time.time - queda >= 1){
            timer += Time.deltaTime;
            if(timer > velocidade){
                    transform.position += new Vector3(0, -1, 0);
                    timer = 0;
            }

                if (posicaoValida()){
                    gManager.atualizaGrade(this);
                }
                else{
                    transform.position += new Vector3(0, 1, 0);
                    //gManager.apagaLinha();

                    if(gManager.acimaGrade(this)){
                        gManager.gameOver();
                    }

                    gManager.score += 10;
                    enabled = false;
                    gSpawner.proximaPeca();
                }
                //queda = Time.time;
            }

            //Queda automatica da peça
            if(Time.time - queda >= 1 && !Input.GetKey(KeyCode.DownArrow)){
                transform.position += new Vector3(0, 1, 0);
                if (posicaoValida()){
                    gManager.atualizaGrade(this);
                }
                else{
                    transform.position += new Vector3(0, 1, 0);  
                    //gManager.apagaLinha();

                    if(gManager.acimaGrade(this)){
                        gManager.gameOver();
                    }

                    gManager.score += 10;
                    enabled = false;
                    gSpawner.proximaPeca();
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
                        gManager.atualizaGrade(this);
                    }
                    else{
                        transform.Rotate(0, 0, -90);
                    }
                }
                else{
                    transform.Rotate(0, 0, -90);

                    if (posicaoValida()){
                        gManager.atualizaGrade(this);
                    }
                    else{
                        transform.Rotate(0, 0, 90);
                    }
                }
            }
            else{
                transform.Rotate(0, 0, -90);

                if (posicaoValida()){
                    gManager.atualizaGrade(this);
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
            Vector2 posBloco = gManager.arredonda(child.position);

            if (gManager.dentroGrade(posBloco) == false)
            {
                return false;
            } 

            if(gManager.posicaoTransformGrade(posBloco) != null && 
                gManager.posicaoTransformGrade(posBloco).parent != transform){
                
                return false;
            }
        }
        return true;
    }
}