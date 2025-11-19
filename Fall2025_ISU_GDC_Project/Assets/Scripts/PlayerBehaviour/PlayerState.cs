using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] private PlayerStun playerStun;

    [SerializeField] private PlayerAttacks playerAttacks;
    [SerializeField] private SpriteRenderer gfx;

    PlayerStateEnum currentState = PlayerStateEnum.Active;

    public enum PlayerStateEnum
    {
        Active, //player is visible, responds to input
        Dormant, //player is visible, does NOT respond to input
        Inactive, //player is NOT visible, does NOT respond to input
        hitstun,  //Player is hit and cannot input anything.
        Attacking //Stops the player from moving while they attack.
    }

    private void Update()
    {
        
/*        if (Input.GetKeyDown("m")) //test call DELETE THIS
        {
            ChangePlayerState(PlayerStateEnum.Inactive);
        }*/
    }

    public void ChangePlayerState(PlayerStateEnum newState)
    {
        currentState = newState;
        switch (newState)
        {
            case PlayerStateEnum.Active:
                gfx.enabled = true;
                playerMovement.enabled = true;
                playerAttacks.enabled = true;
                playerStun.enabled = false;

                //unfreeze x, unfreeze y, freeze rotations
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                break;
            case PlayerStateEnum.Dormant:
                gfx.enabled = true;
                playerMovement.enabled = false;
                playerAttacks.enabled = false;
                playerStun.enabled = false;
                break;
            case PlayerStateEnum.Inactive:
                gfx.enabled = false;
                playerMovement.enabled = false;
                playerAttacks.enabled = false;
                playerStun.enabled = false;
                break;
            case PlayerStateEnum.hitstun:
                gfx.enabled = true;
                playerMovement.enabled = false;
                playerAttacks.enabled = false;
                playerStun.enabled = true;
                break;
            case PlayerStateEnum.Attacking:
                gfx.enabled = true;
                playerMovement.enabled = false;

                //freeze x and rot, keep y unfrozen
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                break;
        }
    }

    public PlayerStateEnum GetCurrentState()
    {
        return currentState;
    }
}
