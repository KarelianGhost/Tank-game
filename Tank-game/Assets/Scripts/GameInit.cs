using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    public static Map map;
    public int playerSpawnPosX = 0;
    public int playerSpawnPosY = 0;

    // Start is called before the first frame update
    void Start()
    {
        map = new Map(playerSpawnPosX,playerSpawnPosY);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
