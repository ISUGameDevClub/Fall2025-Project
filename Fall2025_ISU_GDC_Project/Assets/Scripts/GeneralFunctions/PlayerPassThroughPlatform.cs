using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Recorder.OutputPath;

public class PlayerPassThroughPlatform : MonoBehaviour
{
    [SerializeField] private float timeToReEnableCollider;
    [SerializeField] private float inputThreshold;
    private bool standingOnPlatform;
    private PlayerInput playerInput;
    private Collider2D platformColliderLastStoodOn;

    private void Start()
    {
        //we have a parent, use its PlayerInput component
        if (transform.parent != null)
        {
            GameObject parent = transform.parent.gameObject;
            playerInput = parent.GetComponent<PlayerInput>();
        }
        else //we dont have a parent, enable our own PlayerInput and use that
        {
            GetComponent<PlayerInput>().enabled = true;
            playerInput = GetComponent<PlayerInput>();
        }
    }

    private void Update()
    {
        //initiate temporary collider disable
        Vector2 movement = playerInput.actions["Move"].ReadValue<Vector2>();
        if (movement.y < (0f - inputThreshold) && standingOnPlatform)
        {
            StopAllCoroutines();
            StartCoroutine("DisablePlayerColliderRoutine");
            standingOnPlatform = false;
        }
    }

    private IEnumerator DisablePlayerColliderRoutine()
    {
        if (platformColliderLastStoodOn != null)
        {
            Physics2D.IgnoreCollision(platformColliderLastStoodOn, GetComponent<Collider2D>(), true);
        }
        
        yield return new WaitForSeconds(timeToReEnableCollider);

        if (platformColliderLastStoodOn != null)
        {
            Physics2D.IgnoreCollision(platformColliderLastStoodOn, GetComponent<Collider2D>(), false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool isPlayerAbovePlatform = gameObject.transform.position.y > collision.gameObject.transform.position.y;

        if (collision.gameObject.tag == "Platform" && isPlayerAbovePlatform)
        {
            standingOnPlatform = true;

            //make sure that if we were on another platform previously,
            //we reset that one's collision relationship
            if (platformColliderLastStoodOn != null)
            {
                Physics2D.IgnoreCollision(platformColliderLastStoodOn, GetComponent<Collider2D>(), false);
            }

            //assign new platform to colliderLastStoodOn
            platformColliderLastStoodOn = collision.gameObject.GetComponent<Collider2D>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            standingOnPlatform = false;
        }
    }
}
