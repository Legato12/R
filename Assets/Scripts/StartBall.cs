using UnityEngine;

/// Launches the ball at the start and stores the result when the ball collides with another object.
public class StartBall : MonoBehaviour
{
    [Header("References")]
    public Rigidbody ballRB;
    public ScoreScr scoreScr;

    [Header("Force Settings")]
    public float minForce = 1500f;
    public float maxForce = 2500f;
    private float force;

    void Start()
    {
        scoreScr = FindObjectOfType<ScoreScr>();

        if (ballRB != null)
        {
            // Randomize force within the specified range
            force = Random.Range(minForce, maxForce);
            ballRB.maxAngularVelocity = 1000;
            // Launch the ball in the negative Z direction
            ballRB.AddForce(new Vector3(0, 0, force), ForceMode.Impulse);
        }
        else
        {
            
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        // Store the name of the collider the ball enters (e.g., for scoring)
        if (scoreScr != null)
        {
            scoreScr.resultt = other.gameObject.name;
        }
    }
}
