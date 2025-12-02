using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PotionSpawnMarker : MonoBehaviour
{
    [Header("Prefab ที่จะเกิด")]
    public GameObject potionPrefab;

    [Header("ตั้งค่าเพิ่มเติม")]
    public bool spawnOnStart = true;
    public bool parentUnderThis = false;

    void Start()
    {

        GetComponent<Collider2D>().isTrigger = true;

        if (spawnOnStart && potionPrefab != null)
        {
            var go = Instantiate(potionPrefab, transform.position, Quaternion.identity);
            if (parentUnderThis) go.transform.SetParent(transform);
        }
    }
}
