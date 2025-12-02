using UnityEngine;
using TMPro;

public class HUDOrb : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI orbText;

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
        if (GameManager.Instance == null || orbText == null) return;
        orbText.text = "Orb: " + GameManager.Instance.Orbs;
    }
}
