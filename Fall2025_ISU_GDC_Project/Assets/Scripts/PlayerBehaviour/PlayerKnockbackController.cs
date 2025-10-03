using UnityEngine;

public class PlayerKnockbackController : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void ApplyKnockback(Vector2 knockbackDir, float force, float duration)
    {
        playerMovement.enabled = false;
        rb.AddForce(knockbackDir * force, ForceMode2D.Impulse); //knockback
        Invoke("ResetPlayerMovement", duration);
    }

    private void ResetPlayerMovement()
    {
        playerMovement.enabled = true;
    }
}
