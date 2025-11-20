using System;
using Unity.Mathematics;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //GetComponent<PlayerStun>().gotHit.AddListener(this.AttackInterupted);
        GetComponent<PlayerAttacks>().specialMove.AddListener(this.UseSpecial);
        CoRAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        heldDown = transform.root.gameObject.GetComponent<PlayerInput>().actions["Special"].IsPressed();
       
        if (heldDown)
        {
            heldAmount += chargeRate * Time.deltaTime;
            heldAmount = Mathf.Clamp(heldAmount, 0, maxHeldDuration);
            Debug.Log("Holding Down");
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
        usingSpecial = true;
        if (bowShot != null)
        {
            CoRAnimator.Play(bowShot.name);
        }
    }

    public void FireArrow()
    {
        int totalDamage = baseDamage + Mathf.FloorToInt(damageChargeBonus * heldAmount);
        move projectile = Instantiate(arrow, shooterLocation.position, Quaternion.identity).GetComponent<move>();
        projectile.damage = totalDamage;
        projectile.projectileSpeed = 5 + Mathf.FloorToInt(6 * heldAmount);
        projectile.selfShooter = gameObject;
        projectile.direction = GetComponent<PlayerMovement>().direction;

        //assign the PlayerInput of who shot to this arrow
        //grant ultimate charge to attacker PlayerInput (this script's top-most parent, if it exists)
        PlayerInput attackerPi = null;
        if (this.gameObject.transform.parent != null)
        {
            attackerPi = this.gameObject.transform.parent.gameObject.GetComponent<PlayerInput>();
            projectile.playerWhoShotThisArrow = attackerPi;
        }

        heldAmount = 0;

        SoundManager.PlaySound("Sound/SFX/Combat/COR/COR_HalfChargeArrowSFX", 2f, false);
    }

    public void PauseBow()
    {
        if (heldDown)
        {
            Debug.Log("Should be held down");
            CoRAnimator.speed = 0;
        }
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
