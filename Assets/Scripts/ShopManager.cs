using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    [Header("Currency UI")]
    [SerializeField] private TextMeshProUGUI coinText;   // แสดงเหรียญ (ซื้อกุญแจ)
    [SerializeField] private TextMeshProUGUI gemText;    // แสดงเพชร (อัปเกรด)

    [Header("Key / Unlock (ใช้ COIN)")]
    [SerializeField] private int keyPrice = 10;
    [SerializeField] private int requiredLevel = 5;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private string nextStageSceneName = "2ndFloor";

    [Header("Upgrade Prices (ใช้ GEM)")]
    [SerializeField] private int heartUpgradePrice = 5;
    [SerializeField] private int speedUpgradePrice = 5;
    [SerializeField] private int jumpUpgradePrice  = 5;

    [Header("Upgrade Amount")]
    [SerializeField] private int   heartQuotaAmount = 1;   // +หัวใจ respawn ต่อครั้ง
    [SerializeField] private float speedBonusAmount = 1f;  // +สปีด ต่อครั้ง
    [SerializeField] private float jumpBonusAmount  = 2f;  // +แรงกระโดด ต่อครั้ง

    [Header("Popup")]
    [SerializeField] private GameObject notEnoughPopup;          // Panel popup รวม
    [SerializeField] private TextMeshProUGUI popupMessageText;   // ข้อความใน popup

    [Header("Popup Messages (แก้ได้จาก Inspector)")]
    [SerializeField] private string msgGMNotFound     = "ไม่พบ GameManager\nเช็คว่ามี GameManager ในซีนแรกหรือยัง";
    [SerializeField] private string msgLevelNotEnough = "เลเวลไม่เพียงพอ\nต้องการเลเวลอย่างน้อย {0}";
    [SerializeField] private string msgCoinNotEnough  = "เหรียญไม่เพียงพอ\nต้องใช้ {0} Coin";
    [SerializeField] private string msgGemNotEnough   = "เพชรไม่เพียงพอ\nต้องใช้ {0} Gem";
    [SerializeField] private string msgSpendError     = "เกิดข้อผิดพลาดในการหักสกุลเงิน";

    void Start()
    {
        RefreshCurrencyUI();
        if (notEnoughPopup != null) notEnoughPopup.SetActive(false);
    }

    void Update() => RefreshCurrencyUI();

    // ----------------- UI -----------------
    void RefreshCurrencyUI()
    {
        if (GameManager.Instance == null) return;

        if (coinText != null)
            coinText.text = "Coin: " + GameManager.Instance.Coins;

        if (gemText != null)
            gemText.text  = "Gem: "  + GameManager.Instance.Gems;
    }

    // =========== ซื้อกุญแจ (ใช้ Coin) ===========
    public void BuyKey()
    {
        if (GameManager.Instance == null) { ShowPopup(msgGMNotFound); return; }

        int coins = GameManager.Instance.Coins;
        int lvl   = GameManager.Instance.Level;

        if (lvl < requiredLevel) { ShowLevelNotEnough(requiredLevel); return; }
        if (coins < keyPrice)    { ShowCoinNotEnough(keyPrice);      return; }

        if (!GameManager.Instance.SpendCoins(keyPrice))
        { ShowPopup(msgSpendError); return; }

        RefreshCurrencyUI();

        if (lockIcon != null) lockIcon.SetActive(false);
        SceneManager.LoadScene(nextStageSceneName);
    }

    // =========== อัปเกรด (ใช้ Gem) ===========
    public void BuyHeartUpgrade()
    {
        if (!TrySpendGem(heartUpgradePrice)) return;
        GameManager.Instance.AddHeartQuota(heartQuotaAmount);
        RefreshCurrencyUI();
    }

    public void BuySpeedUpgrade()
    {
        if (!TrySpendGem(speedUpgradePrice)) return;
        GameManager.Instance.AddSpeedBonus(speedBonusAmount);
        RefreshCurrencyUI();
    }

    public void BuyJumpUpgrade()
    {
        if (!TrySpendGem(jumpUpgradePrice)) return;
        GameManager.Instance.AddJumpBonus(jumpBonusAmount);
        RefreshCurrencyUI();
    }

    // ช่วยเช็ค/หัก Gem พร้อม popup
    bool TrySpendGem(int price)
    {
        if (GameManager.Instance == null) { ShowPopup(msgGMNotFound); return false; }
        if (GameManager.Instance.Gems < price) { ShowGemNotEnough(price); return false; }
        if (!GameManager.Instance.SpendGems(price)) { ShowPopup(msgSpendError); return false; }
        return true;
    }

    // ----------------- Popup Helpers -----------------
    void ShowLevelNotEnough(int needLevel)
    {
        ShowPopup(string.Format(msgLevelNotEnough, needLevel));
    }

    void ShowCoinNotEnough(int price)
    {
        ShowPopup(string.Format(msgCoinNotEnough, price));
    }

    void ShowGemNotEnough(int price)
    {
        ShowPopup(string.Format(msgGemNotEnough, price));
    }

    void ShowPopup(string message)
    {
        if (popupMessageText != null) popupMessageText.text = message;
        if (notEnoughPopup    != null) notEnoughPopup.SetActive(true);
    }

    public void ClosePopup()
    {
        if (notEnoughPopup != null) notEnoughPopup.SetActive(false);
    }

    // (ถ้ามีปุ่มกลับไปเล่นด่านเดิม)
    public void BackToStage()
    {
        SceneManager.LoadScene("Stage1");
    }
}
