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

    //public void OnMove(InputAction.CallbackContext context)
    //{
    //    movement = context.ReadValue<Vector2>();
    //}

    private void Update()
    {
        movement = playerInput.actions["Move"].ReadValue<Vector2>();
        this.transform.position += new Vector3(movement.x, movement.y, 0) * cursorSpeed * Time.deltaTime;

        if (playerInput.actions["Jump"].triggered) 
        {
            //Debug.Log("click");

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
                        result.gameObject.GetComponent<Button>().onClick.Invoke();
                    }
                }
            }
        }
    }
}
