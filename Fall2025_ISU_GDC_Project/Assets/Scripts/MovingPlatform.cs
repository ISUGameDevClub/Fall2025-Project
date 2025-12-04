using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float boxSpeed = 2.0f;
    [SerializeField] private float switchDirectionAfterTime;
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform endTransform;
    [SerializeField] private Rigidbody2D rb;
    private float timer;
    private bool goingLeft;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = switchDirectionAfterTime;
        goingLeft = true;
    }

    // Update is called once per frame
    void Update()
    {


        if (this.transform.position.x <= endTransform.position.x)
        {
            goingLeft = false;
        }

        if (this.transform.position.x >= startTransform.position.x)
        {
            goingLeft = true;
        }

    }

    private void FixedUpdate()
    {

        if (goingLeft)
        {
            rb.linearVelocityX = boxSpeed * -1;
        }
        else
        {
            rb.linearVelocityX = boxSpeed * 1;
        }

    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.GetComponent<PlayerMovement>() != null)
        {
            Debug.Log(collider.gameObject.name);

            Rigidbody2D playerRB = collider.gameObject.GetComponent<Rigidbody2D>();
            PlayerMovement playerMovement = collider.gameObject.GetComponent<PlayerMovement>();

            playerMovement.onMovingPlatform = true;

            if (goingLeft)
            {

                playerMovement.platformSpeed = boxSpeed * -1;
            }
            else
            {
                playerMovement.platformSpeed = boxSpeed * 1;
            }
        }

        
    }

    private void OnCollisionExit2D(Collision2D collider)
    {
        if (collider.gameObject.GetComponent<PlayerMovement>() != null)
        {
            PlayerMovement playerMovement = collider.gameObject.GetComponent<PlayerMovement>();

            playerMovement.onMovingPlatform = false;
        }

          
    }
}
