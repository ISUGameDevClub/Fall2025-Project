using UnityEngine;
using UnityEngine.Events;

public class PlayerStun : MonoBehaviour
{
    private float stunDuration = 20;//Set back to .3f for default after testing
    [SerializeField] PlayerState stateMachine;

    private float stunTimer = 0;

    public UnityEvent gotHit;

    void OnEnable()
    {
        HitboxProperties hitboxRef = GetComponentInChildren<HitboxProperties>();
        if (hitboxRef != null)
            hitboxRef.SetCurrentlyAttacking(false);
    }
    void Update()
    {
        if (Time.time > stunTimer)
        {
            GetComponent<Animator>().speed = 1;
            stateMachine.ChangePlayerState(PlayerState.PlayerStateEnum.Active);
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
