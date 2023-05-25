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

    QuadroResultado gQuadroResultado;

    void Start()
    {
        gQuadroResultado = GameObject.FindObjectOfType<QuadroResultado>();
    }

    void Update(){
        //textoScore.text = "Pontos: " + score.ToString();
    }

    //Esta dentro da largura e se é menor que a altura
    public bool dentroGrade(Vector2 posicao)
    {
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
                        grade[x,y] = null;
                    }
                }
            }
        }
        
        foreach (Transform peca in pecaTetris.transform){
            //Não conta filho de pecaTetris que não esteja com a Tag pecaBloco
            if(peca.CompareTag("pecaBloco")){
                Vector2 posicao = arredonda(peca.position);
                if(posicao.y >= 0){
                    grade[(int)posicao.x, (int)posicao.y] = peca;
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
        //string textos = "";
        //ConstroiBloco bloco;
        for (int x = 0; x < largura; x++)
        {
            if (grade[x, y] != null)
            {
                //bloco = grade[x, y].transform.parent.gameObject.GetComponent<ConstroiBloco>();
                //textos += " " + bloco.texto;
                Destroy(grade[x, y].transform.parent.gameObject);
                grade[x, y] = null;
            }
            if (grade[x, (y+1)] != null)
            {
                Destroy(grade[x, (y + 1)].transform.parent.gameObject);
                grade[x, (y + 1)] = null;
            }
        }
        int index = (18 - y)/2;

        gQuadroResultado.AtivarFilhoPorIndex(index);
        gQuadroResultado.PlaySFX();
        gQuadroResultado.DecrementarIndicesFilhos();
    }

    public void moveLinhaCima(int y)
    {
        string nomeBlocoAnterior = "";
        for (int x = 0; x < largura; x++)
        {
            if(grade[x,y] != null){
                string nomeBlocoAtual = grade[x, y].transform.parent.gameObject.name;
                //estrutural de como os blocos estão movendo
                grade[x, y + 2] = grade[x, y];
                if (nomeBlocoAnterior != nomeBlocoAtual)
                {
                    nomeBlocoAnterior = nomeBlocoAtual;
                    Transform partCode = grade[x, y].transform.parent.gameObject.transform.Find("partCode").transform;
                    Vector2 newPosition = partCode.position;
                    newPosition.y = newPosition.y + 1.0f;
                    partCode.position = newPosition;
                }
                grade[x, y] = null;

                //parte visual de movimentar os blocos
                grade[x, y + 2].position += new Vector3(0, 2, 0);
               
            }
            
        }
    }

    public void moveTodasLinhasCima (int y)
    {
        for (int i = y; i >= 0; i--)
        {
            moveLinhaCima(i);
        }
    }

    public void apagaLinha(tetroMov pecaTetris)
    {
        
        for (int y = altura - 1; y >= 0; y--)
        {
            if (linhaCheia(y))
            {
                int auxY;
                if (y % 2 == 0)
                {
                    auxY = y;
                }
                else
                {
                    auxY = y - 1;
                }

                deletaQuadrado(auxY);
                moveTodasLinhasCima(auxY - 1);
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
