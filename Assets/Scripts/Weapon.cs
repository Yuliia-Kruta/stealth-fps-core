using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static UnityEditor.Progress;

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
    private float stunDuration = 0;
    private bool isExplosive = false;

    // Check whether the oject is grounded
    public bool isGrounded;

    /*public bool IsGrounded
    {
        get { return isTemp; }
        set { isTemp = value; }
    }*/
    
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

        isGrounded = false;
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
                isExplosive = true;
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

        Debug.Log($"isGrounded = {isGrounded}");
        if (isGrounded == true)
        {
            Debug.Log("IsGrounded has been set to true");
            // Weapon is grounded, but not from initial scene start

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

            if (isExplosive == true)
            {
                Destroy(gameObject);
                Debug.Log("BOOM!");
            }

            //isGrounded = false;
        }
    } 
}
    