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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitboxRef = GetComponentInChildren<HitboxProperties>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Current heavy attack is :" + heavyComboIndexer);
        if (comboTimer < Time.time)//If too much time has passed in between attacks...
        {
            lightComboIndexer = 0;
            heavyComboIndexer = 0;
            //Then reset the combo.
        }
    }

    public void OnLightAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Current light attack is :" + heavyComboIndexer);
        if (!hitboxRef.GetCurrentlyAttacking())
        {
            comboTimer = Time.time + comboWindowDuration;
            if (lightComboIndexer > lightAttacks.Count() - 1)
            {
                lightComboIndexer = 0;
            }
            playerAnimator.Play(lightAttacks[lightComboIndexer].name);
            lightComboIndexer += 1;
        }
    }

    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if (!hitboxRef.GetCurrentlyAttacking())
        {
            comboTimer = Time.time + comboWindowDuration;
            if (heavyComboIndexer > heavyAttacks.Count() - 1)
            {
                heavyComboIndexer = 0;
            }
            playerAnimator.Play(heavyAttacks[heavyComboIndexer].name);
            heavyComboIndexer += 1;
        }
    }
    

}
