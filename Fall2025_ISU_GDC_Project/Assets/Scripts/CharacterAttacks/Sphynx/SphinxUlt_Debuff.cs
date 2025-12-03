using UnityEngine;

public class SphinxUlt_Debuff : MonoBehaviour
{
    float duration = 12;
    float timer;

    PlayerMovement moveRef;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = Time.time + duration;
        moveRef = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        moveRef.speedBoost = 0.25f;
        if (timer <= Time.time)
        {
            moveRef.speedBoost = 1f;
            Destroy(this);
        }
    }
}
