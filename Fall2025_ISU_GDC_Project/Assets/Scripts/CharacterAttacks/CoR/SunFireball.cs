using UnityEngine;
using UnityEngine.InputSystem;

public class SunFireball : MonoBehaviour
{
    private int damage;

    private float hitStun = .3f;
    private PlayerInput pi; //the reference of the PlayerInput that fired THIS ultimate (for making sure they dont do damage to themselves)

    public void InitializeSunFireball(int damage, PlayerInput pi)
    {
        this.damage = damage;
        this.pi = pi;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hurtbox")
        {
            //look at the parent of the hurtbox, if its the colossus who fired this ultimate, ignore damage

            bool isColossusWhoFiredUltimate = false;
            if (collision.gameObject.transform.parent.parent != null)
            {
                GameObject topMostParent = collision.gameObject.transform.parent.parent.gameObject;
                isColossusWhoFiredUltimate = topMostParent.GetComponent<PlayerInput>() == pi;
            }

            if (!isColossusWhoFiredUltimate)
            {
                collision.gameObject.GetComponentInParent<PlayerHealth>().TakeDamage(damage,hitStun);
                Destroy(gameObject);
            }
        }

    }
}
