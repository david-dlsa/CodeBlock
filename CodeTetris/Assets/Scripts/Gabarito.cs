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

    private int elementoAtual;
    public int indexLinhaAtual;
    public string tituloFase;
    private int contadorMensagemNaoCorrespondente;

    private List<Transform> blocosRemovidos = new List<Transform>();

    LinhaGabarito linhaAtual;
    List<Transform> listaB;

    // Dicionário para armazenar o gabarito comparativo
    Dictionary<string, HashSet<Vector2>> hashComparativo;


    MoveParaProximaFase gMoveParaProximaFase;
    gameController gGameController;
    gameManagerGrade gGameManagerGrade;
    Vida gVida;

    SoundConfig gSoundConfig;
    public AudioClip vitoriaSound;

    void Start()
    {

        gMoveParaProximaFase = GameObject.FindObjectOfType<MoveParaProximaFase>();
        gGameController = GameObject.FindObjectOfType<gameController>();
        gGameManagerGrade = GameObject.FindObjectOfType<gameManagerGrade>();
        gVida = GetComponent<Vida>();
        gSoundConfig = GameObject.FindObjectOfType<SoundConfig>();

        vitoriaSound = gSoundConfig.vitoriaSound;

        AtualizarTituloPagina();

        elementoAtual = 0;
        linhaAtual = linhas[indexLinhaAtual];
        gerarBlocoLinha();

        // Inicializa o dicionário
        hashComparativo = new Dictionary<string, HashSet<Vector2>>();

        listaB = new List<Transform>(); // Criar uma nova lista vazia
    }

    public void gerarBlocoLinha()
    {
            //Se a incrementaçao resultou em um index que não existe no array este
            //array já foi e quero avançar para o próximo incrementando o index da linha

            if(elementoAtual <= (linhaAtual.elementos.Length - 1)){

                // Adiciona os blocos removidos de volta à lista de elementos
                foreach (Transform blocoRemovido in blocosRemovidos)
                {
                    linhaAtual.elementos[elementoAtual] = blocoRemovido;
                    elementoAtual++;
                }

                linhaAtual.CriarPeca(elementoAtual); // se isso me retornar algo eu criei um bloco e quero incrementar pro proximo
                elementoAtual++;
                //Debug.Log(elementoAtual);
            }
            else
            {
                elementoAtual = 0;
                indexLinhaAtual++;
                if (indexLinhaAtual > (linhas.Length - 1))
                {
                    Debug.LogError("Todas as linhas do gabarito já foram esgotadas!");

                    // Limpar o gabarito comparativo antes de iniciar a nova cena
                    LimparGabaritoComparativo();

                    //Habilitar tela de vitoria
                    gGameController.ShowPanel(gGameController.winPanel);
                    PlaySFX();
                    //gMoveParaProximaFase.proximaCena();
                    return;
                }
                linhaAtual = linhas[indexLinhaAtual];
                Debug.LogWarning("Lista de elementos da linha atual está vazia! Avançando para a próxima linha...");
                gerarBlocoLinha();
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
                return;
            }

            Debug.Log("Elemento NÃO tem coordenada correspondente");


            gVida.health--;
            if (gVida.health <= 0)
            {
                gGameController.ShowPanel(gGameController.gameOverPanel);
            }

            //TODO deve deletar, mas voltar ele para a lista de blocos que serão spawnados

            //TODO SE a linha estiver cheia, mas for o bloco errado não deve limpar!!!!
            // Apagar as linhas abaixo da linha atual
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


    public void PlaySFX()
    {
        vitoriaSound = gSoundConfig.vitoriaSound;
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.7f; // Definir a altura do som
        audioSource.priority = 255; // Definir uma prioridade alta
        audioSource.PlayOneShot(vitoriaSound);
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
