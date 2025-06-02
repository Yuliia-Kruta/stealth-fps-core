using UnityEngine;

// Enum for different weapon types
public enum WeaponType
{
    None = -1,
    Stone = 0,
    Stick = 1,
    Grenade = 2
}

public class Weapon : MonoBehaviour
{
    // Weapon properties
    private float stunDuration;
    private bool isExplosive;

    // Whether the weapon is currently on the ground
    public bool isGrounded;

    public bool wasThrown;

    private NoiseSpawner noiseSpawner;

    // Current type of the weapon
    [SerializeField] private WeaponType weaponType;

    // Public property for accessing and setting the weapon type
    public WeaponType WeaponType
    {
        get { return weaponType; }
        set
        {
            weaponType = value;
            WeaponSetup();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set weapon behavior based on its type
        WeaponSetup();

        // Getting the NoiseSpawner component
        noiseSpawner = GetComponent<NoiseSpawner>();
        if (noiseSpawner == null)
        {
            noiseSpawner = gameObject.AddComponent<NoiseSpawner>();
        }

        // Set isGrounded to False on start otherwise it triggers a 'false positive'
        //isGrounded = false;
        
        isGrounded = true;
        wasThrown = false;
    }

    void WeaponSetup()
    {
        // Reset properties to safe defaults
        stunDuration = 0f;
        isExplosive = false;
        
        // Set the weapon's properties to the right type
        switch (weaponType)
        {
            // Set properties for the stone weapon
            case WeaponType.Stone:
                stunDuration = 5f;
                break;
            // Set properties for the stick weapon
            case WeaponType.Stick:
                stunDuration = 2f;
                break;
            // Set properties for the grenade weapon
            case WeaponType.Grenade:
                stunDuration = 10f;
                isExplosive = true;
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // If we hit an enemy while in the air (not grounded)
        EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
        if (enemy != null && !isGrounded)
        {
            // Apply stun based on weapon's stun duration
            enemy.Stun(stunDuration);
            wasThrown = true;
            isGrounded = true;

            // Destroy the weapon if it's explosive
            if (isExplosive)
            {
                GenerateImpactNoise();
                Destroy(gameObject);
                Debug.Log("<color='orange'>BOOM!</color>");
            }
        }
        
        // If weapon was thrown
        if (!isGrounded && wasThrown)
        {
            GenerateImpactNoise();

            // Destroy the weapon if it's explosive
            if (isExplosive)
            {
                Destroy(gameObject);
                Debug.Log("<color='orange'>BOOM!</color>");
            }

            // Set isGrounded back to false so weapons can be reused
            isGrounded = true;
            wasThrown = false;
        }
    }

    // Spawns noise based on weapon type
    void GenerateImpactNoise()
    {
        float noiseRadius = weaponType switch
        {
            WeaponType.Stick => 5f,
            WeaponType.Stone => 10f,
            WeaponType.Grenade => 20f,
            _ => 0f
        };

        noiseSpawner.SpawnNoise(noiseRadius, 1f);
        Debug.Log("<color='yellow'>Weapon has made impact</color>");
    }
}