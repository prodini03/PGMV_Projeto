using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public GameObject hudPanel;
    public Button playButton;
    public Button pauseButton;
    public Button restartButton;
    public PlantManager plantManager;

    void Start()
    {
        playButton.onClick.RemoveAllListeners();
        pauseButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();

        playButton.onClick.AddListener(OnPlayClicked);
        pauseButton.onClick.AddListener(OnPauseClicked);
        restartButton.onClick.AddListener(OnRestartClicked);
    }

    void OnPlayClicked()
    {
        plantManager.Play();
    }

    void OnPauseClicked()
    {
        plantManager.Pause();
    }

    void OnRestartClicked()
    {
        plantManager.Restart();
    }
}