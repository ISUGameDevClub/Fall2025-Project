using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
public class PlayerAttacks : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    public PetrifyDebuff pd;
    private HitboxProperties hitboxRef;
    private PlayerMovement playerMovement;

    public AnimationClip normalAttack;

    public AnimationClip Ultimate;

    public UnityEvent specialMove;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitboxRef = GetComponentInChildren<HitboxProperties>();
        playerMovement = GetComponentInParent<PlayerMovement>();

    }


     // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.J))
        {
            playerAnimator.Play(Ultimate.name);
        }
        PlayerInput pi = null;
        //we have a parent, use its PlayerInput component
        if (transform.parent != null)
        {
            GameObject parent = transform.parent.gameObject;
            pi = parent.GetComponent<PlayerInput>();
        }
        else //we dont have a parent, enable our own PlayerInput and use that
        {
            GetComponent<PlayerInput>().enabled = true;
            pi = GetComponent<PlayerInput>();
        }

        if (pi.actions["Normal Attack"].triggered)//&&!petrified)
        {
            NormalAttack();
        }
        if (pi.actions["Special"].triggered)//&&!petrified)
        {
            if (!hitboxRef.GetCurrentlyAttacking())
                UseSpecialMove();
        }
    }
    
    public void NormalAttack()
    {

        if (playerAnimator != null && hitboxRef != null)
        {
            if (!hitboxRef.GetCurrentlyAttacking())
            {
                playerAnimator.SetTrigger("NeutralAttack");
                if (normalAttack != null)
                {
                    playerAnimator.Play(normalAttack.name);
                }
                SoundManager.PlaySound("Sound/SFX/Combat/WhooshSFX_02", .5f, false);
                //SoundManager.PlaySound("Sound/SFX/Combat/Colossus of Rhoades Charging Bow.wav", .5f, false);
            }
        }
    }

    public void UseSpecialMove()
    {
        specialMove.Invoke();
    }

}
