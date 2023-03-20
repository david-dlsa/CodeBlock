using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameManagerGrade : MonoBehaviour {

    public static int altura = 20;
    public static int largura = 12;

    public int score = 0;
    public Text textoScore;

    public bool pause = false;

    public static Transform[,] grade = new Transform[largura, altura];

    void Update(){
        textoScore.text = "Pontos: " + score.ToString();
    }

    //Esta dentro da largura e se é menor que a altura
    public bool dentroGrade(Vector2 posicao)
    {
        //Debug.Log("MET dentro_grade -- "+"X:" + (int)posicao.x + " // Y:" + (int)posicao.y + " // " + ((int)posicao.x >= 0 && (int)posicao.x < largura) + ((int)posicao.y < altura));
        return ((int)posicao.x >= 0 && (int)posicao.x < largura /*&& (int)posicao.y >= 0*/ && (int)posicao.y < altura);
    }

    public Vector2 arredonda(Vector2 nA)
    {
        return new Vector2(Mathf.Round(nA.x), Mathf.Round(nA.y));
    }

    public void atualizaGrade(tetroMov pecaTetris){
        for (int y=altura-1; y >= 0; y--){
            for (int x=0; x < largura; x++){
                if (grade [x,y] != null){
                    if(grade[x,y].parent == pecaTetris.transform){
                        //Debug.Log("Nao Colocar Ainda, ta se movendo pra cima");
                        grade[x,y] = null;
                    }
                }
            }
        }
        //Debug.Log("Peça tetris: "+pecaTetris+" // Tamanho: "+ pecaTetris.transform.childCount);
        foreach (Transform peca in pecaTetris.transform){
            //Não conta filho de pecaTetris que não esteja com a Tag pecaBloco
            if(peca.CompareTag("pecaBloco")){
                Vector2 posicao = arredonda(peca.position);
                if(posicao.y >= 0){
                    grade[(int)posicao.x, (int)posicao.y] = peca;
                    //Debug.Log("peça colocada " + grade[(int)posicao.x, (int)posicao.y] + " e " + pecaTetris);
                }
            }
        }
    }

    //Define que o bloco está fixo em determinada posição da
    public Transform posicaoTransformGrade(Vector2 posicao){
        if(posicao.y < 0){
            return null;
        }
        else{
           // Debug.Log("PosX: "+ (int)posicao.x + " // PosY: " + (int)posicao.y);
            return grade[(int)posicao.x, (int)posicao.y];
        }
    }

    public bool linhaCheia(int y)
    {
        for (int x = 0; x < largura; x++)
        {
            Debug.Log("LINHA CHEIA posição X = " + x + " e Y = " + y);
            if (grade[x,y] == null)
            {
                return false;
            }
        }
        return true;
    }

    public void deletaQuadrado(int y)
    {
        // if (y >= 0 && y < altura){
            for (int x = 0; x < largura; x++)
                {
                    if (grade[x, y] != null)
                    {
                        Debug.Log("1-Excluindo bloco da posição X = " + x + " e Y = " + y + " // tenho isso na grade: "+grade[x, y]+" isso é oque estou destruindo: "+grade[x, y].transform.parent.gameObject);
                        Destroy(grade[x, y].transform.parent.gameObject);
                        grade[x, y] = null;
                    }
                }
        //}
        
    }

    public void moveLinhaBaixo(int y)
    {
        for (int x = 0; x < largura; x++)
        {
            if(grade[x,y] != null)
            {
                //estrutural de como os blocos estão movendo
                grade[x, y + 1] = grade[x, y];
                grade[x, y] = null;
                //parte visual de movimentar os blocos
                grade[x, y + 1].position += new Vector3(0, 2, 0);
            }
        }
    }

    public void moveTodasLinhasBaixo (int y)
    {
        for (int i = y; i >= 0; i--)
        {
            moveLinhaBaixo(i);
        }
    }

    public void apagaLinha(tetroMov pecaTetris)
    {
        for (int y=altura-1; y >= 0; y--)
        {
            Debug.Log("Estou na altura "+ y + " e abaixo de mim é " + (y-1));
            if (linhaCheia(y)){
                Debug.Log("Linha "+ y + " e linha" + (y-1) + "estão cheias");
                deletaQuadrado(y);
                moveTodasLinhasBaixo(y - 1);
                //moveTodasLinhasBaixo(y - 1); //ver pq + 1
                y++;
                score += 50;
            }
        }
    }

    public bool abaixoGrade(tetroMov pecaTetro)
    {
        for (int x = 0; x < largura; x++)
        {
            foreach (Transform quadrado in pecaTetro.transform)
            {
                Vector2 posicao = arredonda(quadrado.position);

                if (posicao.y < 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void pausarJogo()
    {

        if (pause)
        {
            pause = false;
        }
        else
        {
            pause = true;
        }

    }

}
