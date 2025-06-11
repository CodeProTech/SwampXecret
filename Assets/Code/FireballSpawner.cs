using UnityEngine;

public class FireballSpawner : MonoBehaviour
{
    public GameObject fireballPrefab;  // Das Fireball-Prefab
    public Transform spawnPoint;       // Der Punkt, an dem der Fireball gespawnt wird
    public float fireRate = 1f;        // Die Geschwindigkeit, mit der die Fireballs abgefeuert werden

    private float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && Time.time >= nextFireTime)
        {
            SpawnFireball();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void SpawnFireball()
    {
        FireballManager fireballManager = FindObjectOfType<FireballManager>();

        if (fireballManager != null && fireballManager.TryShootFireball())
        {
            Instantiate(fireballPrefab, spawnPoint.position, Quaternion.identity); // Setze Quaternion.identity, um keine Rotation anzuwenden
        }
        else
        {
            Debug.Log("Keine Fireballs mehr!");
        }
    }
}
