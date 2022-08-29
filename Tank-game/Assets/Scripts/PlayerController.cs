using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int x;
    [SerializeField] private int y;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private int direction;
    [SerializeField] private Rigidbody playerRb;


    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            playerRb.AddRelativeForce(Vector3.forward * speed, ForceMode.Impulse);
        } else if (Input.GetKey(KeyCode.S))
        {
            playerRb.AddRelativeForce(Vector3.back * speed, ForceMode.Impulse);
        } else if (Input.GetKey(KeyCode.A))
        {
            playerRb.AddRelativeTorque(Vector3.down * speed, ForceMode.Impulse);
        } else if (Input.GetKey(KeyCode.D))
        {
            playerRb.AddRelativeTorque(Vector3.up * speed, ForceMode.Impulse);
        }
    }
}
