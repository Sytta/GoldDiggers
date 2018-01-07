using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;

	// Use this for initialization
	void Start()
	{
        playButton.onClick.AddListener(OnPlayClicked);
        exitButton.onClick.AddListener(OnExitClicked);
	}

	void OnPlayClicked()
    {
        Application.LoadLevel("BaseScene");
    }

    void OnExitClicked()
    {
        Application.Quit();
    }
}
