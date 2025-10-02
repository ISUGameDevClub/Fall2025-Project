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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get players RB 
        playerRB = GetComponent<Rigidbody2D>();
        //Add listeners (We must do this here (and not in heirarchy) as player objects are Prefabs)
        //TEMPORARILY TURN OFF LISTENER FOR SPHINX ULT PROTOTYPE ------------
        //playerDeath.AddListener(GameObject.FindGameObjectWithTag("GameController").GetComponent<GameSequenceManager>().doVictoryStuff);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (HP <= 0)
        {
            Destroy(gameObject);
            //This is in fixed update to not mess up my for each loop in the hitbox properties script.
        }
        if (totalStocks <= 0 && !active)
        {
            active = true;
            playerDeath.Invoke();
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
        HP -= dmg;
    }

    public int GetStocks()
    {
        return totalStocks;
    }
}
