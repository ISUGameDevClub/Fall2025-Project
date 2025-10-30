using UnityEditor;
using UnityEngine;

public class FadeScript : MonoBehaviour
{
    //variables 
    private static string fadingMode = "fadeIn"; //"fadeIn" "fadeOut" or "none" 
    public int fadeInDuration; //duration of the fade in, in frames
    private float fadeInPerFrame; //amount of alpha added per frame when fading in
    public int fadeOutDuration; //duration of the fade out, in frames
    private float fadeOutPerFrame; //amount of alpha added per frame when fading out
    private float nextAlpha = 1.0f; //what the alpha will be changed to next frame
    public string sceneToLoadOnPlay = "EmptyGameplayScene"; //scene that will be loaded after script finishes


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fadeOutPerFrame = 1.0f / fadeOutDuration; //sets the amount of alpha removed from the image needed to make the object entirely invisible in duration number of frames
        fadeInPerFrame = 1.0f / fadeInDuration; //sets the amount of alpha added to the image needed to make the object entirely visible in duration number of frames
        //Debug.Log("DEBUG starting script");
    }

    // Update is called once per frame
    void Update()
    {
        if (fadingMode != "none")
        {

            //Debug.Log("DEBUG Fading");

            if (fadingMode == "fadeIn")
            {
                if (nextAlpha > 0.0)
                {
                    nextAlpha -= fadeInPerFrame;
                    GetComponent<UnityEngine.UI.Image>().material.color = new Color(1.0f, 1.0f, 1.0f, nextAlpha);
                    //Debug.Log("DEBUG Decreasing alpha");
                }
                else
                {
                    fadingMode = "none";
                }
            }

            else if (fadingMode == "fadeOut")
            {
                if (nextAlpha < 1.0)
                {
                    nextAlpha += fadeOutPerFrame;
                    GetComponent<UnityEngine.UI.Image>().material.color = new Color(1.0f, 1.0f, 1.0f, nextAlpha);
                    //Debug.Log("DEBUG Increasing alpha");
                }
                else
                {
                    fadingMode = "fadeIn";
                    UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoadOnPlay);
                }
            }

            else
            {
                //Debug.Log("DEBUG" + fadingMode);
            }
        }
    }

    public static void changeFadeMode(string FadeMode)
    {
        fadingMode = FadeMode;
    } 
}
