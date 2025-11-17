using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using UnityEngine.Networking;
using System.Text;

public class PaymentPageManager : MonoBehaviour
{
    [Header("Goal Card References")]
    public TextMeshProUGUI totalGemsText;
    public TextMeshProUGUI timerText;
    public Slider progressBar;
    public TextMeshProUGUI progressText;

    [Header("Redeem Card References")]
    public TextMeshProUGUI redeemableAmountText;
    public Button redeemButton;
    public TMP_InputField emailInputField;
    public TextMeshProUGUI statusText;

    [Header("Settings")]
    public float gemToDollarRate = 0.01f; 

    [Header("Server Settings")]
    public string payoutServerUrl = "https://ballgainplinkoserver.onrender.com";

    private bool isRedeeming = false;

    void Start()
    {
        Time.timeScale = 1f; 
        
        if (progressBar != null)
        {
            progressBar.interactable = false;
        }

        PayoutTimerManager.CheckAndInitializeTimer();
        PayoutTimerManager.CheckForTimerExpiry();
        
        StartCoroutine(UpdateTimer());
        
        UpdateUI();
    }

    void UpdateUI()
    {
        float currentGems = PayoutTimerManager.GetPlayerGems();
        float redeemGoal = PayoutTimerManager.GemRedeemGoal;

        totalGemsText.text = "Total Gems: " + currentGems.ToString("F1");
        
        float progress = 0f;
        if (redeemGoal > 0) {
            progress = Mathf.Clamp01(currentGems / redeemGoal);
        }
        
        progressBar.value = progress;
        progressText.text = $"{currentGems:F1} / {redeemGoal:F1}";

        float potentialAmount = currentGems * gemToDollarRate;
        redeemableAmountText.text = $"You can redeem: A${potentialAmount:F2}";

        redeemButton.interactable = (currentGems >= redeemGoal) && !isRedeeming;
    }

    IEnumerator UpdateTimer()
    {
        while (true)
        {
            bool didReset = PayoutTimerManager.CheckForTimerExpiry();
            
            if(didReset)
            {
                UpdateUI();
            }
            
            TimeSpan remainingTime = PayoutTimerManager.GetRemainingTimeSpan();

            if (remainingTime.TotalSeconds > 0)
            {
                timerText.text = string.Format("Next Payout in: {0:D2}:{1:D2}:{2:D2}", 
                                                remainingTime.Hours, 
                                                remainingTime.Minutes, 
                                                remainingTime.Seconds);
            }
            else
            {
                timerText.text = "Calculating...";
            }
            
            yield return new WaitForSeconds(1.0f); 
        }
    }

    public void OnRedeemClicked()
    {
        if (isRedeeming) return;

        if (string.IsNullOrEmpty(emailInputField.text))
        {
            Debug.LogError("Email field is empty.");
            if (statusText != null) statusText.text = "Please enter your PayPal email.";
            return;
        }

        if (PayoutTimerManager.GetPlayerGems() < PayoutTimerManager.GemRedeemGoal)
        {
            Debug.LogError("Not enough gems.");
            if (statusText != null) statusText.text = "You do not have enough gems.";
            return;
        }

        StartCoroutine(AttemptRedemption());
    }

    private IEnumerator AttemptRedemption()
    {
        isRedeeming = true;
        redeemButton.interactable = false;
        if (statusText != null) statusText.text = "Processing...";

        float amountToRedeem = PayoutTimerManager.GemRedeemGoal * gemToDollarRate; 
        
        PayoutRequestData data = new PayoutRequestData
        {
            email = emailInputField.text,
            amount = amountToRedeem
        };
        string jsonPayload = JsonUtility.ToJson(data);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);

        UnityWebRequest request = new UnityWebRequest(payoutServerUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Payout request successful! Server response: " + request.downloadHandler.text);
            
            PayoutTimerManager.ProcessSuccessfulRedemption();
            
            if (statusText != null) statusText.text = "Redemption Successful!";
            UpdateUI();
        }
        else
        {
            Debug.LogError("Payout request failed! Error: " + request.error);
            Debug.LogError("Server response body: " + request.downloadHandler.text);
            if (statusText != null) statusText.text = "Error. Please try again later.";
        }

        isRedeeming = false;
        UpdateUI();
    }


    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene("1_Level");
    }
}

[System.Serializable]
public class PayoutRequestData
{
    public string email;
    public float amount;
}