using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkSpawner : MonoBehaviour
{
	public enum SpawnDirection
	{
		Right,
		Left,

		Length
	}

	bool myShouldSpawn;
	public Shark myShark;
	public Shark myBackgroundShark;

	Coroutine mySpawningRoutine = null;
	private void OnEnable()
	{
		myShouldSpawn = true;
		mySpawningRoutine = StartCoroutine(Spawning());

		EventManager.OnGameEvent += OnGameEvent;
	}

	private void OnDisable()
	{
		EventManager.OnGameEvent -= OnGameEvent;
	}

	private void OnGameEvent(EventManager.GameEvent obj)
	{
		if(obj.myType == EventManager.GameEvent.EventType.Win || obj.myType == EventManager.GameEvent.EventType.Lose)
		{
			myShouldSpawn = false;
			StopCoroutine(mySpawningRoutine);
		}
		else if(obj.myType == EventManager.GameEvent.EventType.PlayAgain)
		{
			myShouldSpawn = true;
			mySpawningRoutine = StartCoroutine(Spawning());
		}
	}

	IEnumerator Spawning()
	{
		while (myShouldSpawn == true)
		{
			float timeToWait = UnityEngine.Random.Range(4f, 5f);
			yield return new WaitForSeconds(timeToWait);

			SpawnDirection direction = (SpawnDirection)UnityEngine.Random.Range(0, (int)SpawnDirection.Length);
			Vector3 position = new Vector3
			{
				y = UnityEngine.Random.Range(-1300, 500)
			};
			SpawnDirection sharkDiection = SpawnDirection.Length;
			switch (direction)
			{
				case SpawnDirection.Right:
					sharkDiection = SpawnDirection.Left;
					position.x = 2000;
					myBackgroundShark.transform.localPosition = position;
					position.x = -3500;
					myShark.transform.localPosition = position;
					myBackgroundShark.transform.localScale = new Vector3(1, 1, 1);
					myShark.transform.localScale = new Vector3(-1, 1, 1);

					break;
				case SpawnDirection.Left:
					sharkDiection = SpawnDirection.Right;
					position.x = -2000;
					myBackgroundShark.transform.localPosition = position;
					position.x = 3500;
					myShark.transform.localPosition = position;
					myBackgroundShark.transform.localScale = new Vector3(-1, 1, 1);
					myShark.transform.localScale = new Vector3(1, 1, 1);
					break;
			}

			myBackgroundShark.Init(direction);

			yield return new WaitUntil(() => { return myBackgroundShark.myHasMovedAcross == true; });
			yield return new WaitForSeconds(UnityEngine.Random.Range(4, 6));
			myShark.Init(sharkDiection);
			yield return new WaitUntil( () => {return myShark.myHasMovedAcross == true; });

		}
	}

}
