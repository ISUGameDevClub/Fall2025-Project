using UnityEngine;

public class DefencePowerUp : MonoBehaviour
{
    public float duration = 10f;
    public float damageMultiplier = 0.5f; // Player takes 50% damage

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.activateDefBoost(damageMultiplier, duration);
            }

            // Hide or destroy the power-up
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject, 1f);
        }
    }
}
