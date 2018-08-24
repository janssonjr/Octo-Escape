using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanel : Panel
{
	public GameObject capturedOctoPrefab;

	LevelData currenLevelData;
	public override void SetPanelData(PanelData aPanelData)
	{
		GamePanelData pd = aPanelData as GamePanelData;
		if(pd == null)
		{
			Debug.Log("GamePanelData is null");
			return;
		}

		currenLevelData = pd.CurrentLevel;

		SetUpLevel();
	}

	void SetUpLevel()
	{
		GameManager.Instance.ClearCapturedOctos();
		int amountOfCapturedOctos = currenLevelData.CapturedOctos.Length;
		for(int i = 0; i < amountOfCapturedOctos; ++i)
		{
			GameObject go = Instantiate(capturedOctoPrefab, transform);
			CapturedOcto co = go.GetComponent<CapturedOcto>();
			co.Init(currenLevelData.CapturedOctos[i].KeysNeeded);
			RectTransform rt = go.GetComponent<RectTransform>();
			rt.anchorMin = currenLevelData.CapturedOctos[i].Anchor;
			rt.anchorMax = currenLevelData.CapturedOctos[i].Anchor;
			rt.anchoredPosition = currenLevelData.CapturedOctos[i].Position;
			GameManager.Instance.AddCapturedOcto(rt);
		}
		for(int i = 0; i < currenLevelData.keys.Length; ++i)
		{
			//GameObject go = Instantiate(keyPrefab, transform);
			//RectTransform rt = go.GetComponent<RectTransform>();
			//rt.anchorMin = currenLevelData.keys[i].Anchor;
			//rt.anchorMax = currenLevelData.keys[i].Anchor;
			//rt.anchoredPosition = currenLevelData.keys[i].Position;
		}
		GameManager.Instance.SharkScoreGoal = currenLevelData.amounToDefeat;
		GameManager.Instance.SetGoalAmount(amountOfCapturedOctos);
	}


}

public class GamePanelData : PanelData
{
	public LevelData CurrentLevel;
}
