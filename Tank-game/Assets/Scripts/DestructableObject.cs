using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MapObject
{
    public float hp;
    public enum Faction
    {
        ally,
        neutral,
        enemy
    }
    public Faction faction;
    

}
