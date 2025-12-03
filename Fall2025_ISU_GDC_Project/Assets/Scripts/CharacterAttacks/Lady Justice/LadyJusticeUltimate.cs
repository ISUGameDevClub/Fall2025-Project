using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LadyJusticeUltimate : MonoBehaviour
{
    public AnimationClip ultimateAnim;
    private Animator playerAnimator;
    public GameObject scale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //UseUltimate();
            //For testing purposes. can be deleted once it works with the input system.
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

        if (pi.actions["Ultimate"].triggered)
        {
            UseUltimate();
        }
    }

    public void UseUltimate()
    {
        playerAnimator.Play(ultimateAnim.name);
        Instantiate(scale,new Vector2(0,-1),Quaternion.identity);
        List<GameObject> allFighters = FindAnyObjectByType<InputConnectionManager>().GetCurrentPlayerObjectsInGame();
        foreach (GameObject fighter in allFighters)
        {
            if (fighter != gameObject)
                fighter.AddComponent<LJ_Ult_Debuff>();
        }
    }

}
