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
				Destroy(other.gameObject);
				// Go up to parent, then to grandparent, then find sibling named "Camera"
				Transform grandparent = transform.parent?.parent;
				if (grandparent != null)
				{
					Transform cameraTransform = grandparent.Find("Main Camera"); // Make sure this matches the actual name
					if (cameraTransform != null)
					{
						CameraScript cameraScript = cameraTransform.GetComponent<CameraScript>();
						if (cameraScript != null)
						{
							cameraScript.TriggerShake(0.1f, 1f);
						}
						else
						{
							Debug.LogWarning("CameraScript not found on Camera GameObject");
						}
					}
					else
					{
						Debug.LogWarning("Camera GameObject not found as sibling of grandparent");
					}
				}
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
