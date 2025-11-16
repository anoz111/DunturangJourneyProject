using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D rb2d;
    Vector2 moveInput;

    [SerializeField] int level = 1;
    [SerializeField] int currentExp = 0;
    [SerializeField] int expToNextLevel = 5; // ต้องการ EXP เท่าไหร่ถึงจะเลเวลอัพ 1 ครั้ง

    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float jumpCooldown = 0.5f;
    [SerializeField] int maxHP = 3;
    int currentHP;

    [SerializeField] GameObject deathEffect;

    [SerializeField] TextMeshProUGUI scoreText; // แสดงจำนวนเหรียญ
    [SerializeField] Slider hpSlider;

    float jumpCooldownTimer = 0f;

    // --- Checkpoint System ---
    Vector2 respawnPoint;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        currentHP = maxHP;

        respawnPoint = transform.position;

        UpdateScoreDisplay();
        UpdateHPBar();
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

    // --- Health & Death ---
    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        UpdateHPBar();

        if (currentHP <= 0)
        {
            if (deathEffect != null)
                Instantiate(deathEffect, transform.position, Quaternion.identity);

            Respawn();
        }
    }

    void Respawn()
    {
        transform.position = respawnPoint;
        currentHP = maxHP;
        UpdateHPBar();
        rb2d.linearVelocity = Vector2.zero;
    }

    public void SetCheckpoint(Vector2 newCheckpoint)
    {
        respawnPoint = newCheckpoint;
        Debug.Log("Checkpoint set at: " + respawnPoint);
    }

    // --- Coins / Score ---
    // ตอนนี้ใช้สำหรับ "อัปเดต UI" อย่างเดียว ไม่ได้เพิ่มเหรียญแล้ว
    public void AddScore(int amount)
    {
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

    void UpdateHPBar()
    {
        if (hpSlider != null)
            hpSlider.value = (float)currentHP / maxHP;
    }

    public void AddExp(int amount)
    {
        currentExp += amount;
        Debug.Log("EXP + " + amount + " => " + currentExp + "/" + expToNextLevel);

        // เช็คเลเวลอัพ
        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            level++;
            Debug.Log("LEVEL UP! Level ตอนนี้ = " + level);

            // จะเพิ่มค่า expToNextLevel ตามเลเวลก็ได้ เช่น
            // expToNextLevel += 5;
        }
    }
}
