using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float cameraSensitivityX = 2000;
    public float cameraSensitivityY = 2000;

    float xRotation;
    float yRotation;

    // References to every enemy in the level
    // (This code assumes the number of enemies in a level never changes)
    public List<EnemyAI> enemiesWithLineOfSight;
    public List<EnemyAI> enemiesChasingWithLineOfSight;

    // Components
    public new Camera camera;
    private MovementScript movementScript;
    private PlayerInventory playerInventory;

    // For weapons
    public float pickupRange = 3f;
    public LayerMask weaponLayer;
    public float throwForce = 20f;
    
    // Player's health
    private float maxHealth = 100f;
    private float currentHealth;

    // References to other objects
    private UIController UIController;

    void Start()
    {
        movementScript = GetComponent<MovementScript>();
        UIController = GameObject.FindObjectsOfType<UIController>()[0];

        playerInventory = GetComponent<PlayerInventory>();

        // Lock the cursor to the center of the screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Find all enemies in the level
        //enemies = GameObject.FindObjectsOfType<EnemyAI>();
        
        currentHealth = maxHealth;
    }


    void Update()
    {
        // ===================================
        //           Camera Control
        // ===================================

        // Get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * cameraSensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * cameraSensitivityY;

        yRotation += mouseX;
        xRotation -= mouseY;
        // Prevent the player from looking too far up or down
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply rotations to player and camera
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        camera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);


        // ===================================
        //           Movement states
        // ===================================

        // If the sprint button was just pressed
        if (Input.GetButtonDown("Sprint"))
        {
            movementScript.movementState = MovementScript.MovementStates.sprinting;
        }
        // If the sprint button was just released
        // SPRINTING WITH SHIFT DOESN'T ALWAYS WORK PROPERLY IN THE EDITOR,
        // SO YOU CAN ALSO USE "F" TO SPRINT
        else if (Input.GetButtonUp("Sprint"))
        {
            // And if we were sprinting before letting go of the button
            if (movementScript.movementState == MovementScript.MovementStates.sprinting)
            {
                movementScript.movementState = MovementScript.MovementStates.walking;
            }
        }
        // If the crouch button was just pressed
        else if (Input.GetButtonDown("Crouch"))
        {
            // If we aren't currently crouching, start crouching
            if (movementScript.movementState != MovementScript.MovementStates.crouching)
            {
                movementScript.movementState = MovementScript.MovementStates.crouching;
            }
            // Otherwise, stop crouching
            else
            {
                movementScript.movementState = MovementScript.MovementStates.walking;
            }
        }


        // ===================================
        //              Movement
        // ===================================

        // Get the player's input
        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");

        if (HorizontalInput != 0.0f || VerticalInput != 0.0f)
        {
            // Check if the moving script component is applied
            if (movementScript != null)
            {
                // Turn the player input (-1.0 to 1.0) into a normalized direction
                var direction = new Vector2(HorizontalInput, VerticalInput).normalized;

                // Rotate the input direction by the camera direction
                direction = RotateVector(direction, -yRotation);

                // Then move in that direction
                movementScript.Move(direction);
            }
            else
            {
                Debug.Log("<color='red'>Error!</color> Movement script not found!");
            }
        }


        // ===================================
        //             Visibility
        // ===================================

        // Update the visibility eye UI element
        if (enemiesChasingWithLineOfSight.Count > 0)
        {
            UIController.UpdateVisibilityEye("openred");
        }
        else if (enemiesWithLineOfSight.Count > 0)
        {
            UIController.UpdateVisibilityEye("open");
        }
        else
        {
            UIController.UpdateVisibilityEye("shut");
        }


        // ===================================
        //             Weapon pickup
        // ===================================

        // Fix right keys later
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1))
        {
            Debug.Log("Trying to pickup");
            TryPickUpWeapon();
        }
        // Pick Up requires right mouse button click.
        // If (Input.GetMouseButtonDown(1))


        // ===================================
        //             Swap weapon
        // ===================================

        // Swap between weapons if 1,2,3 on the keyboard is pressed
        if (Input.GetKeyDown(KeyCode.Alpha1)) playerInventory.EquipWeapon(WeaponType.Stick);
        if (Input.GetKeyDown(KeyCode.Alpha2)) playerInventory.EquipWeapon(WeaponType.Stone); 
        if (Input.GetKeyDown(KeyCode.Alpha3)) playerInventory.EquipWeapon(WeaponType.Grenade);


        // ===================================
        //             Throw weapon
        // ===================================

        if (Input.GetKeyDown(KeyCode.Y) || Input.GetMouseButtonDown(0)) // Left-click to throw
        {
            Vector3 throwDirection = camera.transform.forward; // throw forward from camera view
            playerInventory.ThrowCurrentWeapon(throwDirection, throwForce);
        }
        
        
        // ===================================
        //             Menu
        // ===================================
        
        // Pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale > 0f)
                UIController.ShowPausePanel();
            else
                UIController.HidePausePanel();
        }
    }

    void TryPickUpWeapon()
    {
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        if (Physics.SphereCast(ray, 0.3f, out hit, pickupRange, weaponLayer))
        {
            Weapon weapon = hit.collider.GetComponent<Weapon>();
            if (weapon != null)
            {
                Debug.Log("Picked up " + weapon.WeaponType);
                playerInventory.AddWeapon(weapon.WeaponType, weapon);
            }
        }
    }

    // Rotate a Vector2 by an angle (used just for movement calculation, can move elsewhere later)
    Vector2 RotateVector(Vector2 vector, float angleDegrees)
    {
        float radians = angleDegrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        );
    }

    // Player gets to the finish point
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            UIController.ShowWinPanel();
        }
    }
    
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("<color='red'>Player took " + amount + " damage. Current health: " + currentHealth + "</color>");

        if (currentHealth <= 0f)
        {
            Die();
        }
    }
    
    public void Die()
    {
        UIController.ShowGameOverPanel();
    }
}
