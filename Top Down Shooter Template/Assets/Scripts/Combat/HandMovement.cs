using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMovement : MonoBehaviour 
{

	public Transform farPointAimerReachRH, farPointAimerReachLH, combatAimer;
	public Transform handMovementRH, handMovementLH;


	
	// Update is called once per frame
	void Update () 
	{
		handMovementRH.LookAt (combatAimer);
		handMovementLH.LookAt (combatAimer);

				if (Input.GetMouseButton(1))//If right mouse button is pushed down, shoot 
		{
			float x = -farPointAimerReachRH.transform.localPosition.x / 6;//moves opposite of x
			float y = -farPointAimerReachRH.transform.localPosition.z / 6;//z axis moves on the y axis opposingly
			float z = 0;//default. The below equations make it move with the above axis.

			//Sets restrictions for hand/arm movement distance.
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
			handMovementRH.transform.localPosition = new Vector3 (x, y, z);//Sets up the new variable numbers that the hand movement will move along with mouse movement.
		}
				if (Input.GetMouseButton(0))//If left mouse button is pushed down, shoot 
		{
			float x = -farPointAimerReachLH.transform.localPosition.x / 6;//moves opposite of x
			float y = -farPointAimerReachLH.transform.localPosition.z / 6;//z axis moves on the y axis opposingly
			float z = 0;//default. The below equations make it move with the above axis.

			//Sets restrictions for hand/arm movement distance.
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
			handMovementLH.transform.localPosition = new Vector3 (x, y, z);//Sets up the new variable numbers that the hand movement will move along with mouse movement.
		}

		if (Input.GetMouseButtonUp(1))//If right mouse button is unclicked (For Hand Position to go back to default)
		{
			handMovementRH.transform.localPosition = new Vector3 (0,0,0);//Snaps Origin point back to Default position
		}

		if (Input.GetMouseButtonUp(0))//If left mouse button is unclicked (For Hand Position to go back to default)
		{
			handMovementLH.transform.localPosition = new Vector3 (0,0,0);//Snaps Origin point back to Default position
		}
	}
}
