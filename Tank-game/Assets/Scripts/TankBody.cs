using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TankBody", menuName = "Tank Body")]
public class TankBody : ScriptableObject
{
    public new string name;
    public float speed;
    public float rotationSpeed;
    public float maxHp;
    public GameObject model;

}
