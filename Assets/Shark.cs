using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour
{

	public float WalkSpeed;
	public bool myHasMovedAcross;
	Vector2 myWalkDirection;
	SharkSpawner.SpawnDirection mySpawnDirection;
	RectTransform myRect;
	RectTransform myParentRect;
	public float myAttackSpeed;

	Transform myTarget = null;

	EdgeCollider2D myCollider;

	EdgeCollider2D mySightCollider;

	private void OnEnable()
	{
		myParentRect = transform.parent as RectTransform;
		myRect = transform as RectTransform;
		mySightCollider = GetComponentInChildren<EdgeCollider2D>();
		myCollider = GetComponent<EdgeCollider2D>();

		EventManager.OnGameEvent += OnGameEvent;
	}

	private void OnDisable()
	{
		EventManager.OnGameEvent -= OnGameEvent;

	}

	private void OnGameEvent(EventManager.GameEvent obj)
	{
		if(obj.myType == EventManager.GameEvent.EventType.PlayAgain)
		{
			transform.localPosition = new Vector3(3800, transform.localPosition.y, 0);
			myTarget = null;
		}
	}

	internal void SetTarget(Transform aTarget)
	{
		if(myTarget == null || aTarget == null)
		{
			myTarget = aTarget;
			//myTarget.GetComponent<Wander>().WalkSpeed = 0;
		}
	}

	public void Init(SharkSpawner.SpawnDirection aSpawnDirection)
	{
		mySpawnDirection = aSpawnDirection;
		myHasMovedAcross = false;
		myWalkDirection.y = 0;
		switch(aSpawnDirection)
		{
			case SharkSpawner.SpawnDirection.Right:
				myWalkDirection.x = -1;
				break;
			case SharkSpawner.SpawnDirection.Left:
				myWalkDirection.x = 1;
				break;
		}
	}

	private void Update()
	{
		if (myHasMovedAcross == false)
		{
			if (myTarget == null)
			{


				Vector2 position = transform.position;
				transform.position = new Vector2(position.x + myWalkDirection.x * Time.deltaTime * WalkSpeed, position.y);


				switch (mySpawnDirection)
				{
					case SharkSpawner.SpawnDirection.Right:
						if (transform.localPosition.x + myRect.rect.width < myParentRect.rect.xMin)
						{
							myHasMovedAcross = true;
							myTarget = null;
							if(mySightCollider != null)
								mySightCollider.enabled = true;

						}
						break;
					case SharkSpawner.SpawnDirection.Left:
						if (transform.localPosition.x - myRect.rect.width > myParentRect.rect.xMax)
						{
							myHasMovedAcross = true;
							myTarget = null;
							if(mySightCollider != null)
								mySightCollider.enabled = true;
						}
						break;
				}
			}
			else
			{
				Vector3 direction = myTarget.position - transform.position;
				transform.position = new Vector3(transform.position.x + direction.normalized.x * Time.deltaTime * myAttackSpeed, transform.position.y);
				//if(myTarget != null)
				//transform.LookAt(myTarget);

				Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
				Vector2 targetPos = new Vector2(myTarget.position.x, myTarget.position.y);



				float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

				if (myWalkDirection.x < 0)
					angle += 180;

				//transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
				//var direction = targetPos - myPos;

				float dot = Vector2.Dot(myPos.normalized, targetPos);
				float rotationAmount = Mathf.Acos(Vector2.Dot(myPos.normalized, targetPos.normalized)) * Mathf.Rad2Deg;
				//transform.rotation = Quaternion.AngleAxis(rotationAmount, Vector3.forward);// = rotationAmount;
				//Debug.Log("DOt: " + rotationAmount);
				//transform.RotateAround();
			}
		}
	}

	public int GetTargetID()
	{
		if (myTarget != null)
			return myTarget.GetInstanceID();

		return -1;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.layer == LayerMask.NameToLayer("Octo"))
		{
			if(myTarget != null && collision.transform.GetInstanceID() == myTarget.GetInstanceID())
			{
				if (myCollider.IsTouching(collision))
				{
					if(myTarget.gameObject.tag == "CapturedOcto")
					{
						//Lose
						EventManager.Lose();
						CanvasManager.OpenPanel(PanelEnum.LosePanel);
					}
					Destroy(myTarget.gameObject);
					myTarget = null;
					GameManager.Instance.AddSharkScore(1);
					transform.rotation = Quaternion.identity;
					mySightCollider.enabled = true;
				}
			}
			//GameManager.Instance.AddSharkScore(1);
			//Destroy(collision.gameObject);
		}
	}
}
