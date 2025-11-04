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
        hitstun   //Player is hit and cannot input anything.
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log(currentState);
            //playerMovement.enabled = false;
            ChangePlayerState(PlayerStateEnum.hitstun);
        }
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
                Debug.Log("the state is hitstun");
                gfx.enabled = true;
                playerMovement.enabled = false;
                playerStun.enabled = true;
                break;
        }
    }
}
