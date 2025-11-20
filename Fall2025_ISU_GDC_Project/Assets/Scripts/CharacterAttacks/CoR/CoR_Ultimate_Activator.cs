using UnityEngine;
using UnityEngine.InputSystem;

public class CoR_Ultimate_Activator : MonoBehaviour
{
    //this class sets up the animation logic for instantiating the SunPrefab after an animation event has been reached

    [SerializeField] private GameObject sunPrefab;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private AnimationClip ultAnimClip;
    
    private Transform sunSpawnLoc;
    private GameObject sunRef; //a stored reference to the sun that this player spawns (for arrow to pathfind to)

    private void Start()
    {
        //look in the scene for a spawn point for the sun
        var sunSpawnLocGo = GameObject.FindWithTag("SunSpawnLocation");
        if (sunSpawnLocGo != null)
        {
            sunSpawnLoc = sunSpawnLocGo.transform;
        }
        else
        {
            Debug.LogError("Could not find SunSpawnLocation for CoR ultimate");
        }
    }

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
            ActivateUltimateAnimation();
        }
    }

    private void ActivateUltimateAnimation()
    {
        PlayerInput pi = this.gameObject.transform.parent.GetComponent<PlayerInput>();
        UltimateTrackerManager ultimateTracker = FindFirstObjectByType<UltimateTrackerManager>();
        if (ultimateTracker.CanPlayerUseUltimate(pi))
        {
            //spawn a sun prefab, that will be interacted with by the animation
            sunRef = Instantiate(sunPrefab, sunSpawnLoc);
            GetComponent<Animator>().Play(ultAnimClip.name);

            ultimateTracker.ResetPlayerUltimateCharge(pi);
        }
    }

    //called from an animation event
    public void ShootArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, this.transform.position, Quaternion.identity);
        arrow.GetComponent<CoR_Ultimate_Arrow>().SetPath(sunSpawnLoc, arrowSpeed, this);
    }

    //called when the arrow hits the sun
    public void StartShootingFireballs()
    {
        sunRef.GetComponent<SunArrowUltimate>().ActivateAttack(transform.parent.gameObject.GetComponent<PlayerInput>());
    }
    
}
