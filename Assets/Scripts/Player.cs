using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody2D rb2d;
    Vector2 moveInput;

    [Header("Movement")]
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float jumpCooldown = 0.5f;
    float jumpCooldownTimer = 0f;

    [Header("Health")]
    [SerializeField] int maxHP = 3;
    int currentHP;
    [SerializeField] Slider hpSlider;

    [Header("Lives (หัวใจ)")]
    [SerializeField] int maxLives = 3;                   // มีหัวใจสูงสุดกี่ดวง (3)
    int currentLives;                                   // หัวใจที่เหลืออยู่ตอนนี้
    [SerializeField] TextMeshProUGUI livesText;         // Text "x 3" ข้างรูปหัวใจ

    [Header("Effects")]
    [SerializeField] GameObject deathEffect;

    [Header("UI Coin")]
    [SerializeField] TextMeshProUGUI scoreText;

    [Header("Scenes")]
    [SerializeField] string gameOverSceneName = "GameOver";


    // Checkpoint
    Vector2 respawnPoint;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        // ตั้งค่าเริ่มต้น
        currentHP = maxHP;
        currentLives = maxLives;

        respawnPoint = transform.position;

        UpdateHPBar();
        UpdateLivesText();
        UpdateScoreDisplay();
    }

    void Update()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), 0);

        if (jumpCooldownTimer > 0f)
            jumpCooldownTimer -= Time.deltaTime;

        if (Input.GetButtonDown("Jump") && jumpCooldownTimer <= 0f)
        {
            rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCooldownTimer = jumpCooldown;
        }
    }

    void FixedUpdate()
    {
        rb2d.linearVelocity = new Vector2(moveInput.x * speed, rb2d.linearVelocity.y);
    }

    // ================= HEALTH & LIVES =================

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHPBar();

        // ถ้า HP หมด = ตาย 1 ครั้ง
        if (currentHP <= 0)
        {
            // เล่นเอฟเฟกต์ตาย (ถ้ามี)
            if (deathEffect != null)
                Instantiate(deathEffect, transform.position, Quaternion.identity);

            // ใช้หัวใจ 1 ดวง
            currentLives--;
            currentLives = Mathf.Clamp(currentLives, 0, maxLives);
            UpdateLivesText();

            if (currentLives > 0)
            {
                // ยังมีหัวใจเหลือ → Respawn ที่ Checkpoint
                Respawn();
            }
            else
            {
                // หัวใจหมด → Game Over
                GameOver();
                SceneManager.LoadScene(gameOverSceneName);
            }
        }
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHPBar();
    }

    void UpdateHPBar()
    {
        if (hpSlider != null)
        {
            hpSlider.value = (float)currentHP / maxHP;
        }
    }

    void UpdateLivesText()
    {
        if (livesText != null)
        {
            livesText.text = "x " + currentLives;
        }
    }

    void Respawn()
    {
        // กลับไปจุด Checkpoint ล่าสุด
        transform.position = respawnPoint;

        // รีเลือดเต็ม
        currentHP = maxHP;
        UpdateHPBar();

        rb2d.linearVelocity = Vector2.zero;
    }

    void GameOver()
    {
        Debug.Log("GAME OVER: หัวใจหมดแล้ว");

        // เลือกได้หลายแบบ:
        // 1) ปิดตัวละคร
        // gameObject.SetActive(false);

        // 2) โหลดซีน Game Over
        // SceneManager.LoadScene("GameOver");

        // ตอนนี้ขอแค่ปิดตัวละครไปก่อน
        gameObject.SetActive(false);
    }

    // ================= CHECKPOINT =================

    public void SetCheckpoint(Vector2 newCheckpoint)
    {
        respawnPoint = newCheckpoint;
        Debug.Log("Checkpoint set at: " + respawnPoint);
    }

    // ================= COIN UI (อิงจาก GameManager) =================

    public void AddScore(int amount)
    {
        if (GameManager.Instance != null)
            GameManager.Instance.AddCoins(amount);

        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            int coinsToShow = 0;

            if (GameManager.Instance != null)
                coinsToShow = GameManager.Instance.Coins;

            scoreText.text = "Coin: " + coinsToShow;
        }
    }
}
