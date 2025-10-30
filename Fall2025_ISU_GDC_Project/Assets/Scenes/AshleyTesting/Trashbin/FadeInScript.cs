using UnityEngine;

public class FadeInScript : MonoBehaviour
{

    //variables 
    public int duration; //duration of the fade in, in frames
    private float fadePerFrame; //amount of alpha removed per frame
    private float nextAlpha = 1.0f; //what the alpha will be changed to next frame


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fadePerFrame = 1.0f / duration; //sets the amount of alpha lost to the amount needed to make object entirelt transparent in duration number of frames
    }

    // Update is called once per frame
    void Update()
    {
        if (nextAlpha > 0.0) 
        {
            nextAlpha -= fadePerFrame;
            GetComponent<UnityEngine.UI.Image>().material.color = new Color(1.0f, 1.0f, 1.0f, nextAlpha); 
            Debug.Log("DEBUG Decreasing alpha");
        }
    }
}
