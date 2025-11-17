using UnityEngine;
using TMPro;

public class BonusBoxController : MonoBehaviour
{
    public enum RewardType { Star, Gem }
    [Header("Reward Settings")]
    public RewardType rewardType = RewardType.Star;
    public int amount = 10;
    [Header("References")]
    public TextMeshProUGUI displayText;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        if (displayText != null)
        {
            displayText.text = $"+{amount}";
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && gameManager != null)
        {
            switch (rewardType)
            {
                case RewardType.Star:
                    gameManager.AddScore(amount);
                    break;
                case RewardType.Gem:
                    gameManager.AddGems(amount);
                    break;
            }
            // Just destroy the ball.
            Destroy(other.gameObject);
            gameManager.OnBallDestroyed();
        }
    }
}