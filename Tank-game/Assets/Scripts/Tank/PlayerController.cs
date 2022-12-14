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
        SetFaction(DestructableObject.Faction.ally);
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        turretTransform = GetTankObject().transform.GetChild(1); //??????? ?????, ????????? ????? ?????? ??????? ?? ??????? ????????????? ????????. ?????? ????? ? Tank.Shoot()
        GameInit.playerHealthBar.SetMaxHealth(GetMaxHealth());
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

    protected override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        GameInit.events.RaiseOnDamageTaken(gameObject, GetHealth(), GetMaxHealth(),Tank.Faction.ally,true);
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
