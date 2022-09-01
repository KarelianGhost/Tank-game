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
    [SerializeField] protected Tank tank = new Tank();
    int targetX = 0, targetY = 0;
    Tank.Direction targetDirection;
    Vector3 movementTarget = Vector3.zero;
    Quaternion rotationTarget;
    private void Start()
    {
        transform.position = GameInit.map.map.GetCellCenterPosition(tank.x, tank.y);
        GameInit.map.map.SetValue(tank.x, tank.y, 1);
    }
    private void Update() {
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
            Debug.Log(GameInit.map.map.GetValue(targetX, targetY));
            if (GameInit.map.map.GetValue(targetX, targetY) == 0) {
                tank.isMoving = true;
                GameInit.map.map.SetValue(targetX, targetY, 1);
                GameInit.map.map.SetValue(tank.x, tank.y, 0);
                movementTarget = GameInit.map.map.GetCellCenterPosition(targetX, targetY);
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

    protected void CalculateMovement(Tank.Direction targetDir) {
        if (targetDir == tank.dir) {
            GetMovementTarget();
        } else {
            GetRotationTarget(targetDir);
        }
    }
}
