using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    [SerializeField] private GameObject damageParticles;
    private ParticleSystem damageParticlesInstance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (HP <= 0)
        {
            Destroy(gameObject);
            //This is in fixed update to not mess up my for each loop in the hitbox properties script.
        }
    }

    public void TakeDamage(int dmg)
    {
        HP -= dmg;
        //play particle effect
        SpawnDamageParticles();

    }

    private void SpawnDamageParticles()
    {
        GameObject particleEffectObject = Instantiate(damageParticles, transform.position, Quaternion.identity);
        particleEffectObject.GetComponent<ParticleSystem>().Play();
    }
}
