using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum WeaponType
{
    None = -1,
    stone = 0,
    stick = 1,
    grenade = 2
}
public class Weapon : MonoBehaviour
{
    // Default weapon properties
    private float damage = 0;
    private float stunDuration = 0;
    private float explosionSize = 0;

    // Check whether the object is in flight
    private bool isGrounded = false;
    public bool IsGrounded
    {
        get { return isGrounded; }
        set { isGrounded = value; }
    }
    
    private NoiseSpawner noiseSpawner;

    // Reference to the WeaponType enum
    [SerializeField]
    private WeaponType weaponType;
    
    
    public WeaponType WeaponType
    {
        get { return weaponType; }
        set { weaponType = value; }
    }


    // Start is called before the first frame update
    void Start()
    { 
        WeaponSetup();
        noiseSpawner = GetComponent<NoiseSpawner>();
        if (noiseSpawner == null)
        {
            noiseSpawner = gameObject.AddComponent<NoiseSpawner>();
        }
    }

    void WeaponSetup()
    {
        // Set the weapon's properties to the right type
        switch (weaponType)
        {
            // Set properties for the stone weapon
            case WeaponType.stone:
                stunDuration = 5f;
                break;
            // Set for the stick weapon
            case WeaponType.stick:
                stunDuration = 2f;
                break;
            // Set for the grenade weapon
            case WeaponType.grenade:
                stunDuration = 10f;
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if we hit an enemy
        EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
        if (enemy != null && !isGrounded)
        {
            // Apply stun based on weapon's stun duration
            enemy.Stun(stunDuration);
        }

        if (!isGrounded)
        {
            // Emit noise based on weapon type
            float noiseRadius = 0f;
            float noiseDuration = 1f;

            switch (weaponType)
            {
                case WeaponType.stick:
                    noiseRadius = 5f;
                    break;
                case WeaponType.stone:
                    noiseRadius = 10f;
                    break;
                case WeaponType.grenade:
                    noiseRadius = 20f;
                    break;
            }
            
            noiseSpawner.SpawnNoise(noiseRadius, noiseDuration);

            isGrounded = true;
            // Destroy(gameObject); // e.g. for grenade
        }
    }

    void HitEnemy()
    {
        // Blank for now
    }
    
}
    