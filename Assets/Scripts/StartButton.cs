using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles logic for the roulette Start button.
/// </summary>
public class StartButton : MonoBehaviour
{
    [Header("References")]
    public Button button;
    public S1tartSpin startSpinScr;
    public SpawnBall spawnBallScr;
    public ScoreScr scoreScr;
    public Button restart;

    [Header("State")]
    public bool roulettSpining = false;

    private void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (roulettSpining || scoreScr == null || scoreScr.totalBet == 0)
            return;

        roulettSpining = true;

        if (scoreScr.cancelBet != null)
            scoreScr.cancelBet.gameObject.SetActive(false);

        scoreScr.CancelDisabled = true;

        if (startSpinScr != null)
            startSpinScr.StartSpinTwist(); // Only call this — it resets all state inside

        if (spawnBallScr != null)
            spawnBallScr.SpawnEnemyWave(1);

        button.gameObject.SetActive(false);

        if (scoreScr.currentBetText != null)
            scoreScr.currentBetText.text = "Spinning...";
    }

    public void Restart()
    {
        if (roulettSpining)
        { 
        
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
