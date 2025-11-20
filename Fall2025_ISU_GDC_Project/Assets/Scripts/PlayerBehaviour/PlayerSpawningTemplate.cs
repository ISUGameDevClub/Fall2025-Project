using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawningTemplate : MonoBehaviour
{
    /*This script and GameObject is what will be spawned when a player connects their input device to the game.
     * When its time to spawn the actual players, this script will act as a template for logic to choose which Variant will be spawned
     */

    [SerializeField] private List<GameObject> validPlayerObjectsToSpawn;
    [SerializeField] private PlayerCharacter defaultCharacter;


    public void SpawnPlayerObjectFromPlayerCharacter(PlayerCharacter playerCharacter)
    {
        GameObject playerObjToSpawn = null;
        foreach (GameObject obj in validPlayerObjectsToSpawn)
        {
            if (obj.GetComponent<PlayerVariantData>().GetPlayerCharacter() == playerCharacter)
            {
                playerObjToSpawn = obj;
            }
        }

        Instantiate(playerObjToSpawn, this.transform);
    }

    public void EnablePlayerObjectFromPlayerCharacter(PlayerCharacter playerCharacter)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject childObj = transform.GetChild(i).gameObject;
            
            //disable the default player obj
            if (childObj.GetComponent<PlayerVariantData>().GetPlayerCharacter() == defaultCharacter)
            {
                childObj.SetActive(false);
            }

            //enable the chosen player obj
            if (childObj.GetComponent<PlayerVariantData>().GetPlayerCharacter() == playerCharacter)
            {
                childObj.GetComponent<SpriteRenderer>().color = FindFirstObjectByType<InputConnectionManager>().GetColorFromPlayerInput(GetComponent<PlayerInput>());
                childObj.SetActive(true);
                FindFirstObjectByType<InputConnectionManager>().realPlayers.Add(childObj);
            }
        }

       
    }
}
