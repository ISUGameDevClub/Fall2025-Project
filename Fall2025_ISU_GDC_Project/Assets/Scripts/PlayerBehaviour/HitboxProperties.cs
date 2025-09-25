using UnityEngine;
using System.Collections.Generic;
using UnityEngine.TextCore.Text;

public class HitboxProperties : MonoBehaviour
{

    [SerializeField] private int damage = 1;
    //Change this value in the animation clips to tie damage values to different attack animations.

    //SerializeField hitstun variable should go here.

    //SerializeField knockback variable should go here.

    [SerializeField] private bool isActive = false;
    //If true then when the hitbox is overlapping with someone, they take damage.
    //Set true only on certain frames of the animation when the attack should be able to hurt the player.

    [SerializeField] private bool currentlyAttacking = false;
    //This is used to see if the player is in a state of attacking. If true then they can't use other attacks.
    //Will use this also to make it so the player can't move while this is true.

    private List<GameObject> hurtEnemies = new List<GameObject>();
    private List<GameObject> inRange = new List<GameObject>();


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hurtbox")
        {
            inRange.Add(collision.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        inRange.Remove(collision.gameObject);
    }


    void Update()
    {
        if (isActive)
        {
            foreach (GameObject enemy in inRange)
            {
                if (hurtEnemies.IndexOf(enemy) == -1)//-1 means not found in list.
                {
                    PlayerHealth enemyHP = enemy.GetComponentInParent<PlayerHealth>();
                    if (enemyHP != null)
                    {
                        hurtEnemies.Add(enemy);
                        enemyHP.TakeDamage(damage);

                      
                    }
                }
            }
        }
        else
        {
            hurtEnemies.Clear();
        }
    }

    public bool GetCurrentlyAttacking()
    {
        return currentlyAttacking;
    }
    

}
