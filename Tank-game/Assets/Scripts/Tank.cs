using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tank
{
    public bool isMoving = false;
    public bool isRotating = false;
    public int x;
    public int y;
    public float speed;
    public float rotationSpeed;
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
    private void Start()
    {
        transform.position = GameInit.map.GetCellCenterPosition(tank.x, tank.y);
        GameInit.map.SetValue(tank.x, tank.y, 1);
    }
    public virtual void Update() {
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
        float step = tank.speed * Time.deltaTime;
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
        float step = tank.rotationSpeed * Time.deltaTime;
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
}
