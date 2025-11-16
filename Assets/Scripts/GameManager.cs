using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // --------- เงิน (Coin) ---------
    public int Coins { get; private set; } = 0;

    // --------- เลเวล / แต้มจากการฆ่ามอน ---------
    public int Level { get; private set; } = 0;   // เริ่มเลเวล 0 (จะ +1 ทุกครั้งที่ฆ่ามอน)
    // ถ้าอยากเรียกว่า KillCount ก็เปลี่ยนชื่อได้เลย แต่หลักการเหมือนกัน

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

    // ====== จัดการเหรียญ ======
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

    // ====== จัดการเลเวล (แต้มจากการฆ่ามอน) ======
    public void AddLevel(int amount)
    {
        Level += amount;
        Debug.Log("Player Level: " + Level);
    }
}
