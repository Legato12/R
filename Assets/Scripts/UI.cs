using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// UI manager for handling panels, volume and scene transitions.
public class UI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject panel;
    public Slider volumeSlider;
    public Text volumeText; // Text component for volume value display

    // Scene names
    private const string GameScene = "GameScene";
    private const string MenuScene = "MenuScene";

    void Start()
    {
        // Initialize slider and text if not set in Inspector
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(SetVolume);
            SetVolume(volumeSlider.value); // Initial update
        }
        else
        {
            Debug.LogWarning("UI: volumeSlider is not assigned!");
        }
    }

    void Update()
    {
        HandleEscape();
    }

    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            string currentScene = SceneManager.GetActiveScene().name;
            if (currentScene == GameScene)
            {
                SceneManager.LoadScene(MenuScene);
            }
            else if (currentScene == MenuScene)
            {
                Application.Quit();
            }
        }
    }

    /// Loads the Game scene.
    public void StartScene()
    {
        SceneManager.LoadScene(GameScene);
    }

    /// Quits the application.
    public void Quit()
    {
        Application.Quit();
    }

    /// Toggles the settings panel.
    public void TogglePanel()
    {
        if (panel != null)
            panel.SetActive(!panel.activeSelf);
    }

    /// Sets the audio volume based on slider value.
    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        UpdateVolumeText(value);
    }

    /// Updates the volume text display.
    private void UpdateVolumeText(float value)
    {
        if (volumeText != null)
        {
            volumeText.text = "Volume " + Mathf.RoundToInt(value * 100) + "%";
        }
    }
}
