using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BeanSpecialActivator : MonoBehaviour
{
    [SerializeField] private HitboxProperties beanSpecialHitbox;

    private void Start()
    {
        // vv Can enable this if or when specialMove is set up right vv
        //GetComponent<PlayerAttacks>().specialMove.AddListener(ActivateBeanSpecialAnim);
    }


    private void Update()
    {
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


        if (pi.actions["Special"].triggered)
        {
            ActivateBeanSpecialAnim();
        }
    }


    public void ActivateBeanSpecialAnim()
    {
        //enable special attack animation
        if (GetComponent<Animator>() != null && !beanSpecialHitbox.GetCurrentlyAttacking())
        {
            SoundManager.PlaySound("Sound/SFX/Combat/WhooshSFX_02", 1.0f, false);
            GetComponent<Animator>().SetTrigger("SpecialAttack");
            //StartCoroutine("DisableBeanMovementRoutine");

            SoundManager.PlaySound("Sound/SFX/Combat/Bean/BeanSlimeSFX", .5f, false);
        }
    }

    //we need to disable the bean's movement for the duration of the animation
    // ^^ this is handled by animation event instead now
    private IEnumerator DisableBeanMovementRoutine()
    {
        GetComponent<PlayerState>().ChangePlayerState(PlayerState.PlayerStateEnum.Attacking);

        
        AnimatorStateInfo stateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        //Debug.Log(stateInfo.ToString());
        
        yield return new WaitForSeconds(stateInfo.length + 0.85f);

        GetComponent<PlayerState>().ChangePlayerState(PlayerState.PlayerStateEnum.Active);
    }

}
