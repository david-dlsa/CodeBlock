using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{

    public GameObject painel1;
    public GameObject painel2;
    
    public void SwitchPanels(GameObject panelA, GameObject panelB)
    {
        bool panelAIsActive = panelA.activeSelf;

        panelA.SetActive(!panelAIsActive);
        panelB.SetActive(panelAIsActive);

    }

    public void OnClickSwitchPainel()
    {
        SwitchPanels(painel1, painel2);
    }
}
