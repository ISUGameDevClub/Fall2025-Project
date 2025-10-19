using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerAttacks : MonoBehaviour
{
    public GameObject selfHurtbox;
    public GameObject player;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private AnimationClip[] lightAttacks;
    [SerializeField] private AnimationClip[] heavyAttacks;
    private string ladyJusticeSpecial = "Lady Justice Special";
    public GameObject projectile;
    public GameObject projectileSpawn;

    private int lightComboIndexer = 0;
    private int heavyComboIndexer = 0;
    private float comboTimer = 0;
    private float comboWindowDuration = 1;
 


    private HitboxProperties hitboxRef;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitboxRef = GetComponentInChildren<HitboxProperties>();
    }

    // Update is called once per frame
    void Update()
    {
        if (comboTimer < Time.time)//If too much time has passed in between attacks...
        {
            lightComboIndexer = 0;
            heavyComboIndexer = 0;
            //Then reset the combo.
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            shootProjectile();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            playerAnimator.Play(ladyJusticeSpecial, 0, 0.0f);
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

        if (pi.actions["Light Attack"].triggered)
        {
            OnLightAttack();
        }
        if (pi.actions["Heavy Attack"].triggered)
        {
            OnHeavyAttack();
        }
    }
    public void OnLightAttack()
    {
        if (lightComboIndexer > lightAttacks.Count() - 1)
        {
            lightComboIndexer = 0;
        }
        if (!hitboxRef.GetCurrentlyAttacking())
        {
            comboTimer = Time.time + comboWindowDuration;
            playerAnimator.Play(lightAttacks[lightComboIndexer].name);
            lightComboIndexer += 1;
        }
    }

    public void OnHeavyAttack()
    {
        if (heavyComboIndexer > heavyAttacks.Count() - 1)
        {
            heavyComboIndexer = 0;
        }
        if (!hitboxRef.GetCurrentlyAttacking())
        {
            comboTimer = Time.time + comboWindowDuration;
            playerAnimator.Play(heavyAttacks[heavyComboIndexer].name);
            heavyComboIndexer += 1;
        }
    }

    public void shootProjectile()
    {
        move bullet = Instantiate(projectile, projectileSpawn.transform.position, Quaternion.identity).GetComponent<move>();

        if (player.transform.rotation.eulerAngles.y == -180f || player.transform.rotation.eulerAngles.y == 180f)
            bullet.direction = 0;
        else
            bullet.direction = 1;
        bullet.selfShooter = selfHurtbox;
        bullet.player = player;
    }
}
