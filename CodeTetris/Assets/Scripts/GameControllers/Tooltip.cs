using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public string message;
    private Button button;
    public Vector2 tooltipPivot;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    private void OnMouseEnter()
    {
        if (button != null && button.interactable)
        {
            TooltipManager._instance.SetAndShowToolTip(message, tooltipPivot);
        }
    }

    private void OnMouseExit()
    {
        TooltipManager._instance.HideToolTip();
    }

    public void SetTooltipPivot(Vector2 pivot)
    {
        tooltipPivot = pivot;
    }
}