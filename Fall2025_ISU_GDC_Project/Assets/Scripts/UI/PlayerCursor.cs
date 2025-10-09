using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

public class PlayerCursor : MonoBehaviour
{
    private PlayerInput playerInput;
    [SerializeField] private float cursorSpeed;
    [SerializeField] private Image cursorImg;
    private Vector2 movement;


    public void AssignPlayerInput(PlayerInput playerInput)
    {
        this.playerInput = playerInput;
    }

    public void AssignColor(Color color)
    {
        cursorImg.color = color;
    }

    private void Update()
    {
        movement = playerInput.actions["Move"].ReadValue<Vector2>();
        this.transform.position += new Vector3(movement.x, movement.y, 0) * cursorSpeed * Time.deltaTime;

        //UI cursor interaction is handled with "Jump" button for now (should be changed later for code clarity)
        if (playerInput.actions["Jump"].triggered) 
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = this.transform.position; 

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if(results.Count > 0)
            {
                foreach(RaycastResult result in results)
                {
                    //Debug.Log("Hit UI Element: " + result.gameObject.name);
                    if (result.gameObject.GetComponent<Button>() != null)
                    {
                        //we hit a button
                        result.gameObject.GetComponent<Button>().onClick.Invoke();

                        if ( result.gameObject.GetComponent<CharacterSelectButton>() != null )
                        {
                            //if this is a character select button, apply extra logic to its CharacterSelectButton component with playerInput
                            result.gameObject.GetComponent<CharacterSelectButton>().OnSelectButton(playerInput);
                        }
                    }
                }
            }
        }
    }
}
