using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class CharacterSelectManager : MonoBehaviour
{
    [SerializeField] private List<PlayerCharacter> playerCharacterList;
    [SerializeField] private GameObject charSelectGroup;
    [SerializeField] private GameObject charSelectButtonPrefab;
    

    private void Start()
    {
        PopulateCharacterSelectData();
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
