using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backgrounds : MonoBehaviour {

	RectTransform myRect;

	private void OnEnable()
	{
		myRect = GetComponent<RectTransform>();
		EventManager.OnGameEvent += OnGameEvent;
	}

	private void OnDisable()
	{
		EventManager.OnGameEvent -= OnGameEvent;
	}

	private void OnGameEvent(EventManager.GameEvent obj)
	{
		if(obj.myType == EventManager.GameEvent.EventType.Win)
		{
			MoveToWin();
		}
		else if(obj.myType == EventManager.GameEvent.EventType.GoToLevelSelect)
		{
			MoveToLevelSelect();
		}
	}

	private void MoveToLevelSelect()
	{
		CanvasManager.ClosePanel(PanelEnum.GamePanel);
		CanvasManager.OpenPanel(PanelEnum.LevelSelect);
		Hashtable hash = new Hashtable();
		hash.Add("from", myRect.anchoredPosition.y);
		hash.Add("to", 0);
		hash.Add("time", 1f);
		hash.Add("easetype", iTween.EaseType.linear);
		hash.Add("delay", 0.5f);
		hash.Add("onupdate", "OnUpdate");
		hash.Add("oncomplete", "OnLevelSelectComplete");
		iTween.ValueTo(gameObject, hash);
	}

	void MoveToWin()
	{
		CanvasManager.OpenPanel(PanelEnum.WinPanel);
		Hashtable hash = new Hashtable();
		hash.Add("from", 0);
		hash.Add("to", -4048);
		hash.Add("time", 1f);
		hash.Add("easetype", iTween.EaseType.linear);
		hash.Add("delay", 0.5f);
		hash.Add("onupdate", "OnUpdate");
		hash.Add("oncomplete", "OnWinComplete");
		iTween.ValueTo(gameObject, hash);
	}

	void OnUpdate(float aValue)
	{
		myRect.anchoredPosition = new Vector2(myRect.anchoredPosition.x, aValue);
	}

	void OnWinComplete()
	{
		CanvasManager.OpenPanel(PanelEnum.WinPanel);
	}

	void OnLevelSelectComplete()
	{
		CanvasManager.ClosePanel(PanelEnum.WinPanel);
	}

}
