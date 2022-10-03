using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    public delegate void OnDamageTaken(GameObject damageTaker, float remainingHealth, float maxHealth, DestructableObject.Faction faction, bool isPlayer);
    public delegate void OnTankDestroy(Tank tank, Vector3 modelPos);
    public static event OnDamageTaken onDamageTaken;
    public static event OnTankDestroy onTankDestroy;
    public void RaiseOnDamageTaken(GameObject damageTaker, float remainingHealth, float maxHealth, DestructableObject.Faction faction, bool isPlayer)
    {
        if (onDamageTaken != null)
        {
            onDamageTaken(damageTaker, remainingHealth, maxHealth, faction, isPlayer);
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
