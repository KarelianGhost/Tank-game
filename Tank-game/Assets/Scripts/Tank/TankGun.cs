using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TankGun", menuName = "Tank Gun")]
public class TankGun : ScriptableObject
{
    public new string name;
    public float fireRate;
    public float damage;
    public float projectileSpeed;
    public GameObject projectile;
    public GameObject model;
}
