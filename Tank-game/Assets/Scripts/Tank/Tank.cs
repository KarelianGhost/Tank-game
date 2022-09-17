using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tank : DestructableObject
{
    public TankGun gun;
    public TankBody body;
    public bool isMoving = false;
    public bool isRotating = false;
    public bool isShooting = false;
    public bool isReloaded = true;

}

public class TankController : MonoBehaviour
{
    [SerializeField] private Tank tank = new Tank();
    MapLocation target = new MapLocation(0, 0);
    Tank.Direction targetDirection;
    Vector3 movementTarget = Vector3.zero;
    Quaternion rotationTarget;
    protected virtual void Start()
    {
        transform.position = GameInit.map.GetCellCenterPosition(new MapLocation(tank.pos.x, tank.pos.y));
        GameInit.map.SetValue(new MapLocation(tank.pos.x, tank.pos.y), 1);

        if (tank.gun != null && tank.body != null)
            LoadTank(tank.gun,tank.body);

        switch (tank.dir)
        {
            case Tank.Direction.top:
                transform.Rotate(new Vector3(0, 0, 0));
                break;
            case Tank.Direction.right:
                transform.Rotate(new Vector3(0, 90, 0));
                break;
            case Tank.Direction.down:
                transform.Rotate(new Vector3(0, 180, 0));
                break;
            case Tank.Direction.left:
                transform.Rotate(new Vector3(0, 270, 0));
                break;
        }
    }
    protected virtual void Update() {
        if (tank.hp <= 0)
        {
            DestroyTank();
        }
        if (tank.isShooting)
        {
            Shoot();
        }
        if (tank.isRotating) {
            Rotate(rotationTarget);
        }
        if (tank.isMoving) {
            Move(movementTarget);
        }
    }

    private void GetMovementTarget() {
        if (!tank.isMoving && !tank.isRotating) {
            switch (tank.dir) {
                case Tank.Direction.top:
                    target = new MapLocation(tank.pos.x, tank.pos.y + 1);
                    break;
                case Tank.Direction.right:
                    target = new MapLocation(tank.pos.x + 1, tank.pos.y);
                    break;
                case Tank.Direction.down:
                    target = new MapLocation(tank.pos.x, tank.pos.y - 1);
                    break;
                case Tank.Direction.left:
                    target = new MapLocation(tank.pos.x - 1, tank.pos.y);
                    break;
            }
            //Debug.Log(GameInit.map.GetValue(targetX, targetY));
            if (GameInit.map.GetValue(target) == 0) {
                tank.isMoving = true;
                GameInit.map.SetValue(target, 1);
                GameInit.map.SetValue(tank.pos, 0);
                tank.pos = target;
                movementTarget = GameInit.map.GetCellCenterPosition(target);
            }
        }
    }
    private void Move(Vector3 targetPos) {
        if (transform.position == targetPos) {
            tank.isMoving = false;
            //tank.pos = target; 
            return;
        }
        float step = tank.body.speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
    }

    private void GetRotationTarget(Tank.Direction targetDir) {
        if (!tank.isMoving && !tank.isRotating) {
            switch (targetDir) {
                case Tank.Direction.top:
                    rotationTarget = Quaternion.Euler(0, 0, 0);
                    targetDirection = Tank.Direction.top;
                    break;
                case Tank.Direction.right:
                    rotationTarget = Quaternion.Euler(0, 90, 0);
                    targetDirection = Tank.Direction.right;
                    break;
                case Tank.Direction.down:
                    rotationTarget = Quaternion.Euler(0, 180, 0);
                    targetDirection = Tank.Direction.down;
                    break;
                case Tank.Direction.left:
                    rotationTarget = Quaternion.Euler(0, 270, 0);
                    targetDirection = Tank.Direction.left;
                    break;
            }
            tank.isRotating = true;
        }
    }
    
    private void Rotate(Quaternion targetQ) {
        if (transform.rotation == targetQ) {
            tank.isRotating = false;
            tank.dir = targetDirection;
            return;
        }
        float step = tank.body.rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetQ, step);
    }

    protected int CalculateMovement(Tank.Direction targetDir) {
        if (targetDir == tank.dir) {
            GetMovementTarget();
            return 0;
        } else {
            GetRotationTarget(targetDir);
            return 1;
        }
    }

    protected void TriggerShooting()
    {
        if (tank.isReloaded)
        {
            tank.isShooting = true;
            tank.isReloaded = false;
            StartCoroutine(Reload(tank.gun.fireRate));
        }
    }

    private void Shoot()
    {
        Transform gun = gameObject.transform.GetChild(1); //Опять же опасная вещь, зависящая от порядка загрузки частей танка. Нужно найти иной способ. Вторая такая в PlayerController.Start()
        GameObject projectile = Instantiate(tank.gun.projectile, gun.Find("Projectile Origin").position, gun.rotation);
        ProjectileController pController = projectile.GetComponent<ProjectileController>();
        pController.damage = tank.gun.damage;
        pController.projectileSpeed = tank.gun.projectileSpeed;
        pController.faction = tank.faction;
        tank.isShooting = false;
    }

    private void TakeDamage(float damage)
    {
        tank.hp -= damage;
    }
    protected MapLocation GetPosition()
    {
        return tank.pos;
    }

    public void SetPosition(MapLocation pos)
    {
        tank.pos = pos;
    }
    public void SetRotation(MapObject.Direction dir)
    {
        tank.dir = dir;
    }

    protected bool CheckMovement()
    {
        return tank.isMoving;
    }
    protected void SetFaction(DestructableObject.Faction faction)
    {
        tank.faction = faction;
    }
    protected bool CheckRotation()
    {
        return tank.isRotating;
    }

    protected GameObject GetTankObject()
    {
        return gameObject;
    }

    protected virtual void DestroyTank()
    {
        GameInit.map.SetValue(tank.pos, 0);
        Destroy(gameObject);
    }

    private void LoadTank(TankGun gun, TankBody body)
    {
        LoadTankBody(body);
        LoadTankGun(gun);
    }

    private void LoadTankBody(TankBody body)
    {
        tank.hp = tank.body.maxHp;
        foreach (Transform child in this.transform)
        {
            if (Application.isEditor)
                DestroyImmediate(child.gameObject);
            else
                Destroy(child.gameObject);
        }
        GameObject model = Instantiate(body.model);
        model.transform.SetParent(this.transform);
        model.transform.localPosition = new Vector3(0, 0.5f, 0);
        model.transform.rotation = Quaternion.identity;
    }

    private void LoadTankGun(TankGun gun)
    {
        foreach (Transform child in this.transform)
        {
            if (Application.isEditor)
                DestroyImmediate(child.gameObject);
            else
                Destroy(child.gameObject);
        }
        GameObject model = Instantiate(gun.model);
        model.transform.SetParent(this.transform);
        model.transform.localPosition = new Vector3(0, 0.8f, 0);
        model.transform.rotation = Quaternion.identity;
    }

    private void OnTriggerEnter(Collider other)
    {
        ProjectileController pController = other.GetComponent<ProjectileController>();
        if (pController != null)
        {
            if (pController.faction != tank.faction)
                TakeDamage(pController.damage);
            Destroy(other.gameObject);
        }
    }

    IEnumerator Reload(float reloadingTime)
    {
        yield return new WaitForSeconds(reloadingTime);
        tank.isReloaded = true;
    }
}
