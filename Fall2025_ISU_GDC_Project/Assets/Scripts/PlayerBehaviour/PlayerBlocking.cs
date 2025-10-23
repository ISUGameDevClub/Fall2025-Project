using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class PlayerBlocking : MonoBehaviour
{

    [SerializeField] private SpriteRenderer shield;
    public float blockCoefficient = 1;
    private Rigidbody2D rb;
    private bool blockedThisFrame = false;
    private bool blockBeingHeld = false;
    public bool blocking = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shield.enabled = false;
        //shield.sortingOrder = 0;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput pi = null;
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

        PlayerState ps = GetComponent<PlayerState>();
        PlayerMovement pm = GetComponent<PlayerMovement>();

        blockedThisFrame = pi.actions["Block"].triggered;
        blockBeingHeld = pi.actions["Block"].IsPressed();

        if(blockedThisFrame && pm.grounded && pm.timer_jumpLock < 0f)
        {
            //activate block
            blocking = true;
            //shield.sortingOrder = 6;
            shield.enabled = true;
            ps.ChangePlayerState(PlayerState.PlayerStateEnum.Dormant);
            rb.linearVelocityX = 0;
        }
        if(blocking && !blockBeingHeld)
        {
            //deactivate block
            blocking = false;
            //shield.sortingOrder = 0;
            shield.enabled = false;
            ps.ChangePlayerState(PlayerState.PlayerStateEnum.Active);
        }

    }
}
