using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BeanUltimateActivator : MonoBehaviour
{
    [SerializeField] private HitboxProperties beanUltimateHitbox;

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

        if (pi.actions["Ultimate"].triggered)
        {
            ActivateBeanUltimate();
        }
    }

    private void ActivateBeanUltimate()
    {
        //enable ultimate attack animation
        if (GetComponent<Animator>() != null && !beanUltimateHitbox.GetCurrentlyAttacking())
        {
            PlayerInput pi = this.gameObject.transform.parent.GetComponent<PlayerInput>();
            UltimateTrackerManager ultimateTracker = FindFirstObjectByType<UltimateTrackerManager>();
            if (ultimateTracker.CanPlayerUseUltimate(pi))
            {
                SoundManager.PlaySound("Sound/SFX/Combat/WhooshSFX_02", 1.0f, false);
                GetComponent<Animator>().SetTrigger("UltimateAttack");
                //StartCoroutine("DisableBeanMovementRoutine");
                ultimateTracker.ResetPlayerUltimateCharge(pi);
            }

            SoundManager.PlaySound("Sound/SFX/Combat/Bean/Beam Laser_mixdown", .5f, false);
        }
    }

    //we need to disable the bean's movement for the duration of the animation
    // ^^ this is done by animation event now
    private IEnumerator DisableBeanMovementRoutine()
    {
        GetComponent<PlayerState>().ChangePlayerState(PlayerState.PlayerStateEnum.Attacking);


        AnimatorStateInfo stateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        //Debug.Log(stateInfo.ToString());

        yield return new WaitForSeconds(stateInfo.length + 0.85f);

        GetComponent<PlayerState>().ChangePlayerState(PlayerState.PlayerStateEnum.Active);
    }
}
