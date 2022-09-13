using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : TankController
{
    public override void Update()
    {
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
    }
}
