using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Image fill;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI livesCountText;
    [SerializeField] private TextMeshProUGUI ultimatePercentageText;
    private PlayerInput playerInput;

    public void AssignPlayerData(PlayerInput playerInput, PlayerCharacter playerCharacter)
    {
        this.playerInput = playerInput;

        icon.sprite = playerCharacter.characterSelectIcon;
        if (FindFirstObjectByType<InputConnectionManager>() != null)
        {
            Color color = FindFirstObjectByType<InputConnectionManager>().GetColorFromPlayerInput(playerInput);
            fill.color = color;
        }
        else
        {
            Debug.LogError("Cant find InputConnectionManager to assign color");
        } 
    }

    private void Update()
    {
        //update the UI every frame
        GameObject playerObj = null;

        for (int i = 0; i < playerInput.gameObject.transform.childCount; i++) //get a reference to the active child, which corresponds to the actual playerObj
        {
            if (playerInput.gameObject.transform.GetChild(i).gameObject.activeSelf)
            {
                playerObj = playerInput.gameObject.transform.GetChild(i).gameObject;
            }
        }

        if (playerObj != null)
        {
            if (playerObj.GetComponent<PlayerHealth>() != null)
            {
                PlayerHealth playerHealth = playerObj.GetComponent<PlayerHealth>();

                //update health bar
                healthSlider.maxValue = playerHealth.GetStartingHP();
                healthSlider.minValue = 0;
                healthSlider.value = playerHealth.GetPlayerHealth();

                //update total lives
                livesCountText.text = "Lives: " + playerHealth.GetTotalStocks();

                //update ultimate charge
                float ultPercent = Math.Clamp(FindFirstObjectByType<UltimateTrackerManager>().GetUltimatePercentageForPlayer(playerInput), 0, 100);
                ultimatePercentageText.text = Math.Truncate(ultPercent) + "%";
            }
        }
    }
}
