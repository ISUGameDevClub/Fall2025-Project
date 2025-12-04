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

    [SerializeField] private GameObject deathParticles;
    private float damagePercent = 1;
    public float defMultiplier = 1f;
    private Coroutine defenceCoroutine;
    [SerializeField] private float cracksIntensity = .55f;
    [SerializeField] private float lowHealthCracksIntensity = .35f;
    [SerializeField] private HitShade _HitShade;

    [SerializeField] Material crackShader;
    [SerializeField] SpriteRenderer spriteRend;

    private PlayerBlocking block;

    private PlayerState stateMachine;

    private void Awake()
    {
        startingHP = HP;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    

    void Start()
    {
        block = GetComponent<PlayerBlocking>();
        _HitShade = GetComponent<HitShade>();

        //Get players RB 
        playerRB = GetComponent<Rigidbody2D>();
        stateMachine = GetComponent<PlayerState>();
        //Add listeners (We must do this here (and not in heirarchy) as player objects are Prefabs)
        if (FindFirstObjectByType<GameSequenceManager>() != null)
        {
            playerDeath.AddListener(FindFirstObjectByType<GameSequenceManager>().doVictoryStuff);
        }
    }
    private void Update()
    {
        PlayerBlocking pb = GetComponent<PlayerBlocking>();
        if (pb.blocking)
        {
            damagePercent = 1 - pb.blockCoefficient;
        }
        else
        {
            damagePercent = 1;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (HP <= 0)
        {
            //This is in fixed update to not mess up my for each loop in the hitbox properties script.

            //when the player dies, they need to respawn
            PlayDeathParticles();
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

            if(WasLastPlayerToDie())
            {
                //only invoke victory event when we were the final player killed
                playerDeath.Invoke();
            }
            
            //Destroy(gameObject);
            this.gameObject.SetActive(false);
            
        }
    }

    private bool WasLastPlayerToDie()
    {
        //Need to find winning player(s)
        List<GameObject> currPlayers = FindFirstObjectByType<InputConnectionManager>().GetCurrentPlayerObjectsInGame();
        int winnerCount = 0;
        foreach (var player in currPlayers)
        {
            if (player != null)
            {
                    PlayerHealth ph = player.GetComponent<PlayerHealth>();
                    if (ph.GetTotalStocks() > 0)
                    {
                        winnerCount++;
                    }
            }
        }
        return winnerCount == 1;
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
    public void TakeDamage(int dmg, float hitstun)
    {
        
        int newDamage = Mathf.CeilToInt(dmg * defMultiplier * damagePercent);
        HP -= newDamage;
        if (!block.blocking)
        {
            stateMachine.ChangePlayerState(PlayerState.PlayerStateEnum.hitstun);
            GetComponent<PlayerStun>().setHitstunDuration(hitstun);
        }

        //play particle effect
        SpawnDamageParticles();

        _HitShade.CallDamageFlash();
    }

    private void PlayDeathParticles()
    {
        if (deathParticles != null)
        {
            GameObject deathParticleEffect = Instantiate(deathParticles, transform.position, Quaternion.identity);
            deathParticleEffect.GetComponent<ParticleSystem>().Play();

            Destroy(deathParticleEffect, 5f);
        }
        stateMachine.ChangePlayerState(PlayerState.PlayerStateEnum.hitstun);
        GetComponent<PlayerStun>().setHitstunDuration(50f);
        
        //play particle effect
        SpawnDamageParticles();
        SpriteRenderer spriteRend = GetComponent<SpriteRenderer>();
        if ((float)HP / (float)startingHP <= .25f)
        {
            spriteRend.material.SetFloat("_CracksAmount", lowHealthCracksIntensity);
        }
        else if ((float)HP / (float)startingHP <= .5f)
        {
            spriteRend.material.SetFloat("_CracksAmount", cracksIntensity);
        }
        
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

    public void ApplyCracks()
    {
        
        spriteRend.material = crackShader;
         if ((float)HP / (float)startingHP <= .25f)
        {
            spriteRend.material.SetFloat("_CracksAmount", lowHealthCracksIntensity);
        }
        else if ((float)HP / (float)startingHP <= .5f)
        {
            spriteRend.material.SetFloat("_CracksAmount", cracksIntensity);
        }
    }
}