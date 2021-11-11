using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement")]
    public float runForce;
    public float direction;

    // check if the enemy is going to walk off
    public LayerMask groundLayerMask;
    public LayerMask wallLayerMask;
    public bool isGroundAhead;
    public bool isInFront;

    public Transform lookInFrontPoint;
    public Transform lookAheadPoint;

    Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LookAhead();
        LookInFront();
        MoveEnemy();
    }

    private void LookAhead()
    {
        var hit = Physics2D.Linecast(transform.position, lookAheadPoint.position, groundLayerMask);
        isGroundAhead = (hit) ? true : false;
    }
    private void LookInFront()
    {
        var frontHit = Physics2D.Linecast(transform.position, lookAheadPoint.position, wallLayerMask);
        if (frontHit)
            Flip();
    }

    private void MoveEnemy()
    {
        //transform.position += new Vector3(speed * direction * Time.deltaTime, 0.0f);
        if (isGroundAhead)
        {
            rigidBody.AddForce(Vector2.left * runForce * transform.localScale.x);
            rigidBody.velocity *= 0.9f; // scale factor?
        } else
        {
            Flip();
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 6) // Layer mask 6 is the platform
        {
            transform.SetParent(other.transform);
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == 6)
        {
            transform.SetParent(null);
        }
    }

    // UTILITIES
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, lookAheadPoint.position);
        Gizmos.DrawLine(transform.position, lookInFrontPoint.position);
    }
}
