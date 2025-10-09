using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour
{
    [SerializeField] private Image charImage;
    [SerializeField] private TextMeshProUGUI charText;
    [SerializeField] private GameObject selectHighlight;

    private CharacterSelectManager charSelectManager;
    private PlayerCharacter characterForThisButton;

    private void Start()
    {
        charSelectManager = FindFirstObjectByType<CharacterSelectManager>();
    }

    public void UpdateButtonWithPlayerCharacterData(PlayerCharacter pc)
    {
        charImage.sprite = pc.characterSelectIcon;
        charText.text = pc.characterName;
        characterForThisButton = pc;
    }

    public void OnSelectButton(PlayerInput playerInput)
    {
        //make sure we cannot select a character already selected by another player
        //and that this player has not selected someone already
        //if so, ignore logic
        if ( !charSelectManager.IsCharacterSelectedYet(characterForThisButton) && !charSelectManager.HasPlayerSelected(playerInput) )
        {
            charSelectManager.RecordCharacterSelect(playerInput, characterForThisButton);
            selectHighlight.SetActive(true);
            selectHighlight.GetComponent<Image>().color = FindFirstObjectByType<InputConnectionManager>().GetColorFromPlayerInput(playerInput);
            Debug.Log("You selected: " + charText.text);
        }
    }

}
