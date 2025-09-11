using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundedCheckLength;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private LayerMask floorLayer;
    private Rigidbody2D rb;
    private Vector2 movement;

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
        bool grounded = Physics2D.Raycast(this.transform.position, Vector2.down, groundedCheckLength, floorLayer);

        //jump
        if (playerInput.actions["Jump"].triggered && grounded)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        //apply movement value
        rb.linearVelocityX = movement.x * horizontalSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.gameObject.tag == "Platform" )
        {
            //Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
        }
    }
}
