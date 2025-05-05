using UnityEngine;

public class TrailCollider : MonoBehaviour
{
	public GameObject ExplosionParticleFX;
	public CameraScript cameraScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		if (gameObject.CompareTag("Team1Trail"))
		{
			if (other.gameObject.CompareTag("Team2"))
			{
				Debug.Log("Collision on the asshole");
				Instantiate(ExplosionParticleFX, other.gameObject.transform.position, Quaternion.identity);
				cameraScript.TriggerShake(0.1f, 1f);
			}
		}
		else if (gameObject.CompareTag("Team2Trail"))
		{
			if (other.gameObject.CompareTag("Team1"))
			{
				Debug.Log("Collision on the asshole");
				Instantiate(ExplosionParticleFX, other.gameObject.transform.position, Quaternion.identity);
				cameraScript.TriggerShake(0.1f, 1f);
			}
		}
	}
}
