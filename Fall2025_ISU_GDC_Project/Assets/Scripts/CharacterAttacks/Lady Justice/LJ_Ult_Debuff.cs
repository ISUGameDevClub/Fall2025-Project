using UnityEngine;

public class LJ_Ult_Debuff : MonoBehaviour
{
    float duration = 12;
    float timer;

    HitboxProperties hitboxRef;
    PlayerHealth healthRef;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = Time.time + duration;
        healthRef = GetComponent<PlayerHealth>();
        hitboxRef = GetComponentInChildren<HitboxProperties>();

    }

    // Update is called once per frame
    void Update()
    {
        healthRef.defMultiplier = 2;
        hitboxRef.damageBoost = .5f;
        if (timer <= Time.time)
        {
            healthRef.defMultiplier = 1;
            hitboxRef.damageBoost = 1;
            Destroy(this);
        }
    }
}
