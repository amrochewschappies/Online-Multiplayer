using UnityEngine;

public class TrailSpawner : MonoBehaviour
{
    public GameObject colliderPrefab; // Small object with a collider
    public float spawnInterval = 0.1f;
    public float lifeTime;
    private float timer = 0f;
    public Transform grandparent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grandparent = transform.parent?.parent;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            GameObject trailCollider = Instantiate(colliderPrefab, transform.position, Quaternion.LookRotation(grandparent.transform.forward));
            Destroy(trailCollider, lifeTime);
        }
    }
}
