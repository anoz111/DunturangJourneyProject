using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int Coins { get; private set; } = 0;   
    public int Gems  { get; private set; } = 0;   
    public int Orbs  { get; private set; } = 0;  

    public int Level { get; private set; } = 1;
    public int CurrentExp { get; private set; } = 0;
    public int ExpToNext  { get; private set; } = 10;

    public int   ExtraHeartQuota { get; private set; } = 0;
    public float SpeedBonus      { get; private set; } = 0f;
    public float JumpBonus       { get; private set; } = 0f;

    public bool KeyUnlocked { get; private set; } = false;

    public event Action OnCurrencyChanged;
    public event Action OnExpChanged;
    public event Action OnLevelChanged;

    private bool hasSnapshot = false;
    private int  snapCoins, snapGems, snapOrbs, snapLevel, snapCurrentExp;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        ExpToNext = GetExpNeededForLevel(Level);
        LoadPersistentFlags();
    }

    void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    int GetExpNeededForLevel(int level) => 10 + 5 * (level - 1);

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

        Debug.Log($"[GM] EXP {CurrentExp}/{ExpToNext} | Lv {Level}");
    }

    public float ExpProgress01 => ExpToNext <= 0 ? 1f : (float)CurrentExp / ExpToNext;

    public void AddLevel(int amount)
    {
        for (int i = 0; i < amount; i++)
            AddExp(ExpToNext - CurrentExp);
    }

    public void AddCoins(int amount)
    {
        Coins += Mathf.Max(0, amount);
        Debug.Log("[GM] Coins total: " + Coins);
        OnCurrencyChanged?.Invoke();
    }

    public bool SpendCoins(int amount)
    {
        if (amount <= 0) return true;
        if (Coins < amount) return false;
        Coins -= amount;
        Debug.Log("[GM] Coins left: " + Coins);
        OnCurrencyChanged?.Invoke();
        return true;
    }

    public void AddGems(int amount)
    {
        Gems += Mathf.Max(0, amount);
        Debug.Log("[GM] Gems total: " + Gems);
        OnCurrencyChanged?.Invoke();
    }

    public bool SpendGems(int amount)
    {
        if (amount <= 0) return true;
        if (Gems < amount) return false;
        Gems -= amount;
        Debug.Log("[GM] Gems left: " + Gems);
        OnCurrencyChanged?.Invoke();
        return true;
    }

    public void AddOrbs(int amount)
    {
        Orbs += Mathf.Max(0, amount);
        Debug.Log("[GM] Orbs total: " + Orbs);
        OnCurrencyChanged?.Invoke();
    }

    public bool SpendOrbs(int amount)
    {
        if (amount <= 0) return true;
        if (Orbs < amount) return false;
        Orbs -= amount;
        Debug.Log("[GM] Orbs left: " + Orbs);
        OnCurrencyChanged?.Invoke();
        return true;
    }

    public bool ConvertOrbToGem()
    {
        if (!SpendOrbs(1)) return false;
        AddGems(1);
        return true;
    }
    public bool ConvertOrbToCoins(int coinPerOrb = 10)
    {
        if (!SpendOrbs(1)) return false;
        AddCoins(coinPerOrb);
        return true;
    }
    public void AddHeartQuota(int amount)  { ExtraHeartQuota += amount; Debug.Log("[GM] ExtraHeartQuota = " + ExtraHeartQuota); }
    public void AddSpeedBonus(float amount){ SpeedBonus += amount;      Debug.Log("[GM] SpeedBonus = " + SpeedBonus); }
    public void AddJumpBonus(float amount) { JumpBonus += amount;       Debug.Log("[GM] JumpBonus = " + JumpBonus); }

    public void UnlockKey()
    {
        KeyUnlocked = true;
        PlayerPrefs.SetInt("KEY_UNLOCKED", 1);
        PlayerPrefs.Save();
        Debug.Log("[GM] Key unlocked");
    }
    void LoadPersistentFlags()
    {
        KeyUnlocked = PlayerPrefs.GetInt("KEY_UNLOCKED", 0) == 1;
    }

    public void SaveRunSnapshot()
    {
        snapCoins      = Coins;
        snapGems       = Gems;
        snapOrbs       = Orbs;
        snapLevel      = Level;
        snapCurrentExp = CurrentExp;
        hasSnapshot    = true;

        Debug.Log($"[GM] Snapshot saved: coin={snapCoins}, gem={snapGems}, orb={snapOrbs}, lv={snapLevel}, exp={snapCurrentExp}");
    }

    public void RestoreRunSnapshot()
    {
        if (!hasSnapshot) return;

        Coins      = snapCoins;
        Gems       = snapGems;
        Orbs       = snapOrbs;
        Level      = snapLevel;
        CurrentExp = snapCurrentExp;
        ExpToNext  = GetExpNeededForLevel(Level);

        OnCurrencyChanged?.Invoke();
        OnExpChanged?.Invoke();
        OnLevelChanged?.Invoke();

        Debug.Log($"[GM] Restored: coin={Coins}, gem={Gems}, orb={Orbs}, lv={Level}, exp={CurrentExp}");
    }

    public void ClearRunSnapshot() => hasSnapshot = false;


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool isPlayableStage = scene.name == "MainStage" || scene.name == "2ndStage";
        if (isPlayableStage)
        {
            PlayerPrefs.SetString("LAST_STAGE", scene.name);
            PlayerPrefs.Save();

            SaveRunSnapshot();
            Debug.Log($"[GM] Stage loaded -> snapshot saved for {scene.name}");
        }
        else
        {
            Debug.Log($"[GM] Non-playable scene loaded: {scene.name}");
        }
    }
}
