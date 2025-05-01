using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    protected SO_AggressiveWeaponData aggressiveWeaponData;

    protected override void Awake()
    {
        base.Awake();

        //if (weaponData.GetType() == typeof(SO_AggressiveWeaponData))
        //{
        //    aggressiveWeaponData = (SO_AggressiveWeaponData)weaponData;
        //}
        //else
        //{
        //    Debug.LogError("Wrong data for the weapon");
        //}
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();
    }

 
}
