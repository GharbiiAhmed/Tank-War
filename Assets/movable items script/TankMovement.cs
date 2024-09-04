using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public float speed = 5f; // Speed of the tank's movement
    public Vector3 startPosition = new Vector3(25.9699993f, 0.755748868f, -0.0177249536f);
    public Vector3 endPosition = new Vector3(-49f, 0.755748868f, -0.0199999996f);

    void Start()
    {
        // Set the initial position of the tank
        transform.position = startPosition;
    }

    void Update()
    {
        // Move the tank
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Check if the tank has moved past the end position
        if (transform.position.x <= endPosition.x)
        {
            // Reset the tank to the start position
            transform.position = startPosition;
        }
    }
}