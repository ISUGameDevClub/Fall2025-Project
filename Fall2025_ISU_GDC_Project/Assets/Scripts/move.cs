using UnityEngine;
using UnityEngine.InputSystem;

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
    public float ultChargePerHit;
    public PlayerInput playerWhoShotThisArrow;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //set direction of arrow GFX
        if (selfShooter.transform.rotation.eulerAngles.y != 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
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
            //grant ultimate charge to attacker PlayerInput
            FindFirstObjectByType<UltimateTrackerManager>().AddUltimateCharge(playerWhoShotThisArrow, ultChargePerHit);

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