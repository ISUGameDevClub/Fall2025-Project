using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SphinxUltimate : MonoBehaviour
{
    public AnimationClip ultimateAnim;
    private Animator playerAnimator;
    public GameObject sphinxStatue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
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
            ActivateUltimate();
        }
    }

    public void InflictDebuffs()
    {
        playerAnimator.Play(ultimateAnim.name);
        Instantiate(sphinxStatue, new Vector2(0, -1), Quaternion.identity);
        List<GameObject> allFighters = FindAnyObjectByType<InputConnectionManager>().GetCurrentPlayerObjectsInGame();
        foreach (GameObject fighter in allFighters)
        {
            if (fighter != gameObject)
                fighter.AddComponent<SphinxUlt_Debuff>();
        }
    }

    private void ActivateUltimate()
    {
        SoundManager.PlaySound("Sound/SFX/Combat/Sphinx/Sphinx Sandstorm SFX", 1f, false);

        //enable ultimate attack animation
        if (GetComponent<Animator>() != null)
        {
            PlayerInput pi = this.gameObject.transform.parent.GetComponent<PlayerInput>();
            UltimateTrackerManager ultimateTracker = FindFirstObjectByType<UltimateTrackerManager>();
            if (ultimateTracker.CanPlayerUseUltimate(pi))
            {
                //SoundManager.PlaySound("Sound/SFX/Combat/WhooshSFX_02", 1.0f, false);

                InflictDebuffs();
                //playerAnimator.Play(ultimateAnim.name);
                //StartCoroutine("DisableBeanMovementRoutine");
                ultimateTracker.ResetPlayerUltimateCharge(pi);
            }

            //SoundManager.PlaySound("Sound/SFX/Combat/Bean/Beam Laser_mixdown", .5f, false);
        }
    }

}
