using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class WeaponSlot
{
    public Weapon weaponPrefab;
    public int count;
}

public class PlayerInventory : MonoBehaviour
{
    // Where the held weapon is attached
    [SerializeField] private Transform weaponHoldPoint;

    //Reference for visually displaying weapon count and selection  
    [SerializeField] private UIController UIController;

    // Current weapon equipped
    public Weapon currentWeapon;

    // Mapping weapon types to their slots
    private Dictionary<WeaponType, WeaponSlot> weaponInventory = new();

    private void Awake()
    {
        // Prepopulate the weapon inventory dictionary with all WeaponType values
        foreach (WeaponType type in System.Enum.GetValues(typeof(WeaponType)))
        {
            weaponInventory[type] = new WeaponSlot { count = 0 };
        }
    }

    void Start()
    {
        // Get components 
        UIController = FindObjectOfType<UIController>();
        if (UIController == null)
        {
            Debug.LogError("UIController not found in scene!");
        }
    }

    public void AddWeapon(WeaponType type, Weapon weapon)
    {
        if (!weaponInventory.ContainsKey(type))
        {
            weaponInventory[type] = new WeaponSlot();
        }

        // Only store the prefab reference once
        if (weaponInventory[type].weaponPrefab == null)
        {
            weaponInventory[type].weaponPrefab = weapon;
        }

        weaponInventory[type].count++;
        TrackWeaponCount();

        if (currentWeapon == null || !currentWeapon.gameObject.activeInHierarchy)
        {
            // Only equip if current is null or hidden
            EquipWeapon(type);
        }

        // Hide the picked weapon object
        weapon.gameObject.SetActive(false);
        Debug.Log("Inventory after pickup: ");
        LogWeaponInventory();
    }

    public void EquipWeapon(WeaponType type)
    {
        if (UIController != null)
        {
            // Swap selected weapon in the UI
            UIController.UpdateWeaponSelection(type);
        }
        
        if (!weaponInventory.TryGetValue(type, out WeaponSlot slot) || slot.count <= 0)
        {
            Debug.Log($"You do not have any {type}s left!");
            return;
        }

        // Remove old weapon
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
            currentWeapon = null;
        }

        // Instantiate a new weapon object
        if (weaponInventory[type].weaponPrefab != null)
        {
            Weapon newWeapon = Instantiate(weaponInventory[type].weaponPrefab);
            newWeapon.gameObject.SetActive(true);
            newWeapon.transform.SetParent(weaponHoldPoint);
            newWeapon.transform.localPosition = Vector3.zero;
            newWeapon.transform.localRotation = Quaternion.identity;

            // 🧷 Disable physics while holding
            Collider col = newWeapon.GetComponent<Collider>();
            if (col != null) col.enabled = false;
            
            Rigidbody rb = newWeapon.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            currentWeapon = newWeapon;
        }
        else
        {
            Debug.LogWarning($"Weapon prefab for {type} is null!");
        }
    }

    public void ThrowCurrentWeapon(Vector3 direction, float force)
    {
        
        if (currentWeapon == null)
        {
            Debug.LogWarning("No weapon equipped to throw!");
            return;
        }
        
        WeaponType currentType = currentWeapon.WeaponType;
        
        if (!weaponInventory.TryGetValue(currentType, out WeaponSlot slot) || slot.count <= 0)
        {
            Debug.LogWarning($"Trying to throw a {currentType} but no weapons left!");
            return;
        }

        // Decrease count
        weaponInventory[currentType].count--;
        TrackWeaponCount();

        // Detach and throw
        currentWeapon.transform.SetParent(null);
        Collider col = currentWeapon.GetComponent<Collider>();
        if (col != null) col.enabled = true;

        // Check if the weapon is no longer initially grounded
        if (!currentWeapon.isGrounded)
        {
            Debug.Log("<color='yellow'>Weapon is now falling</color>");
            currentWeapon.isGrounded = true;
        }


        Rigidbody rb = currentWeapon.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            Vector3 curvedDirection = (direction + Vector3.up * 0.5f).normalized;
            rb.AddForce(curvedDirection * force, ForceMode.Impulse);
        }

        currentWeapon = null;

        // Auto re-equip if more left
        EquipWeapon(currentType);
    }


    public void LogWeaponInventory()
    {
        foreach (var entry in weaponInventory)
        {
            WeaponType type = entry.Key;
            WeaponSlot slot = entry.Value;

            string weaponName = slot.weaponPrefab != null ? slot.weaponPrefab.name : "No Weapon Prefab";
            int count = slot.count;

            Debug.Log($"WeaponType: {type}, WeaponPrefab: {weaponName}, Count: {count}");
        }
    }


    // Separate from LogWeaponInventory so it can be called at different times
    public void TrackWeaponCount()
    {
        foreach (var entry in weaponInventory)
        {
            WeaponType type = entry.Key;
            WeaponSlot slot = entry.Value;

            int count = slot.count;

            // Update weapon count visually
            if (type == WeaponType.stick)
            {
                UIController.stickCount.text = count.ToString();
            }
            else if (type == WeaponType.stone)
            {
                UIController.stoneCount.text = count.ToString();
            }
            else if (type == WeaponType.grenade)
            {
                UIController.grenadeCount.text = count.ToString();
            }
        }
    }
}