using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkSight : MonoBehaviour {

	Shark myShark;
	EdgeCollider2D myEdgeCollider;

	private void OnEnable()
	{
		myShark = GetComponentInParent<Shark>();
		myEdgeCollider = GetComponent<EdgeCollider2D>();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.layer == LayerMask.NameToLayer("Octo"))
		{
			myShark.SetTarget(collision.transform);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Octo"))
		{
			myShark.SetTarget(collision.transform);
			//myEdgeCollider.enabled = false;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if(collision.gameObject.layer == LayerMask.NameToLayer("Octo"))
		{
			int targetID = myShark.GetTargetID();
			if(targetID != -1 && targetID == collision.transform.GetInstanceID())
			{
				myShark.SetTarget(null);
			}
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Octo"))
		{
			int targetID = myShark.GetTargetID();
			if (targetID != -1 && targetID == collision.transform.GetInstanceID())
			{
				myShark.SetTarget(null);
			}
		}
	}

}
