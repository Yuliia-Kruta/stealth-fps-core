using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    // The velocity this rigidbody has when it loads into the scene
    public Vector3 initialVelocity = Vector3.zero;
    // The speeds for the different states of movement
    public float walkSpeed = 5.0f;
    public float sprintSpeed = 8.5f;
    public float crouchSpeed = 2.5f;
    // Noise levels for the different states of movement
    public float walkNoise = 3.0f;
    public float sprintNoise = 8.0f;
    public float crouchNoise = 1.0f;
    // How quickly the rigidbody accelerates
    public float accelerationSpeed = 500.0f;

    // The different states of movement
    public enum MovementStates
    {
        walking = 0,
        sprinting = 1,
        crouching = 2
    }
    public MovementStates movementState = MovementStates.walking;


    private Rigidbody rb;
    private NoiseSpawner noiseSpawner;



    void Start()
    {
        // Get components
        rb = GetComponent<Rigidbody>();

        // Set the initial velocity
        UpdateVelocity(initialVelocity);
        
        noiseSpawner = GetComponent<NoiseSpawner>();
        if (noiseSpawner == null)
        {
            noiseSpawner = gameObject.AddComponent<NoiseSpawner>();
        }
    }


    // Change the rigidbody's velocity once
    public void UpdateVelocity(Vector3 velocity)
    {
        rb.velocity = velocity;
    }


    // Increase the rigidbody's velocity by accelerationSpeed, but not exceeding maxSpeed
    // Uses the current movementState (walking, sprinting, crouching) as maxSpeed
    public void Move(Vector2 direction)
    {
        float maxSpeed = walkSpeed;
        float noiseRadius = walkNoise;

        // Decide the maxSpeed
        switch (movementState)
        {
            case MovementStates.walking:
                maxSpeed = walkSpeed;
                noiseRadius = walkNoise;
                break;
            case MovementStates.sprinting:
                maxSpeed = sprintSpeed;
                noiseRadius = sprintNoise;
                break;
            case MovementStates.crouching:
                maxSpeed = crouchSpeed;
                noiseRadius = crouchNoise;
                break;
        }


        // Get the current horizontal velocity
        Vector2 currentVelocity = new Vector2(rb.velocity.x, rb.velocity.z);
        
        // Calculate the target velocity based on direction
        Vector2 targetVelocity = direction * maxSpeed;
        
        // Calculate the force needed to reach the target velocity
        Vector2 velocityChange = targetVelocity - currentVelocity;

        // Adjust that velocityChange based on the accelerationSpeed
        Vector2 forceToAdd = velocityChange * accelerationSpeed * Time.deltaTime;
        
        // Apply the force
        rb.AddForce(forceToAdd.x, 0f, forceToAdd.y);
        
        //Debug.Log($"Current Speed: {currentVelocity.magnitude}, Target Speed: {targetVelocity.magnitude}, Movement State: {movementState}");
        
        if (direction.sqrMagnitude > 0.01f)
        {
            noiseSpawner.SpawnNoise(noiseRadius, 0.5f);
        }
        
    }


    // Add to the rigidbody's velocity once
    public void Accelerate(float speed, Vector3 direction)
    {
        // Calculate the force to add
        Vector3 forceToAdd = direction * speed * Time.deltaTime;
        // Apply the force to the rigidbody
        rb.AddForce(forceToAdd);
    }
}
