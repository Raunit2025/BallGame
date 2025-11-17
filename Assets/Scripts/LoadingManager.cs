using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    [Header("UI References")]
    public Slider loadingSlider;
    public TextMeshProUGUI progressText; 

    [Header("Settings")]
    public string sceneToLoad = "0_MainMenu"; 
    public float minimumLoadTime = 1.5f; 

    void Start()
    {
        // Make sure the slider starts at 0
        if (loadingSlider != null)
        {
            loadingSlider.value = 0;
        }

        // Start the asynchronous loading coroutine
        StartCoroutine(LoadSceneAsyncCoroutine());
    }

    private IEnumerator LoadSceneAsyncCoroutine()
    {

        // Get the time we started
        float elapsedTime = 0f;

        // Start loading the scene in the background
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);

        // Prevent the new scene from activating as soon as it's ready
        asyncOperation.allowSceneActivation = false;

        float timeProgress = 0f;
        float loadProgress = 0f;

        // Loop until BOTH the minimum time has passed AND the scene is loaded
        while (loadProgress < 1f || timeProgress < 1f)
        {
            // 1. Track the actual load progress
            // (asyncOperation.progress stops at 0.9, so we divide by 0.9 to get a 0.0-1.0 value)
            loadProgress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            // 2. Track the timer progress
            elapsedTime += Time.deltaTime;
            timeProgress = Mathf.Clamp01(elapsedTime / minimumLoadTime);
            
           
            float displayProgress = Mathf.Min(loadProgress, timeProgress);

            UpdateProgressUI(displayProgress);

            yield return null;
        }
        
        
        // Ensure the bar is 100% full before we switch
        UpdateProgressUI(1f);
        
        // A tiny delay just to let the 100% register visually
        yield return new WaitForSeconds(0.1f);

        // We're ready! Allow the new scene to activate and take over.
        asyncOperation.allowSceneActivation = true;
    }

    private void UpdateProgressUI(float progress)
    {
        if (loadingSlider != null)
        {
            loadingSlider.value = progress;
        }
        if (progressText != null)
        {
            // "F0" formats the number as an integer (no decimals)
            progressText.text = (progress * 100f).ToString("F0") + "%";
        }
    }
}