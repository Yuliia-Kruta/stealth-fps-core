using System.Collections;
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
    public Transform weaponHoldPoint; 

    // Mapping weapon types to their slots
    public Dictionary<WeaponType, WeaponSlot> weaponInventory = new Dictionary<WeaponType, WeaponSlot>();

    public Weapon currentWeapon;

    // Reference for visually displaying weapon count and selection  
    private UIController UIController;

    // Reference for getting bool isFlying 
    private Weapon weaponScript;

    void Start()
    {
        weaponInventory[WeaponType.stone] = new WeaponSlot { count = 0 };
        weaponInventory[WeaponType.stick] = new WeaponSlot { count = 0 };
        weaponInventory[WeaponType.grenade] = new WeaponSlot { count = 0 };

        UIController = GameObject.FindObjectsOfType<UIController>()[0];
        weaponScript = GameObject.FindObjectsOfType<Weapon>()[0];
        //weaponScript = GetComponent<Weapon>();
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
        TrackWeaponCount(); // <-------------------------

        if (currentWeapon == null || !currentWeapon.gameObject.activeInHierarchy)
        {
            // Only equip if current is null or hidden
            EquipWeapon(type); 
        }

        //Destroy(weapon.gameObject);

        // Hide the picked weapon object
        weapon.gameObject.SetActive(false);
        Debug.Log("Inventory after pickup: ");
        LogWeaponInventory();
    }

    public void EquipWeapon(WeaponType type)
    {
        // Swap selected weapon in the UI
        UIController.UpdateWeaponSelection(type);

        if (!weaponInventory.ContainsKey(type) || weaponInventory[type].count <= 0)
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
        /*
        //Clear currentWeapon if it's inactive
        if (currentWeapon != null && !currentWeapon.gameObject.activeInHierarchy)
        {
            currentWeapon = null;
        }

        //Disable the currently held (visible) weapon
        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false);
            currentWeapon = null;
        }*/

        // Instantiate a new weapon object
        if (weaponInventory[type].weaponPrefab != null)
        {
            Weapon newWeapon = Instantiate(weaponInventory[type].weaponPrefab);
            newWeapon.gameObject.SetActive(true);
            newWeapon.transform.SetParent(weaponHoldPoint);
            newWeapon.transform.localPosition = Vector3.zero;
            newWeapon.transform.localRotation = Quaternion.identity;

            // 🧷 Disable physics while holding
            newWeapon.GetComponent<Collider>().enabled = false;
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
        WeaponType currentType = currentWeapon.WeaponType;

        if (!weaponInventory.ContainsKey(currentType) || weaponInventory[currentType].count <= 0)
        {
            Debug.LogWarning($"Trying to throw a {currentType} but no weapons left!");
            return;
        }

        if (currentWeapon == null || !currentWeapon.gameObject.activeInHierarchy)
        {
            Debug.LogWarning("No weapon equipped to throw!");
            return;
        }

        // Decrease count
        weaponInventory[currentType].count--;
        TrackWeaponCount(); // <-------------------------

        // Detach and throw
        currentWeapon.transform.SetParent(null);
        currentWeapon.GetComponent<Collider>().enabled = true;

        // Weapon is longer initally grounded
        if (!weaponScript.isGrounded) // <-------------------------
        {
            Debug.Log("Weapon is now falling");
            weaponScript.isGrounded = true;
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
  

    // Seperate from LogWeaponInventory so it can be called at different times
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