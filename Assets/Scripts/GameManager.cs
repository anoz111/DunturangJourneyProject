using UnityEngine;
using System;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)] // ให้ GameManager ตื่นก่อนตัวอื่น ลดปัญหา null
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // ===== เงิน 2 สกุล =====
    public int Coins { get; private set; } = 0;   // ใช้ซื้อกุญแจ
    public int Gems { get; private set; } = 0;   // ใช้อัปเกรด

    // ===== LEVEL & EXP =====
    public int Level { get; private set; } = 1;        // เลเวลปัจจุบัน
    public int CurrentExp { get; private set; } = 0;   // EXP ที่มีในเลเวลนี้
    public int ExpToNext { get; private set; } = 10;  // EXP ที่ต้องใช้เพื่ออัปเลเวล

    // แจ้งเตือน UI ให้รีเฟรช
    public event Action OnExpChanged;
    public event Action OnLevelChanged;

    // ===== ค่าสเตตัสอัปเกรด (พกข้ามซีนได้) =====
    public int ExtraHeartQuota { get; private set; } = 0;
    public float SpeedBonus { get; private set; } = 0f;
    public float JumpBonus { get; private set; } = 0f;

    // ===== Snapshot ของรอบด่านนี้ (ใช้สำหรับ Retry) =====
    private bool hasSnapshot = false;
    private int snapCoins, snapGems, snapLevel, snapCurrentExp;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        // กำหนด EXP ที่ต้องใช้ครั้งแรกตามสูตร
        ExpToNext = GetExpNeededForLevel(Level);
    }

    // ---------- สูตร EXP ต่อเลเวล (ปรับได้) ----------
    int GetExpNeededForLevel(int level)
    {
        // ตัวอย่าง: เริ่ม 10 เพิ่ม +5 ต่อเลเวล: 10, 15, 20, ...
        return 10 + 5 * (level - 1);
        // โตแบบยกกำลัง (ตัวเลือก): return Mathf.RoundToInt(10 * Mathf.Pow(1.25f, level - 1));
    }

    // ---------- EXP API ----------
    public void AddExp(int amount)
    {
        if (amount <= 0) return;

        CurrentExp += amount;

        bool leveled = false;
        while (CurrentExp >= ExpToNext)
        {
            CurrentExp -= ExpToNext;
            Level++;
            ExpToNext = GetExpNeededForLevel(Level);
            leveled = true;
        }

        OnExpChanged?.Invoke();
        if (leveled) OnLevelChanged?.Invoke();

        Debug.Log($"EXP: {CurrentExp}/{ExpToNext} | Level: {Level}");
    }

    // สะดวกใช้กับ Slider (0..1)
    public float ExpProgress01 => ExpToNext <= 0 ? 1f : (float)CurrentExp / ExpToNext;

    // ---------- Coins ----------
    public void AddCoins(int amount)
    {
        Coins += amount;
        Debug.Log("Coins total: " + Coins);
    }

    public bool SpendCoins(int amount)
    {
        if (Coins < amount) return false;
        Coins -= amount;
        Debug.Log("Coins left: " + Coins);
        return true;
    }

    // ---------- Gems ----------
    public void AddGems(int amount)
    {
        Gems += amount;
        Debug.Log("Gems total: " + Gems);
    }

    public bool SpendGems(int amount)
    {
        if (Gems < amount) return false;
        Gems -= amount;
        Debug.Log("Gems left: " + Gems);
        return true;
    }

    // ---------- อัปเกรดค่าสเตตัส ----------
    public void AddHeartQuota(int amount) { ExtraHeartQuota += amount; }
    public void AddSpeedBonus(float amount) { SpeedBonus += amount; }
    public void AddJumpBonus(float amount) { JumpBonus += amount; }

    // เผื่อโค้ดเก่าที่เรียก AddLevel (ตีความว่าเลเวลอัปทันที)
    public void AddLevel(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            // เติม EXP ให้พอดีเพื่อเลเวลอัป 1 ครั้ง
            AddExp(ExpToNext - CurrentExp);
        }
    }

    // ====== SNAPSHOT API ======
    // เรียกตอน "เข้าเล่นด่าน" เพื่อจำค่าก่อนเริ่มรอบนั้น
    public void SaveRunSnapshot()
    {
        snapCoins = Coins;
        snapGems = Gems;
        snapLevel = Level;
        snapCurrentExp = CurrentExp;
        hasSnapshot = true;

        Debug.Log($"[GM] Snapshot saved: coins={snapCoins}, gems={snapGems}, lvl={snapLevel}, exp={snapCurrentExp}");
    }

    // เรียกตอน "ตาย/Retry" เพื่อย้อนค่ากลับก่อนเริ่มด่าน
    public void RestoreRunSnapshot()
    {
        if (!hasSnapshot) return;

        Coins = snapCoins;
        Gems = snapGems;
        Level = snapLevel;
        CurrentExp = snapCurrentExp;
        ExpToNext = GetExpNeededForLevel(Level);

        OnExpChanged?.Invoke();
        OnLevelChanged?.Invoke();

        Debug.Log($"[GM] Snapshot restored: coins={Coins}, gems={Gems}, lvl={Level}, exp={CurrentExp}");
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // เซ็ตว่า "ซีนนี้เป็นด่านเล่นจริงไหม"
        bool isPlayableStage = scene.name == "MainStage" || scene.name == "2ndStage";

        if (isPlayableStage)
        {
            // บันทึกชื่อด่านล่าสุดสำหรับปุ่ม Restart
            PlayerPrefs.SetString("LAST_STAGE", scene.name);
            PlayerPrefs.Save();

            // เซฟสแน็ปช็อตก่อนเริ่มรอบด่าน (ของที่เก็บในรอบนี้จะย้อนคืนได้ถ้าตาย)
            SaveRunSnapshot();
            Debug.Log($"[GM] Stage loaded -> snapshot saved for {scene.name}");
        }
        else
        {
            // ไม่ทำอะไรเป็นพิเศษกับ ShopScene / MainMenu / GameOver
            Debug.Log($"[GM] Non-playable scene loaded: {scene.name}");
        }
    }

}
