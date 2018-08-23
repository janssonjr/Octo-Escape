using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

	public static GameManager Instance { get; private set; }

	public int Score { get; set; }
	public int ScoreGoal;

	internal LevelData GetLevel(int aLevelIndex)
	{
		return levels.levels[aLevelIndex];
	}

	public int SharkScore { get; set; }
	public int SharkScoreGoal;

	public Image myScoreImage;
	public Image mySharkScoreImage;

	public List<RectTransform> myCapturedOctos = new List<RectTransform>();

	public RectTransform TopGoal;

	int myGoal;

	LevelContainer levels;

	public LevelContainer Levels
	{
		get { return levels; }
	}

	public static float DeltaTime
	{
		get
		{
			return Instance.myDeltaTime * Instance.myDeltaTimeMultiplier;
		}
	}

	float myDeltaTime = 0f;
	float myDeltaTimeMultiplier = 1f;
	//public Text scoreText;

	private void Awake()
	{
		Instance = this;
		ResetScores();
		//scoreText.text = "Score: " + Score.ToString();
	}

	void ResetScores()
	{	
		Score = 0;
		myScoreImage.fillAmount = (float)Score / ScoreGoal;
		SharkScore = 0;
		mySharkScoreImage.fillAmount = (float)SharkScore / SharkScoreGoal;
	}

	public void OctoFreed(RectTransform aCapturedOcto)
	{
		bool success = myCapturedOctos.Remove(aCapturedOcto);
		if (success == false)
			Debug.LogError("Could not find captured octo to remove!!");

		if (myCapturedOctos.Count <= 0)
			EventManager.AllOctosFreed();
	}

	internal void SetGoalAmount(int amountOfCapturedOctos)
	{
		myGoal = amountOfCapturedOctos;
	}

	private void OnEnable()
	{
		EventManager.OnGameEvent += OnGameEvent;
		CreateLevelData();
	}

	private void OnDisable()
	{
		EventManager.OnGameEvent -= OnGameEvent;
	}

	private void OnGameEvent(EventManager.GameEvent obj)
	{
		if(obj.myType == EventManager.GameEvent.EventType.PlayAgain)
		{
			ResetScores();
		}
	}

	public void CheckWinCondition()
	{
		if(Score >= myGoal)
		{
			StartCoroutine(WinDelay());
		}
	}

	IEnumerator WinDelay()
	{
		yield return new WaitForSeconds(0.5f);
		EventManager.Win();
	}

	public void AddScore(int aScore)
	{
		Score += aScore;
		CheckWinCondition();
		//myScoreImage.fillAmount = (float)Score / ScoreGoal;
		//if(Score >= ScoreGoal)
		//{
		//	EventManager.Win();
		//	CanvasManager.OpenPanel(PanelEnum.WinPanel);
		//	//myDeltaTimeMultiplier = 0f;
		//}
	}

	public void AddSharkScore(int aScore)
	{
		SharkScore += aScore;
		mySharkScoreImage.fillAmount = (float)SharkScore / SharkScoreGoal;
	}

	private void Update()
	{
		myDeltaTime = Time.deltaTime;
	}

	void CreateLevelData()
	{
		TextAsset levelText = Resources.Load("Levels") as TextAsset;
		levels = JsonUtility.FromJson<LevelContainer>(levelText.text);
		Debug.Log("LevelDataCreated");
	}

	public RectTransform GetRandomCapturedOcto()
	{
		if(myCapturedOctos.Count <= 0)
		{
			return null;
		}
		return myCapturedOctos[UnityEngine.Random.Range(0, myCapturedOctos.Count)];
	}

	public void AddCapturedOcto(RectTransform aCapturedOcto)
	{
		myCapturedOctos.Add(aCapturedOcto);
	}

	public void ClearCapturedOctos()
	{
		for(int i = 0; i < myCapturedOctos.Count; ++i)
		{
			Destroy(myCapturedOctos[i].gameObject);
		}
		myCapturedOctos.Clear();
	}
}
