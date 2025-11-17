using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody2D rb;
    // --- We removed the 'isGameOver' variable as it was unused ---

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Public method to be called to stop the ball's physics
    public void SetGameOver()
    {
        // --- 'isGameOver = true;' was removed ---
        
        // FIX: The property is 'velocity', not 'linearVelocity'
        rb.velocity = Vector2.zero; 
        rb.angularVelocity = 0f;
    }
}