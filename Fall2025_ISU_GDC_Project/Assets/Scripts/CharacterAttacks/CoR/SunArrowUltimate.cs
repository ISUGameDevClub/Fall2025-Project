using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class SunArrowUltimate : MonoBehaviour
{
    [SerializeField] private GameObject sunFireBallPrefab;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private int numberOfShots;
    [SerializeField] private int damagePerShot;
    [SerializeField] private float shotSpeedModifier;

    private bool canFireSunCannons = true;
    private PlayerInput pi;

    private void Update()
    {

        //if (Input.GetKeyDown("m") && canFireSunCannons) //temp debug fire sun cannons
        //{
        //    StartCoroutine("FireSunCannons");
        //    canFireSunCannons = false;
        //}
    }

    public void ActivateAttack(PlayerInput pi)
    {
        this.pi = pi;

        if (canFireSunCannons)
        {
            StartCoroutine("FireSunCannons");
            canFireSunCannons = false;
        }
    }

    private IEnumerator FireSunCannons()
    {
        for (int i = 0; i < numberOfShots; i++)
        {
            //randomly select one cannon, and shoot
            List<Transform> cannons = GetComponentsInChildren<Transform>().Where(x => x.gameObject.tag == "SunCannon").ToList();
            Transform randomCannon = cannons[Random.Range(0, cannons.Count)];
            Vector2 trajectory = randomCannon.gameObject.GetComponent<SunCannon>().GetTrajectory();
            SpawnFireBall(randomCannon, trajectory, shotSpeedModifier);

            //wait a set amount of time before shooting again
            yield return new WaitForSeconds(timeBetweenShots);
        }

        canFireSunCannons = true;
        Destroy(this.gameObject);
    }

    private void SpawnFireBall(Transform spawnLoc, Vector2 fireBallTrajectory, float speedModifier)
    {
        GameObject fireBall = Instantiate(sunFireBallPrefab, spawnLoc.position, Quaternion.identity);
        fireBall.GetComponent<Rigidbody2D>().linearVelocity = fireBallTrajectory * speedModifier;
        fireBall.GetComponent<SunFireball>().InitializeSunFireball(damagePerShot, pi);
    }
}
