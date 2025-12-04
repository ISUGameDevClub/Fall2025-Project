using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectMenu;
    [SerializeField] private GameObject creditsMenu;

    [SerializeField] private AudioSource mainMenuAudioSource;
    [SerializeField] private AudioClip creditsSong;
    [SerializeField] private AudioClip mainMenuSong;

    private void Start()
    {
        levelSelectMenu.SetActive(false);
        mainMenuAudioSource.clip = mainMenuSong;
        mainMenuAudioSource.Play();
    }

    //called by button OnClick
    public void OpenLevelSelectMenu()
    {
        levelSelectMenu.SetActive(true);
    }

    public void OpenCreditsMenu()
    {
        creditsMenu.SetActive(true);
        mainMenuAudioSource.clip = creditsSong;
        mainMenuAudioSource.Play();
    }

    public void CloseCreditsMenu()
    {
        creditsMenu.SetActive(false);
        mainMenuAudioSource.clip = mainMenuSong;
        mainMenuAudioSource.Play();
    }
}
