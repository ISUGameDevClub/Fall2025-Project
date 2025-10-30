using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour {
	public Button playButton;

	[SerializeField] public string sceneToLoadOnPlay = "EmptyScene";

	

	void Start () {
		

		Button btn = playButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
        FadeScript.changeFadeMode("fadeOut");
	}

}