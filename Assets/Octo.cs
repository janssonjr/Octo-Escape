using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Octo : MonoBehaviour
{
	public enum OctoState
	{
		Up,
		Down
	}
	public Sprite myUpSprite;
	public Sprite myDownSprite;

	public GameObject myParticles;

	OctoState myState;

	Wander myWander;
	Image myImage;
	BoxCollider2D myCollider;

	bool myHasCollidedWithCapturedOcto;
	bool myIsCaptruedOctoFreed;

	private void OnEnable()
	{
		myWander = GetComponent<Wander>();
		myImage = GetComponent<Image>();
		myCollider = GetComponent<BoxCollider2D>();
		myState = OctoState.Down;
		UpdateImage();
		EventManager.OnGameEvent += OnGameEvent;
		myHasCollidedWithCapturedOcto = false;
		myIsCaptruedOctoFreed = false;
	}

	private void OnDisable()
	{
		EventManager.OnGameEvent -= OnGameEvent;
	}

	private void OnGameEvent(EventManager.GameEvent obj)
	{
		if(obj.myType == EventManager.GameEvent.EventType.AllOctosFreed)
		{
			myWander.SetTarget(GameManager.Instance.TopGoal);
			myIsCaptruedOctoFreed = true;
		}
	}

	private void Update()
	{
		if(myWander.WalkSpeed < 0 && myState == OctoState.Up)
		{
			myState = OctoState.Down;
			UpdateImage();
		}
		else if(myWander.WalkSpeed > 0 && myState == OctoState.Down)
		{
			myState = OctoState.Up;
			UpdateImage();
		}
	}

	void UpdateImage()
	{
		switch(myState)
		{
			case OctoState.Down:
				myImage.sprite = myDownSprite;
				break;
			case OctoState.Up:
				myImage.sprite = myUpSprite;
				break;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		
		bool isCollidingWithGoal = collision.gameObject.layer == LayerMask.NameToLayer("Goal");
		if(isCollidingWithGoal == true && myCollider.isTrigger == false)
		{
			if (myHasCollidedWithCapturedOcto == true || myIsCaptruedOctoFreed == true)
			{
				myWander.WalkSpeed *= 3f;//Camera.main.pixelHeight / 4;
										 //GameManager.Instance.AddScore(1);
				myParticles.SetActive(true);
				Destroy(gameObject, 1f);
			}
			//else
			//{

			//	var pos = collision.bounds.ClosestPoint(transform.position);
			//	Debug.Log(pos);

			//	//collision.GetContacts(new Collider2D[] { myCollider });
			//	//foreach (ContactPoint2D cp in collision.)
			//	//{
			//	//	myWalkDirection = Vector2.Reflect(myWalkDirection, cp.normal);
			//	//	return;
			//	//}
			//}
		}
		else if(collision.gameObject.layer == LayerMask.NameToLayer("KillZone"))
		{
			//GameManager.Instance.AddSharkScore(1);
			Destroy(gameObject, 1f);
		}
		else if(collision.gameObject.layer == LayerMask.NameToLayer("CapturedOcto"))
		{
			CapturedOcto captured = collision.GetComponent<CapturedOcto>();
			captured.UseKey();
			myWander.SetTarget(GameManager.Instance.TopGoal);
			myHasCollidedWithCapturedOcto = true;
			//myWander.SetTarget();
		}
		
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") && myCollider.isTrigger == true)
		{
			myCollider.isTrigger = false;
			myWander.myWalkDirection.y = 1f;
			myWander.WalkSpeed /= 3;
			//if (myWander.myWalkDirection.x > 0)
			//	myWander.myWalkDirection.x = 0.7f;
			//else
			//	myWander.myWalkDirection.x = -0.7f;
			myWander.SetTarget(GameManager.Instance.GetRandomCapturedOcto());
			//myWander.WalkSpeed *= -1;
		}
	}
}
