using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YQuadMovement : MonoBehaviour
{
    public float speed = 5f; // Speed of the quad's movement
    private Vector3 startPosition = new Vector3(-6, 0, 5.0999999f);
    private Vector3 endPosition = new Vector3(6, 0, 5.0999999f);

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