using UnityEngine;

public class LJ_Ult_Buffs : MonoBehaviour
{
     float duration = 12;
    float timer;

    HitboxProperties hitboxRef;
    PlayerHealth healthRef;
    PlayerMovement moveRef;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = Time.time + duration;
        healthRef = GetComponent<PlayerHealth>();
        moveRef = GetComponent<PlayerMovement>();
        hitboxRef = GetComponentInChildren<HitboxProperties>();

    }

    // Update is called once per frame
    void Update()
    {
        healthRef.defMultiplier = .5f;
        hitboxRef.damageBoost = 1.5f;
        moveRef.speedBoost = 1.5f;
        if (timer <= Time.time)
        {
            healthRef.defMultiplier = 1;
            hitboxRef.damageBoost = 1;
            moveRef.speedBoost = 1;
            Destroy(this);
        }
    }
}
