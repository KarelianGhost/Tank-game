using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : TankController
{
    private Transform turretTransform;
    private Camera mainCamera;

    protected override void Start()
    {
        base.Start();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        turretTransform = GetTankObject().transform.GetChild(1); //Опасное место, поскольку очень сильно зависит от порядка прикреплённых объектов. Вторая такая в Tank.Shoot()
    }
    protected override void Update()
    {
        RotateTurret();
        base.Update();
        if (Input.GetKey(KeyCode.W)) {
            CalculateMovement(Tank.Direction.top);
        } else if (Input.GetKey(KeyCode.S)) {
            CalculateMovement(Tank.Direction.down);
        } else if (Input.GetKey(KeyCode.A)) {
            CalculateMovement(Tank.Direction.left);
        } else if (Input.GetKey(KeyCode.D)) {
            CalculateMovement(Tank.Direction.right);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TriggerShooting();
        }
    }

    protected override void DestroyTank()
    {
        Debug.Log("Player killed");
        base.DestroyTank();
    }

    private void RotateTurret()
    {
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            turretTransform.LookAt(new Vector3(pointToLook.x, turretTransform.position.y, pointToLook.z));
        }
    }
}
