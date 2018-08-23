using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public enum SpawnDirection
	{
		TopRight,
		BottomRight,
		TopLeft,
		BottomLeft
	}

	public static Spawner Instance;
	bool isRunning;

	public GameObject octo;
	public List<RectTransform> spawnPoints = new List<RectTransform>();

	public float mySpawnTime;
	public float myAmountToReduceSpawnTime;
	public float myTimeWhenShouldReduce;
	public float myMinSpawnTime;
	float myTimerToReduceSpawnTime = 0f;

	Coroutine mySpawnRoutine = null;

	private void Awake()
	{
		Instance = this;
	}

	private void OnEnable()
	{
		isRunning = true;
		mySpawnRoutine = StartCoroutine(Spawning());
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
			StopCoroutine(mySpawnRoutine);
		}
		else if(obj.myType == EventManager.GameEvent.EventType.PlayAgain)
		{
			DestroyAllOctos();
		}
		else if(obj.myType == EventManager.GameEvent.EventType.AllOctosFreed)
		{
			StopCoroutine(mySpawnRoutine);
		}
	}

	void DestroyAllOctos()
	{
		for(int i = 0; i < spawnPoints.Count; ++i)
		{
			for(int j = 0; j < spawnPoints[i].childCount; ++j)
			{
				Destroy(spawnPoints[i].GetChild(j).gameObject);
			}
		}
	}

	private void Update()
	{
		myTimerToReduceSpawnTime += GameManager.DeltaTime;
	}

	IEnumerator Spawning()
	{
		while(isRunning)
		{
			yield return new WaitForSeconds(mySpawnTime);
			SpawnDirection direction = (SpawnDirection)UnityEngine.Random.Range(0, (spawnPoints.Count));
			GameObject obj = Instantiate(octo, spawnPoints[(int)direction]);
			obj.GetComponent<Wander>().Init(direction);
			obj.transform.localPosition = Vector3.zero;
			if (myTimerToReduceSpawnTime >= myTimeWhenShouldReduce)
			{
				myTimerToReduceSpawnTime = 0f;
				mySpawnTime -= myAmountToReduceSpawnTime;
				if (mySpawnTime <= myMinSpawnTime)
					mySpawnTime = myMinSpawnTime;
			}
			//obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(1f, 1f));
		}
	}

}
