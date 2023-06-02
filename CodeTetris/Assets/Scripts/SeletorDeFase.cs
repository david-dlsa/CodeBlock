using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SeletorDeFase : MonoBehaviour
{
    public Button[] lvlButtons;
    public Sprite spriteAtivo;
    public Sprite spriteInativo;
    public Sprite spriteAtivoSeta;
    public Sprite spriteInativoSeta;
    public Button avancaPaginaButton;
    public Button voltaPaginaButton;

    public int currentPageIndex = 0;
    public int fasesConcluidas = 0;
    public int fasesPorPagina = 6;

    public int numPaginas = 1;

    public GameObject loadingScreen;
    public Image loadingBarFill;

    [System.Serializable]
    public struct Pagina
    {
        public string titulo;
        // Outras propriedades da página, se necessário
    }

    public Pagina[] paginas; // Array contendo os títulos e propriedades de cada página

    // Start is called before the first frame update
    void Start()
    {
        if (fasesConcluidas >= fasesPorPagina)
        {
            avancaPaginaButton.gameObject.GetComponent<Image>().sprite = spriteAtivoSeta;
        }
        else
        {
            avancaPaginaButton.gameObject.GetComponent<Image>().sprite = spriteInativoSeta;
        }

        AtualizarBotoesPagina();

        int levelAtual = PlayerPrefs.GetInt("levelAt", 2);

        for (int i = 0; i < lvlButtons.Length; i++)
        {
            if (i + 2 > levelAtual)
            {
                lvlButtons[i].interactable = false;
                TextMeshProUGUI text = lvlButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                if (text != null)
                {
                    Color textColor = text.color;
                    textColor.a = 0.5f; // Defina o valor de opacidade desejado (0 a 1)
                    text.color = textColor;
                }
            }
            else
            {
                fasesConcluidas++; // Incrementa o contador de fases concluídas
            }

            SetButtonSprite(lvlButtons[i], lvlButtons[i].interactable, spriteAtivo, spriteInativo);
        }

        // Carregar os valores salvos do PlayerPrefs
        fasesConcluidas = PlayerPrefs.GetInt("FasesConcluidas", fasesConcluidas);
        currentPageIndex = PlayerPrefs.GetInt("PageIndex", currentPageIndex);
        fasesPorPagina = PlayerPrefs.GetInt("FasesPorPagina", fasesPorPagina);

        // Atualiza o título com base no índice da página atual
        AtualizarTituloPagina();

    }

    private void SetButtonSprite(Button button, bool interactable, Sprite spriteA, Sprite spriteB)
    {
        Image image = button.GetComponent<Image>();

        if (interactable)
        {
            image.sprite = spriteA; // Sprite quando o botão for interativo (true)
        }
        else
        {
            image.sprite = spriteB; // Sprite quando o botão não for interativo (false)
        }
    }

    public void OpenScene(int index)
    {
        StartCoroutine(LoadSceneAsync(index));
    }

    private IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBarFill.fillAmount = progressValue;
            yield return null;
        }
    }

    public void AvancaPaginaMenu()
    {
        // Verifica se todas as fases do objeto atual foram concluídas
        if (fasesConcluidas >= fasesPorPagina)
        {
            // Encontra o objeto com a tag "scrollFases"
            GameObject objetoScroll = GameObject.FindWithTag("scrollFases");

            // Verifica se o objeto foi encontrado
            if (objetoScroll != null)
            {
                // Obtém o componente RectTransform do objeto
                RectTransform rectTransform = objetoScroll.GetComponent<RectTransform>();

                // Define o valor de deslocamento para avançar a página para a esquerda
                float deslocamentoX = 802.428f;

                // Subtrai o valor de deslocamento da posição X atual
                rectTransform.anchoredPosition -= new Vector2(deslocamentoX, 0f);

                // Atualiza o índice da página atual
                currentPageIndex++;

                // Atualiza o título com base no índice da página atual
                AtualizarTituloPagina();

                // Incrementa 6 em fasesPorPagina
                fasesPorPagina += 6;

                AtualizarBotoesPagina();

                avancaPaginaButton.gameObject.GetComponent<Image>().sprite = spriteAtivoSeta;
            }
        }
        else
        {
            avancaPaginaButton.gameObject.GetComponent<Image>().sprite = spriteInativoSeta;
        }
    }

    public void VoltaPaginaMenu()
    {
        // Encontra o objeto com a tag "scrollFases"
        GameObject objetoScroll = GameObject.FindWithTag("scrollFases");

        // Verifica se o objeto foi encontrado
        if (objetoScroll != null)
        {
            // Obtém o componente RectTransform do objeto
            RectTransform rectTransform = objetoScroll.GetComponent<RectTransform>();

            // Define o valor de deslocamento para avançar a página para a esquerda
            float deslocamentoX = 802.428f;

            // Subtrai o valor de deslocamento da posição X atual
            rectTransform.anchoredPosition += new Vector2(deslocamentoX, 0f);

            // Atualiza o índice da página atual
            currentPageIndex--;

            // Atualiza o título com base no índice da página atual
            AtualizarTituloPagina();

            // Decrementa 6 em fasesPorPagina
            fasesPorPagina -= 6;

            AtualizarBotoesPagina();
        }
    }

    private void AtualizarTituloPagina()
    {
        GameObject tituloObj = GameObject.Find("Frame_LineFrame_TitleLine");

        if (tituloObj != null)
        {
            TextMeshProUGUI textoFilho = tituloObj.GetComponentInChildren<TextMeshProUGUI>();

            // Verifica se o índice da página atual está dentro do intervalo válido
            if (currentPageIndex >= 0 && currentPageIndex < paginas.Length)
            {
                // Obtém o título da página atual
                string tituloPagina = paginas[currentPageIndex].titulo;
                textoFilho.text = tituloPagina;
            }
        }
    }

    private void AtualizarBotoesPagina()
    {
        // Verifica se está na última página
        if (currentPageIndex == paginas.Length - 1)
        {
            // Desativa o botão de avançar página
            avancaPaginaButton.gameObject.SetActive(false);
        }
        else
        {
            // Ativa o botão de avançar página
            avancaPaginaButton.gameObject.SetActive(true);
        }

        // Verifica se está na primeira página
        if (currentPageIndex == 0)
        {
            // Desativa o botão de voltar página
            voltaPaginaButton.gameObject.SetActive(false);
        }
        else
        {
            // Ativa o botão de voltar página
            voltaPaginaButton.gameObject.SetActive(true);
        }
    }

    // Atualiza as informações salvas no PlayerPrefs ao fechar o jogo
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("FasesConcluidas", fasesConcluidas);
        PlayerPrefs.SetInt("PageIndex", currentPageIndex);
        PlayerPrefs.SetInt("FasesPorPagina", fasesPorPagina);
        PlayerPrefs.Save();
    }
}