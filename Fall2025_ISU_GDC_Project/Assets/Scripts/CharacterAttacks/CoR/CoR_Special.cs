using System;
using Unity.Mathematics;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class CoR_Special : MonoBehaviour
{

    [SerializeField] AnimationClip bowShot;
    float heldAmount = 0;
    float maxHeldDuration = 1f;
    float chargeRate = .75f;
    int baseDamage = 6; //The bare minimum damage value this will deal
    float damageChargeBonus = 4; //This is multiplied by how long the move is held down.

    float baseSpeed = 5;
    float speedChargeBonus = 6;
    float baseKnockback = 2f;
    //float fullChargeKnockbackBonus = 2f;
    bool heldDown = false;
    bool usingSpecial = false;

    public Transform shooterLocation;

    public GameObject arrow;

    private Animator CoRAnimator;

    public AnimationClip test;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<PlayerStun>().gotHit.AddListener(this.AttackInterupted);
        GetComponent<PlayerAttacks>().specialMove.AddListener(this.UseSpecial);
        CoRAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            //CoRAnimator.Play(test.name);
            CoRAnimator.Play(bowShot.name);
        }
        if (heldDown)
        {
            heldAmount += chargeRate * Time.deltaTime;
            heldAmount = Mathf.Clamp(heldAmount, 0, maxHeldDuration);
        }
        else
        {
            if (heldAmount > 0)
            {
                //ShootArrow();
                //heldAmount = 0;
            }
        }
        if (!heldDown)
        {
            CoRAnimator.speed = 1;
        }

        //if (!heldDown && Animator.speed)
    }

    public void UseSpecial()
    {
        Debug.Log("A special has been used!");
        usingSpecial = true;
        if (bowShot != null)
            CoRAnimator.Play(bowShot.name);
    }

    public void FireArrow()
    {
        Debug.Log("Im gonna shoot an arrow");
        int totalDamage = baseDamage + Mathf.FloorToInt(damageChargeBonus * heldAmount);
        move projectile = Instantiate(arrow, shooterLocation.position, Quaternion.identity).GetComponent<move>();
        projectile.damage = totalDamage;
        projectile.projectileSpeed = 5 + Mathf.FloorToInt(6 * heldAmount);
        projectile.selfShooter = gameObject;
        projectile.direction = GetComponent<PlayerMovement>().direction;
        heldAmount = 0;
    }

    public void PauseBow()
    {
        if (heldDown)
            CoRAnimator.speed = 0;
    }

    public void CheckIfHeldDown(InputAction.CallbackContext context)
    {
        if (usingSpecial)
            heldDown = true;
        if (context.canceled)
        {
            heldDown = false;
            usingSpecial = false;
        }
    }

    private void AttackInterupted()
    {
        usingSpecial = false;
        heldDown = false;
        heldAmount = 0;
        CoRAnimator.speed = 1;
    }

}
