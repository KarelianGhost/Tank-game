using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tank
{
    public TankGun gun;
    public TankBody body;
    public bool isMoving = false;
    public bool isRotating = false;
    public bool isShooting = false;
    public bool isReloaded = true;
    public int x;
    public int y;
    public float hp;
    public enum Direction {
        top,
        left,
        down,
        right
    };
    public Direction dir;
}

public class TankController : MonoBehaviour
{
    [SerializeField] private Tank tank = new Tank();
    int targetX = 0, targetY = 0;
    Tank.Direction targetDirection;
    Vector3 movementTarget = Vector3.zero;
    Quaternion rotationTarget;
    protected virtual void Start()
    {
        transform.position = GameInit.map.GetCellCenterPosition(tank.x, tank.y);
        GameInit.map.SetValue(tank.x, tank.y, 1);

        if (tank.gun != null & tank.body != null)
            LoadTank(tank.gun,tank.body);
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
                    targetX = tank.x;
                    targetY = tank.y + 1;
                    break;
                case Tank.Direction.right:
                    targetX = tank.x + 1;
                    targetY = tank.y;
                    break;
                case Tank.Direction.down:
                    targetX = tank.x;
                    targetY = tank.y - 1;
                    break;
                case Tank.Direction.left:
                    targetX = tank.x - 1;
                    targetY = tank.y;
                    break;
            }
            //Debug.Log(GameInit.map.GetValue(targetX, targetY));
            if (GameInit.map.GetValue(targetX, targetY) == 0) {
                tank.isMoving = true;
                GameInit.map.SetValue(targetX, targetY, 1);
                GameInit.map.SetValue(tank.x, tank.y, 0);
                movementTarget = GameInit.map.GetCellCenterPosition(targetX, targetY);
            }
        }
    }
    private void Move(Vector3 target) {
        if (transform.position == target) {
            tank.isMoving = false;
            tank.x = targetX;
            tank.y = targetY;
            return;
        }
        float step = tank.body.speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
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
    
    private void Rotate(Quaternion target) {
        if (transform.rotation == target) {
            tank.isRotating = false;
            tank.dir = targetDirection;
            return;
        }
        float step = tank.body.rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, step);
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
        tank.isShooting = false;
    }

    private void TakeDamage(float damage)
    {
        tank.hp -= damage;
    }

    protected int GetPositionX()
    {
        return tank.x;
    }

    protected int GetPositionY()
    {
        return tank.y;
    }

    protected bool CheckMovement()
    {
        return tank.isMoving;
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
        GameInit.map.SetValue(tank.x, tank.y, 0);
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
