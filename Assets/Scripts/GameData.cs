// In Assets/Scripts/GameData.cs
using UnityEngine;

public static class GameData
{
    // These are the "keys" we use to store the data
    private const string BallsKey = "PlayerBalls";
    private const string ScoreKey = "PlayerScore";

    // We use -1 to show that the data hasn't been loaded from storage yet
    private static int ballsRemaining = -1;
    private static int currentScore = -1;

    // This is the "getter" and "setter" for BallsRemaining
    public static int BallsRemaining
    {
        get
        {
            // If ballsRemaining is -1, it means we need to load it from storage
            if (ballsRemaining == -1)
            {
                // Load the value from PlayerPrefs. If it doesn't exist, set it to -1.
                ballsRemaining = PlayerPrefs.GetInt(BallsKey, -1);
            }
            return ballsRemaining;
        }
        set
        {
            // When we set the value, also save it straight to PlayerPrefs
            ballsRemaining = value;
            PlayerPrefs.SetInt(BallsKey, ballsRemaining);
        }
    }

    // This is the "getter" and "setter" for CurrentScore
    public static int CurrentScore
    {
        get
        {
            // If currentScore is -1, it means we need to load it from storage
            if (currentScore == -1)
            {
                // Load the value from PlayerPrefs. If it doesn't exist, set it to 0.
                currentScore = PlayerPrefs.GetInt(ScoreKey, 0);
            }
            return currentScore;
        }
        set
        {
            // When we set the value, also save it straight to PlayerPrefs
            currentScore = value;
            PlayerPrefs.SetInt(ScoreKey, currentScore);
        }
    }

    // --- NEW FUNCTION ---
    // This is a special function to reset the data for a "New Game"
    public static void ResetData(int startingBalls)
    {
        BallsRemaining = startingBalls;
        CurrentScore = 0;
    }
}