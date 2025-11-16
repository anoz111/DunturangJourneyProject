using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    [Header("Key / Unlock Settings")]
    [SerializeField] private int keyPrice = 10;                    // เหรียญที่ต้องใช้ซื้อกุญแจ
    [SerializeField] private int requiredLevel = 5;                // เลเวลขั้นต่ำ!!
    [SerializeField] private GameObject lockIcon;                  // ไอคอนล็อค
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
        int currentLevel = GameManager.Instance.Level;   // <<— ดึงเลเวลจาก GameManager

        // 1) เช็คเลเวลก่อน — ถ้าไม่ถึง ห้ามซื้อ!
        if (currentLevel < requiredLevel)
        {
            Debug.Log("❌ เลเวลไม่ถึง! ต้องมีเลเวลอย่างน้อย " + requiredLevel);
            return;
        }

        // 2) เช็คเหรียญ
        if (currentCoins < keyPrice)
        {
            Debug.Log("❌ เหรียญไม่พอ ต้องใช้ " + keyPrice + " เหรียญ");
            return;
        }

        // 3) หักเหรียญ
        bool spent = GameManager.Instance.SpendCoins(keyPrice);
        if (!spent)
        {
            Debug.LogWarning("ไม่สามารถหักเหรียญได้");
            return;
        }

        // 4) อัปเดต UI
        UpdateCoinText();

        // 5) เอาตัวล็อคออก
        if (lockIcon != null)
            lockIcon.SetActive(false);

        Debug.Log("✔ ซื้อกุญแจสำเร็จ! ไปด่านถัดไป: " + nextStageSceneName);

        // 6) โหลดซีนใหม่
        SceneManager.LoadScene(nextStageSceneName);
    }

    public void BackToStage()
    {
        SceneManager.LoadScene("Stage1");
    }
}
