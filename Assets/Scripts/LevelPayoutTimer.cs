using System;
using UnityEngine;
using TMPro;

public class LevelPayoutTimer : MonoBehaviour
{
    // Drag your new timer TextMeshPro object here
    public TextMeshProUGUI timerText;
    
    void Start()
    {
        // 1. Make sure the global timer is initialized
        PayoutTimerManager.CheckAndInitializeTimer();
        
        // 2. Check if the timer expired while this scene was not loaded
        // This will reset gems if needed.
        PayoutTimerManager.CheckForTimerExpiry();
    }

    void Update()
    {
        if (timerText == null)
            return;

        // 3. Check for expiry *every frame*
        // This ensures the reset happens instantly when the timer hits zero.
        PayoutTimerManager.CheckForTimerExpiry();

        // 4. Get the remaining time
        TimeSpan remainingTime = PayoutTimerManager.GetRemainingTimeSpan();

        if (remainingTime.TotalSeconds > 0)
        {
            // Format the timer as HH:MM:SS
            timerText.text = string.Format("Next Payout: {0:D2}:{1:D2}:{2:D2}",
                                           remainingTime.Hours,
                                           remainingTime.Minutes,
                                           remainingTime.Seconds);
        }
        else
        {
            // This should only appear for a single frame before the timer resets
            timerText.text = "Calculating..."; 
        }
    }
}