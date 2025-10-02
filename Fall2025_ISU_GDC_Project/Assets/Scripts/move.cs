using UnityEngine;

public class move : MonoBehaviour
{
    public GameObject selfShooter;
    private Rigidbody2D rb;
    public int damage = 200;
    public float projectileSpeed = 0f;
    public float projectileLifetime = 5f;
    private float time = 0;
  //  private bool isMoving = false;
   // private bool isJumpPressed = false;
 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collider2D col)
    {
        //if(col.gameObject!=selfShooter)
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.tag == "Hurtbox"&& collision.gameObject!=selfShooter)
        {
            collision.GetComponentInParent<PlayerHealth>().TakeDamage(damage);
        }
    }
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(projectileSpeed, 0f);

        time = time + Time.fixedDeltaTime;
        if (time > projectileLifetime)
        {
            Destroy(gameObject);
        }

        
    }
}