using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectPanel : Panel{


	public void GoToLevel(int aLevelIndex)
	{
		CanvasManager.OpenPanel(PanelEnum.GamePanel, panelType, new GamePanelData { CurrentLevel = GameManager.Instance.GetLevel(aLevelIndex - 1)});
	}

}
