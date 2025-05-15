using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float cameraSensitivityX = 2000;
    public float cameraSensitivityY = 2000;

    float xRotation;
    float yRotation;
    
    public Camera camera;
    private MovementScript movementScript;

    void Start()
    {
        movementScript = GetComponent<MovementScript>();

        // Lock the cursor to the center of the screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
                Debug.Log("Movement script not found!");
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
        
}
