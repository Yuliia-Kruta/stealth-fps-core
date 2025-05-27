using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    // Default weapon properties
    private float damage = 0;
    private float stunDuration = 0;
    private float explosionSize = 0;

    // Check whether the object is in flight
    private bool isGrounded = false;

    // 
    public WeaponType weaponType;

    // Start is called before the first frame update
    void Start()
    {

        // Change the weapon to the right type 
        WeaponSetup(damage, stunDuration, explosionSize);

    }

    void WeaponSetup(float damage, float stunDuration, float explosionSizefloat)
    {
        // Should WeaponSetup be public so the NoiseScript can read it?
        switch (weaponType)
        {
            // Set properties for the stone weapon
            case WeaponType.stone:
              
                break;

            // Set properties for the stick weapon
            case WeaponType.stick:
             
                break;

            // Set properties for the grenade weapon
            case WeaponType.grenade:
          
                break;
        }
        
    }

    void OnCollisionEnter(Collision collision)
    {
        // Blank for now
    }

    void HitEnemy()
    {
        // Blank for now
    }
}
public enum WeaponType
{
    stone = 0,
    stick = 1,
    grenade = 2
}