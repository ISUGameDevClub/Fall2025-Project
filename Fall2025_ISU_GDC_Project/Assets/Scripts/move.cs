using UnityEngine;

public class move : MonoBehaviour
{
    public GameObject selfShooter;
    public GameObject player;
    private Rigidbody2D rb;
    public int damage = 200;
    public float projectileSpeed = 0f;
    public float projectileLifetime = 5f;
    private float time = 0;
    public int direction = 1;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject!=selfShooter&&col.gameObject!=player)//&&col.gameObject!=gameObject)
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hurtbox" && collision.gameObject != selfShooter)
        {
            collision.GetComponentInParent<PlayerHealth>().TakeDamage(damage,1f);
            Destroy(gameObject);
        }
    }
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(projectileSpeed * direction, 0f); 
        time = time + Time.fixedDeltaTime;
        if (time > projectileLifetime)
        {
            Destroy(gameObject);
        }

        
    }
}