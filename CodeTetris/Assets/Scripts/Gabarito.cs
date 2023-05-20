using System.Linq;
using UnityEngine;

public class Gabarito : MonoBehaviour
{
    public LinhaGabarito[] linhas;

    private int elementoAtual;
    public int indexLinhaAtual;

    LinhaGabarito linhaAtual;

    void Start()
    {
        elementoAtual = 0;
        linhaAtual = linhas[indexLinhaAtual];
        gerarBlocoLinha();
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
   
}
