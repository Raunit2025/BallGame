using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RewardAdButton : MonoBehaviour
{
    private Button adButton;
    private GameManager gameManager;

    void Start()
    {
        adButton = GetComponent<Button>();
        adButton.onClick.AddListener(ShowRewardedAd);
        gameManager = GameManager.Instance;
    }

    void ShowRewardedAd()
    {
        adButton.interactable = false; 

        AdsManager.Instance.ShowRewardedAd(OnRewardSuccess);
    }

    void OnRewardSuccess()
    {
        Debug.Log("AdButton: Reward callback received.");
        
        if (gameManager != null)
        {
            gameManager.RewardPlayerWithBalls(); 
        }

        adButton.interactable = true; 
    }
}