using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CapturedOcto : MonoBehaviour
{
	public enum WalkState
	{
		Up,
		Down,
		Stay
	}

	int myAmountOfKeysNeeded;
	public float mySpeedPercent;
	public float myDeacceleratingPerccent;

	float myDeacceleration;
	float mySpeed;

	BoxCollider2D myCollider;
	float myActuallSpeed;
	bool myIsReleased;
	//bool myIsPressed;

	WalkState myWalkState;

	private void Awake()
	{
		myCollider = GetComponent<BoxCollider2D>();
	}

	private void OnEnable()
	{
		myIsReleased = false;
		myActuallSpeed = 0f;
		//myIsPressed = false;
		mySpeed = (float)Camera.main.pixelHeight / mySpeedPercent;
		myDeacceleration = Camera.main.pixelHeight / 100f * myDeacceleratingPerccent;
		myWalkState = WalkState.Stay;
	}


	public void Init(int aAmountOfKeys)
	{
		myAmountOfKeysNeeded = aAmountOfKeys;
	}

	public void UseKey()
	{
		myAmountOfKeysNeeded--;
		if(myAmountOfKeysNeeded <= 0)
		{
			//gameObject.layer = LayerMask.NameToLayer("Octo");
			GameManager.Instance.OctoFreed(GetComponent<RectTransform>());
			myIsReleased = true;
			myCollider.isTrigger = false;
			myCollider.gameObject.layer = LayerMask.NameToLayer("Octo");
			transform.GetChild(0).gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if(myIsReleased == true && myWalkState != WalkState.Stay)
		{

			Vector3 position = transform.position;
			transform.position = position + Vector3.up * myActuallSpeed * GameManager.DeltaTime;
			myActuallSpeed -= myDeacceleration * GameManager.DeltaTime;
			if (myActuallSpeed <= -mySpeed)
				myActuallSpeed = -mySpeed;

			if (myActuallSpeed <= 0)
				myWalkState = WalkState.Down;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Goal"))
		{
			StartCoroutine(WinDelay());
		}
	}

	public void OnPressed()
	{
		if(myIsReleased == true /*&& (myWalkState == WalkState.Stay || myWalkState == WalkState.Down)*/)
		{
			myActuallSpeed = mySpeed;
			myWalkState = WalkState.Up;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Goal"))
		{
			GameManager.Instance.AddScore(1);
			myActuallSpeed *= 5;
			Destroy(gameObject, 1f);
			//StartCoroutine(WinDelay());
		}
		else if(collision.gameObject.layer == LayerMask.NameToLayer("KillZone"))
		{
			myWalkState = WalkState.Stay;
		}
	}

	IEnumerator WinDelay()
	{
		yield return new WaitForSeconds(0.5f);
		//CanvasManager.OpenPanel(PanelEnum.WinPanel);
		EventManager.Win();
		yield return new WaitForSeconds(2);
		myIsReleased = false;
	}

}
