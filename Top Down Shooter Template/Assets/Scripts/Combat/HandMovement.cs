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
		handMovementRH.LookAt (combatAimer);
		handMovementLH.LookAt (combatAimer);

				if (Input.GetMouseButton(1))//If right mouse button is pushed down, shoot 
		{
			float x = -farPointAimerReach.transform.localPosition.x;
			float y = -farPointAimerReach.transform.localPosition.z;
			float z = 0;

			x = Mathf.Clamp (x, -0.4f, 0.4f);
			y = Mathf.Clamp (y, -0.35f, 0.25f);
			z = Mathf.Clamp (z, -0.4f, 0f);

			if (x > 0.1f) 
			{
				z -= x;
			}

			 if (x < -0.1f) 
			{
				z -= -x;
			}
		
			if (y > 0.1f) 
			{
				z -= y;
				//Debug.Log ("z: " + z);
			}
/*
			if (y < -0.1f) 
			{
				z -= -y;
			}
*/

				
			handMovementRH.transform.localPosition = new Vector3 (x, y, z);
			//handMovementRH.localRotation = Quaternion.Euler (0,farPointAimerReach.localRotation.y,0);
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
