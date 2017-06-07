using UnityEngine;
using System.Collections;

public class Weapon1 : MonoBehaviour
{
	public Transform muzzle;//position of muzzle where is shot from
	public Projectile projectile;//which projectile/bullet you are shooting
	public float msBetweenShots = 100;//rate of fire (milliseconds)
	public float muzzleVelocity = 35;//speed at which the bullet will leave the gun

	float nextShotTime;

	public void ShootRH()
	{
		if (Time.time > nextShotTime) //Can only shoot if the current time is greater than next shot time
		{
			nextShotTime = Time.time + msBetweenShots / 1000; //having just shot, sets the next shot time = to the current time (Time.time) + how many ms between shots

		Projectile newProjectile = Instantiate (projectile, muzzle.position, muzzle.rotation) as Projectile;//when we shoot we want to instantiate a new projectile, which includes position and rotation of muzzle
		newProjectile.SetSpeed (muzzleVelocity);
		}
	}
}
