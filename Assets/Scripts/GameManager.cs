using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int Coins { get; private set; } = 0;

    public int Level { get; private set; } = 0;


    void Awake()
    {
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

    public void AddLevel(int amount)
    {
        Level += amount;
        Debug.Log("Player Level: " + Level);
    }
}
