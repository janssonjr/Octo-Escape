using UnityEngine;

public class WinPanel : Panel {

	public void NextLevel()
	{ 
		EventManager.GoToLevelSelect();
	}
}
