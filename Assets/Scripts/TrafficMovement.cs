using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f; // Adjust this to change the speed of movement

    void Update()
    {
        Vector3 currentPosition = transform.position;

        if(currentPosition.z > 0f)
        {
            currentPosition.x -= moveSpeed * Time.deltaTime;
        }
        else 
        {
            currentPosition.x += moveSpeed * Time.deltaTime;
        }
        
        transform.position = currentPosition;
    }
}
