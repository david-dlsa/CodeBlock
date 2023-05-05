using System.Linq;
using UnityEngine;

public class Gabarito : MonoBehaviour
{
    public LinhaGabarito[] linhas;

    private int elementoAtual = 0;
    int indexLinhaAtual;

    LinhaGabarito gLinhaGabarito;

    void Start()
    {
        gLinhaGabarito = GetComponent<LinhaGabarito>();
        
        gerarBlocoLinha();
    }

    public void gerarBlocoLinha()
    {
//TODO tnho que ver como pra fzr pra manejar entre verificar qual linha eestar, onde att a linha, verificar se os elementos da linha ja foram
        LinhaGabarito linhaAtual = linhas[indexLinhaAtual];
        linhaAtual.verificaLinhaAtual();
      
        //Cria um novo bloco
        linhaAtual.CriarPeca(elementoAtual);
        elementoAtual++;
           
        }

       
    }
}
