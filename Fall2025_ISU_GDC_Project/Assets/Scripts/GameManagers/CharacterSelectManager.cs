using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelectManager : MonoBehaviour
{
    [SerializeField] private List<PlayerCharacter> playerCharacterList;
    [SerializeField] private GameObject charSelectGroup;
    [SerializeField] private GameObject charSelectButtonPrefab;
    

    //main dict for storing which players select which characters | PlayerInput -> PlayerCharacter
    private Dictionary<PlayerInput, PlayerCharacter> currentlySelectedChars = new Dictionary<PlayerInput, PlayerCharacter>();

    private void Start()
    {
        PopulateCharacterSelectData();
    }

    public void RecordCharacterSelect(PlayerInput playerInput, PlayerCharacter character)
    {
        currentlySelectedChars.Add(playerInput, character);
    }

    public void RemoveCharacterSelect(PlayerInput playerInput)
    {
        currentlySelectedChars.Remove(playerInput);
    }

    public bool IsCharacterSelectedYet(PlayerCharacter character)
    {
        return currentlySelectedChars.ContainsValue(character);
    }

    public bool HasPlayerSelected(PlayerInput playerInput)
    {
        return currentlySelectedChars.ContainsKey(playerInput);
    }

    public PlayerCharacter GetCharacterSelectionFromPlayerInput(PlayerInput playerInput)
    {
        return currentlySelectedChars[playerInput];
    }

    public List<PlayerCharacter> getPlayerCharacterList()
    {
        return playerCharacterList;
    }


    //fills out the menu for all possible characters to pick in the game
    private void PopulateCharacterSelectData()
    {
        foreach (PlayerCharacter character in playerCharacterList)
        {
            var selectButton = Instantiate(charSelectButtonPrefab, charSelectGroup.transform);
            selectButton.GetComponent<CharacterSelectButton>().UpdateButtonWithPlayerCharacterData(character);
        }
    }
}
