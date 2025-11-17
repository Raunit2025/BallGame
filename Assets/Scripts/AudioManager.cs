using UnityEngine;

// Add 'RequireComponent' to ensure there is always an AudioSource
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    // --- 1. Singleton Pattern ---
    public static AudioManager Instance;

    private AudioSource backgroundMusic;
    private bool isMuted = false;
    private const string MutePrefKey = "IsMusicMuted"; // Key for PlayerPrefs

    void Awake()
    {
        // --- Start Singleton Setup ---
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // <-- This makes it persistent
        }
        else
        {
            // If another AudioManager exists, destroy this new one.
            Destroy(gameObject);
            return; // Stop running code on this duplicate
        }
        // --- End Singleton Setup ---

        // Get the AudioSource component
        backgroundMusic = GetComponent<AudioSource>();
        
        // --- 2. Load Mute Preference ---
        // Load the saved setting (0 for false, 1 for true)
        // Default to 0 (not muted) if no setting is found
        isMuted = PlayerPrefs.GetInt(MutePrefKey, 0) == 1;
        backgroundMusic.mute = isMuted;
    }

    void Start()
    {
        // Auto-play the background music if it's not already playing
        if (!backgroundMusic.isPlaying)
        {
            backgroundMusic.loop = true; // Ensure it loops
            backgroundMusic.Play();
        }
    }

    // --- 3. Public Mute Function ---
    // This is what the button will call
    public void ToggleMusic()
    {
        isMuted = !isMuted;
        backgroundMusic.mute = isMuted;

        // Save the preference (1 for true, 0 for false)
        PlayerPrefs.SetInt(MutePrefKey, isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    // --- 4. Public Getter ---
    // This lets the UI check the current mute state
    public bool IsMuted()
    {
        return isMuted;
    }
}