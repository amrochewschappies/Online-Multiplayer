using UnityEngine;

namespace rayzngames 
{
	public class ContactProvider : MonoBehaviour
	{
		bool contact;  
		public bool GetCOntact() 
		{
			return contact;
		}
		private void OnTriggerStay(Collider other)
		{
			contact = true;
			Debug.Log("Collision on the asshole");
		}
		private void OnTriggerExit(Collider other)
		{
			contact = false;
		}
	}
}