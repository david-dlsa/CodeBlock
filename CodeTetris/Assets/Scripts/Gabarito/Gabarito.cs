using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Gabarito : MonoBehaviour
{
    public LinhaGabarito[] linhas;

    public int indexLinhaMatriz;
    public int indexLinhaAtual;
    public string tituloFase;

    private List<Transform> blocosRemovidos = new List<Transform>();

    List<Transform> listaB;

    // Dicionário para armazenar o gabarito comparativo
    Dictionary<string, HashSet<Vector2>> hashComparativo;


    MoveParaProximaFase gMoveParaProximaFase;
    gameController gGameController;
    gameManagerGrade gGameManagerGrade;
    Vida gVida;

    public List<ElementoMatriz[]> matriz; // Declaração da matriz como uma variável de membro da classe

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

        // Inicializa o dicionário
        hashComparativo = new Dictionary<string, HashSet<Vector2>>();

        listaB = new List<Transform>(); // Criar uma nova lista vazia
    }

    // Método para montar a matriz com base nas linhas
    private List<ElementoMatriz[]> MontarMatriz()
    {
        List<ElementoMatriz[]> matriz = new List<ElementoMatriz[]>();
        int cloneID = 0;

        // Adicionar linhas à matriz
        for (int linhaIndex = 0; linhaIndex < linhas.Length; linhaIndex++)
        {
            LinhaGabarito linha = linhas[linhaIndex];
            ElementoMatriz[] linhaArray = new ElementoMatriz[linha.elementos.Length];

            // Preencher os elementos da linha com os índices e definir a disponibilidade
            for (int i = 0; i < linha.elementos.Length; i++)
            {
                cloneID++;
                string nomeBloco = linha.elementos[i].gameObject.name + "(Clone)" + cloneID;
                bool spawnDisponivel = linhaIndex == 0 && i == 0; // Apenas o primeiro elemento da primeira linha é true
                bool foiUtilizado = false;
                int largura = linha.elementos[i].gameObject.GetComponent<ConstroiBloco>().largura;
                linhaArray[i] = new ElementoMatriz(nomeBloco, i, spawnDisponivel, linhaIndex, foiUtilizado, largura);
            }

            matriz.Add(linhaArray);
        }

        return matriz;
    }

    // Método para gerar os blocos com base na matriz
    public void gerarBlocoLinha()
    {
        // Chamar o método ListaElementosDisponiveis e retorna a lista de elementos disponiveis 
        List<ElementoMatriz> elementosDisponiveis = ListaElementosDisponiveis(matriz);

        for (int i = 0; i < elementosDisponiveis.Count; i++)
        {
            Debug.LogError(i + " - Elemento: " + elementosDisponiveis[i].nome + ' ' + elementosDisponiveis[i].valor + ' ' + elementosDisponiveis[i].indexLinha);
            Debug.LogError("======================================");
        }

        // Verificar se há elementos disponíveis na lista
        if (elementosDisponiveis.Count > 0)
        {
            // Randomizar e pegar um elemento da lista elementosDisponiveis
            int randomIndex = UnityEngine.Random.Range(0, elementosDisponiveis.Count);
            ElementoMatriz elementoEscolhido = elementosDisponiveis[randomIndex];

            // Passar o elemento escolhido para o método CriarPeca
            linhas[elementoEscolhido.indexLinha].CriarPeca(elementoEscolhido.valor, elementoEscolhido.nome);

            // Chamar um novo método para atualizar a matriz, desativando o elemento escolhido
            AtualizarMatriz(matriz, elementoEscolhido, elementoEscolhido.indexLinha);
        }
        else
        {
            // Não há elementos disponíveis na lista, logo todos os blocos do gabarito já foram
            Debug.LogError("Não há elementos disponíveis para criar a peça!");
            Debug.LogError("Todas as linhas da matriz já foram percorridas!");
            // Definindo o valor do índice
            if (gMoveParaProximaFase.nextSceneLoad > PlayerPrefs.GetInt("levelAt"))
            {
                PlayerPrefs.SetInt("levelAt", gMoveParaProximaFase.nextSceneLoad);
            }
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

        foreach (ElementoMatriz[] linha in matriz)
        {
            elementosDisponiveis.AddRange(linha.Where(elemento => elemento.spawnDisponivel));
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
            //TODO um jeito de acessar todos os elementos da linha que estão com foiUtilizado == true calculando a soma da largura deles e comparando cm o item da largura atual se der maior que da linha anterior nao pode
            // verifica se a peca seguinte nao cobre a anterior e se a linha atual é maior que a anterior
            if (CompararLarguras(indexLinha, matriz[indexLinha][indexProximaPeca].largura) && !matriz[indexLinha][indexProximaPeca].foiUtilizado)
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

        // Encontrar o elemento na linha e desativá-lo
        ElementoMatriz elementoEncontrado = Array.Find(linhaArray, e => e.nome == elemento.nome);
        if (elementoEncontrado != null)
        {
            // Define a primeira linha encontrada que a bloco com spawn disponivel
            int primeiraLinhaDisponivel = getLinhaDisponivel();

            // Todos com foiUtilizado igual a true não devem virar spawnDisponivel == true
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

                }// Se o index da linha não for 0 verificar se a linha atual é maior do que a anterior
                //bool naoBloqueiaPeca = indexLinha != 0 ? (matriz[indexLinha].Length > matriz[indexLinha - 1].Length || matriz[indexLinha].All(e => e.foiUtilizado)) : true;
                // libera a peca na mesma posicao da proxima linha se existir uma proxima linha na matriz
                if (indexLinha + 1 <= matriz.Count - 1)
                {
                    liberaProximaPeca(indexLinha + 1);
                }

            } else
            {
                // verifica se há um proximo elemento na linha
                // verifica se a quantidade de elementos na linha atual é maior do que a linha anterior (Impendindo bloqueio de peças)
                if (elemento.valor + 1 <= matriz[indexLinha].Length - 1)
                {
                    //int indexElementoLinhaAnterior = Array.FindLastIndex(matriz[primeiraLinhaDisponivel], e => e.spawnDisponivel);

                    // verifica se a peca seguinte nao cobre a anterior
                    //TODO fazer outro if, caso a linha nao tenha nenhum spawn disponivel pegar o primeiro elemento OU pegar a partir do elemento que tem spawn disponivel
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
            // O objeto foi destruído, faça o tratamento apropriado aqui
            return;
        }

        // Verifique se o componente ConstroiBloco está presente no objeto
        ConstroiBloco blocoElemento = elemento.GetComponent<ConstroiBloco>();
        if (blocoElemento == null)
        {
            // O componente ConstroiBloco não está presente no objeto, faça o tratamento apropriado aqui
            return;
        }

        foreach (LinhaGabarito linha in linhas)
        {
            foreach (Transform bloco in linha.elementos)
            {
                // Verifica se a chave já existe no dicionário
                if (hashComparativo.ContainsKey(bloco.GetComponent<ConstroiBloco>().texto))
                {
                    // A chave existe, então adiciona a coordenada no hash já existente
                    HashSet<Vector2> coordenadas = hashComparativo[bloco.GetComponent<ConstroiBloco>().texto];
                    coordenadas.Add(new Vector2(bloco.GetComponent<ConstroiBloco>().x, bloco.GetComponent<ConstroiBloco>().y));
                }
                else
                {
                    // A chave não existe, então cria uma nova entrada no dicionário
                    HashSet<Vector2> coordenadas = new HashSet<Vector2>();
                    coordenadas.Add(new Vector2(bloco.GetComponent<ConstroiBloco>().x, bloco.GetComponent<ConstroiBloco>().y));

                    hashComparativo.Add(bloco.GetComponent<ConstroiBloco>().texto, coordenadas);
                }
            }
        }

        // Verifique se o elemento atual existe no dicionário de hashes
        if (hashComparativo.ContainsKey(blocoElemento.texto))
        {
            HashSet<Vector2> coordenadas = hashComparativo[blocoElemento.texto];

            // Verifique se alguma das coordenadas do hash corresponde às coordenadas do elemento atual
            if (coordenadas.Contains(new Vector2(((elemento.position.x + blocoElemento.largura) - 1), elemento.position.y)))
            {
                listaB.Add(elemento); // Adicionar o elemento atual à listaB
                Debug.Log("Elemento TEM coordenada correspondente");
                gAudioManager.PlaySFX(gAudioManager.conectadoSound);
                return;
            }

            Debug.Log("Elemento NÃO tem coordenada correspondente");

            
            gVida.health--;
            if (gVida.health <= 0)
            {
                gGameController.ShowPanel(gGameController.gameOverPanel);
                gAudioManager.PlaySFX(gAudioManager.derrotaSound);

            }
            

            //TODO deve deletar, mas voltar ele para a lista de blocos que serão spawnados

            //TODO SE a linha estiver cheia, mas for o bloco errado não deve limpar!!!!
            // Apagar as linhas abaixo da linha atual
            gAudioManager.PlaySFX(gAudioManager.erradoSound);

            //TODO criar metodo para deletar linhas logicas, definindo todas elas como não utilizado e definir o primeiro
            //elemento da linha que errou como spawnDisponivel

            //TODO quando limpa uma linha deve diminuir o indice do indexLinhaMatriz
            indexLinhaMatriz = matriz.FindIndex(linha => linha.Any(e => e.nome == elemento.name));
            int indexElemento = Array.FindIndex(matriz[indexLinhaMatriz], e => e.nome == elemento.name);

            int indexLinhaFisica = (18 - (int)elemento.position.y) / 2;
            limparLinhasLogicas(indexLinhaFisica, indexLinhaMatriz, indexElemento);

            /*for (int i = ((int)elemento.position.y) ; i > 0; i--)
            {
                gGameManagerGrade.deletaLinhasErradas(i);
            }*/
            return;
        }
        else
        {
            Debug.Log("Elemento não existe no dicionário de hashes");
        }

        // Faça o que for necessário com as informações comparativas
        // ...
    }

    //indexLinhaFisica é de onde ele errou
    public void limparLinhasLogicas(int indexLinhaFisica, int indexLinhaMatriz, int indexElemento)
    {
        matriz[indexLinhaMatriz][indexElemento].foiUtilizado = false;
        //int indexUltimoQueFoiUtilizado = Array.FindLastIndex(matriz[indexLinhaMatriz], e => e.foiUtilizado);

        if (indexLinhaFisica == indexLinhaMatriz)
        {
            for (int i = indexLinhaFisica; i < matriz.Count; i++)
            {
                Array.ForEach(matriz[i], elemento =>
                {
                    elemento.foiUtilizado = false;
                    elemento.spawnDisponivel = false;
                });
            }

            matriz[indexLinhaFisica][0].spawnDisponivel = true;
            excluirLinhas(indexLinhaFisica);
        }
        else
        {
            for (int i = indexLinhaMatriz; i < matriz.Count; i++)
            {
                for (int j = 0; j < matriz[i].Length; j++)
                {

                    matriz[i][j].foiUtilizado = false;
                    matriz[i][j].spawnDisponivel = false;

                    //Apenas para os elementos que não foram utilizados
                    /*if (!matriz[i][j].foiUtilizado)
                    {*/
                    //Se for o mesmo index da linha, marcar apartir do index do elemento
                    /*if (i == indexLinhaMatriz && j >= indexElemento)
                    {
                        matriz[i][j].foiUtilizado = false;
                        matriz[i][j].spawnDisponivel = false;
                    }
                    else if (i > indexLinhaMatriz)
                    {
                        matriz[i][j].foiUtilizado = false;
                        matriz[i][j].spawnDisponivel = false;
                    }*/
                    //}
                }
            }

            matriz[indexLinhaMatriz][indexElemento].spawnDisponivel = true;

            //matriz[indexLinhaMatriz][indexElemento].spawnDisponivel = true;
            if (matriz[indexLinhaMatriz][indexElemento - 1] != null)
            {
                //Não é o primeiro da linha e define o elemento anterior como elementoEscolhido, para definir o spawnDisponivel dos proximos
                AtualizarMatriz(matriz, matriz[indexLinhaMatriz][indexElemento - 1], matriz[indexLinhaMatriz][indexElemento - 1].indexLinha);
            }
            else
            {
                // É o primeiro elemento da linha
                AtualizarMatriz(matriz, matriz[indexLinhaMatriz][0], matriz[indexLinhaMatriz][0].indexLinha);
            }
            int index = (18 - (indexLinhaMatriz * 2));
            excluirLinhas(index);
        }


        /*int altura = (18 - (indexLinhaMatriz * 2));

        for (int y = altura; y > 0; y--)
        {
            for (int x = 0; x < gameManagerGrade.largura; x++)
            {
                *//* string nome = gameManagerGrade.grade[x, y].transform.parent.gameObject.name;
                 int indexConvertidoLinha = matriz.FindIndex(linha => linha.Any(e => e.nome == nome));
                 int indexConvertidoElemento = Array.FindIndex(matriz[indexConvertidoLinha], e => e.nome == nome);*//*

                int IndexPodeExcluir = gGameManagerGrade.CountUniqueParentObjectsInLine(y, indexUltimoQueFoiUtilizado);
                if (y == altura && x >= IndexPodeExcluir) // && x >= indexElemento
                {
                    if (gameManagerGrade.grade[x, y] != null)
                    {
                        Destroy(gameManagerGrade.grade[x, y].transform.parent.gameObject);
                        gameManagerGrade.grade[x, y] = null;
                    }
                    if (gameManagerGrade.grade[x, (y + 1)] != null)
                    {
                        Destroy(gameManagerGrade.grade[x, (y + 1)].transform.parent.gameObject);
                        gameManagerGrade.grade[x, (y + 1)] = null;
                    }
                }
                else if (y > altura)
                {
                    if (gameManagerGrade.grade[x, y] != null)
                    {
                        Destroy(gameManagerGrade.grade[x, y].transform.parent.gameObject);
                        gameManagerGrade.grade[x, y] = null;
                    }
                    if (gameManagerGrade.grade[x, (y + 1)] != null)
                    {
                        Destroy(gameManagerGrade.grade[x, (y + 1)].transform.parent.gameObject);
                        gameManagerGrade.grade[x, (y + 1)] = null;
                    }
                }
                
            }
        }*/
    }

    public void excluirLinhas(int indexLinha)
    {
        for (int i = indexLinha; i > 0; i--)
        {
            gGameManagerGrade.deletaLinhasErradas(i);
        }
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
            Console.WriteLine("A largura da linha atual é maior do que a largura da linha anterior.");
            return false;
        }
        else
        {
            Console.WriteLine("A largura da linha atual é menor do que a largura da linha anterior.");
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
