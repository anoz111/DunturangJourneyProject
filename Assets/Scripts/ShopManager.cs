using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    [Header("Key / Unlock Settings")]
    [SerializeField] private int keyPrice = 10;
    [SerializeField] private int requiredLevel = 5;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private string nextStageSceneName = "2ndFloor";

    void Start()
    {
        UpdateCoinText();
    }

    void Update()
    {
        UpdateCoinText();
    }

    void UpdateCoinText()
    {
        if (coinText != null)
        {
            int coins = 0;
            if (GameManager.Instance != null)
                coins = GameManager.Instance.Coins;

            coinText.text = "Coin: " + coins;
        }
    }

    public void BuyKey()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("GameManager.Instance == null : ยังไม่มี GameManager ในซีนแรก?");
            return;
        }

        int currentCoins = GameManager.Instance.Coins;
        int currentLevel = GameManager.Instance.Level;

        if (currentLevel < requiredLevel)
        {
            Debug.Log("เลเวลไม่ถึง! ต้องมีเลเวลอย่างน้อย " + requiredLevel);
            return;
        }

        if (currentCoins < keyPrice)
        {
            Debug.Log("เหรียญไม่พอ ต้องใช้ " + keyPrice + " เหรียญ");
            return;
        }

        bool spent = GameManager.Instance.SpendCoins(keyPrice);
        if (!spent)
        {
            Debug.LogWarning("ไม่สามารถหักเหรียญได้");
            return;
        }

        UpdateCoinText();

        if (lockIcon != null)
            lockIcon.SetActive(false);

        Debug.Log("ซื้อกุญแจสำเร็จ ไปด่านถัดไป: " + nextStageSceneName);

        SceneManager.LoadScene(nextStageSceneName);
    }

    public void BackToStage()
    {
        SceneManager.LoadScene("Stage1");
    }
}
