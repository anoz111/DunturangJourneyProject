using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    // สมมติว่ามีของขาย 3 อย่าง
    [SerializeField] private int item1Price = 3;
    [SerializeField] private int item2Price = 5;
    [SerializeField] private int item3Price = 10;

    void Start()
    {
        UpdateCoinText();
    }

    void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = "Coin: " + GameData.Coins;
        }
    }

    public void BuyItem1()
    {
        TryBuy(item1Price, "Item 1");
    }

    public void BuyItem2()
    {
        TryBuy(item2Price, "Item 2");
    }

    public void BuyItem3()
    {
        TryBuy(item3Price, "Item 3");
    }

    void TryBuy(int price, string itemName)
    {
        if (GameData.Coins >= price)
        {
            GameData.Coins -= price;
            UpdateCoinText();

            Debug.Log("ซื้อ " + itemName + " สำเร็จ! เหรียญที่เหลือ: " + GameData.Coins);
        }
        else
        {
            Debug.Log("เหรียญไม่พอซื้อ " + itemName);
        }
    }

    public void BackToStage()
    {
        // เปลี่ยนชื่อซีนให้ตรงกับของจริง
        SceneManager.LoadScene("Stage1");
    }
}
