using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    [Header("Currency UI")]
    [SerializeField] TextMeshProUGUI coinText;  // ใช้ซื้อกุญแจ
    [SerializeField] TextMeshProUGUI gemText;   // ใช้อัปเกรด

    [Header("Key / Unlock Settings (ใช้ COIN)")]
    [SerializeField] int keyPrice = 10;
    [SerializeField] int requiredLevel = 5;
    [SerializeField] GameObject lockIcon;
    [SerializeField] string nextStageSceneName = "2ndFloor";

    [Header("Popup ไม่พอ")]
    [SerializeField] GameObject notEnoughPopup;
    [SerializeField] TextMeshProUGUI popupMessageText;

    [Header("Upgrade Prices (ใช้ GEM)")]
    [SerializeField] int heartUpgradePrice = 5;
    [SerializeField] int speedUpgradePrice = 5;
    [SerializeField] int jumpUpgradePrice = 5;

    [Header("Upgrade Amount")]
    [SerializeField] int heartQuotaAmount = 1;
    [SerializeField] float speedBonusAmount = 1f;
    [SerializeField] float jumpBonusAmount = 2f;

    void Start()
    {
        RefreshCurrencyUI();
        if (notEnoughPopup != null) notEnoughPopup.SetActive(false);
    }

    void Update() => RefreshCurrencyUI();

    void RefreshCurrencyUI()
    {
        if (GameManager.Instance == null) return;
        if (coinText) coinText.text = "Coin: " + GameManager.Instance.Coins;
        if (gemText) gemText.text = "Gem: " + GameManager.Instance.Gems;
    }

    // ===== ซื้อกุญแจด้วย COIN =====
    public void BuyKey()
    {
        if (GameManager.Instance == null) { ShowPopup("ไม่พบ GameManager"); return; }

        int coins = GameManager.Instance.Coins;
        int lvl = GameManager.Instance.Level;

        if (lvl < requiredLevel)
        {
            ShowPopup($"เลเวลไม่เพียงพอ\nต้องการเลเวลอย่างน้อย {requiredLevel}");
            return;
        }
        if (coins < keyPrice)
        {
            ShowPopup($"เหรียญไม่เพียงพอ\nต้องใช้ {keyPrice} Coin");
            return;
        }
        if (!GameManager.Instance.SpendCoins(keyPrice))
        {
            ShowPopup("เกิดข้อผิดพลาดในการหักเหรียญ");
            return;
        }

        RefreshCurrencyUI();
        if (lockIcon) lockIcon.SetActive(false);
        SceneManager.LoadScene(nextStageSceneName);
    }

    // ===== อัปเกรดด้วย GEM =====
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

    bool TrySpendGem(int price)
    {
        if (GameManager.Instance == null) { ShowPopup("ไม่พบ GameManager"); return false; }
        if (GameManager.Instance.Gems < price)
        { ShowPopup($"เพชรไม่เพียงพอ\nต้องใช้ {price} Gem"); return false; }
        if (!GameManager.Instance.SpendGems(price))
        { ShowPopup("เกิดข้อผิดพลาดในการหักเพชร"); return false; }
        return true;
    }

    // ===== Popup =====
    public void ClosePopup() { if (notEnoughPopup) notEnoughPopup.SetActive(false); }
    void ShowPopup(string m)
    {
        if (popupMessageText) popupMessageText.text = m;
        if (notEnoughPopup) notEnoughPopup.SetActive(true);
    }

    public void BackToStage() => SceneManager.LoadScene("Stage1");
}
