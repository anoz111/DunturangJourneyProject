using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelExpUI : MonoBehaviour
{
    [SerializeField] private Slider expSlider;            // Slider EXP (min 0, max 1)
    [SerializeField] private TextMeshProUGUI levelText;   // "Level : X"
    [SerializeField] private TextMeshProUGUI expText;     // "A / B" (ถ้าอยากโชว์ตัวเลข)

    void OnEnable()
    {
        Subscribe();
        Refresh();
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnExpChanged -= Refresh;
            GameManager.Instance.OnLevelChanged -= Refresh;
        }
    }

    void Subscribe()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.OnExpChanged += Refresh;
        GameManager.Instance.OnLevelChanged += Refresh;
    }

    void Refresh()
    {
        if (GameManager.Instance == null) return;

        if (expSlider != null)
        {
            expSlider.minValue = 0f;
            expSlider.maxValue = 1f;
            expSlider.wholeNumbers = false; // สำคัญห้ามติ๊ก Whole Numbers
            expSlider.value = GameManager.Instance.ExpProgress01;
        }

        if (levelText != null)
            levelText.text = $"Level : {GameManager.Instance.Level}";

        if (expText != null)
            expText.text = $"{GameManager.Instance.CurrentExp} / {GameManager.Instance.ExpToNext}";
    }
}
