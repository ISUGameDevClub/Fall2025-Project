using UnityEngine;

public class HurtPlayerOnContact : MonoBehaviour
{
    [SerializeField] private int damage;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hurtbox")
        {
            collision.gameObject.GetComponentInParent<PlayerHealth>().TakeDamage(damage, 1.0f);
        }

    }
}
