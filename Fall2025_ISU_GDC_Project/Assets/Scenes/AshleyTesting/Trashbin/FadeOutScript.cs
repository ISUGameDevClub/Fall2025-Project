using UnityEngine;

public class FadeOutScript : MonoBehaviour
{
    //variables 
    public int duration; //duration of the fade in, in frames
    private float fadePerFrame; //amount of alpha added per frame
    private float nextAlpha = 0.0f; //what the alpha will be changed to next frame
    public string sceneToLoadOnPlay = "EmptyGameplayScene"; //scene that will be loaded after script finishes


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fadePerFrame = 1.0f / duration; //sets the amount of alpha added to the amount needed to make object entirely visible in duration number of frames
    }

    // Update is called once per frame
    void Update()
    {
        if (nextAlpha < 1.0)
        {
            nextAlpha += fadePerFrame;
            GetComponent<UnityEngine.UI.Image>().material.color = new Color(1.0f, 1.0f, 1.0f, nextAlpha);
            Debug.Log("DEBUG Increasing alpha");
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoadOnPlay);
        }
    }
}
