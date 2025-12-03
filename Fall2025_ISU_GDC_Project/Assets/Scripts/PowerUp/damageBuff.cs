using Unity.VisualScripting;
using UnityEngine;



public class damageBuff : MonoBehaviour
{
    
    public int damageBoostAmt=1;
    public float duration=5;

    private HitboxProperties whoWeHit = null;
    
    public Collider2D collider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        if (collider.gameObject.tag == "player")
        {
            //if (duration > 0)
            //{
                
            //    duration -= Time.deltaTime;
            //    print(damage);
            //}
        }
    }
    //invoke
    public void OnTriggerEnter2D(Collider2D collision)
    {


        //start danage debuff
        Debug.Log(collision.gameObject.name);
        GameObject hitboxObj = collision.gameObject.transform.parent.GetChild(0).gameObject;
       
        whoWeHit = hitboxObj.GetComponent<HitboxProperties>();
        startDamageBuff(collision);

        //invoke endDamageBuff after 5 seconds
        Invoke("endDamageDebuff", duration); //should I use duration variable
        
    }

    private void startDamageBuff(Collider2D collision)
    {

       
        GameObject hitboxObj = collision.gameObject.transform.parent.GetChild(0).gameObject;
        hitboxObj.GetComponent<HitboxProperties>().damageBoost = damageBoostAmt;

        //damage *= buff;
        //Destroy(gameObject);
        this.gameObject.SetActive(false);
    }
    private void endDamageBuff()
    {
        whoWeHit.damageBoost = 0;
        Destroy(this.gameObject);
    }

}