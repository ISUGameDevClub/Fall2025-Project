using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;



public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    [SerializeField] private int totalStocks = 3;

    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private bool active = false;

    //Event for death of character (no more stocks)
    public UnityEvent playerDeath;

    private int startingHP;

    [SerializeField] private GameObject damageParticles;

    public float defMultiplier = 1f;
    private Coroutine defenceCoroutine;

    private void Awake()
    {
        startingHP = HP;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get players RB 
        playerRB = GetComponent<Rigidbody2D>();
        //Add listeners (We must do this here (and not in heirarchy) as player objects are Prefabs)
        if (FindFirstObjectByType<GameSequenceManager>() != null)
        {
            playerDeath.AddListener(FindFirstObjectByType<GameSequenceManager>().doVictoryStuff);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (HP <= 0)
        {
            //This is in fixed update to not mess up my for each loop in the hitbox properties script.

            //when the player dies, they need to respawn
            totalStocks--;
            ResetPlayerHealth();
            if (FindFirstObjectByType<RespawnManager>() != null)
            {
                FindFirstObjectByType<RespawnManager>().RespawnPlayer(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        if (totalStocks <= 0 && !active)
        {
            active = true;
            playerDeath.Invoke();
            Destroy(gameObject);
        }
    }

    //WORRY ABOUT THIS LATER (DISABLE PLAYER MOVE)----

    /* Disables further player movement upon death
       To be subscribed to playerDeath event*/
    // private void disablePlayerMovement()
    // {
    // }

    //WORRY ABOUT THIS LATER (DISABLE PLAYER MOVE)---- ^^

    /* Deal damage to player
    /  @param dmg - dmg to take
    */
    public void TakeDamage(int dmg)
    {
        int newDamage = Mathf.CeilToInt(dmg * defMultiplier);
        HP -= newDamage;
        //play particle effect
        SpawnDamageParticles();

    }

    private void SpawnDamageParticles()
    {
        if (damageParticles != null)
        {
            GameObject particleEffectObject = Instantiate(damageParticles, transform.position, Quaternion.identity);
            particleEffectObject.GetComponent<ParticleSystem>().Play();
        }
    }

    public int GetStocks()
    {
        return totalStocks;
    }

    public void ResetPlayerHealth()
    {
        HP = startingHP;
    }

    public int GetPlayerHealth()
    {
        return HP;
    }

    public int GetStartingHP()
    {
        return startingHP;
    }

    public int GetTotalStocks()
    {
        return totalStocks;
    }

    public void activateDefBoost(float newMultiplier, float duration)
    {
        if (defenceCoroutine != null)
        {
            StopCoroutine(defenceCoroutine);
        }
        defenceCoroutine = StartCoroutine(defBoostCoroutine(newMultiplier, duration));
    }

    private IEnumerator defBoostCoroutine(float newMultiplier, float duration)
    {
        defMultiplier = newMultiplier;

        yield return new WaitForSeconds(duration);
        defMultiplier = 1f;

    }
}
