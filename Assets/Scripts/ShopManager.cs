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

    [Header("Popup ถ้าเลเวล/เหรียญไม่พอ")]
    [SerializeField] private GameObject notEnoughPopup;          // Panel popup
    [SerializeField] private TextMeshProUGUI popupMessageText;   // ข้อความใน popup

    void Start()
    {
        UpdateCoinText();

        // เผื่อเราลืมปิดใน Inspector
        if (notEnoughPopup != null)
            notEnoughPopup.SetActive(false);
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
            ShowPopup("ไม่พบ GameManager\nเช็คซีนแรกว่ามี GameManager หรือยัง");
            return;
        }

        int currentCoins = GameManager.Instance.Coins;
        int currentLevel = GameManager.Instance.Level;

        // เช็คเลเวลก่อน
        if (currentLevel < requiredLevel)
        {
            Debug.Log("เลเวลไม่ถึง! ต้องมีเลเวลอย่างน้อย " + requiredLevel);
            ShowPopup($"เลเวลไม่เพียงพอ\nต้องการเลเวลอย่างน้อย {requiredLevel}");
            return;
        }

        // เช็คเหรียญ
        if (currentCoins < keyPrice)
        {
            Debug.Log("เหรียญไม่พอ ต้องใช้ " + keyPrice + " เหรียญ");
            ShowPopup($"เหรียญไม่เพียงพอ\nต้องใช้ {keyPrice} เหรียญ");
            return;
        }

        // หักเหรียญ
        bool spent = GameManager.Instance.SpendCoins(keyPrice);
        if (!spent)
        {
            Debug.LogWarning("ไม่สามารถหักเหรียญได้");
            ShowPopup("เกิดข้อผิดพลาดในการหักเหรียญ");
            return;
        }

        UpdateCoinText();

        // ปลดล็อกไอคอน
        if (lockIcon != null)
            lockIcon.SetActive(false);

        Debug.Log("ซื้อกุญแจสำเร็จ ไปด่านถัดไป: " + nextStageSceneName);

        SceneManager.LoadScene(nextStageSceneName);
    }

    // เรียกจากปุ่ม "OK" บน popup
    public void ClosePopup()
    {
        if (notEnoughPopup != null)
            notEnoughPopup.SetActive(false);
    }

    void ShowPopup(string message)
    {
        if (popupMessageText != null)
            popupMessageText.text = message;

        if (notEnoughPopup != null)
            notEnoughPopup.SetActive(true);
    }

    public void BackToStage()
    {
        SceneManager.LoadScene("Stage1");
    }
}
