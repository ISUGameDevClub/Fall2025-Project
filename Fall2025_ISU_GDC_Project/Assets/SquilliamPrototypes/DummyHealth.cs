using UnityEngine;

public class DummyHealth : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    [SerializeField] private bool invincible = false;
    [SerializeField] private bool reviveOnDeath = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (HP <= 0)
        {
            if (reviveOnDeath)
            {
                HP = 100;
                transform.position = new Vector2(0, 0);
            }
            else
            {
                Destroy(gameObject);
            }
            
            //This is in fixed update to not mess up my for each loop in the hitbox properties script.
        }
    }

    public void TakeDamage(int dmg)
    {
        if (!invincible)
        {
            HP -= dmg;
        }
    }
}
