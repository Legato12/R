using UnityEngine;

/// <summary>
/// Handles spinning the roulette top and UI transitions.
/// </summary>
public class S1tartSpin : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody top;
    [SerializeField] private ScoreScr scoreScr;
    [SerializeField] private AudioClip roulette;

    [Header("Settings")]
    [SerializeField] private float minSpeed = 200f;
    [SerializeField] private float maxSpeed = 300f;

    [Header("State")]
    private float rotateSpeed;
    private bool rouletteSoundPlayed = false;
    public bool TwistStart = false;
    public bool Stopped = false;

    private void Start()
    {
        if (top != null)
            top.maxAngularVelocity = 1000f;

        ResetSpinState();
    }

    public void StartSpinTwist()
    {
        rotateSpeed = Random.Range(minSpeed, maxSpeed);
        TwistStart = true;
        Stopped = false;
        rouletteSoundPlayed = false;

        Debug.Log($"[S1tartSpin] New spin started with speed: {rotateSpeed}");
    }

    private void FixedUpdate()
    {
        if (TwistStart && !Stopped)
        {
            PerformTwist();
        }
    }

    private void PerformTwist()
    {
        if (rotateSpeed > 0f)
        {
            PlayRouletteSoundOnce();
            rotateSpeed -= 10f * Time.deltaTime;
            transform.Rotate(Vector3.up, Time.deltaTime * rotateSpeed, Space.World);
        }
        else
        {
            StopSpin();
        }
    }

    private void PlayRouletteSoundOnce()
    {
        if (!rouletteSoundPlayed && SoundManager.Instance != null && roulette != null)
        {
            SoundManager.Instance.PlaySFX(roulette);
            rouletteSoundPlayed = true;
        }
    }

    public void StopSpin()
    {
        rotateSpeed = 0f;
        TwistStart = false;
        Stopped = true;

        if (scoreScr != null)
        {
            if (scoreScr.currentBetText != null)
                scoreScr.currentBetText.gameObject.SetActive(false);

            if (scoreScr.keepPlay != null)
                scoreScr.keepPlay.gameObject.SetActive(true);
        }

        Debug.Log("[S1tartSpin] Spin stopped.");
    }

    private void ResetSpinState()
    {
        rotateSpeed = 0f;
        TwistStart = false;
        Stopped = false;
        rouletteSoundPlayed = false;
    }
}
