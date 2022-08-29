using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurret : MonoBehaviour
{
    [SerializeField] private Transform turretTransform;
    [SerializeField] private Camera mainCamera;

    // Update is called once per frame
    private void Start()
    {

    }
    void Update()
    {
        RotateTurret();
    }
    
    private void RotateTurret()
    {
        //Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        //moveVelocity = moveInput * moveSpeed;

        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            //Debug.DrawLine(cameraRay.origin, pointToLook, Color.cyan);

            turretTransform.LookAt(new Vector3(pointToLook.x, turretTransform.position.y, pointToLook.z));
        }
    }
}
