using UnityEngine;
using System.Collections.Generic;
using UnityEngine.TextCore.Text;
using System.Security.Cryptography;
using UnityEngine.InputSystem;
public class PetrifyCollider : MonoBehaviour
{
    [SerializeField] private bool isActive = false;
    private List<GameObject> hurtEnemies = new List<GameObject>();

    private List<GameObject> inRange = new List<GameObject>();
    private List<GameObject> petrifiedEnemies = new List<GameObject>();

    private List<GameObject> enemiesToUnpetrify = new List<GameObject>();

    private float petrifyTimer = 5.0f;
    public float petrifyTime = 5.0f;

    public float ultChargeOnPetrify = 5f;

    
    //petrifyTimer = petrifyTime;
    [SerializeField] bool petrifyActive;

    private void Start()
    {
        petrifyTimer = petrifyTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
              //  Debug.Log("we hit something with petrify collider");

        if (collision.gameObject.tag == "Hurtbox" || collision.gameObject.tag == "Hazard")
        {
            //Debug.Log("we hit hurtbox");
            inRange.Add(collision.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        inRange.Remove(collision.gameObject);
    }
    private void petrify(Rigidbody2D rb)
    {
        //grant ultimate charge to attacker PlayerInput (this script's top-most parent, if it exists)
        PlayerInput attackerPi = null;
        if (this.gameObject.transform.parent.parent != null)
        {
            attackerPi = this.gameObject.transform.parent.parent.gameObject.GetComponent<PlayerInput>();
            FindFirstObjectByType<UltimateTrackerManager>().AddUltimateCharge(attackerPi, ultChargeOnPetrify);
        }

        // Debug.Log("tried to petrify");
        rb.gameObject.GetComponent<PlayerState>().ChangePlayerState(PlayerState.PlayerStateEnum.Petrified);

        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    }
    private void unpetrify(Rigidbody2D rb)
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.gameObject.GetComponent<PlayerState>().ChangePlayerState(PlayerState.PlayerStateEnum.Active);

        //get rid of petrify overlay graphic
        //rb.gameObject.transform.Find("Petrified_Overlay").gameObject.SetActive(false);
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
                        //Debug.Log("hurtbox");
                        PlayerHealth enemyHP = enemy.GetComponentInParent<PlayerHealth>();
                        if (enemyHP != null)
                        {
                            hurtEnemies.Add(enemy);
                            petrify(enemy.GetComponentInParent<Rigidbody2D>());
                            petrifiedEnemies.Add(enemy);
                            petrifyActive = true;
                            
                            //add petrify overlay graphic (THIS IS DONE NOW IN PlayerState)
                            //enemy.transform.parent.Find("Petrified_Overlay").gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
        else
        {
            hurtEnemies.Clear();
        }

        enemiesToUnpetrify = new List<GameObject>();
        foreach (GameObject enemy in petrifiedEnemies)
        {
            Debug.Log(petrifyTimer);
            if (petrifyTimer <= 0.0f)
            {
                unpetrify(enemy.GetComponentInParent<Rigidbody2D>());
                petrifyTimer = petrifyTime;
                petrifyActive = false;
                enemiesToUnpetrify.Add(enemy);
                //petrifiedEnemies.Remove(enemy);
            }
        }

        foreach(GameObject enemy in enemiesToUnpetrify)
        {
            if (petrifiedEnemies.Contains(enemy))
            {
                petrifiedEnemies.Remove(enemy);
            }
        }

        if (petrifyActive)
            petrifyTimer -= Time.deltaTime;
        
    }
}