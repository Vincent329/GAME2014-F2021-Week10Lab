using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Movement")] 
    public float horizontalForce;
    public float verticalForce;
    public bool isGrounded;
    public Transform groundOrigin;
    public float groundRadius;
    public LayerMask groundLayerMask;
    public float airControlFactor;

    [Header("Animation State")]
    private Animator animController;

    private Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        animController = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        CheckIfGrounded();
    }

    private void Move()
    {     // Keyboard Input
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        // Touch Input
        Vector2 worldTouch = new Vector2();
        foreach (var touch in Input.touches)
        {
            worldTouch = Camera.main.ScreenToWorldPoint(touch.position);
        }

        float mass = rigidbody.mass * rigidbody.gravityScale;

        rigidbody.velocity *= 0.99f; // scaling / stopping hack

        if (isGrounded)
        {
            //float deltaTime = Time.deltaTime;

       

            // Check for Flip

            if (x != 0)
            {
                x = FlipAnimation(x);
                animController.SetInteger("AnimationState", (int)PlayerAnimState.RUN);
            }
            else
            {
                animController.SetInteger("AnimationState", (int)PlayerAnimState.IDLE);
            }
            float horizontalMoveForce = x * horizontalForce;// * deltaTime;

            float jump = Input.GetAxisRaw("Jump");
            float jumpMoveForce = jump * verticalForce; // * deltaTime;
            rigidbody.AddForce(new Vector2(horizontalMoveForce, jumpMoveForce) * mass);


        }
        else
        {
            // regulate the animation
            animController.SetInteger("AnimationState", (int)PlayerAnimState.JUMP);
            if (x != 0)
            {
                x = FlipAnimation(x);
                float horizontalMoveForce = x * horizontalForce * airControlFactor;// * deltaTime;
                rigidbody.AddForce(new Vector2(horizontalMoveForce, 0.0f) * mass);

            }
        }

    }

    private void CheckIfGrounded()
    {
        // different kind of raycast
        RaycastHit2D hit = Physics2D.CircleCast(groundOrigin.position, groundRadius, Vector2.down, groundRadius, groundLayerMask);

        isGrounded = (hit) ? true : false;
    }

    private float FlipAnimation(float x)
    {
        // depending on direction scale across the x-axis either 1 or -1
        x = (x > 0) ? 1 : -1;

        transform.localScale = new Vector3(x, 1.0f);
        return x;
    }

    // EVENTS
  
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 6)
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
    // allows us to see the wiresphere in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundOrigin.position, groundRadius);
    }

}
