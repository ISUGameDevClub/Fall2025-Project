using UnityEngine;
using System.Collections.Generic;
using UnityEngine.TextCore.Text;
using System.Security.Cryptography;
using UnityEngine.InputSystem;

public class HitboxProperties : MonoBehaviour
{

    [SerializeField] private int damage = 1;
    //Change this value in the animation clips to tie damage values to different attack animations.

    //SerializeField hitstun variable should go here.

    [SerializeField] private float knockbackForce = 5f;
    //knockback

    [SerializeField] private float hitStun = .3f; 

    [SerializeField] private float knocbackDuration = 0.5f;

    [SerializeField] private bool isActive = false;
    //If true then when the hitbox is overlapping with someone, they take damage.
    //Set true only on certain frames of the animation when the attack should be able to hurt the player.

    [SerializeField] private bool currentlyAttacking = false;
    //This is used to see if the player is in a state of attacking. If true then they can't use other attacks.
    //Will use this also to make it so the player can't move while this is true.

    [SerializeField] private float ultimateChargePerHit;
    //controls how much ultimate charge is granted upon actually hitting an enemy. This will be granted to the user of the attack,
    //not the one getting attacked

    private List<GameObject> hurtEnemies = new List<GameObject>();
    private List<GameObject> inRange = new List<GameObject>();


    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Hurtbox" || collision.gameObject.tag == "Hazard")
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
                    if (enemy.tag == "Hurtbox")
                    {
                        PlayerHealth enemyHP = enemy.GetComponentInParent<PlayerHealth>();
                        Rigidbody2D enemyRB = enemy.GetComponentInParent<Rigidbody2D>(); //for the knockback
                        PetrifyDebuff enemyPetrify = enemy.GetComponentInParent<PetrifyDebuff>();
                        PlayerKnockbackController playerKnockbackController = enemy.GetComponentInParent<PlayerKnockbackController>();
                        if (enemyHP != null)
                        {
                            hurtEnemies.Add(enemy);
                            enemyHP.TakeDamage(damage,hitStun);
                            //apply force backwards to enemy
                            bool onLeft;
                            if (this.gameObject.transform.parent.position.x < playerKnockbackController.gameObject.transform.position.x)
                            {
                                onLeft = true;
                            }
                            else
                            {
                                onLeft = false;
                            }

                            Vector2 knockbackDirection = new Vector2(onLeft ? 1f : -1f, 0f); //knockback
                            playerKnockbackController.ApplyKnockback(knockbackDirection, knockbackForce, knocbackDuration);

                            //grant ultimate charge to attacker PlayerInput (this script's top-most parent, if it exists)
                            PlayerInput attackerPi = null;
                            if (this.gameObject.transform.parent.parent != null)
                            {
                                attackerPi = this.gameObject.transform.parent.parent.gameObject.GetComponent<PlayerInput>();
                                FindFirstObjectByType<UltimateTrackerManager>().AddUltimateCharge(attackerPi, ultimateChargePerHit);
                            }
                        }
                    }
                    else if (enemy.tag == "Hazard")
                    {
                        FallingTile tileHP = enemy.GetComponentInParent<FallingTile>();
                        if (tileHP != null)
                        {
                            hurtEnemies.Add(enemy);
                            tileHP.TakeDamage(damage);
                        }
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
