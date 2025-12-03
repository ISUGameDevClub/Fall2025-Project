using System.Collections;
using Unity.VisualScripting;
using UnityEngine;



public class damageBuff : MonoBehaviour
{
    
    public int damageBoostAmt=2;
    public float duration=10;
    private HitboxProperties whoWeHit = null;
    public SpriteRenderer spriteRend;
    public Collider2D objCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objCollider = GetComponent<Collider2D>();
        spriteRend = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {


    }
    //invoke
    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Hurtbox")
        {
            
            whoWeHit = collision.GetComponentInParent<PlayerHealth>().gameObject.GetComponentInChildren<HitboxProperties>();
            StartCoroutine(applyDamageBuff());
        }
        
    }

    
    private void endDamageBuff()
    {
        whoWeHit.damageBoost = 1;
        Destroy(this.gameObject);
    }

    IEnumerator applyDamageBuff()
    {
        whoWeHit.damageBoost = 2;
        spriteRend.enabled = false;
        objCollider.enabled = false;
        yield return new WaitForSeconds(duration);
        whoWeHit.damageBoost = 1;
        Destroy(gameObject);
    }

}