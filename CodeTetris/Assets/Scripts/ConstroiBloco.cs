using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConstroiBloco : MonoBehaviour
{
    public int largura;
    private int altura = 2;
    public GameObject quadradoPrefab;
    public Sprite sprite;
    public string texto;
    public int x, y;
    public float tamanhoMaximo = 0.8f;
    private string nomeDoObjeto = "partCode";
    private string tagDoObjeto = "partCode";

    void Start()
    {
        ConstruirBloco();
    }

    void ConstruirBloco()
    {
        for (int i = 0; i < largura; i++)
        {
            for (int j = 0; j < altura; j++)
            {
                GameObject quadrado = Instantiate(quadradoPrefab, transform);
                quadrado.transform.localPosition = new Vector3(i, j, 0);

                // Configura o sprite do quadrado
                SpriteRenderer spriteRenderer = quadrado.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = sprite;
            }
        }

        // Adiciona o texto centralizado
        GameObject textoObjeto = new GameObject(nomeDoObjeto);
        textoObjeto.tag = tagDoObjeto;
        textoObjeto.transform.parent = transform;
        textoObjeto.transform.localPosition = new Vector3((largura - 1) * 0.5f, (altura - 1) * 0.5f, 0f);

        TextMeshPro textMeshPro = textoObjeto.AddComponent<TextMeshPro>();
        textMeshPro.text = texto;
        textMeshPro.alignment = TextAlignmentOptions.Center;

        // Definir a fonte desejada
        TMP_FontAsset fontAsset = Resources.Load<TMP_FontAsset>("Fonts & Materials/Oswald Bold SDF");
        textMeshPro.font = fontAsset;

        // Ativar o contorno com espessura personalizada
        textMeshPro.enableVertexGradient = true;
        textMeshPro.enableAutoSizing = false; // Desativar o ajuste automático do tamanho do texto
        textMeshPro.outlineWidth = 0.2f; // Espessura do contorno
        textMeshPro.outlineColor = Color.black; // Cor do contorno

        // Ajusta o tamanho da label
        RectTransform rectTransform = textMeshPro.GetComponent<RectTransform>();
        float menorLado = Mathf.Min(rectTransform.rect.width, rectTransform.rect.height);
        float tamanhoDesejado = tamanhoMaximo * menorLado;
        float fontSize = tamanhoDesejado / menorLado;
        textMeshPro.fontSize = fontSize;
    }
}