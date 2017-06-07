using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMovement : MonoBehaviour 
{

	public Transform farPointAimerReach, combatAimer;
	public Transform handMovementRH, handMovementLH;


	
	// Update is called once per frame
	void Update () 
	{
		handMovementRH.LookAt (farPointAimerReach);
		handMovementLH.LookAt (farPointAimerReach);

				if (Input.GetMouseButton(1))//If right mouse button is pushed down, shoot 
		{
			handMovementRH.transform.localPosition = new Vector3 (farPointAimerReach.transform.localPosition.x, farPointAimerReach.transform.localPosition.y, farPointAimerReach.transform.localPosition.z);
		}
				if (Input.GetMouseButton(0))//If left mouse button is pushed down, shoot 
		{
			handMovementLH.transform.localPosition = new Vector3 (farPointAimerReach.transform.localPosition.x, farPointAimerReach.transform.localPosition.y, farPointAimerReach.transform.localPosition.z);
		}

		if (Input.GetMouseButtonUp(1))//If right mouse button is unclicked (For Hand Position to go back to default)
		{
			handMovementRH.transform.localPosition = transform.TransformPoint (Vector3.zero);//Snaps Origin point back to Default position
		}

		if (Input.GetMouseButtonUp(0))//If right mouse button is unclicked (For Hand Position to go back to default)
		{
			handMovementLH.transform.localPosition = transform.TransformPoint (Vector3.zero);//Snaps Origin point back to Default position
		}
	}
}
