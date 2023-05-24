using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class Gabarito : MonoBehaviour
{
    public LinhaGabarito[] linhas;

    private int elementoAtual;
    public int indexLinhaAtual;

    LinhaGabarito linhaAtual;
    List<Transform> listaB;

    // Dicionário para armazenar o gabarito comparativo
    Dictionary<int, List<LinhaGabarito>> hashComparativo;

    void Start()
    {
        elementoAtual = 0;
        linhaAtual = linhas[indexLinhaAtual];
        gerarBlocoLinha();

        // Inicializa o dicionário
        hashComparativo = new Dictionary<int, List<LinhaGabarito>>();

        listaB = new List<Transform>(); // Criar uma nova lista vazia
    }

    public void gerarBlocoLinha()
    {
        //TODO tnho que ver como pra fzr pra manejar entre verificar qual linha eestar, onde att a linha, verificar se os elementos da linha ja foram

        //Verifica se ainda há linhas para serem geradas se acabar retorna
       
            //LinhaGabarito linhaAtual = linhas[indexLinhaAtual];

            //TODO Depois que vai pra prox linha ele consegue por um elemento, mas volta pra primeira linha

            //Cria um novo bloco
           

            //Se a incrementaçao resultou em um index que não existe no array este
            //array já foi e quero avançar para o próximo incrementando o index da linha

            if(elementoAtual <= (linhaAtual.elementos.Length - 1)){
                linhaAtual.CriarPeca(elementoAtual); // se isso me retornar algo eu criei um bloco e quero incrementar pro proximo
                elementoAtual++;
                Debug.Log(elementoAtual);
            }
            else
            {
                elementoAtual = 0;
                indexLinhaAtual++;
                if (indexLinhaAtual > (linhas.Length - 1))
                {
                    Debug.LogError("Todas as linhas do gabarito já foram esgotadas!");
                    return;
                }
                linhaAtual = linhas[indexLinhaAtual];
                Debug.LogWarning("Lista de elementos da linha atual está vazia! Avançando para a próxima linha...");
                gerarBlocoLinha();
            }
    }

    public void gabaritoComparativo(Transform elemento)
    {
        if (elemento == null)
        {
            // O objeto foi destruído, faça o tratamento apropriado aqui
            return;
        }

        // Crie um novo hash para armazenar as informações comparativas
        Dictionary<int, List<Transform>> hashComparativo = new Dictionary<int, List<Transform>>();

        // Copie os elementos de linhas[] para a listaA
        List<Transform> listaA = new List<Transform>();
        foreach (LinhaGabarito linha in linhas)
        {
            listaA.AddRange(linha.elementos);
        }

        // Adicione a listaA ao hash usando o índice 0
        hashComparativo.Add(0, listaA);

        // Verifique se o elemento atual existe na listaA e está na mesma posição
        bool elementoEncontrado = false;
        List<int> indicesElementoA = new List<int>(); // Lista de índices de elementos correspondentes

        for (int i = 0; i < listaA.Count; i++)
        {
            Transform elementoA = listaA[i];
            ConstroiBloco blocoA = elementoA.GetComponent<ConstroiBloco>();
            ConstroiBloco blocoElemento = elemento.GetComponent<ConstroiBloco>();
            if (blocoElemento == null)
            {
                // O componente ConstroiBloco não está presente no objeto, faça o tratamento apropriado aqui
                return;
            }

            if (blocoA.texto == blocoElemento.texto &&
                blocoA.x == blocoElemento.x &&
                blocoA.y == blocoElemento.y)
            {
                elementoEncontrado = true;
                indicesElementoA.Add(i); // Adicionar o índice do elemento correspondente encontrado
            }
        }

        if (elementoEncontrado)
        {
            listaB.Add(elemento); // Adicionar o elemento atual à listaB

            int indiceElementoB = listaB.FindIndex(x =>
            {
                ConstroiBloco blocoB = x.GetComponent<ConstroiBloco>();
                ConstroiBloco blocoElemento = elemento.GetComponent<ConstroiBloco>();

                return blocoB.texto == blocoElemento.texto &&
                       blocoB.x == blocoElemento.x &&
                       blocoB.y == blocoElemento.y;
            });

            int[] coordA = new int[2];
            int ultimoIndiceA = indicesElementoA.Count - 1;
            coordA[0] = listaA[indicesElementoA[ultimoIndiceA]].GetComponent<ConstroiBloco>().x;
            coordA[1] = listaA[indicesElementoA[ultimoIndiceA]].GetComponent<ConstroiBloco>().y;

            int[] coordB = new int[2];
            coordB[0] = (int)listaB[indiceElementoB].position.x;
            coordB[1] = (int)listaB[indiceElementoB].position.y;

            if (coordA[0] == coordB[0] && coordA[1] == coordB[1])
            {
                // O elemento está na mesma posição
                Debug.LogError("Elemento Atual ListaB == na listaA");
            }
            else
            {
                // O elemento está em uma posição diferente
                Debug.LogError("Elemento Atual ListaB NAO esta na listaA");
            }
        }
        else
        {
            // O elemento atual não existe na listaA
            Debug.Log("Elemento não existe na listaA");
        }

        // Faça o que for necessário com as informações comparativas
        // ...
    }

}
