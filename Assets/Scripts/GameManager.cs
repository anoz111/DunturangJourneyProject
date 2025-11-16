using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // จำนวนเหรียญรวม (ใช้ได้ทุกซีน)
    public int Coins { get; private set; } = 0;

    void Awake()
    {
        // ทำให้เป็น Singleton + อยู่ข้ามซีน
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        Debug.Log("Coins total: " + Coins);
    }

    public bool SpendCoins(int amount)
    {
        if (Coins < amount)
        {
            Debug.Log("Not enough coins!");
            return false;
        }

        Coins -= amount;
        Debug.Log("Coins left: " + Coins);
        return true;
    }
}
