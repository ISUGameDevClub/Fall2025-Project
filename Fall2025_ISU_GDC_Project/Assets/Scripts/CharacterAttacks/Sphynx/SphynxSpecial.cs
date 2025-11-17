using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using Unity.VisualScripting;

public class SphynxSpecial : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider2D hitboxCollider;
    [SerializeField] private AnimationClip idleAnimation;
    [SerializeField] private AnimationClip specialAnimation;
    [SerializeField] private AnimationClip cooldownAnimation;
    [SerializeField] private float speed;
    [SerializeField] private float duration;
    [SerializeField] private float endLag;
    private Rigidbody2D rb;
    private bool active = false;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<PlayerAttacks>().specialMove.AddListener(UseSpecial);
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            //HitboxProperties hp = GetComponent<HitboxProperties>();
            HitboxProperties hp = GetComponentInChildren<HitboxProperties>();
            if (hp.getInRange().Count > 0)
            {
                StartCoroutine(cancelSpecial());
            }
        }
    }

    public void UseSpecial()
    {
        
        animator.Play(specialAnimation.name);
        StartCoroutine("specialAttack");
    }

    private IEnumerator specialAttack()
    {
        active = true;
        PlayerState ps = GetComponent<PlayerState>();
        PlayerMovement pm = GetComponent<PlayerMovement>();
        ps.ChangePlayerState(PlayerState.PlayerStateEnum.Dormant);
        if (pm.direction > 0)
        {
            rb.linearVelocity = new Vector3(speed, 0, 0);
        }
        else
        {
            rb.linearVelocity = new Vector3(-1*speed, 0, 0);
        }

        yield return new WaitForSeconds(duration);

        StartCoroutine(cancelSpecial());
    }

    private IEnumerator cancelSpecial()
    {
        if (active)
        {
            PlayerState ps = GetComponent<PlayerState>();
            rb.linearVelocity = new Vector3(rb.linearVelocityX * .2f,0,0);
            animator.Play(cooldownAnimation.name);
            yield return new WaitForSeconds(endLag);
            animator.Play(idleAnimation.name);
            ps.ChangePlayerState(PlayerState.PlayerStateEnum.Active);
            active = false;
        }
        
    }
    
}
