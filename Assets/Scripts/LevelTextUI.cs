using UnityEngine;
using TMPro;

public class LevelTextUI : MonoBehaviour
{
    private TextMeshProUGUI tmp;

    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLevelChanged += Refresh;
            GameManager.Instance.OnExpChanged += Refresh;
        }
        Refresh();
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLevelChanged -= Refresh;
            GameManager.Instance.OnExpChanged -= Refresh;
        }
    }

    void Refresh()
    {
        if (tmp == null || GameManager.Instance == null) return;
        tmp.text = $"Level : {GameManager.Instance.Level}";
    }
}
