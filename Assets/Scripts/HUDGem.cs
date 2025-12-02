using UnityEngine;
using TMPro;

public class HUDGem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gemText; // ใส่ TextTMP ที่เขียนว่า "Gem: 0"

    void OnEnable()
    {
        Refresh();
        if (GameManager.Instance != null)
            GameManager.Instance.OnCurrencyChanged += Refresh;
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnCurrencyChanged -= Refresh;
    }

    void Refresh()
    {
        if (GameManager.Instance == null || gemText == null) return;
        gemText.text = "Gem: " + GameManager.Instance.Gems;
    }
}
