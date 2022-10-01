using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    public delegate void OnDamageTaken();
    public delegate void OnTankDestroy(Tank tank, Vector3 modelPos);
    public static event OnDamageTaken onDamageTaken;
    public static event OnTankDestroy onTankDestroy;
    public void RaiseOnDamageTaken()
    {
        if (onDamageTaken != null)
        {
            onDamageTaken();
        }
    }
    public void RaiseOnTankDestroy(Tank tank, Vector3 modelPos)
    {
        if (onTankDestroy != null)
        {
            onTankDestroy(tank, modelPos);
        }
    }
}
