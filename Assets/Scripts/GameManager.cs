using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // ‡ß‘π 2  °ÿ≈
    public int Coins { get; private set; } = 0;
    public int Gems { get; private set; } = 0;

    public int Level { get; private set; } = 1;

    //  ‡µµ— Õ—ª‡°√¥
    public int ExtraHeartQuota { get; private set; } = 0;
    public float SpeedBonus { get; private set; } = 0f;
    public float JumpBonus { get; private set; } = 0f;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ----- COIN -----
    public void AddCoins(int amount) { Coins += amount; }
    public bool SpendCoins(int amount)
    {
        if (Coins < amount) return false;
        Coins -= amount; return true;
    }

    // ----- GEM -----
    public void AddGems(int amount) { Gems += amount; }
    public bool SpendGems(int amount)
    {
        if (Gems < amount) return false;
        Gems -= amount; return true;
    }

    // ----- EXP / LEVEL -----
    public void AddExp(int amount) { Level += amount; }

    // ----- UPGRADE APPLY -----
    public void AddHeartQuota(int a) { ExtraHeartQuota += a; }
    public void AddSpeedBonus(float a) { SpeedBonus += a; }
    public void AddJumpBonus(float a) { JumpBonus += a; }
}
