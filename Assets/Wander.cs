using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
	public enum MoveDirection
	{
		Up,
		Down
	}

	public float WalkSpeed;
	public Vector2 myWalkDirection;
	public float myTimeToReachBottom = 1;
	public float myAccelerationPercent;
	public float myDeaccelerationPercent;

	float myAcceleration;
	float myDeacceleration;

	Rigidbody2D rigidBody;
	BoxCollider2D myCollider;
	float worldScreenHeight;
	float mySpeed;
	MoveDirection myMoveDirection;
	RectTransform myTarget;

	bool myIsPressed;

	private void OnEnable()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<BoxCollider2D>();
		//SetRandomDirection();
		worldScreenHeight = Camera.main.orthographicSize * 2;
		WalkSpeed = Camera.main.pixelHeight / myTimeToReachBottom;
		mySpeed = WalkSpeed;
		myAcceleration = (float)Camera.main.pixelHeight / 100f * myAccelerationPercent;
		myDeacceleration = (float)Camera.main.pixelHeight / 100f * myDeaccelerationPercent;
		myIsPressed = false;
	}

	public void SetTarget(RectTransform aTarget)
	{
		myTarget = aTarget;
	}

	public void Init(Spawner.SpawnDirection spawnDirection)
	{
		myMoveDirection = MoveDirection.Down;
		switch (spawnDirection)
		{
			case Spawner.SpawnDirection.TopRight:
				myWalkDirection.y = 0f;
				myWalkDirection.x = 1;
				break;
			case Spawner.SpawnDirection.BottomRight:
				myWalkDirection.y = 0f;
				myWalkDirection.x = 1;
				break;
			case Spawner.SpawnDirection.TopLeft:
				myWalkDirection.y = 0f;
				myWalkDirection.x = -1;
				break;
			case Spawner.SpawnDirection.BottomLeft:
				myWalkDirection.y = 0f;
				myWalkDirection.x = -1;
				break;
			default:
				break;
		}
		WalkSpeed = -(WalkSpeed*3);
	}

	void Update ()
	{
		Vector2 position = transform.position;

		if (myIsPressed)
		{
			transform.position = position - myWalkDirection * GameManager.DeltaTime * WalkSpeed;

			if(myMoveDirection == MoveDirection.Up)
			{
				WalkSpeed += myDeacceleration * GameManager.DeltaTime;
				if(WalkSpeed >= 0)
				{
					MoveDown();
				}

			}
			else if(myMoveDirection == MoveDirection.Down)
			{
				WalkSpeed -= myAcceleration * GameManager.DeltaTime;
				if(WalkSpeed <= -mySpeed)
				{
					WalkSpeed = -mySpeed;
					myIsPressed = false;
				}

			}
		}
		else if(myTarget != null)
		{
			var walkDir = new Vector2(myTarget.position.x - position.x, myTarget.position.y - position.y).normalized;
			transform.position = position + walkDir * GameManager.DeltaTime * mySpeed;
		}
		else
		{
			transform.position = position + myWalkDirection * GameManager.DeltaTime * WalkSpeed;
		}
		//if(myTarget != null)
		//{
		//	myWalkDirection = new Vector2(myTarget.position.x - position.x, myTarget.position.y - position.y).normalized;
		//	transform.position = position - myWalkDirection * GameManager.DeltaTime * WalkSpeed;
		//}
		//else
		//{
		//	transform.position = position + myWalkDirection * GameManager.DeltaTime * WalkSpeed;
		//	if (myCollider.isTrigger == false)
		//	{
		//		if (myMoveDirection == MoveDirection.Up)
		//		{
		//			WalkSpeed -= myDeacceleration * GameManager.DeltaTime;
		//			if (WalkSpeed < 0)
		//			{
		//				//MoveDown();
		//				myMoveDirection = MoveDirection.Down;
		//			}
		//		}
		//		else
		//		{
		//			WalkSpeed -= myAcceleration * GameManager.DeltaTime;
		//			if (WalkSpeed < -mySpeed)
		//				MoveDown();
		//		}
		//	}
		//}
	}

	internal void OnPressed(Vector2 aPressedPosition)
	{
		if(myMoveDirection == MoveDirection.Down)
		{
			myIsPressed = true;
			Vector2 position = new Vector2(transform.position.x, transform.position.y);
			myWalkDirection.x = (position - aPressedPosition).normalized.x;
			myMoveDirection = MoveDirection.Up;
		}

		//if(WalkSpeed < 0)
		//{
		//	MoveUp();
		//}
	}

	void SetRandomDirection()
	{
		float x = UnityEngine.Random.Range(-1f, 1f);
		float y = UnityEngine.Random.Range(-1f, 1f);
		myWalkDirection = new Vector2(x, y);
		//rigidBody.velocity = new Vector2(x, y);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		foreach (ContactPoint2D cp in collision.contacts)
		{
			myWalkDirection = Vector2.Reflect(myWalkDirection, cp.normal);
			return;
		}
	}

	void MoveDown()
	{
		WalkSpeed = -mySpeed;
		myMoveDirection = MoveDirection.Down;
	}

	void MoveUp()
	{
		WalkSpeed = mySpeed;
		myMoveDirection = MoveDirection.Up;
	}
}
