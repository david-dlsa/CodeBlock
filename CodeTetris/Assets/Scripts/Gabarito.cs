using System.Linq;
using UnityEngine;

public class Gabarito : MonoBehaviour
{
    public LinhaGabarito[] linhas;

    private int elementoAtual = 0;
    public int indexLinhaAtual;

    LinhaGabarito gLinhaGabarito;

    void Start()
    {
        LinhaGabarito linhaAtual = linhas[indexLinhaAtual];
        linhaAtual.fazCopiaElementos();
        gerarBlocoLinha();
    }

    public void gerarBlocoLinha()
    {
        //TODO tnho que ver como pra fzr pra manejar entre verificar qual linha eestar, onde att a linha, verificar se os elementos da linha ja foram

        //Verifica se ainda há linhas para serem geradas se acabar retorna
        if (indexLinhaAtual >= (linhas.Length - 1) && indexLinhaAtual!= 0)
        {
            Debug.LogError("Todas as linhas do gabarito já foram esgotadas!");
        }
        else
        {
            LinhaGabarito linhaAtual = linhas[indexLinhaAtual];
            linhaAtual = linhas[verificaLinhaAtual(linhaAtual, indexLinhaAtual)];

            //TODO Depois que vai pra prox linha ele consegue por um elemento, mas volta pra primeira linha

            //Cria um novo bloco
            linhaAtual.CriarPeca(elementoAtual); // se isso me retornar algo eu criei um bloco e quero incrementar pro proximo
            elementoAtual++;

            //Se a incrementaçao resultou em um index que não existe no array este
            //array já foi e quero avançar para o próximo incrementando o index da linha
        }


        
      
       
           
    }

    public int verificaLinhaAtual(LinhaGabarito linhaAtual, int indexLinhaAtual)
    {
        //Se acabar os elementos dentro da linha atual vai para a proxima linha
        if (linhaAtual.countElementoAtual > (linhaAtual.elementosAux.Length-1))
        {
            for (int i = 0; i < linhas.Length; i++)
            {
                linhas[i].countElementoAtual = 0;
                linhas[i].indexElementoAtual = 0;
            }
            elementoAtual = 0;
            indexLinhaAtual++;
            Debug.LogWarning("Lista de elementos da linha atual está vazia! Avançando para a próxima linha...");
            return indexLinhaAtual;
        }

        return indexLinhaAtual;
    }


}
