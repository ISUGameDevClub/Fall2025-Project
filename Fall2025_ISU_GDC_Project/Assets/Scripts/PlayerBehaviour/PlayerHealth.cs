using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    //private ScoreManagerStartup scoreMangerReference;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //scoreMangerReference = FindAnyObjectByType<ScoreManagerStartup>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (HP <= 0)
        {
            //ScoreKill(enemykiller, thisplayer) here
            Destroy(gameObject);
            //This is in fixed update to not mess up my for each loop in the hitbox properties script.

        }
    }

    public void TakeDamage(int dmg)
    {
        HP -= dmg;
    }
}
