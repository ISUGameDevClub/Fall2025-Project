using UnityEngine;
using UnityEngine.Events;

public class PlayerStun : MonoBehaviour
{
    private float stunDuration = 20;//Set back to .3f for default after testing
    [SerializeField] PlayerState stateMachine;

    private float stunTimer = 0;

    public UnityEvent gotHit;

    [SerializeField] AnimationClip stunAnim;
    [SerializeField] AnimationClip idleAnim;

    void Start()
    {
        this.enabled = false;
    }
    void OnEnable()
    {
        HitboxProperties hitboxRef = GetComponentInChildren<HitboxProperties>();
        if (stunAnim != null)
            GetComponent<Animator>().Play(stunAnim.name);
        if (hitboxRef != null)
            hitboxRef.SetCurrentlyAttacking(false);
    }
    void Update()
    {
        if (Time.time > stunTimer)
        {
            if (idleAnim != null)
                GetComponent<Animator>().Play(idleAnim.name);

            GetComponent<Animator>().SetBool("Idle", true);
            stateMachine.ChangePlayerState(PlayerState.PlayerStateEnum.Active);
        }
        else
        {   
            if (stunAnim != null)
                GetComponent<Animator>().Play(stunAnim.name);
        }
    }

    public void setHitstunDuration(float hitstun)
    {
        stunDuration = hitstun;
        stunTimer = Time.time + stunDuration;
        gotHit.Invoke();//Dont know if most moves will need this but the CoR special will
    }

    public void pauseHitStunAnim()
    {
        GetComponent<Animator>().speed = 0;
    }
}
