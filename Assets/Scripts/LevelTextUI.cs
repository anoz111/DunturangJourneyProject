using UnityEngine;
using TMPro;

public class LevelTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;

    void Start()
    {
        UpdateLevelText();
    }

    void Update()
    {
        UpdateLevelText();
    }

    void UpdateLevelText()
    {
        if (GameManager.Instance == null || levelText == null)
            return;

        levelText.text = "Level : " + GameManager.Instance.Level;
    }
}
