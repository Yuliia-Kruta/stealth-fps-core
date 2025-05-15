using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private MovementScript movementScript;
    

    void Start()
    {
        movementScript = GetComponent<MovementScript>();
    }

    
    void Update()
    {
        // ===================================
        //           Movement states
        // ===================================

        // If the sprint button was just pressed
        if (Input.GetButtonDown("Sprint"))
        {
            movementScript.movementState = MovementScript.MovementStates.sprinting;
        }
        // If the sprint button was just released
        // SPRINTING WITH SHIFT DOESN'T ALWAYS WORK PROPERLY IN THE EDITOR, SO YOU CAN ALSO USE "F" TO SPRINT
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
                // Then move in that direction
                movementScript.Move(direction);
            }
            else
            {
                Debug.Log("Movement script not found!");
            }
        }
    }
}
