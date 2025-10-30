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
    [SerializeField] private float jumpHeldTime;
    [SerializeField] private LayerMask floorLayer;
    public float speedBoost = 1f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private float timer_coyoteTime;
    private float timer_jumpLock;
    private float timer_jumpHeld;
    private bool grounded;
    private bool jumpedThisFrame;
    private bool queueJump;
    private bool jumpBeingHeld;
    private bool jumpHoldOnce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {

        movement = context.ReadValue<Vector2>() * speedBoost;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumpedThisFrame = context.action.triggered;
        jumpBeingHeld = context.action.IsPressed();
    }

    private void Update()
    {
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
        if (jumpedThisFrame && timer_coyoteTime > 0f && timer_jumpLock < 0f)
        {
            queueJump = true;
            timer_coyoteTime = 0f;
            timer_jumpLock = jumpLock;
            jumpedThisFrame = false;
        }

        if ( !jumpBeingHeld )
        {
            jumpHoldOnce = false;
        }
    }

    private void FixedUpdate()
    {
        //apply movement value
        rb.linearVelocityX = movement.x * horizontalSpeed;

        //apply jump value
        if( queueJump )
        {
            Vector2 v = rb.linearVelocity;
            v.y = jumpForce;
            rb.linearVelocity = v;

            queueJump = false;
            timer_jumpHeld = jumpHeldTime;
            jumpHoldOnce = true;
        }

        //if jump held, continue to go higher until key is released
        if (jumpBeingHeld && jumpHoldOnce )
        {
            if (timer_jumpHeld > 0f)
            {
                timer_jumpHeld -= Time.deltaTime;
                Vector2 v = rb.linearVelocity;
                v.y = jumpForce;
                rb.linearVelocity = v;
            }
            else
            {
                jumpHoldOnce = false;
            }
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
