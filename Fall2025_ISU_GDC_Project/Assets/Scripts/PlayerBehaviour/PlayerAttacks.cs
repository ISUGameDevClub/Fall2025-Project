using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerAttacks : MonoBehaviour
{

    [SerializeField] private Animator playerAnimator;
    [SerializeField] private AnimationClip[] lightAttacks;
    [SerializeField] private AnimationClip[] heavyAttacks;

    private int lightComboIndexer = 0;
    private int heavyComboIndexer = 0;
    private float comboTimer = 0;
    private float comboWindowDuration = 1;

    private HitboxProperties hitboxRef;
    private PlayerMovement playerMovement;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitboxRef = GetComponentInChildren<HitboxProperties>();
        playerMovement = GetComponentInParent<PlayerMovement>();

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
    }

    public void OnLightAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
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
    }

    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
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
    }
}
