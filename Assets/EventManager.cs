using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour {

    public static Action<GameEvent> OnGameEvent;

	public class GameEvent
    {
        public enum EventType
        {
           Win,
		   Lose,
		   PlayAgain,
		   AllOctosFreed,
		   GoToLevelSelect
		}
        //public LevelType myLevelType;
        public EventType myType;
        //public LevelData myLevelData;
    }

	public static void Win()
	{
		if (OnGameEvent != null)
			OnGameEvent.Invoke(new GameEvent { myType = GameEvent.EventType.Win});
	}

	public static void Lose()
	{
		if (OnGameEvent != null)
			OnGameEvent.Invoke(new GameEvent { myType = GameEvent.EventType.Lose });
	}

	public static void PlayAgain()
	{
		if (OnGameEvent != null)
			OnGameEvent.Invoke(new GameEvent { myType = GameEvent.EventType.PlayAgain });
	}

	public static void AllOctosFreed()
	{
		if (OnGameEvent != null)
			OnGameEvent.Invoke(new GameEvent { myType = GameEvent.EventType.AllOctosFreed });
	}

	public static void GoToLevelSelect()
	{
		if (OnGameEvent != null)
			OnGameEvent.Invoke(new GameEvent { myType = GameEvent.EventType.GoToLevelSelect });
	}
}