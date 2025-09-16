using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour
{
    [SerializeField] private Image charImage;
    [SerializeField] private TextMeshProUGUI charText;
    [SerializeField] private GameObject selectHighlight;
    private PlayerInput playerInputForThisButton;

    public void UpdateButtonWithPlayerCharacterData(PlayerCharacter pc)
    {
        charImage.sprite = pc.characterSelectIcon;
        charText.text = pc.characterName;
    }

    public void OnSelectButton()
    {
        selectHighlight.SetActive(true);
        Debug.Log("You selected: "+charText.text);
    }

}
