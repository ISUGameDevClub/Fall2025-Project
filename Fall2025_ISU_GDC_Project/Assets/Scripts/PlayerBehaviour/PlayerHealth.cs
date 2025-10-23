using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    [SerializeField] private GameObject damageParticles;
    [SerializeField] private GameObject deathParticles;
    private ParticleSystem damageParticlesInstance;
    private ParticleSystem deathParticlesInstance;
    private PlayerMovement playerMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (HP <= 0)
        {
            Destroy(gameObject);
            //This is in fixed update to not mess up my for each loop in the hitbox properties script.

            PlayDeathParticles();
        }
    }

    public void TakeDamage(int dmg)
    {
        HP -= dmg;
        //play particle effect
        SpawnDamageParticles();
    }

    private void PlayDeathParticles()
    {
        if (deathParticles != null)
        {
            GameObject deathParticleEffect = Instantiate(deathParticles, transform.position, Quaternion.identity);
            deathParticleEffect.GetComponent<ParticleSystem>().Play();

            Destroy(deathParticleEffect, 3f);
        }

        Destroy(gameObject);
    }

    private void SpawnDamageParticles()
    {
        GameObject particleEffectObject = Instantiate(damageParticles, transform.position, Quaternion.identity);
        particleEffectObject.GetComponent<ParticleSystem>().Play();
    }
}
