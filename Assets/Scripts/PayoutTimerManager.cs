using System;
using UnityEngine;


public static class PayoutTimerManager
{
    public const string PayoutEndTimeKey = "PayoutEndTime";
    public const string GemsKey = "PlayerGems";
    
    public const float PayoutCycleHours = 3.0f;       
    public const float GemRedeemGoal = 10000.0f; 


    public static void CheckAndInitializeTimer()
    {
        if (!PlayerPrefs.HasKey(PayoutEndTimeKey))
        {
            Debug.Log("No timer found. Creating new 3-hour timer.");
            ResetTimer();
        }
    }

 
    public static bool CheckForTimerExpiry()
    {
        DateTime endTime = GetTimerEndTime();
        
        if (DateTime.UtcNow >= endTime)
        {
            Debug.Log("Payout timer expired! Resetting gems and timer.");
            
            ResetGems();
            
            ResetTimer();
            
            return true; 
        }

        return false; 
    }

   
    public static void ResetTimer()
    {
        DateTime newEndTime = DateTime.UtcNow.AddHours(PayoutCycleHours);
        PlayerPrefs.SetString(PayoutEndTimeKey, newEndTime.ToBinary().ToString());
        PlayerPrefs.Save();
        Debug.Log("Timer has been reset. Next payout in 3 hours.");
    }

   
    public static void ResetGems()
    {
        PlayerPrefs.SetFloat(GemsKey, 0f);
        PlayerPrefs.Save();
        Debug.Log("Player gems have been reset to 0.");
    }

 
    public static DateTime GetTimerEndTime()
    {
        string endTimeString = PlayerPrefs.GetString(PayoutEndTimeKey, "");
        
        if (string.IsNullOrEmpty(endTimeString))
        {
          
            Debug.LogWarning("Timer end time not set! Resetting timer.");
            ResetTimer();
            endTimeString = PlayerPrefs.GetString(PayoutEndTimeKey);
        }

        return DateTime.FromBinary(Convert.ToInt64(endTimeString));
    }

  
    public static TimeSpan GetRemainingTimeSpan()
    {
        DateTime endTime = GetTimerEndTime();
        return endTime - DateTime.UtcNow;
    }

    
    public static float GetPlayerGems()
    {
        return PlayerPrefs.GetFloat(GemsKey, 0f);
    }

       public static void AddGems(float amount)
    {
       
        CheckForTimerExpiry(); 
        
        float newGems = GetPlayerGems() + amount;
        PlayerPrefs.SetFloat(GemsKey, newGems);
    }

   
    public static void ProcessSuccessfulRedemption()
    {
        float currentGems = GetPlayerGems();
        currentGems -= GemRedeemGoal; 
        
       
        if (currentGems < 0) { currentGems = 0; } 
        
        PlayerPrefs.SetFloat(GemsKey, currentGems);
        
       
        ResetTimer();
        
        PlayerPrefs.Save();
    }
}