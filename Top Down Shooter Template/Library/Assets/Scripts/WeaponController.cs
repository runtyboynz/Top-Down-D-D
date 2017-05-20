using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour 
{
	public Transform weaponHoldRH; //the position of the weapon, held by the RIGHT HAND.
	public Transform weaponHoldLH; //the position of the weapon, held by the RIGHT HAND.

	public Weapon1 WeaponRH; //Attach projectile for right hand
	public Weapon2 WeaponLH; //Attach projectile for left hand

	Weapon1 equippedWeaponRH; //stores currently equipped gun in a variable
	Weapon2 equippedWeaponLH; //stores currently equipped gun in a variable


	void Start() 
	{
		if (WeaponRH != null)  //If we have assigned the default projectile (not = none)
		{
			EquipWeaponRH (WeaponRH);		
		}
		if (WeaponLH != null)  //If we have assigned the default projectile (not = none)
		{
			EquipWeaponLH (WeaponLH);		
		}
	}


	public void EquipWeaponRH (Weapon1 weaponToEquip) //FOR RIGHT HAND
	{
		if (equippedWeaponRH != null) //If equipped projectile is not = to none, destroy the current gameobject
		{
			Destroy (equippedWeaponRH.gameObject);
		}
		equippedWeaponRH = Instantiate (weaponToEquip, weaponHoldRH.position, weaponHoldRH.rotation) as Weapon1;//Instantiates/Equips the projectile and it's position/rotation in relation to WEAPONHOLD
		equippedWeaponRH.transform.parent = weaponHoldRH; //Weapon is child of the WEAPONHOLD so it moves with the player
	}

	public void EquipWeaponLH (Weapon2 weaponToEquip) //FOR LEFT HAND
	{
		if (equippedWeaponLH != null) //If equipped projectile is not = to none, destroy the current gameobject
		{
			Destroy (equippedWeaponLH.gameObject);
		}
		equippedWeaponLH = Instantiate (weaponToEquip, weaponHoldLH.position, weaponHoldLH.rotation) as Weapon2;//Instantiates/Equips the projectile and it's position/rotation in relation to WEAPONHOLD
		equippedWeaponLH.transform.parent = weaponHoldLH; //Weapon is child of the WEAPONHOLD so it moves with the player
	}

	public void ShootRH()//For shooting Right hand
	{
		if (equippedWeaponRH != null) //If weapon is currently equipped Right Hand
		{
			equippedWeaponRH.ShootRH ();
		}
	}
	public void ShootLH()//For shooting Left hand
	{
		if (equippedWeaponLH != null) //If weapon is currently equipped Right Hand
		{
			equippedWeaponLH.ShootLH ();
		}
	}
}
