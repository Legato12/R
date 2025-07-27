using UnityEngine;

/// Spawn ball and chips
public class SpawnBall : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject sBall;
    public GameObject sBlue;
    public GameObject sRed;
    public GameObject sBlack;
    public GameObject sGreen;

    [Header("Settings")]
    public Vector3 spawnPos = new Vector3(3.663f, 0.711f, -0.456f);

    /// Spawn Chip with color/type on position
    /// <param name="chipPrefab">prefab chip/param>
    /// <param name="pos">position of spawn</param>
    /// <returns>link to created object</returns>
    public GameObject SpawnBChip(GameObject chipPrefab, Vector3 pos)
    {
        if (chipPrefab == null)
        {
            Debug.LogWarning("SpawnBChip: chipPrefab is null!");
            return null;
        }

        GameObject chip = Instantiate(chipPrefab, pos, chipPrefab.transform.rotation);
        Rigidbody rb = chip.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        return chip;
    }

    /// spawn in spawn position
    /// <param name="count">how many spawn</param>
    /// <param name="prefab">spawn what (по умолчанию sBall)</param>
    public void SpawnEnemyWave(int count, GameObject prefab = null)
    {
        if (prefab == null)
            prefab = sBall;
        if (prefab == null)
        {
            Debug.LogWarning("SpawnEnemyWave: prefab is null!");
            return;
        }
        for (int i = 0; i < count; i++)
        {
            Instantiate(prefab, spawnPos, prefab.transform.rotation);
        }
    }
}
