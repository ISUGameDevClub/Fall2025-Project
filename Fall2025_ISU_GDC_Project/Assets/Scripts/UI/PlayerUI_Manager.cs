using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUI_Manager : MonoBehaviour
{
    [SerializeField] private GameObject playerInfo_UI_prefab;
    [SerializeField] private GameObject playerInfo_group; //horizontal layout group

    public void CreateAllPlayerUI()
    {
        List<GameObject> curPlayerInputObjects = FindFirstObjectByType<InputManager>().GetPlayerInputsCurrentlyInGame();

        foreach (GameObject playerInputObj in curPlayerInputObjects) 
        {
            PlayerInput pi = playerInputObj.GetComponent<PlayerInput>();

            //create the player UI, and assign all relevant data
            GameObject playerInfoObj = Instantiate(playerInfo_UI_prefab, playerInfo_group.transform);
            PlayerCharacter pc = FindFirstObjectByType<CharacterSelectManager>().GetCharacterSelectionFromPlayerInput(pi);
            playerInfoObj.GetComponent<PlayerInfo>().AssignPlayerData(pi, pc);
        }
    }
}
