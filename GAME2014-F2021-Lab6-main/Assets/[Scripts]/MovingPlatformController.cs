using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    [Header("Movement")]
    public MovingPlatformDirection direction;
    [Range(0.1f, 10.0f)] public float speed;
    [Range(0.1f, 10.0f)] public float distance;

    public bool isLooping;
    private Vector2 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        Vector3 displacement = transform.position - new Vector3(startPosition.x, startPosition.y, 0);
        float dispMag = displacement.magnitude;
        if (Mathf.Abs(dispMag) >= distance)
        {
            isLooping = false;
        }

        if (isLooping)
        {
            switch (direction)
            {
                case MovingPlatformDirection.HORIZONTAL:
                {
                    transform.position = new Vector2(startPosition.x + Mathf.PingPong(Time.time * speed, distance), transform.position.y);
                    break;
                } case MovingPlatformDirection.VERTICAL:
                {
                    transform.position = new Vector2(transform.position.x, startPosition.y + Mathf.PingPong(Time.time * speed, distance));
                    break;
                } case MovingPlatformDirection.DIAGONAL_UP:
                {
                    transform.position = new Vector2(startPosition.x + Mathf.PingPong(Time.time * speed, distance), startPosition.y + Mathf.PingPong(Time.time * speed, distance));
                    break;
                    }
                case MovingPlatformDirection.DIAGONAL_DOWN:
                {
                    transform.position = new Vector2(startPosition.x + Mathf.PingPong(Time.time * speed, distance), startPosition.y - Mathf.PingPong(Time.time * speed, distance));
                    break;
                }
            }
        } 
    }
}
