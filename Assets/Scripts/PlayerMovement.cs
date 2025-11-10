using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb2d;
    Vector2 moveInput;

    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float jumpCooldown = 0.5f;
    [SerializeField] int maxHP = 3;
    int currentHP;

    [SerializeField] GameObject deathEffect;

    [SerializeField] TextMeshProUGUI scoreText;
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
        rb2d.velocity = new Vector2(moveInput.x * speed, rb2d.velocity.y);
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
        rb2d.velocity = Vector2.zero;
    }

    // --- Checkpoint ---
    public void SetCheckpoint(Vector2 newCheckpoint)
    {
        respawnPoint = newCheckpoint;
        Debug.Log("Checkpoint set at: " + respawnPoint);
    }

    // --- Score ---
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    void UpdateHPBar()
    {
        if (hpSlider != null)
            hpSlider.value = (float)currentHP / maxHP;
    }
}
