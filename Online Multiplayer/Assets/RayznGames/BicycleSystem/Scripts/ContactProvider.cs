using UnityEngine;

namespace rayzngames 
{
	public class ContactProvider : MonoBehaviour
	{
		public CameraController cameraController;
		bool contact;
		public GameObject ExplosionParticleFX;
		public bool GetCOntact() 
		{
			return contact;
		}
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Team 1"))
			{
				contact = true;
				Debug.Log("Collision on the asshole");
				Instantiate(ExplosionParticleFX, other.gameObject.transform.position, Quaternion.identity);
				cameraController.TriggerShake(0.1f, 1f);
			}
		}
		private void OnTriggerExit(Collider other)
		{
			contact = false;
		}
	}
}