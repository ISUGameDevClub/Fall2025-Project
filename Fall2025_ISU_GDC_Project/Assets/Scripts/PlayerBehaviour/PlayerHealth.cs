using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private HitShade _HitShade;

    void Start()
    {

        _HitShade = GetComponent<HitShade>();
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
        _HitShade.CallDamageFlash();
    }
}