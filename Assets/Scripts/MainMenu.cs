using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Settings Popup")]
    public GameObject settingsPanel;
    public Button muteButton;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

    [Header("Info Popup")]
    public GameObject termsPanel;
    public int startingBallCountForReset = 15;


    public void StartNewGame()
    {
        GameData.ResetData(startingBallCountForReset);

        SceneManager.LoadScene(2);
    }
    void Start()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        if (termsPanel != null)
        {
            termsPanel.SetActive(false);
        }

        UpdateMuteButtonIcon();
    }


    public void LoadGame()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT GAME");
        Application.Quit();
    }



    public void OnSettingsButton()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }
    }

    public void OnMuteButton()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ToggleMusic();
            UpdateMuteButtonIcon();
        }
    }

    private void UpdateMuteButtonIcon()
    {
        if (AudioManager.Instance == null || muteButton == null)
        {
            return;
        }

        if (AudioManager.Instance.IsMuted())
        {
            muteButton.image.sprite = musicOffSprite;
        }
        else
        {
            muteButton.image.sprite = musicOnSprite;
        }
    }


    public void OnInfoButton()
    {
        if (termsPanel != null)
        {
            termsPanel.SetActive(true);
        }
    }

    public void OnCloseTermsButton()
    {
        if (termsPanel != null)
        {
            termsPanel.SetActive(false);
        }
    }
}