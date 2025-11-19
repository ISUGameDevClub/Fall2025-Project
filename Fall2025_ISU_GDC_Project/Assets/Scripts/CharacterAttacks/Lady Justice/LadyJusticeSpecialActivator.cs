using UnityEngine;
using UnityEngine.InputSystem;

public class LadyJusticeSpecialActivator : MonoBehaviour
{
    [SerializeField] AnimationClip petrifyClip;

    private void Update()
    {
        bool activatedAttackThisFrame = transform.root.gameObject.GetComponent<PlayerInput>().actions["Special"].triggered;

        if(activatedAttackThisFrame)
        {
            PlayPetrifyAnimation();
        }
    }

    private void PlayPetrifyAnimation()
    {
        if (petrifyClip != null)
            GetComponent<Animator>().Play(petrifyClip.name);
    }
}
