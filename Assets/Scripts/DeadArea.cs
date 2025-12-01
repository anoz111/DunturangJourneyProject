using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DeadArea : MonoBehaviour
{
    [Tooltip("ความเสียหายที่จะใส่ให้ผู้เล่นเมื่อเข้า DeadArea")]
    [SerializeField] int damage = 10;

    // --- ถ้าใช้ Trigger ---
    void OnTriggerEnter2D(Collider2D other)
    {
        TryDamage(other.gameObject);
    }

    // --- เผื่อเผลอไม่ได้ติ๊ก IsTrigger ก็ยังทำงานได้ ---
    void OnCollisionEnter2D(Collision2D other)
    {
        TryDamage(other.gameObject);
    }

    void TryDamage(GameObject go)
    {
        if (!go.CompareTag("Player")) return;

        // เผื่อคอลลายเดอร์อยู่บนลูกของ Player ให้ไล่หาในพาเรนต์ด้วย
        var player = go.GetComponent<Player>() ?? go.GetComponentInParent<Player>();
        if (player != null)
        {
            player.TakeDamage(damage); // ถ้าโดนดอกเดียวตาย ระบบใน Player จะ Respawn ให้เอง
        }
    }

#if UNITY_EDITOR
    // วาดกรอบช่วยดูใน Scene
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
        var col = GetComponent<Collider2D>();
        if (col is BoxCollider2D box)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(box.offset, box.size);
        }
    }
#endif
}
