using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PanelEnum
{
	GamePanel,
	WinPanel,
	LosePanel,
	LevelSelect
}

public class PanelData{}

public class Panel : MonoBehaviour
{
    public PanelEnum panelType;

    public virtual void SetPanelData(PanelData aPanelData)
    {

    }



}
