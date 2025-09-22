using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private SpriteRenderer gfx;

    public enum PlayerStateEnum
    {
        Active, //player is visible, responds to input
        Dormant, //player is visible, does NOT respond to input
        Inactive //player is NOT visible, does NOT respond to input
    }

    private void Awake()
    {
        //playerMovement = GetComponent<PlayerMovement>(); // have to assign before runtime, otherwise callbacks don't register in time
        //gfx = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("m")) //test call DELETE THIS
        {
            ChangePlayerState(PlayerStateEnum.Inactive);
        }
    }

    public void ChangePlayerState(PlayerStateEnum newState)
    {
        switch (newState)
        {
            case PlayerStateEnum.Active:
                gfx.enabled = true;
                playerMovement.enabled = true;
                break;
            case PlayerStateEnum.Dormant:
                gfx.enabled = true;
                playerMovement.enabled = false;
                break;
            case PlayerStateEnum.Inactive:
                gfx.enabled = false;
                playerMovement.enabled = false;
                break;
        }
    }
}
