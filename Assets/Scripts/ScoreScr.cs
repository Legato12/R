using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Linq;

/// Handles all scoring, bet logic, win/loss checking, and UI updates for the roulette game.
public class ScoreScr : MonoBehaviour
{
    [Header("References")]
    public S1tartSpin startSpinScr;
    public StartButton startButtonScr;
    public SpawnBall spawnBallScr;
    
    [Header("UI Elements")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI currentBetText;
    public TextMeshProUGUI wonText;
    public Button cancelBet;
    public Button keepPlay;

    [Header("Settings")]
    public AudioClip money;

    [Header("State")]
    public int score = 200;
    public string resultt;
    public bool betdone = false;
    public bool clicked = false;
    public string objNameClicked;
    public int SetValue;
    public bool CancelDisabled = false;
    public int totalBet = 0;
    public Dictionary<string, int> bets = new Dictionary<string, int>();

    private void Start()
    {
        if (cancelBet != null)
            cancelBet.onClick.AddListener(CancelBetClick);

        UpdateScoreUI();
        currentBetText.text = "Choose where you want to put your bet!";
    }

    private void Update()
    {
        ActiveCancelBet();

        if (startSpinScr != null && startSpinScr.Stopped)
        {
            startSpinScr.Stopped = false;
            CheckWin();
            Continue();
        }
    }

    /// Adds a bet to the current bets dictionary and updates UI.
    public bool AddBet(string area, int amount)
    {
        if (amount <= 0 || score < amount) return false;

        score -= amount;
        totalBet += amount;

        if (bets.ContainsKey(area))
            bets[area] += amount;
        else
            bets[area] = amount;

        UpdateScoreUI();
        UpdateBetText();

        betdone = true;
        return true;
    }

    /// Updates the player's cash UI text.
    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"Cash: {score}";
    }

    /// Updates bet summary text in the UI.
    private void UpdateBetText()
    {
        if (currentBetText == null) return;

        currentBetText.text = bets.Count == 0
            ? "Choose where you want to put your bet!"
            : "Your bets: " + string.Join(", ", bets.Select(kv => $"{kv.Value}$ on {kv.Key}"));
    }

    /// Continues the game after a spin (reset state and UI).
    private void Continue()
    {
        if (startButtonScr?.button != null)
            startButtonScr.button.gameObject.SetActive(true);

        betdone = false;

        if (startButtonScr != null)
            startButtonScr.roulettSpining = false;

        if (startSpinScr != null)
        {
            startSpinScr.Stopped = false;
            startSpinScr.TwistStart = false;
        }

        UpdateBetText();

        if (currentBetText != null)
            currentBetText.gameObject.SetActive(true);

        if (keepPlay != null)
        {
            keepPlay.gameObject.SetActive(true);
            var text = keepPlay.GetComponentInChildren<Text>();
            if (text != null)
                text.text = $"Last result: {resultt}";
        }

        cancelBet?.gameObject.SetActive(false);
        CancelDisabled = false;

        StartCoroutine(ClearAfterDelay());

        bets.Clear();
        totalBet = 0;
    }

    /// Delays clearing of win text and chips after round.
    private IEnumerator ClearAfterDelay()
    {
        yield return new WaitForSeconds(2);
        wonText?.gameObject.SetActive(false);

        DestroyGO("Ball");
        DestroyGO("CheapBlack");
        DestroyGO("CheapBlue");
        DestroyGO("CheapRed");
        DestroyGO("CheapGreen");

    }

    /// Destroy all GameObjects with given tag.
    public void DestroyGO(string tag)
    {
        foreach (var obj in GameObject.FindGameObjectsWithTag(tag))
            Destroy(obj);
    }

    /// Cancel all placed bets and refund player.
    private void CancelBetClick()
    {
        if (startButtonScr != null && startButtonScr.roulettSpining)
            return;

        score += totalBet;
        UpdateScoreUI();

        totalBet = 0;
        bets.Clear();
        betdone = false;
        UpdateBetText();

        DestroyGO("CheapBlack");
        DestroyGO("CheapBlue");
        DestroyGO("CheapRed");
        DestroyGO("CheapGreen");
    }

    /// Display win text and sound.
    public void YouWon(int prize)
    {
        if (wonText != null)
        {
            wonText.gameObject.SetActive(true);
            wonText.color = Color.green;
            wonText.text = $"YOU WIN {prize}$";
        }

        SoundManager.Instance?.PlaySFX(money);

        if (keepPlay != null)
            keepPlay.gameObject.SetActive(true);
    }

    /// Display lose text.
    public void YouLose()
    {
        if (wonText != null)
        {
            wonText.gameObject.SetActive(true);
            wonText.color = Color.grey;
            wonText.text = "You lose";
        }
    }

    /// Enables or disables the cancel bet button depending on state.
    private void ActiveCancelBet()
    {
        if (cancelBet == null) return;

        cancelBet.gameObject.SetActive(totalBet > 0 && !CancelDisabled);
    }

    /// Deselects all chip objects.
    public void DeselectAllChips()
    {
        foreach (var chipObj in GameObject.FindGameObjectsWithTag("Chip"))
        {
            var chipScript = chipObj.GetComponent<Chips>();
            chipScript?.Deselect();
        }

        clicked = false;
        SetValue = 0;
        objNameClicked = "";
    }

    /// Checks if the player won and pays out accordingly.
    private void CheckWin()
    {
        if (string.IsNullOrEmpty(resultt)) return;

        Match numMatch = Regex.Match(resultt, @"\d+");
        if (!numMatch.Success) return;

        int num = int.Parse(numMatch.Value);
        string col = Regex.Replace(resultt, @"\d+", "").Trim();

        int totalReturn = 0;
        int netWin = 0;

        foreach (var kv in bets)
        {
            int payout = GetPayoutRatio(kv.Key, num, col);
            if (payout > 0)
            {
                int win = kv.Value * payout;
                totalReturn += kv.Value + win;
                netWin += win;
            }
        }

        score += totalReturn;
        UpdateScoreUI();

        if (netWin > 0)
            YouWon(netWin);
        else
            YouLose();
    }

    /// Determines payout multiplier for given bet area.
    private int GetPayoutRatio(string area, int num, string col)
    {
        if (int.TryParse(Regex.Match(area, @"\d+").Value, out int areaNum) && areaNum == num)
            return 35;

        if (area == "Red" && col == "Red") return 1;
        if (area == "Black" && col == "Black") return 1;
        if (area == "Even" && num % 2 == 0 && num != 0) return 1;
        if (area == "Odd" && num % 2 == 1) return 1;
        if (area == "1to18" && num >= 1 && num <= 18) return 1;
        if (area == "19to36" && num >= 19 && num <= 36) return 1;
        if (area == "First12" && num >= 1 && num <= 12) return 2;
        if (area == "Second12" && num >= 13 && num <= 24) return 2;
        if (area == "Third12" && num >= 25 && num <= 36) return 2;
        if (area == "2to1(1)" && num % 3 == 1) return 2;
        if (area == "2to1(2)" && num % 3 == 2) return 2;
        if (area == "2to1(3)" && num % 3 == 0 && num != 0) return 2;

        return 0;
    }
}
