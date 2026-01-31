using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MobileDebugger : MonoBehaviour
{
    public static MobileDebugger Instance;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject panelObject; // The actual panel to hide/show

    [Header("Settings")]
    [SerializeField] private bool showOnStart = true;
    [SerializeField] private int maxChars = 5000;

    private string accumulatedLog = "";

    private void Awake()
    {
        // Singleton pattern to keep this alive across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (panelObject != null)
            panelObject.SetActive(showOnStart);
    }

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    // This function runs automatically whenever ANY script calls Debug.Log
    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        string color = "white";
        string prefix = "";

        switch (type)
        {
            case LogType.Error:
            case LogType.Exception:
                color = "red";
                prefix = "[ERROR] ";
                break;
            case LogType.Warning:
                color = "yellow";
                prefix = "[WARN] ";
                break;
            case LogType.Log:
                color = "#00FF00"; // Greenish for standard logs
                prefix = "[LOG] ";
                break;
        }

        // Add Timestamp
        string time = System.DateTime.Now.ToString("HH:mm:ss");
        string newEntry = $"<color={color}><b>{time} {prefix}</b> {logString}</color>\n";

        accumulatedLog += newEntry;

        // Prevent memory overflow
        if (accumulatedLog.Length > maxChars)
        {
            accumulatedLog = accumulatedLog.Substring(accumulatedLog.Length - maxChars);
        }

        if (debugText != null)
        {
            debugText.text = accumulatedLog;
            
            // Auto-scroll to bottom
            if(scrollRect != null)
                Canvas.ForceUpdateCanvases();
                scrollRect.verticalNormalizedPosition = 0f; 
        }
    }

    // Call this from a UI button to toggle the log view
    public void ToggleConsole()
    {
        if (panelObject != null)
            panelObject.SetActive(!panelObject.activeSelf);
    }

    public void ClearLogs()
    {
        accumulatedLog = "";
        if (debugText != null) debugText.text = "";
    }
}