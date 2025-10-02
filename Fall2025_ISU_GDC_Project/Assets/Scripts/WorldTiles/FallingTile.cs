using System;
using Unity.VisualScripting;
using UnityEngine;

public class FallingTile : MonoBehaviour
{
    [SerializeField] int objectHitPoints;

    [SerializeField] bool isFalling;


    //temp color stuff
    [SerializeField] Color32 redColor = new Color32(255, 0, 0, 255);
    [SerializeField] Color32 yellowColor = new Color32(255, 255, 0, 255);


    private Rigidbody2D rb;
    private PlatformEffector2D pe2D;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        pe2D = gameObject.GetComponent<PlatformEffector2D>();
    }


    void Start()
    {

    }

    public void TakeDamage(int dmg)
    {
        objectHitPoints -= dmg;
        if (objectHitPoints <= 25)
        {
            gameObject.GetComponent<SpriteRenderer>().color = redColor;
        }
        else if (objectHitPoints <= 50)
        {
            gameObject.GetComponent<SpriteRenderer>().color = yellowColor;
        }

        if (objectHitPoints <= 0)
        {
            rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            pe2D.useOneWay = false;
            isFalling = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hurtbox" && isFalling)
        {
            collision.gameObject.GetComponentInParent<PlayerHealth>().TakeDamage(10);
            Destroy(gameObject);
        }
        
    }
}
