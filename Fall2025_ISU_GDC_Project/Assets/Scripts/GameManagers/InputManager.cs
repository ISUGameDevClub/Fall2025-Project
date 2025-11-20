using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //This class is used to store the literal PlayerInputs components that are active in the game

    [SerializeField] private List<GameObject> curPlayerInputGameObjects;
    [SerializeField] private List<PlayerInput> curPlayerInputs;

    private void Start()
    {
        GameSequenceManager.RegisterSequenceChangeCallback(GameSequenceManager.Sequence.Battle, OnBattleBegin);
    }

    private void InitPlayerInputObjects()
	{
        //Each player has made their character selection by now
        //We need to add a prefab instance of the PlayerVariant that corresponds to their character choice
        var charSelectManager = FindFirstObjectByType<CharacterSelectManager>();
        PlayerInput[] inputList = new PlayerInput[ curPlayerInputGameObjects.Count ];
        for (int i = 0; i < curPlayerInputGameObjects.Count; i++)
        {
            GameObject playerObj = curPlayerInputGameObjects[i];

            var template = playerObj.GetComponentInParent<PlayerSpawningTemplate>();
            PlayerCharacter playerCharacter = charSelectManager.GetCharacterSelectionFromPlayerInput(playerObj.GetComponent<PlayerInput>());
            template.EnablePlayerObjectFromPlayerCharacter(playerCharacter);

            inputList[i] = playerObj.GetComponent<PlayerInput>();
        }
        FindFirstObjectByType<UltimateTrackerManager>().InitializeDictionary(inputList);
	}
    
    private void OnBattleBegin()
    {
        Debug.LogFormat( "Initializing player input..." );
        InitPlayerInputObjects();

		//create player info UI
		//FindFirstObjectByType<PlayerUI_Manager>()?.CreateAllPlayerUI();
	}

    public void PlayerJoined(PlayerInput pi)
    {
        curPlayerInputGameObjects.Add(pi.gameObject);
        curPlayerInputs.Add(pi);
    }

    public void PlayerLeft(PlayerInput pi)
    {
        curPlayerInputGameObjects.Remove(pi.gameObject);
        curPlayerInputs.Remove(pi);
    }

    public List<GameObject> GetPlayerInputsCurrentlyInGame()
    {
        return curPlayerInputGameObjects;
    }
}
