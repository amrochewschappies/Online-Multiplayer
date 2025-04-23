using UnityEngine;
using Mirror;

public class BikeController : NetworkBehaviour
{
    public float speed = 10f;
    public float turnSpeed = 50f;

    void Update()
    {
        if (!isOwned) return; // Correct way to check authority in latest Mirror

        float move = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

        transform.Translate(Vector3.forward * move);
        transform.Rotate(Vector3.up * turn);
    }
}