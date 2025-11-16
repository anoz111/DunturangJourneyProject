using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D rb2d;
    Vector2 moveInput;

    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float jumpCooldown = 0.5f;
    [SerializeField] int maxHP = 3;
    int currentHP;

    [SerializeField] GameObject deathEffect;

    [SerializeField] TextMeshProUGUI scoreText; // แสดงจำนวนเหรียญในซีนแรก
    int score = 0;

    [SerializeField] Slider hpSlider;

    float jumpCooldownTimer = 0f;

    // --- Checkpoint System ---
    Vector2 respawnPoint;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        currentHP = maxHP;

        // จุดเกิดเริ่มต้น
        respawnPoint = transform.position;

        // โหลดเหรียญรวมจาก GameData มาใช้เป็นค่าตั้งต้นในซีนนี้
        score = GameData.Coins;

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
        // ถ้าโปรเจกต์คุณใช้ velocity ปกติ ก็เปลี่ยนเป็น rb2d.velocity ได้
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

            Debug.Log("Player Died! Respawning...");

            Respawn();
        }
    }

    void Respawn()
    {
        transform.position = respawnPoint; // Checkpoint ล่าสุด
        currentHP = maxHP;
        UpdateHPBar();
        rb2d.linearVelocity = Vector2.zero;
    }

    // --- Checkpoint ---
    public void SetCheckpoint(Vector2 newCheckpoint)
    {
        respawnPoint = newCheckpoint;
        Debug.Log("Checkpoint set at: " + respawnPoint);
    }

    // --- Score / Coins ---
    public void AddScore(int amount)
    {
        // เพิ่มเหรียญในตัวเก็บกลาง
        GameData.Coins += amount;

        // sync ค่าใน Player ให้ตรงกับเหรียญรวม
        score = GameData.Coins;

        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Coin: " + GameData.Coins;
        }
    }

    void UpdateHPBar()
    {
        if (hpSlider != null)
            hpSlider.value = (float)currentHP / maxHP;
    }
}
