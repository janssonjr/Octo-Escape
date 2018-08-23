using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
	public GraphicRaycaster raycaster; 
	void Update ()
	{
		if(Input.GetMouseButtonDown(0))
		{
			UnityEngine.EventSystems.PointerEventData eventData = new UnityEngine.EventSystems.PointerEventData(null);
			eventData.position = Input.mousePosition;

			List<UnityEngine.EventSystems.RaycastResult> results = new List<UnityEngine.EventSystems.RaycastResult>();
			raycaster.Raycast(eventData, results);
			foreach(var result in results)
			{
				if (result.gameObject.layer == LayerMask.NameToLayer("Octo"))
				{
					Wander wander = result.gameObject.GetComponent<Wander>();
					if(wander != null)
					{
						wander.OnPressed(Input.mousePosition);
					}
					else
					{
						CapturedOcto captured = result.gameObject.GetComponent<CapturedOcto>();
						if(captured != null)
						{
							captured.OnPressed();
						}
					}
				}
				else if(result.gameObject.layer == LayerMask.NameToLayer("CapturedOcto"))
				{
					CapturedOcto captured = result.gameObject.GetComponent<CapturedOcto>();
					captured.OnPressed();
				}
			}

		}
	}
}
