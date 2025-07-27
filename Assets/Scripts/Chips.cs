using UnityEngine;

/// Handles chip selection, scaling, and visual feedback.
public class Chips : MonoBehaviour
{
    [Header("References")]
    public ScoreScr scoreScr;

    [Header("Chip Settings")]
    public int value;
    public AudioClip chipSound;

    private Vector3 defaultScale;
    private Color defaultColor;
    private Renderer chipRenderer;

    // Scale of chip on mouseover
    private const float selectedScale = 1.5f;

    void Start()
    {
        chipRenderer = GetComponent<Renderer>();
        if (chipRenderer == null)
            Debug.LogWarning("Chips: No Renderer attached!");

        defaultScale = transform.localScale;
        defaultColor = chipRenderer.material.color;
    }

    private void OnMouseEnter()
    {
        if (scoreScr != null && !scoreScr.clicked)
        {
            SoundManager.Instance?.PlaySFX(chipSound);
            SetScale(selectedScale);
        }
    }

    private void OnMouseExit()
    {
        if (scoreScr != null && !scoreScr.clicked)
        {
            SetScale(1f);
        }
    }

    private void OnMouseDown()
    {
        if (scoreScr == null) return;

        if (scoreScr.clicked && scoreScr.objNameClicked == gameObject.name)
        {
            Deselect();
            scoreScr.clicked = false;
            scoreScr.SetValue = 0;
            scoreScr.objNameClicked = "";
        }
        else
        {
            scoreScr.DeselectAllChips();
            scoreScr.objNameClicked = gameObject.name;
            SetScale(selectedScale);
            scoreScr.clicked = true;
            scoreScr.SetValue = value;
        }
    }

    public void Deselect()
    {
        SetScale(1f);
    }

    void Update()
    {
        if (scoreScr == null) return;

        // Only when changed, not each frame
        if (scoreScr.CancelDisabled)
        {
            if (chipRenderer.material.color != Color.gray)
                chipRenderer.material.color = Color.gray;
            if (transform.localScale != defaultScale)
                SetScale(1f);

            scoreScr.clicked = false;
            scoreScr.SetValue = 0;
        }
        else
        {
            if (chipRenderer.material.color != defaultColor)
                chipRenderer.material.color = defaultColor;
        }
    }

    private void SetScale(float scale)
    {
        transform.localScale = defaultScale * scale;
    }
}
