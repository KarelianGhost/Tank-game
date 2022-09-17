using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EnemyGroup
{
    public GameObject mob;
    public int amount;
}

[CreateAssetMenu(fileName = "Wave", menuName = "Wave")]
public class Wave : ScriptableObject
{
    public int maxActive;
    public List<EnemyGroup> wave;
}
