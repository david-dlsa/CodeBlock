using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Gabarito : MonoBehaviour
{
    public LinhaGabarito[] linhas;

    private int elementoAtualIndex;
    public int indexLinhaAtual;
    public string tituloFase;

    private List<Transform> blocosRemovidos = new List<Transform>();

    LinhaGabarito linhaAtual;
    List<Transform> listaB;

    // Dicionário para armazenar o gabarito comparativo
    Dictionary<string, HashSet<Vector2>> hashComparativo;


    MoveParaProximaFase gMoveParaProximaFase;
    gameController gGameController;
    gameManagerGrade gGameManagerGrade;
    Vida gVida;

    List<ElementoMatriz[]> matriz; // Declaração da matriz como uma variável de membro da classe

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
        elementoAtualIndex = 0;
        matriz = MontarMatriz();
        linhaAtual = linhas[indexLinhaAtual];
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
                string nomeBloco = linha.elementos[i].gameObject.name;
                nomeBloco = linha.elementos[i].gameObject.name + "(clone)" + cloneID;
                bool spawnDisponivel = linhaIndex == 0 && i == 0; // Apenas o primeiro elemento da primeira linha é true
                bool foiUtilizado = false;
                linhaArray[i] = new ElementoMatriz(nomeBloco, i, spawnDisponivel, linhaIndex, foiUtilizado);
            }

            matriz.Add(linhaArray);
        }

        return matriz;
    }

    // Método para gerar os blocos com base na matriz
    public void gerarBlocoLinha()
    {
        // Chamar o método IterarDuasLinhasPorVez e atribuir o resultado a uma variável (elementosDisponiveis e indexLinha)
        List<ElementoMatriz> elementosDisponiveis = IterarDuasLinhasPorVez(matriz);
        for (int i = 0; i < elementosDisponiveis.Count; i++)
        {
            Debug.LogError(i + " - Elemento: "+ elementosDisponiveis[i].nome + ' ' + elementosDisponiveis[i].valor + ' ' + elementosDisponiveis[i].indexLinha);
            Debug.LogError("======================================");
        }

        // Verificar se há elementos disponíveis na lista
        if (elementosDisponiveis.Count > 0)
        {
            // Randomizar e pegar um elemento da lista elementosDisponiveis
            int randomIndex = UnityEngine.Random.Range(0, elementosDisponiveis.Count);
            ElementoMatriz elementoEscolhido = elementosDisponiveis[randomIndex];

            // Passar o elemento escolhido para o método CriarPeca
            linhas[elementoEscolhido.indexLinha].CriarPeca(elementoEscolhido.valor);

            // Chamar um novo método para atualizar a matriz, desativando o elemento escolhido
            AtualizarMatriz(matriz, elementoEscolhido, elementoEscolhido.indexLinha);
        }
        else
        {
            // Não há elementos disponíveis na lista, fazer algo de acordo com a lógica do seu programa
            Debug.LogError("Não há elementos disponíveis para criar a peça!");
            Debug.LogError("Todas as linhas da matriz já foram percorridas!");
            gGameController.ShowPanel(gGameController.winPanel);
            gAudioManager.PlaySFX(gAudioManager.vitoriaSound);
        }
    }

    // Retorna a primeira linha da matiz com ao menos um elemento disponivel
    private int getLinhaDisponivel()
    {

        return matriz.FindIndex(linha => linha.Any(e => e.spawnDisponivel));
    }

    private List<ElementoMatriz> IterarDuasLinhasPorVez(List<ElementoMatriz[]> matriz)
    {
        List<ElementoMatriz> elementosDisponiveis = new List<ElementoMatriz>();

        int linha1Index = getLinhaDisponivel();
        //Debug.LogError("Linha index" + linha1Index);
        int linha2Index = linha1Index + 1;

        while (linha1Index != - 1 && linha1Index <= matriz.Count - 1 && matriz[linha1Index].Any(e => e.spawnDisponivel))
        {
            ElementoMatriz[] linha1 = matriz[linha1Index];
            // Verificar quais elementos estão disponíveis na linha 1
            List<ElementoMatriz> elementosDisponiveisLinha1 = linha1.Where(elemento => elemento.spawnDisponivel).ToList();
            elementosDisponiveis.AddRange(elementosDisponiveisLinha1);

            if(linha2Index <= matriz.Count - 1)
            {
                ElementoMatriz[] linha2 = matriz[linha2Index];
                // Verificar quais elementos estão disponíveis na linha 2
                List<ElementoMatriz> elementosDisponiveisLinha2 = linha2.Where(elemento => elemento.spawnDisponivel).ToList();
                // Unificar elementos disponíveis das duas linhas  
                elementosDisponiveis.AddRange(elementosDisponiveisLinha2);
            }

            // Verificar se todos os elementos da linha 1 foram utilizados
            bool todosUtilizadosLinha1 = linha1.All(elemento => elemento.foiUtilizado);

            // Avançar para a próxima iteração apenas se todos os elementos da linha1 foram utilizados
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
            Debug.LogError("Todas as linhas da matriz já foram percorridas!");
            gGameController.ShowPanel(gGameController.winPanel);
            gAudioManager.PlaySFX(gAudioManager.vitoriaSound);
        }

        return elementosDisponiveis;
    }

    private void liberaProximaPeca(int indexLinha, int indexUltimoDisponivel)
    {
        // verificar se náo vai tampar a peca da linha anteior
        // verificar se náo vai extrapolar a quantidade de intens na linha
        int indexLinhaAnterior;
        if (indexLinha != 0 && !matriz[indexLinha -1].All(elemento => elemento.foiUtilizado))
        {
            indexLinhaAnterior = Array.FindLastIndex(matriz[indexLinha - 1], e => e.spawnDisponivel);
        } else
        {
            indexLinhaAnterior = indexLinha + 99;
        }
        

        int indexProximaPeca = Array.FindLastIndex(matriz[indexLinha], e => e.foiUtilizado) + 1;

        // libera a peca seguinte
        if (indexProximaPeca < matriz[indexLinha].Length)
        {
            // verifica se a peca seguinte nao cobre a anterior
            if (indexProximaPeca < indexLinhaAnterior)
            {
                // Liber a peca seguinte
                matriz[indexLinha][indexProximaPeca].spawnDisponivel = true;
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
            int primeiraLinhaDisponivel = getLinhaDisponivel();

            elementoEncontrado.spawnDisponivel = false;
            elementoEncontrado.foiUtilizado = true;
            
            // Se a primeira linha disponivel encontrada for igual ao index do elemento na linha
            if (primeiraLinhaDisponivel == indexLinha)
            {
                // libera a peca seguinte na linha de mesmo index do elemento
                if (elemento.valor + 1 <= matriz[indexLinha].Length - 1)
                {
                    matriz[indexLinha][elemento.valor + 1].spawnDisponivel = true;

                }
                bool naoBloqueiaPeca = indexLinha != 0 ? (matriz[indexLinha].Length > matriz[indexLinha - 1].Length || matriz[indexLinha].All(e => e.foiUtilizado)) : true;
                // libera a peca na mesma posicao da proxima linha se existir uma proxima linha na matriz
                if (indexLinha + 1 <= matriz.Count - 1 && naoBloqueiaPeca)
                {
                    liberaProximaPeca(indexLinha + 1, primeiraLinhaDisponivel);
                }

            } else
            {
                // verifica se há um proximo elemento an linha
                // verifica se a quantidade de elementos na linha atual é maior do que a linha anterior (Impendindo bloqueio de peças)
                if (elemento.valor + 1 <= matriz[indexLinha].Length - 1 && matriz[indexLinha].Length > matriz[indexLinha - 1].Length)
                {
                    int indexElementoLinhaAnterior = Array.FindLastIndex(matriz[primeiraLinhaDisponivel], e => e.spawnDisponivel);

                    // verifica se a peca seguinte nao cobre a anterior
                    if (elemento.valor + 1 < indexElementoLinhaAnterior)
                    {
                        // Liber a peca seguinte
                        matriz[indexLinha][elemento.valor + 1].spawnDisponivel = true;
                    }

                }
            }
        }
        // verificar a linha anterior foi colocada
        //  se foi:
        //      - libera elemento proxima linha
        //  se nao foi?:
        //      - nao faz nada
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
            for (int i = (int)elemento.position.y; i > 0; i--)
            {
                gGameManagerGrade.deletaLinhasErradas(i);
            }
            return;
        }
        else
        {
            Debug.Log("Elemento não existe no dicionário de hashes");
        }

        // Faça o que for necessário com as informações comparativas
        // ...
    }


    public void LimparGabaritoComparativo()
    {
        hashComparativo.Clear();
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
