using UnityEngine;
using System.Collections.Generic;
using UnityEngine.TextCore.Text;
using System.Security.Cryptography;
using UnityEngine.InputSystem;
public class PetrifyCollider : MonoBehaviour
{
    [SerializeField] private bool isActive = false;
    [SerializeField] private int damage = 5;
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
    private void petrify(Rigidbody2D rb)
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    }
    private void unpetrify(Rigidbody2D rb)
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
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
                        if (enemyHP != null)
                        {
                            hurtEnemies.Add(enemy);
                            enemyHP.TakeDamage(damage);
                            petrify(enemy.GetComponentInParent<Rigidbody2D>());
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
}