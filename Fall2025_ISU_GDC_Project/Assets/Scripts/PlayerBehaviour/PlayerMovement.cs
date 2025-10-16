using System.Runtime.CompilerServices;
using Unity.VisualScripting;
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
    private Rigidbody2D rb;
    private Vector2 movement;
    private float timer_coyoteTime;
    private float timer_jumpLock;
    private float timer_jumpHeld;
    private float timer_hitStun = 0.5f;
    private bool grounded;
    private bool jumpedThisFrame;
    private bool queueJump;
    private bool jumpBeingHeld;
    private bool jumpHoldOnce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        PlayerInput pi = null;

    //This is the general concept of hitstun, but the "timer_hitStun" isn't ever instantiated, which will lead to some problems.
    //I'm going to set a default value of .5f but see my other, somewhat scattered, notes. If not done by someone else, I will finish on 10/22
        if (rb.GetComponent<PlayerState.PlayerStateEnum>() == PlayerState.PlayerStateEnum.Stun)
        {
            if (timer_hitStun <= 0f)
            {
                rb.GetComponent<PlayerState>().ChangePlayerState(PlayerState.PlayerStateEnum.Active);
            }
            else
            {
                timer_hitStun -= Time.deltaTime;
            }
        }

        //we have a parent, use its PlayerInput component
        if (transform.parent != null)
        {
            GameObject parent = transform.parent.gameObject;
            pi = parent.GetComponent<PlayerInput>();
        }
        else //we dont have a parent, enable our own PlayerInput and use that
        {
            GetComponent<PlayerInput>().enabled = true;
            pi = GetComponent<PlayerInput>();
        }

        //grab input from PlayerInput component
        movement = pi.actions["Move"].ReadValue<Vector2>();
        jumpedThisFrame = pi.actions["Jump"].triggered;
        jumpBeingHeld = pi.actions["Jump"].IsPressed();

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

        //flip object based on movement direction
        if ( movement.x > 0f )
        {
            this.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        if ( movement.x < 0f )
        {
            this.transform.localEulerAngles = new Vector3(0, 180, 0);
        }
    }

    private void FixedUpdate()
    {
        //apply movement value
        rb.linearVelocityX = movement.x * horizontalSpeed;

        //apply jump value
        if (queueJump)
        {
            Vector2 v = rb.linearVelocity;
            v.y = jumpForce;
            rb.linearVelocity = v;

            queueJump = false;
            timer_jumpHeld = jumpHeldTime;
            jumpHoldOnce = true;
        }

        //if jump held, continue to go higher until key is released
        if (jumpBeingHeld && jumpHoldOnce)
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
    
    public void updateHitStunTimer(float time)
    {
        //ok gang, this might be bad practice and doesn't work also, so take this with a grain of salt
        //but my thought for hitstun is that we update the timer with this function in the hitboxProperties
        //i.e. this gets called there, then we decriment the timer above in the update loop
        //As mentioned, I don't fully know what I'm doing, so I didn't finish this just yet due to some confusion
        //if this isn't finished by 10/22 I will do it
    }
}
