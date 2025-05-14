using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    // The velocity this object has when it loads into the scene
    [SerializeField]
    public Vector3 initialVelocity = Vector3.zero;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Set the initial velocity
        updateVelocity(initialVelocity);
    }

    public void updateVelocity(Vector3 velocity)
    {
        rb.velocity = velocity;
    }

    public void Move(float speed, Vector3 direction)
    {
        // Calculate the force to add
        Vector3 forceToAdd = direction * speed * Time.deltaTime;
        // Apply the force to the rigidbody
        rb.AddForce(forceToAdd);
    }
}
