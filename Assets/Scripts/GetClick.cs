using UnityEngine;

/// Handles click on betting areas, updates score and spawns chips.
public class GetClick : MonoBehaviour
{
    [Header("References")]
    public ScoreScr scoreScr;
    public StartButton startButtonScr;
    public SpawnBall spawnBallScr;

    /// Called when object is clicked.
    void OnMouseDown()

    {
        if (startButtonScr == null || scoreScr == null || spawnBallScr == null)
        {
            Debug.LogWarning("GetClick: Missing script reference!");
            return;
        }

        if (startButtonScr.roulettSpining || !scoreScr.clicked)
            return;

        // Add bet, if all good, spawn chip.
        if (scoreScr.AddBet(gameObject.name, scoreScr.SetValue))
        {
            var chipPrefab = GetChipByName(scoreScr.objNameClicked);
            if (chipPrefab != null)
            {
                Vector3 spawnPos = transform.position + new Vector3(0, 5, 0);
                spawnBallScr.SpawnBChip(chipPrefab, spawnPos);
            }
        }
    }

    /// Returns corresponding chip prefab based on clicked object name.
    GameObject GetChipByName(string objName)
    {
        if (string.IsNullOrEmpty(objName) || spawnBallScr == null)
            return null;

        if (objName.Contains("Black")) return spawnBallScr.sBlack;
        if (objName.Contains("Blue")) return spawnBallScr.sBlue;
        if (objName.Contains("Red")) return spawnBallScr.sRed;
        if (objName.Contains("Green")) return spawnBallScr.sGreen;

        return null;
    }
}
