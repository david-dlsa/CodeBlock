using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Gabarito : MonoBehaviour
{
    public LinhaGabarito[] linhas;

    public int indexLinhaAtual;
    public string tituloFase;

    private List<Transform> blocosRemovidos = new List<Transform>();

    List<Transform> listaB;

    // Dicion�rio para armazenar o gabarito comparativo
    Dictionary<string, HashSet<Vector2>> hashComparativo;


    MoveParaProximaFase gMoveParaProximaFase;
    gameController gGameController;
    gameManagerGrade gGameManagerGrade;
    Vida gVida;

    List<ElementoMatriz[]> matriz; // Declara��o da matriz como uma vari�vel de membro da classe

    AudioManager gAudioManager;

    void Start()
    {

        gMoveParaProximaFase = GameObject.FindObjectOfType<MoveParaProximaFase>();
        gGameController = GameObject.FindObjectOfType<gameController>();
        gGameManagerGrade = GameObject.FindObjectOfType<gameManagerGrade>();
        gVida = GetComponent<Vida>();
        gAudioManager = GameObject.FindObjectOfType<AudioManager>();
        
        AtualizarTituloPagina();

        indexLinhaAtual = 0;
        matriz = MontarMatriz();
        gerarBlocoLinha();

        // Inicializa o dicion�rio
        hashComparativo = new Dictionary<string, HashSet<Vector2>>();

        listaB = new List<Transform>(); // Criar uma nova lista vazia
    }

    // M�todo para montar a matriz com base nas linhas
    private List<ElementoMatriz[]> MontarMatriz()
    {
        List<ElementoMatriz[]> matriz = new List<ElementoMatriz[]>();
        int cloneID = 0;

        // Adicionar linhas � matriz
        for (int linhaIndex = 0; linhaIndex < linhas.Length; linhaIndex++)
        {
            LinhaGabarito linha = linhas[linhaIndex];
            ElementoMatriz[] linhaArray = new ElementoMatriz[linha.elementos.Length];

            // Preencher os elementos da linha com os �ndices e definir a disponibilidade
            for (int i = 0; i < linha.elementos.Length; i++)
            {
                cloneID++;
                string nomeBloco = linha.elementos[i].gameObject.name + "(clone)" + cloneID;
                bool spawnDisponivel = linhaIndex == 0 && i == 0; // Apenas o primeiro elemento da primeira linha � true
                bool foiUtilizado = false;
                int largura = linha.elementos[i].gameObject.GetComponent<ConstroiBloco>().largura;
                linhaArray[i] = new ElementoMatriz(nomeBloco, i, spawnDisponivel, linhaIndex, foiUtilizado, largura);
            }

            matriz.Add(linhaArray);
        }

        return matriz;
    }

    // M�todo para gerar os blocos com base na matriz
    public void gerarBlocoLinha()
    {
        // Chamar o m�todo ListaElementosDisponiveis e retorna a lista de elementos disponiveis 
        List<ElementoMatriz> elementosDisponiveis = ListaElementosDisponiveis(matriz);

        for (int i = 0; i < elementosDisponiveis.Count; i++)
        {
            Debug.LogError(i + " - Elemento: " + elementosDisponiveis[i].nome + ' ' + elementosDisponiveis[i].valor + ' ' + elementosDisponiveis[i].indexLinha);
            Debug.LogError("======================================");
        }

        // Verificar se h� elementos dispon�veis na lista
        if (elementosDisponiveis.Count > 0)
        {
            // Randomizar e pegar um elemento da lista elementosDisponiveis
            int randomIndex = UnityEngine.Random.Range(0, elementosDisponiveis.Count);
            ElementoMatriz elementoEscolhido = elementosDisponiveis[randomIndex];

            // Passar o elemento escolhido para o m�todo CriarPeca
            linhas[elementoEscolhido.indexLinha].CriarPeca(elementoEscolhido.valor);

            // Chamar um novo m�todo para atualizar a matriz, desativando o elemento escolhido
            AtualizarMatriz(matriz, elementoEscolhido, elementoEscolhido.indexLinha);
        }
        else
        {
            // N�o h� elementos dispon�veis na lista, logo todos os blocos do gabarito j� foram
            Debug.LogError("N�o h� elementos dispon�veis para criar a pe�a!");
            Debug.LogError("Todas as linhas da matriz j� foram percorridas!");
            gGameController.ShowPanel(gGameController.winPanel);
            gAudioManager.PlaySFX(gAudioManager.vitoriaSound);
        }
    }

    private int getLinhaDisponivel()
    {
        // Retorna a primeira linha da matiz com ao menos um elemento disponivel
        return matriz.FindIndex(linha => linha.Any(e => e.spawnDisponivel));
    }

    private List<ElementoMatriz> ListaElementosDisponiveis(List<ElementoMatriz[]> matriz)
    {
        List<ElementoMatriz> elementosDisponiveis = new List<ElementoMatriz>();

        int linha1Index = getLinhaDisponivel();
        //Debug.LogError("Linha index" + linha1Index);
        int linha2Index = linha1Index + 1;

        while (linha1Index != - 1 && linha1Index <= matriz.Count - 1 && matriz[linha1Index].Any(e => e.spawnDisponivel))
        {
            ElementoMatriz[] linha1 = matriz[linha1Index];
            // Verificar quais elementos est�o dispon�veis na linha 1
            List<ElementoMatriz> elementosDisponiveisLinha1 = linha1.Where(elemento => elemento.spawnDisponivel).ToList();
            elementosDisponiveis.AddRange(elementosDisponiveisLinha1);

            if(linha2Index <= matriz.Count - 1)
            {
                ElementoMatriz[] linha2 = matriz[linha2Index];
                // Verificar quais elementos est�o dispon�veis na linha 2
                List<ElementoMatriz> elementosDisponiveisLinha2 = linha2.Where(elemento => elemento.spawnDisponivel).ToList();
                // Unificar elementos dispon�veis das duas linhas  
                elementosDisponiveis.AddRange(elementosDisponiveisLinha2);
            }

            // Verificar se todos os elementos da linha 1 foram utilizados
            bool todosUtilizadosLinha1 = linha1.All(elemento => elemento.foiUtilizado);

            // Avan�ar para a pr�xima itera��o apenas se todos os elementos da linha1 foram utilizados
            if (todosUtilizadosLinha1)
            {
            /*    linha1Index++;
                linha2Index++;*/
            }
            else
            {
                break;
            }
        }

        if (linha1Index >= matriz.Count)
        {
            Debug.LogError("Todas as linhas da matriz j� foram percorridas!");
            gGameController.ShowPanel(gGameController.winPanel);
            gAudioManager.PlaySFX(gAudioManager.vitoriaSound);
        }

        return elementosDisponiveis;
    }

    private void liberaProximaPeca(int indexLinha)
    {
        // verificar se nao vai extrapolar a quantidade de itens na linha
        int indexLinhaAnterior;
        if (indexLinha != 0 && !matriz[indexLinha -1].All(elemento => elemento.foiUtilizado))
        {
            indexLinhaAnterior = Array.FindLastIndex(matriz[indexLinha - 1], e => e.spawnDisponivel);
        } 
        else
        {
            indexLinhaAnterior = indexLinha + 99;
        }
        

        int indexProximaPeca = Array.FindLastIndex(matriz[indexLinha], e => e.foiUtilizado) + 1;

        // libera a peca seguinte
        if (indexProximaPeca < matriz[indexLinha].Length)
        {
            //TODO um jeito de acessar todos os elementos da linha que est�o com foiUtilizado == true calculando a soma da largura deles e comparando cm o item da largura atual se der maior que da linha anterior nao pode
            // verifica se a peca seguinte nao cobre a anterior e se a linha atual � maior que a anterior
            if (CompararLarguras(indexLinha, matriz[indexLinha][indexProximaPeca].largura))
            {
                // Liber a peca seguinte
                matriz[indexLinha][indexProximaPeca].spawnDisponivel = true;
                //Debug.Log("Libera caso elemento NAO CUBRA elemento da linha anterior");
            }
        } // NAO PEGAR SE A LINHA JA TYA CONCLUIDA
    }

    private void AtualizarMatriz(List<ElementoMatriz[]> matriz, ElementoMatriz elemento, int indexLinha)
    {
        ElementoMatriz[] linhaArray = matriz[indexLinha];

        // Encontrar o elemento na linha e desativ�-lo
        ElementoMatriz elementoEncontrado = Array.Find(linhaArray, e => e.nome == elemento.nome);
        if (elementoEncontrado != null)
        {
            // Define a primeira linha encontrada que a bloco com spawn disponivel
            int primeiraLinhaDisponivel = getLinhaDisponivel();

            // Todos com foiUtilizado igual a true n�o devem virar spawnDisponivel == true
            elementoEncontrado.spawnDisponivel = false;
            elementoEncontrado.foiUtilizado = true;

            // Se a primeira linha disponivel encontrada for igual ao index da linha do elemento atual
            if (primeiraLinhaDisponivel == indexLinha)
            {
                // libera a peca seguinte na linha de mesmo index do elemento se tiver proximo elemento na linha
                if (elemento.valor + 1 <= matriz[indexLinha].Length - 1)
                {
                    //Debug.Log("Libera PROX elemento na MESMA linha");
                    matriz[indexLinha][elemento.valor + 1].spawnDisponivel = true;

                }// Se o index da linha n�o for 0 verificar se a linha atual � maior do que a anterior
                //bool naoBloqueiaPeca = indexLinha != 0 ? (matriz[indexLinha].Length > matriz[indexLinha - 1].Length || matriz[indexLinha].All(e => e.foiUtilizado)) : true;
                // libera a peca na mesma posicao da proxima linha se existir uma proxima linha na matriz
                if (indexLinha + 1 <= matriz.Count - 1)
                {
                    liberaProximaPeca(indexLinha + 1);
                }

            } else
            {
                // verifica se h� um proximo elemento na linha
                // verifica se a quantidade de elementos na linha atual � maior do que a linha anterior (Impendindo bloqueio de pe�as)
                if (elemento.valor + 1 <= matriz[indexLinha].Length - 1)
                {
                    int indexElementoLinhaAnterior = Array.FindLastIndex(matriz[primeiraLinhaDisponivel], e => e.spawnDisponivel);

                    // verifica se a peca seguinte nao cobre a anterior
                    if (CompararLarguras(indexLinha, matriz[indexLinha][elemento.valor + 1].largura))
                    {
                        // Liber a peca seguinte
                        //Debug.Log("Libera PROX elemento na MESMA linha");
                        matriz[indexLinha][elemento.valor + 1].spawnDisponivel = true;
                    }

                }
            }
        }
    }

        public void AtualizarCoordenadasY()
    {
        Dictionary<string, HashSet<Vector2>> novoHashComparativo = new Dictionary<string, HashSet<Vector2>>();

        foreach (var pair in hashComparativo)
        {
            string chave = pair.Key;
            List<Vector2> coordenadas = pair.Value.ToList(); // Converter List<Vector2>

            // Atualizar as coordenadas Y
            for (int i = 0; i < coordenadas.Count; i++)
            {
                Vector2 coordenada = coordenadas[i];
                coordenada.y = coordenada.y + 2; // Incrementa o valor de Y
                coordenadas[i] = coordenada;
            }

            HashSet<Vector2> coordenadasHashSet = new HashSet<Vector2>(coordenadas); // Converter para HashSet<Vector2>
            novoHashComparativo.Add(chave, coordenadasHashSet);
        }

        hashComparativo = novoHashComparativo;
    }

    public void gabaritoComparativo(Transform elemento)
    {
        if (elemento == null || elemento.gameObject == null)
        {
            // O objeto foi destru�do, fa�a o tratamento apropriado aqui
            return;
        }

        // Verifique se o componente ConstroiBloco est� presente no objeto
        ConstroiBloco blocoElemento = elemento.GetComponent<ConstroiBloco>();
        if (blocoElemento == null)
        {
            // O componente ConstroiBloco n�o est� presente no objeto, fa�a o tratamento apropriado aqui
            return;
        }

        foreach (LinhaGabarito linha in linhas)
        {
            foreach (Transform bloco in linha.elementos)
            {
                // Verifica se a chave j� existe no dicion�rio
                if (hashComparativo.ContainsKey(bloco.GetComponent<ConstroiBloco>().texto))
                {
                    // A chave existe, ent�o adiciona a coordenada no hash j� existente
                    HashSet<Vector2> coordenadas = hashComparativo[bloco.GetComponent<ConstroiBloco>().texto];
                    coordenadas.Add(new Vector2(bloco.GetComponent<ConstroiBloco>().x, bloco.GetComponent<ConstroiBloco>().y));
                }
                else
                {
                    // A chave n�o existe, ent�o cria uma nova entrada no dicion�rio
                    HashSet<Vector2> coordenadas = new HashSet<Vector2>();
                    coordenadas.Add(new Vector2(bloco.GetComponent<ConstroiBloco>().x, bloco.GetComponent<ConstroiBloco>().y));

                    hashComparativo.Add(bloco.GetComponent<ConstroiBloco>().texto, coordenadas);
                }
            }
        }

        // Verifique se o elemento atual existe no dicion�rio de hashes
        if (hashComparativo.ContainsKey(blocoElemento.texto))
        {
            HashSet<Vector2> coordenadas = hashComparativo[blocoElemento.texto];

            // Verifique se alguma das coordenadas do hash corresponde �s coordenadas do elemento atual
            if (coordenadas.Contains(new Vector2(((elemento.position.x + blocoElemento.largura) - 1), elemento.position.y)))
            {
                listaB.Add(elemento); // Adicionar o elemento atual � listaB
                Debug.Log("Elemento TEM coordenada correspondente");
                gAudioManager.PlaySFX(gAudioManager.conectadoSound);
                return;
            }

            Debug.Log("Elemento N�O tem coordenada correspondente");

            
            gVida.health--;
            if (gVida.health <= 0)
            {
                gGameController.ShowPanel(gGameController.gameOverPanel);
                gAudioManager.PlaySFX(gAudioManager.derrotaSound);

            }
            

            //TODO deve deletar, mas voltar ele para a lista de blocos que ser�o spawnados

            //TODO SE a linha estiver cheia, mas for o bloco errado n�o deve limpar!!!!
            // Apagar as linhas abaixo da linha atual
            gAudioManager.PlaySFX(gAudioManager.erradoSound);

            //TODO criar metodo para deletar linhas logicas, definindo todas elas como n�o utilizado e definir o primeiro
            //elemento da linha que errou como spawnDisponivel
            int indexLinhaMatriz = matriz.FindIndex(linha => linha.Any(e => e.nome == elemento.name));
            //limparLinhasLogicas(indexLinhaMatriz);

            int indexPrimeiraLinhaVisualExcluivel = 18 - (indexLinhaMatriz * 2);
            for (int i = indexLinhaMatriz; i > 0; i--)
            {
                gGameManagerGrade.deletaLinhasErradas(i);
            }
            return;
        }
        else
        {
            Debug.Log("Elemento n�o existe no dicion�rio de hashes");
        }

        // Fa�a o que for necess�rio com as informa��es comparativas
        // ...
    }

    public void LimparGabaritoComparativo()
    {
        hashComparativo.Clear();
    }

    private bool CompararLarguras(int indexLinha, int larguraProximoElemento)
    {
        ElementoMatriz[] linhaAnterior = matriz[indexLinha - 1];
        ElementoMatriz[] linhaAtual = matriz[indexLinha];

        int larguraLinhaAnterior = CalcularSomaLargura(linhaAnterior);
        int larguraLinhaAtual = CalcularSomaLargura(linhaAtual);

        if (larguraLinhaAtual + larguraProximoElemento > larguraLinhaAnterior)
        {
            Console.WriteLine("A largura da linha atual � maior do que a largura da linha anterior.");
            return false;
        }
        else
        {
            Console.WriteLine("A largura da linha atual � menor do que a largura da linha anterior.");
            return true;
        }
    }

    private int CalcularSomaLargura(ElementoMatriz[] linha)
    {
        int somaLargura = 0;
        foreach (ElementoMatriz elemento in linha)
        {
            if (elemento.foiUtilizado)
            {
                somaLargura += elemento.largura;
            }
        }
        return somaLargura;
    }


    private void AtualizarTituloPagina()
    {
        GameObject objTituloFase = GameObject.FindWithTag("tituloFase");
        if (objTituloFase != null)
        {
            objTituloFase.GetComponent<TextMeshPro>().text = tituloFase;
        }
    }

}