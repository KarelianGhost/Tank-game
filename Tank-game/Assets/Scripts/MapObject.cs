using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject
{
    public MapLocation pos;
    public float hp;
    public enum Faction
    {
        ally,
        neutral,
        enemy
    }
    public Faction faction;
}

