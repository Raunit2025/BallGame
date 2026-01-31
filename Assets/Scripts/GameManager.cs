using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Button References")]
    public Button buyBallsButton; 

    [Header("UI Panels")]
    public GameObject outOfBallsPanel; 

    [Header("Game Variables")]
    public int startingBallCount = 15;
    public int ballsToAddForReward = 15;
    public int ballsToAddForIAP = 100;
    
    [Header("UI Text References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gemsText;
    public TextMeshProUGUI ballsRemainingText;
    public Slider gemProgressBar;
    public TextMeshProUGUI gemProgressText;
    private int currentLevelIndex;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        IAPManager.OnPurchaseSuccessful += RewardPlayerWithBalls_IAP;
    }

    void OnDisable()
    {
        IAPManager.OnPurchaseSuccessful -= RewardPlayerWithBalls_IAP;
    }

    void Start()
    {
        PayoutTimerManager.CheckAndInitializeTimer();
        PayoutTimerManager.CheckForTimerExpiry();

        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        if (outOfBallsPanel != null) outOfBallsPanel.SetActive(false); 
        
        Time.timeScale = 1f; 
        if(GameData.BallsRemaining == -1)
        {
            GameData.ResetData(startingBallCount);
        }
        
        UpdateScoreDisplay(); 

        if (buyBallsButton != null)
        {
            buyBallsButton.onClick.AddListener(OnBuyBallsClicked);
        }
    }
    
    public void OnBuyBallsClicked()
    {
        if (IAPManager.Instance != null)
        {
            IAPManager.Instance.Buy100Balls();
        }
    }

    public void OnWatchAdButton()
    {
        if (AdsManager.Instance != null)
        {
            AdsManager.Instance.ShowRewardedAd(RewardPlayerWithBalls);
        }
        else
        {
            Debug.LogError("AdsManager is not initialized!");
        }
    }

    public void RewardPlayerWithBalls()
    {
        StopAllCoroutines();
        GameData.BallsRemaining += ballsToAddForReward;
        if (outOfBallsPanel != null) outOfBallsPanel.SetActive(false);
        UpdateScoreDisplay();
    }
    
    public void RewardPlayerWithBalls_IAP(int quantity)
    {
        StopAllCoroutines();
        int totalBallsToAdd = ballsToAddForIAP * quantity;
        GameData.BallsRemaining += totalBallsToAdd;
        
        Debug.Log($"GameManager: Added {totalBallsToAdd} balls (Quantity: {quantity})");

        if (outOfBallsPanel != null) outOfBallsPanel.SetActive(false);
        UpdateScoreDisplay();
    }
    
    public bool UseBall()
    {
        if (GameData.BallsRemaining > 0)
        {
            GameData.BallsRemaining--;
            UpdateScoreDisplay();
            return true;
        }
        else
        {
            ShowOutOfBallsPanel();
            return false;
        }
    }

    public void OnBallDestroyed()
    {
        if (GameData.BallsRemaining <= 0)
        {
            StartCoroutine(CheckForLastBallCoroutine());
        }
    }

    private IEnumerator CheckForLastBallCoroutine()
    {
        yield return new WaitForEndOfFrame();
        if (FindObjectsOfType<BallController>().Length == 0 && GameData.BallsRemaining <= 0)
        {
            ShowOutOfBallsPanel();
        }
    }

    public void AddScore(int amount)
    {
        GameData.CurrentScore += amount;
        UpdateScoreDisplay();
    }

    public void AddGems(float amount)
    {
        PayoutTimerManager.AddGems(amount);
        UpdateScoreDisplay();
    }

    public void ShowOutOfBallsPanel()
    {
        if (outOfBallsPanel != null && !outOfBallsPanel.activeSelf)
        {
            StopAllBalls();
            outOfBallsPanel.SetActive(true);
        }
    }
    
    public void UpdateScoreDisplay()
    {
        float currentGems = PayoutTimerManager.GetPlayerGems();
        float gemRedeemGoal = PayoutTimerManager.GemRedeemGoal;
        
        if (scoreText != null) scoreText.text = GameData.CurrentScore.ToString();
        if (gemsText != null) gemsText.text = currentGems.ToString("F1");
        if (ballsRemainingText != null) ballsRemainingText.text = GameData.BallsRemaining.ToString();

        if (gemProgressText != null)
        {
            gemProgressText.text = $"{currentGems:F1} / {gemRedeemGoal:F1}"; // Use F1 for float
        }
        
        if(gemProgressBar != null)
        {
            float progress = 0f; 
            if (gemRedeemGoal > 0)
            {
                progress = currentGems / gemRedeemGoal;
            }
            else if (currentGems > 0)
            {
                progress = 1f; 
            }
            gemProgressBar.value = Mathf.Clamp01(progress); 
        }
    }
    
    private void StopAllBalls()
    {
        BallController[] allBalls = FindObjectsOfType<BallController>();
        foreach (BallController ball in allBalls)
        {
            ball.SetGameOver();
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1); 
    }

    public void GoToPaymentPage()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(4);
    }
}