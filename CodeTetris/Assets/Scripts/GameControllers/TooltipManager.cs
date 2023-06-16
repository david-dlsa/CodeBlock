using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager _instance;

    public TextMeshProUGUI textComponent;
    private RectTransform tooltipRectTransform;
    private Canvas canvas;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);

        tooltipRectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        // Convertendo as coordenadas do mouse para coordenadas locais do botão
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera,
            out Vector2 localPoint);

        // Atualizando a posição da tooltip com as coordenadas locais do botão
        tooltipRectTransform.localPosition = localPoint;
    }

    public void SetAndShowToolTip(string message)
    {
        gameObject.SetActive(true);
        textComponent.text = message;
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
        textComponent.text = string.Empty;
    }

    public void SetAndShowToolTip(string message, Vector2 pivot)
    {
        gameObject.SetActive(true);
        textComponent.text = message;
        tooltipRectTransform.pivot = pivot; // Define o pivot do painel com base no valor recebido
    }
}