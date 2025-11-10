using UnityEngine;

public class Projectiles2D : MonoBehaviour
{
    [Header("Shoot Settings")]
    [SerializeField] Transform shootPoint;
    [SerializeField] Rigidbody2D bulletPrefab;
    [SerializeField] float shootCooldown = 1f;
    float shootCooldownTimer = 0f;
    public int damage = 1;

    [Header("Crosshair Settings")]
    [SerializeField] GameObject crosshairPrefab;
    private GameObject crosshairInstance;

    void Start()
    {
        // ซ่อนเคอร์เซอร์ปกติ
        Cursor.visible = false;

        // สร้าง Crosshair prefab
        if (crosshairPrefab != null)
            crosshairInstance = Instantiate(crosshairPrefab);
    }

    void Update()
    {
        // อัพเดทตำแหน่ง Crosshair ให้ตามเมาส์
        if (crosshairInstance != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            crosshairInstance.transform.position = mousePos;
        }

        // ลดเวลาคูลดาวน์
        if (shootCooldownTimer > 0f)
            shootCooldownTimer -= Time.deltaTime;

        // ยิงกระสุนเมื่อคลิกซ้ายและคูลดาวน์หมด
        if (Input.GetMouseButtonDown(0) && shootCooldownTimer <= 0f)
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // คำนวณความเร็วกระสุนแบบโพรเจกไทล์
            Vector2 projectileVelocity = CalculateProjectileVelocity(shootPoint.position, mouseWorldPos, 1f);

            Rigidbody2D shootBullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            shootBullet.velocity = projectileVelocity;

            // รีเซ็ตคูลดาวน์
            shootCooldownTimer = shootCooldown;
        }
    }

    Vector2 CalculateProjectileVelocity(Vector2 origin, Vector2 target, float time)
    {
        Vector2 distance = target - origin;
        float velocityX = distance.x / time;
        float velocityY = distance.y / time + 0.5f * Mathf.Abs(Physics2D.gravity.y) * time;

        return new Vector2(velocityX, velocityY);
    }
}
