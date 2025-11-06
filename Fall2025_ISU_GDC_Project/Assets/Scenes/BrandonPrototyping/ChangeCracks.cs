using UnityEngine;

public class ChangeCracks : MonoBehaviour
{
    public SpriteRenderer sprite_rend;

    public float cracksIntensity = .35f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite_rend.material.SetFloat("_CracksAmount", cracksIntensity);
    }

    // Update is called once per frame
    void Update()
    {
        sprite_rend.material.SetFloat("_CracksAmount", cracksIntensity);
    }
}
