using System;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class CoR_Special : MonoBehaviour
{

    float heldAmount = 0;
    float maxHeldDuration = 1f;
    float chargeRate = .75f;
    int baseDamage = 6; //The bare minimum damage value this will deal
    float damageChargeBonus = 4; //This is multiplied by how long the move is held down.
    float baseKnockback = 2f;
    float fullChargeKnockbackBonus = 2f;
    bool heldDown = false;
    bool usingSpecial = false;

    public Transform shooterLocation;

    public GameObject arrow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<PlayerStun>().gotHit.AddListener(this.AttackInterupted);
        GetComponent<PlayerAttacks>().specialMove.AddListener(this.UseSpecial);
    }

    // Update is called once per frame
    void Update()
    {
        if (heldDown)
        {
            heldAmount += chargeRate * Time.deltaTime;
            heldAmount = Mathf.Clamp(heldAmount, 0, maxHeldDuration);
        }
        else
        {
            if (heldAmount > 0)
            {
                Debug.Log(heldAmount);
                ShootArrow();
                heldAmount = 0;
            }
        }
    }

    public void UseSpecial()
    {
        usingSpecial = true;
        Debug.Log("Using special");
    }

    private void ShootArrow()
    {
        Debug.Log("Fired a projectile");
        int totalDamage = baseDamage + Mathf.FloorToInt(damageChargeBonus * heldAmount);
        move projectile = Instantiate(arrow, shooterLocation.position, Quaternion.identity).GetComponent<move>();
        projectile.damage = totalDamage;
        projectile.projectileSpeed = 5f;
        projectile.selfShooter = gameObject;
        projectile.direction = GetComponent<PlayerMovement>().direction;
    }

    public void CheckIfHeldDown(InputAction.CallbackContext context)
    {
        Debug.Log("Input recieved");
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
    }

}
