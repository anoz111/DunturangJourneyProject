using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // เงิน 2 สกุล
    public int Coins { get; private set; } = 0;   // ใช้ซื้อกุญแจ
    public int Gems  { get; private set; } = 0;   // ใช้อัปเกรด

    // เลเวล (ใช้เป็นเงื่อนไขซื้อกุญแจ)
    public int Level { get; private set; } = 1;

    // ค่าสเตตัสที่อัปเกรดแล้ว (ข้ามซีนได้)
    public int   ExtraHeartQuota { get; private set; } = 0;
    public float SpeedBonus      { get; private set; } = 0f;
    public float JumpBonus       { get; private set; } = 0f;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ===== Coins =====
    public void AddCoins(int amount)  { Coins += amount; Debug.Log("Coins total: " + Coins); }
    public bool SpendCoins(int amount)
    {
        if (Coins < amount) return false;
        Coins -= amount; Debug.Log("Coins left: " + Coins); return true;
    }

    // ===== Gems =====
    public void AddGems(int amount)   { Gems += amount; Debug.Log("Gems total: " + Gems); }
    public bool SpendGems(int amount)
    {
        if (Gems < amount) return false;
        Gems -= amount; Debug.Log("Gems left: " + Gems); return true;
    }

    // ===== Level / EXP =====
    public void AddExp(int amount) { Level += amount; Debug.Log("Player Level: " + Level); }

    // ===== Upgrades =====
    public void AddHeartQuota(int amount)   { ExtraHeartQuota += amount; }
    public void AddSpeedBonus(float amount) { SpeedBonus      += amount; }
    public void AddJumpBonus(float amount)  { JumpBonus       += amount; }

    // (เผื่อโค้ดเก่าเรียกชื่อเดิม)
    public void AddLevel(int amount) { AddExp(amount); }
}
