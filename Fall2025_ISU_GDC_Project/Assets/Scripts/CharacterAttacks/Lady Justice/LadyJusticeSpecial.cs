using System.Security.Cryptography;
using UnityEngine;

public class LadyJusticeSpecialScript : MonoBehaviour
{
    public float timer;
    public GameObject LadyJusticeSpecialBackground;

    bool mPressed = false;

    private float startTime;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = timer;
    }

    // Update is called once per frame
    void Update()
    {
        bool isActive = LadyJusticeSpecialBackground.activeSelf;
        mPressed = Input.GetKey(KeyCode.M);
        

        if (mPressed == true)
        {
            timer = startTime;
            LadyJusticeSpecialBackground.gameObject.SetActive(true);
        }

        if (isActive == true)
        {
            timer = timer - Time.deltaTime;
            if (timer <= 0)
            {
                LadyJusticeSpecialBackground.gameObject.SetActive(false);
            }
        }
    }
        

        
}
