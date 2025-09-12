using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundedCheckLength;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float jumpLock;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private LayerMask floorLayer;
    private Rigidbody2D rb;
    private Vector2 movement;
    private float timer_coyoteTime;
    private float timer_jumpLock;
    private bool grounded;
    private bool jumpedThisFrame;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //read movement input
        movement = playerInput.actions["Move"].ReadValue<Vector2>();

        //ground check
        Debug.DrawRay(this.transform.position, new Vector2(0, -groundedCheckLength), Color.yellow);
        grounded = Physics2D.Raycast(this.transform.position, Vector2.down, groundedCheckLength, floorLayer);

        //Grounded logic, timers
        if ( grounded )
        {
            timer_coyoteTime = coyoteTime;
        }
        else
        {
            timer_coyoteTime -= Time.deltaTime;
        }
        timer_jumpLock -= Time.deltaTime;

        //jump
        if (playerInput.actions["Jump"].triggered && timer_coyoteTime > 0f && timer_jumpLock < 0f)
        {
            jumpedThisFrame = true;
            timer_coyoteTime = 0f;
            timer_jumpLock = jumpLock;
        }
    }

    private void FixedUpdate()
    {
        //apply movement value
        rb.linearVelocityX = movement.x * horizontalSpeed;

        //apply jump value
        if( jumpedThisFrame )
        {
            Vector2 v = rb.linearVelocity;
            v.y = jumpForce;
            rb.linearVelocity = v;

            jumpedThisFrame = false;
        }

        ////bring player down faster on downward movement
        //if (rb.linearVelocity.y < 0.3f)
        //{
        //    Vector2 v = rb.linearVelocity;
        //    v.y = rb.linearVelocity.y * 1.1f;
        //    rb.linearVelocity = v;
        //}
    }
}
