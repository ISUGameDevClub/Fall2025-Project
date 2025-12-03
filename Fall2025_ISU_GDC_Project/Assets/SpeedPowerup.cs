using UnityEngine;

public class SpeedPowerup : MonoBehaviour
{

    public int speedIncrease = 1;
    public float duration = 10.0f;
    public PlayerMovement horizontalSpeed;
    public Collider2D playerCollision;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hurtbox")
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false; 
            playerCollision = collision;
            collision.GetComponentInParent<PlayerMovement>().speedBoost += speedIncrease;
            Debug.Log("Hit player");
            Invoke("EndSpeedBoost", duration);
        }
    }

    public void EndSpeedBoost()
    {
        Debug.Log("Ending boost");
        PlayerMovement moveRef = playerCollision.GetComponentInParent<PlayerMovement>();
        if (moveRef.speedBoost != 1)
            playerCollision.GetComponentInParent<PlayerMovement>().speedBoost -= speedIncrease;

        Destroy(gameObject);
    }
}
