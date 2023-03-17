using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour {

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
        Debug.Log("MET dentro_grade -- "+"X:" + (int)posicao.x + " // Y:" + (int)posicao.y + " // " + ((int)posicao.x >= 0 && (int)posicao.x < largura) + ((int)posicao.y < altura - 1));
        return ((int)posicao.x >= 0 && (int)posicao.x < largura /*&& (int)posicao.y >= 0*/ && (int)posicao.y < altura - 2);
    }

    public Vector2 arredonda(Vector2 nA)
    {
        return new Vector2(Mathf.Round(nA.x), Mathf.Round(nA.y));
    }

    public void atualizaGrade(tetroMov pecaTetris){
        for (int y=0; y < altura; y++){
            for (int x=0; x < largura; x++){
                if (grade [x,y] != null){
                    if(grade[x,y].parent == pecaTetris.transform){
                        grade[x,y] = null;
                    }
                }
            }
        }
        Debug.Log("Peça tetris: "+pecaTetris);
        foreach (Transform peca in pecaTetris.transform){
            Vector2 posicao = arredonda(peca.position);
            Debug.Log("posição: "+posicao+" // Peça: "+peca + " // grade antes: "+grade.Length);
            if(posicao.y >= 0 && posicao.y < altura - 1){
                grade[(int)posicao.x, (int)posicao.y] = peca;
                Debug.Log("grade DEPOIS: "+grade.Length + "// peça colocada " + grade[(int)posicao.x, (int)posicao.y]);
            }
        }
    }

    public Transform posicaoTransformGrade(Vector2 posicao){
        if(posicao.y < altura - 1){
            return null;
        }
        else{
            return grade[(int)posicao.x, (int)posicao.y];
        }
    }

    public bool linhaCheia(int y)
    {
        for (int x = 0; x < largura; x++)
        {
            if (grade[x,y] == null)
            {
                return false;
            }
        }
        return true;
    }

    public void deletaQuadrado(int y)
    {
        for (int x = 0; x < largura; x++)
        {
            Destroy(grade[x, y].gameObject);

            grade[x, y] = null;
        }
    }

    public void moveLinhaBaixo(int y)
    {
        for (int x = 0; x < largura; x++)
        {
            if(grade[x,y] != null)
            {
                grade[x, y - 1] = grade[x, y];
                grade[x, y] = null;
                grade[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public void moveTodasLinhasBaixo (int y)
    {
        for (int i = y; i < altura; i++)
        {
            moveLinhaBaixo(i);
        }
    }

    public void apagaLinha()
    {
        for (int y = 0; y < altura; y++)
        {
            if (linhaCheia(y)){
                deletaQuadrado(y);
                moveTodasLinhasBaixo(y + 1);
                y--;
                score += 100;
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

    public void gameOver()
    {
        SceneManager.LoadScene("gameOver");
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
