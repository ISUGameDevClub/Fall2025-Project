using UnityEngine;

public class SunFireball : MonoBehaviour
{
    private int damage;

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hurtbox")
        {
            collision.gameObject.GetComponentInParent<PlayerHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }

    }
}
